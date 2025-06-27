using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using AntdUI;
using OllamaSharp.Models;
using Onllama.Tiny.Properties;
using Message = AntdUI.Message;
using System.Text.Json.Serialization;

namespace Onllama.Tiny
{
    public partial class FormImport : Form
    {
        private string tempDownloadPath = GetOllamaModelsPath();

        public FormImport()
        {
            InitializeComponent();
            
            // Загружаем языковые настройки
            LocalizationManager.LoadLanguagePreference();

            // Создаем временную директорию для загрузок
            if (!Directory.Exists(tempDownloadPath))
            {
                Directory.CreateDirectory(tempDownloadPath);
            }

            // Радикальное отключение SSL проверки для работы с HuggingFace
            ServicePointManager.ServerCertificateValidationCallback = 
                (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.DefaultConnectionLimit = 100;

            // Обновляем UI
            UpdateUITexts();
        }

        private static string GetOllamaModelsPath()
        {
            // Проверяем переменную окружения OLLAMA_MODELS
            string ollamaModels = Environment.GetEnvironmentVariable("OLLAMA_MODELS");
            if (!string.IsNullOrEmpty(ollamaModels) && Directory.Exists(ollamaModels))
            {
                return Path.Combine(ollamaModels, "downloads");
            }

            // Путь по умолчанию в Windows
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ollama", "models", "downloads");
            
            // Альтернативные пути
            string[] possiblePaths = {
                defaultPath,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ollama", "models", "downloads"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ollama", "models", "downloads"),
                Path.Combine(Path.GetTempPath(), "ollama_downloads")
            };

            foreach (string path in possiblePaths)
            {
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    return path;
                }
                catch
                {
                    continue;
                }
            }

            // Fallback
            return Path.Combine(Path.GetTempPath(), "ollama_downloads");
        }

        private static bool IsValidGGUFFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                // Проверяем расширение файла
                if (!filePath.ToLower().EndsWith(".gguf"))
                    return false;

                // Проверяем магические байты GGUF файла
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (fileStream.Length < 4)
                        return false;

                    var buffer = new byte[4];
                    fileStream.Read(buffer, 0, 4);
                    
                    // GGUF файлы начинаются с магических байтов "GGUF"
                    return buffer[0] == 0x47 && buffer[1] == 0x47 && buffer[2] == 0x55 && buffer[3] == 0x46; // "GGUF"
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateUITexts()
        {
            // Обновляем тексты на форме в соответствии с выбранным языком
            this.Text = LocalizationManager.GetTranslation("import_model");
            labelFile.Text = LocalizationManager.GetTranslation("model_file");
            labelName.Text = LocalizationManager.GetTranslation("model_name");
            labelType.Text = LocalizationManager.GetTranslation("model_type");
            labelQuantization.Text = LocalizationManager.GetTranslation("quantization");
            buttonOpen.Text = LocalizationManager.GetTranslation("change");
            buttonSave.Text = LocalizationManager.GetTranslation("import");
            
            // HuggingFace элементы
            tabPage1.Text = LocalizationManager.GetTranslation("model_file");
            tabPage2.Text = LocalizationManager.GetTranslation("huggingface");
            labelHfModel.Text = LocalizationManager.GetTranslation("hf_model_id");
            buttonHfSearch.Text = LocalizationManager.GetTranslation("hf_search");
            buttonHfDownload.Text = LocalizationManager.GetTranslation("hf_download");
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var f = new OpenFileDialog();
                f.Filter = "GGUF files (*.gguf)|*.gguf|All files (*.*)|*.*";
                f.Title = LocalizationManager.GetTranslation("select_model_file");
                
                if (f.ShowDialog() == DialogResult.OK)
                {
                    input1.Text = f.FileName;
                    
                    // Устанавливаем имя модели на основе имени файла
                    var fileName = Path.GetFileNameWithoutExtension(f.FileName);
                    inputName.Text = fileName.Split('.', '-').First();
                    
                    // Автоматически определяем тип модели на основе имени файла
                    foreach (var item in select1.Items)
                    {
                        var itemText = item.ToString() ?? string.Empty;
                        if (fileName.ToLower().Contains(itemText.ToLower()))
                        {
                            select1.SelectedValue = itemText;
                            break;
                        }
                    }
                    
                    // Для embedding моделей устанавливаем "none"
                    if (fileName.ToLower().Contains("embed") || 
                        fileName.ToLower().Contains("bge") || 
                        fileName.ToLower().Contains("m3e"))
                    {
                        select1.SelectedValue = "none";
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, LocalizationManager.GetTranslation("error"), 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(input1.Text) || string.IsNullOrEmpty(inputName.Text))
            {
                new Modal.Config(this, LocalizationManager.GetTranslation("empty_fields"), 
                    new[] {new Modal.TextLine(LocalizationManager.GetTranslation("please_select_file"), Style.Db.Error)},
                    TType.Error)
                {
                    OkType = TTypeMini.Error,
                }.open();
                return;
            }

            if (!File.Exists(input1.Text))
            {
                new Modal.Config(this, LocalizationManager.GetTranslation("error"), 
                    new[] {new Modal.TextLine(LocalizationManager.GetTranslation("file_not_found"), Style.Db.Error)},
                    TType.Error)
                {
                    OkType = TTypeMini.Error,
                }.open();
                return;
            }

            new Modal.Config(this, LocalizationManager.GetTranslation("confirm_import"), 
                new[] {new Modal.TextLine(inputName.Text, Style.Db.Primary)},
                TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = LocalizationManager.GetTranslation("import"),
                OnOk = _ =>
                {
                    Task.Run(ImportModelAsync);
                    return true;
                }
            }.open();
        }

        private async Task ImportModelAsync()
        {
            try
            {
                // Отключаем кнопки во время импорта
                buttonSave.Enabled = false;
                buttonOpen.Enabled = false;
                
                // Показываем прогресс
                Text = LocalizationManager.GetTranslation("importing_model");

                // Проверяем, что файл действительно в формате GGUF
                if (!IsValidGGUFFile(input1.Text))
                {
                    MessageBox.Show(LocalizationManager.GetTranslation("invalid_gguf_format"), 
                        LocalizationManager.GetTranslation("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var ggufBytes = await File.ReadAllBytesAsync(input1.Text);
                var ggufDigest = $"sha256:{BitConverter.ToString(SHA256.HashData(ggufBytes)).Replace("-", string.Empty).ToLower()}";
                var textInfo = new CultureInfo("en-US", false).TextInfo;

                var req = new CreateModelRequest
                {
                    Files = new Dictionary<string, string>
                    {
                        {new FileInfo(input1.Text).Name, ggufDigest}
                    },
                    Model = inputName.Text
                };
                
                if (!string.IsNullOrWhiteSpace(select2.Text) && select2.Text != LocalizationManager.GetTranslation("no_quantization"))
                    req.Quantize = select2.Text;

                if (!string.IsNullOrWhiteSpace(inputMf.Text)) 
                    req.Template = inputMf.Text;

                // Загружаем blob
                await Form1.OllamaApi.PushBlobAsync(ggufDigest, ggufBytes);
                
                // Создаем модель
                await foreach (var x in Form1.OllamaApi.CreateModelAsync(req))
                {
                    if (IsDisposed) return;
                    
                    Invoke(() => 
                    {
                        if (!IsDisposed)
                        {
                            Text = LocalizationManager.GetTranslation("importing_model") + " - " + textInfo.ToTitleCase(x.Status);
                        }
                    });
                }

                if (!IsDisposed)
                {
                    Invoke(() =>
                    {
                        Message.success(this, LocalizationManager.GetTranslation("import_complete"));
                        Close();
                    });
                }
            }
            catch (Exception exception)
            {
                if (!IsDisposed)
                {
                    Invoke(() =>
                    {
                        buttonSave.Enabled = true;
                        buttonOpen.Enabled = true;
                        Text = LocalizationManager.GetTranslation("import_model");
                        
                        string errorMessage = exception.Message;
                        if (exception.InnerException != null)
                            errorMessage += "\n\n" + exception.InnerException.Message;
                            
                        MessageBox.Show(errorMessage, LocalizationManager.GetTranslation("import_error"), 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
            }
        }

        private void select1_SelectedValueChanged(object sender, ObjectNEventArgs value)
        {
            inputMf.Text = @"FROM " + input1.Text + Environment.NewLine;
            switch (value.Value.ToString())
            {
                case "qwen2":
                    inputMf.Text += Resources.qwen2Tmp;
                    break;
                case "qwen1.5":
                    inputMf.Text += Resources.qwenTmp;
                    break;
                case "yi-1.5":
                    inputMf.Text += Resources.yi15Tmp;
                    break;
                case "yi":
                    inputMf.Text += Resources.yiTmp;
                    break;
                case "gemma":
                    inputMf.Text += Resources.gemmaTmp;
                    break;
                case "mistral":
                    inputMf.Text += Resources.mistralTmp;
                    break;
                case "deepseek-v2":
                    inputMf.Text += Resources.deekseekv2Tmp;
                    break;
                case "deepseek":
                    inputMf.Text += Resources.deekseekTmp;
                    break;
            }
        }

        private void FormImport_Load(object sender, EventArgs e)
        {
            UpdateUITexts();
            
            // Добавляем "Без квантизации" в список квантизации
            if (!select2.Items.Contains(LocalizationManager.GetTranslation("no_quantization")))
            {
                select2.Items.Add(LocalizationManager.GetTranslation("no_quantization"));
            }
        }

        private async void buttonHfSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputHfModel.Text))
                return;

            try
            {
                progressBarHf.Show();
                listBoxHfModels.Items.Clear();
                buttonHfDownload.Enabled = false;

                using (var handler = new HttpClientHandler() 
                { 
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true 
                })
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Onllama.Tiny/1.0");
                    client.Timeout = TimeSpan.FromSeconds(30); // Таймаут для поиска
                    var apiUrl = $"https://huggingface.co/api/models?search={WebUtility.UrlEncode(inputHfModel.Text)}&filter=gguf";
                    var response = await client.GetStringAsync(apiUrl);
                    
                    var models = JsonSerializer.Deserialize<List<HuggingFaceModel>>(response);
                    
                    if (models != null && models.Count > 0)
                    {
                        foreach (var model in models)
                        {
                            listBoxHfModels.Items.Add($"{model.ModelId} ({model.Downloads:N0} {LocalizationManager.GetTranslation("downloads_count")})");
                        }
                    }
                    else
                    {
                        listBoxHfModels.Items.Add(LocalizationManager.GetTranslation("no_models"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationManager.GetTranslation("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarHf.Hide();
            }
        }

        private void listBoxHfModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonHfDownload.Enabled = listBoxHfModels.SelectedIndex >= 0;
        }

        private async void buttonHfDownload_Click(object sender, EventArgs e)
        {
            if (listBoxHfModels.SelectedIndex < 0)
                return;

            try
            {
                progressBarHf.Show();
                
                // Получаем ID модели из выбранной строки
                string selectedModel = listBoxHfModels.SelectedItem.ToString();
                string modelId = selectedModel.Split(' ')[0]; // Берем только ID модели

                // Получаем доступные файлы модели
                using (var handler = new HttpClientHandler() 
                { 
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true 
                })
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Onllama.Tiny/1.0");
                    client.Timeout = TimeSpan.FromMinutes(10); // Увеличиваем таймаут для больших файлов
                    
                    // Получаем список файлов в репозитории модели
                    Message.info(this, LocalizationManager.GetTranslation("downloading_from_hf"));
                    
                    // Сначала пробуем API с revision=main
                    List<HuggingFaceFile> files = null;
                    string[] apiUrls = new[]
                    {
                        $"https://huggingface.co/api/models/{modelId}/tree/main",
                        $"https://huggingface.co/api/models/{modelId}/tree",
                        $"https://huggingface.co/api/repos/{modelId}/contents"
                    };
                    
                    foreach (var apiUrl in apiUrls)
                    {
                        try
                        {
                            Console.WriteLine($"Trying API URL: {apiUrl}");
                            var response = await client.GetStringAsync(apiUrl);
                            files = JsonSerializer.Deserialize<List<HuggingFaceFile>>(response);
                            if (files != null && files.Count > 0)
                                break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed API URL {apiUrl}: {ex.Message}");
                            continue;
                        }
                    }
                    
                    if (files == null || files.Count == 0)
                    {
                        // Если API не работает, попробуем напрямую найти популярные файлы
                        files = new List<HuggingFaceFile>();
                        string[] commonGgufNames = new[]
                        {
                            "model.gguf",
                            "ggml-model-q4_0.gguf",
                            "ggml-model-q5_0.gguf",
                            "ggml-model-q8_0.gguf",
                            $"{modelId.Split('/').Last()}.gguf",
                            $"{modelId.Split('/').Last()}-q4_0.gguf",
                            $"{modelId.Split('/').Last()}-q5_0.gguf"
                        };
                        
                        foreach (var fileName in commonGgufNames)
                        {
                            files.Add(new HuggingFaceFile 
                            { 
                                Type = "file", 
                                Path = fileName, 
                                Size = 1000000 // Примерный размер
                            });
                        }
                    }
                    
                    // Находим GGUF файлы
                    var ggufFiles = files.Where(f => f.Type == "file" && f.Path.EndsWith(".gguf")).ToList();
                    
                    if (ggufFiles.Count > 0)
                    {
                        // Берем первый GGUF файл (или самый маленький, если их несколько)
                        var fileToDownload = ggufFiles.OrderBy(f => f.Size).First();
                        
                        // Загружаем файл
                        var fileName = Path.GetFileName(fileToDownload.Path);
                        var filePath = Path.Combine(tempDownloadPath, fileName);
                        
                                                    // Пробуем разные варианты URL для скачивания файла
                            string[] urlTemplates = new[]
                            {
                                $"https://huggingface.co/{modelId}/resolve/main/{fileToDownload.Path}",
                                $"http://huggingface.co/{modelId}/resolve/main/{fileToDownload.Path}", // HTTP fallback
                                $"https://huggingface.co/{modelId}/resolve/master/{fileToDownload.Path}",
                                $"http://huggingface.co/{modelId}/resolve/master/{fileToDownload.Path}", // HTTP fallback
                                $"https://huggingface.co/{modelId}/blob/main/{fileToDownload.Path}?download=true",
                                $"https://huggingface.co/{modelId}/blob/master/{fileToDownload.Path}?download=true",
                                $"https://cdn-lfs.huggingface.co/repos/{modelId.Replace("/", "--")}/blobs/{fileToDownload.Path}",
                                $"https://huggingface.co/{modelId}/raw/main/{fileToDownload.Path}",
                                $"https://huggingface.co/{modelId}/raw/master/{fileToDownload.Path}"
                            };
                        
                        bool downloadSuccessful = false;
                        Exception lastException = null;
                        
                                                    foreach (var downloadUrl in urlTemplates)
                            {
                                try
                                {
                                    Console.WriteLine($"Trying download URL: {downloadUrl}");
                                    

                                    
                                    // Создаем новый HttpClient для каждой попытки
                                    using (var downloadHandler = new HttpClientHandler() 
                                    { 
                                        MaxConnectionsPerServer = 10,
                                        UseCookies = false,
                                        UseProxy = false,
                                        CheckCertificateRevocationList = false,
                                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true // Игнорируем ошибки SSL для HuggingFace
                                    })
                                    using (var downloadClient = new HttpClient(downloadHandler))
                                {
                                    downloadClient.DefaultRequestHeaders.Add("User-Agent", "Onllama.Tiny/1.0");
                                    downloadClient.DefaultRequestHeaders.Add("Accept", "*/*");
                                    downloadClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                                    downloadClient.Timeout = TimeSpan.FromMinutes(30); // Увеличиваем таймаут
                                    
                                    using (var response2 = await downloadClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                                    {
                                        if (!response2.IsSuccessStatusCode)
                                        {
                                            Console.WriteLine($"Failed URL {downloadUrl}: {response2.StatusCode}");
                                            continue;
                                        }
                                        
                                        var totalBytes = response2.Content.Headers.ContentLength ?? 0;
                                        Console.WriteLine($"Starting download from {downloadUrl}, size: {totalBytes} bytes");
                                        
                                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 65536))
                                        using (var contentStream = await response2.Content.ReadAsStreamAsync())
                                        {
                                            var buffer = new byte[65536]; // Увеличиваем размер буфера
                                            var bytesRead = 0;
                                            var totalBytesRead = 0L;
                                            var lastProgressUpdate = DateTime.Now;
                                            var downloadStartTime = DateTime.Now;
                                            var lastMeasurementTime = DateTime.Now;
                                            var lastMeasurementBytes = 0L;
                                            
                                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                            {
                                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                                totalBytesRead += bytesRead;
                                                
                                                // Обновляем прогресс не чаще чем раз в секунду
                                                var now = DateTime.Now;
                                                if (totalBytes > 0 && (now - lastProgressUpdate).TotalSeconds >= 1)
                                                {
                                                    var progress = (double)totalBytesRead / totalBytes;
                                                    progressBarHf.Value = (float)progress;
                                                    
                                                    // Вычисляем скорость загрузки
                                                    var elapsedSeconds = (now - lastMeasurementTime).TotalSeconds;
                                                    if (elapsedSeconds > 0)
                                                    {
                                                        var bytesDownloaded = totalBytesRead - lastMeasurementBytes;
                                                        var speedBytesPerSecond = bytesDownloaded / elapsedSeconds;
                                                        var speedMBps = speedBytesPerSecond / (1024 * 1024);
                                                        
                                                        // Рассчитываем оставшееся время
                                                        var remainingBytes = totalBytes - totalBytesRead;
                                                        var remainingSeconds = speedBytesPerSecond > 0 ? remainingBytes / speedBytesPerSecond : 0;
                                                        var remainingTime = TimeSpan.FromSeconds(remainingSeconds);
                                                        
                                                        string timeStr = remainingSeconds > 3600 
                                                            ? $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}"
                                                            : $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                                                        
                                                        Text = $"{LocalizationManager.GetTranslation("downloading_from_hf")} {(progress * 100):N1}% - " +
                                                               $"{LocalizationManager.GetTranslation("download_speed")}: {speedMBps:N1} MB/s - " +
                                                               $"{LocalizationManager.GetTranslation("remaining_time")}: {timeStr}";
                                                        
                                                        lastMeasurementTime = now;
                                                        lastMeasurementBytes = totalBytesRead;
                                                    }
                                                    
                                                    lastProgressUpdate = now;
                                                }
                                            }
                                            
                                            await fileStream.FlushAsync();
                                        }
                                        
                                                                Console.WriteLine($"Download completed successfully from {downloadUrl}");
                        
                        // Проверяем, что загруженный файл действительно в формате GGUF
                        if (!IsValidGGUFFile(filePath))
                        {
                            Console.WriteLine($"Downloaded file is not a valid GGUF file: {filePath}");
                            File.Delete(filePath);
                            continue;
                        }
                        
                        downloadSuccessful = true;
                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error downloading from {downloadUrl}: {ex.Message}");
                                
                                // Специальная обработка SSL ошибок
                                if (ex.Message.Contains("SSL") || ex.Message.Contains("certificate") || 
                                    ex.Message.Contains("SecurityException") || ex.Message.Contains("AuthenticationException"))
                                {
                                    Console.WriteLine("SSL certificate error detected, trying next URL...");
                                }
                                
                                lastException = ex;
                                
                                // Удаляем частично загруженный файл
                                try
                                {
                                    if (File.Exists(filePath))
                                        File.Delete(filePath);
                                }
                                catch { }
                                
                                continue;
                            }
                        }
                        

                        
                        // Если HttpClient не сработал, попробуем альтернативный метод через WebRequest
                        if (!downloadSuccessful)
                        {
                            foreach (var downloadUrl in urlTemplates.Take(3)) // Попробуем первые 3 URL
                            {
                                try
                                {
                                    Console.WriteLine($"Trying WebRequest download from {downloadUrl}");
                                    
                                    var request = (HttpWebRequest)WebRequest.Create(downloadUrl);
                                    request.Method = "GET";
                                    request.UserAgent = "Onllama.Tiny/1.0";
                                    request.Timeout = 1800000; // 30 минут
                                    request.ReadWriteTimeout = 1800000;
                                    request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                                    
                                    using (var response = (HttpWebResponse)request.GetResponse())
                                    using (var responseStream = response.GetResponseStream())
                                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                    {
                                        responseStream.CopyTo(fileStream);
                                    }
                                    
                                    // Проверяем, что загруженный файл действительно в формате GGUF
                                    if (IsValidGGUFFile(filePath))
                                    {
                                        Console.WriteLine($"Download completed successfully with WebRequest from {downloadUrl}");
                                        downloadSuccessful = true;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Downloaded file is not a valid GGUF file: {filePath}");
                                        File.Delete(filePath);
                                    }
                                }
                                catch (Exception webRequestEx)
                                {
                                    Console.WriteLine($"WebRequest download failed: {webRequestEx.Message}");
                                    lastException = webRequestEx;
                                }
                            }
                        }
                        
                        if (!downloadSuccessful)
                        {
                            string errorMsg = lastException?.Message ?? LocalizationManager.GetTranslation("download_failed");
                            
                            // Проверяем, является ли это SSL ошибкой
                            if (lastException != null && (lastException.Message.Contains("SSL") || 
                                lastException.Message.Contains("certificate") || 
                                lastException.Message.Contains("AuthenticationException")))
                            {
                                errorMsg = LocalizationManager.GetTranslation("ssl_error_with_recommendation");
                            }
                            
                            MessageBox.Show($"{LocalizationManager.GetTranslation("download_failed")}\n\n{errorMsg}", 
                                LocalizationManager.GetTranslation("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        var successMessage = $"{LocalizationManager.GetTranslation("download_complete")}\n{LocalizationManager.GetTranslation("file_saved_to")}: {filePath}";
                        Message.success(this, successMessage);
                        
                        // Установка пути загруженного файла в UI
                        input1.Text = filePath;
                        tabControl1.SelectedIndex = 0; // Переключение на вкладку импорта файла
                        
                        // Устанавливаем имя модели
                        inputName.Text = modelId.Replace("/", "_");
                    }
                    else
                    {
                        MessageBox.Show(LocalizationManager.GetTranslation("no_gguf_files"), 
                            LocalizationManager.GetTranslation("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationManager.GetTranslation("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarHf.Hide();
                Text = LocalizationManager.GetTranslation("import_model");
            }
        }
    }

    // Классы для работы с HuggingFace API
    public class HuggingFaceModel
    {
        [JsonPropertyName("modelId")]
        public string ModelId { get; set; }
        
        [JsonPropertyName("downloads")]
        public long Downloads { get; set; }
        
        [JsonPropertyName("lastModified")]
        public string LastModified { get; set; }
    }
    
    public class HuggingFaceFile
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("path")]
        public string Path { get; set; }
        
        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
}
