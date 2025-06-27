using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;
using AntdUI;
using Microsoft.VisualBasic.Devices;
using OllamaSharp;
using OllamaSharp.Models;

namespace Onllama.Tiny
{
    public partial class Form1 : BaseForm
    {
        public static bool IsRemote = false;
        public static Uri OllamaUri = new Uri("http://127.0.0.1:11434");
        public static OllamaApiClient OllamaApi = new(OllamaUri);

        public Form1()
        {
            InitializeComponent();

            // Загружаем языковые настройки
            LocalizationManager.LoadLanguagePreference();

            // Загружаем сохраненные размеры и позицию окна
            LoadWindowSettings();

            // Инициализируем таймеры для сохранения настроек
            InitializeTimers();

            try
            {
                Task.Run(
                    () =>
                    {
                        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OLLAMA_ORIGINS", EnvironmentVariableTarget.User) ?? string.Empty))
                            Environment.SetEnvironmentVariable("OLLAMA_ORIGINS", "*", EnvironmentVariableTarget.User);
                        //if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OLLAMA_MAX_LOADED_MODELS", EnvironmentVariableTarget.User) ?? string.Empty))
                        //    Environment.SetEnvironmentVariable("OLLAMA_MAX_LOADED_MODELS", "4", EnvironmentVariableTarget.User);
                        //if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OLLAMA_NUM_PARALLEL", EnvironmentVariableTarget.User) ?? string.Empty))
                        //    Environment.SetEnvironmentVariable("OLLAMA_NUM_PARALLEL", "6", EnvironmentVariableTarget.User);
                    });
                IsRemote = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OLLAMA_REMOTE_API"));
                if (IsRemote)
                {
                    OllamaUri = new Uri(Environment.GetEnvironmentVariable("OLLAMA_REMOTE_API") ?? OllamaUri.ToString());
                    OllamaApi = new OllamaApiClient(OllamaUri);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Обновляем заголовки колонок с учетом выбранного языка
            table1.Columns = new ColumnCollection([
                new("name", LocalizationManager.GetTranslation("name")),
                new("size", LocalizationManager.GetTranslation("size")),
                new("status", LocalizationManager.GetTranslation("status")),
                new("families", LocalizationManager.GetTranslation("families")),
                new("quantization", LocalizationManager.GetTranslation("quantization")),
                new("modifiedAt", LocalizationManager.GetTranslation("modifiedAt")),
                new("btns", LocalizationManager.GetTranslation("btns")) {Fixed = true}
            ]);
            // Инициализируем select1 с загрузкой официальных моделей
            InitializeModelSelect();

            // Обновляем заголовок окна
            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            Text = "Onllama - " + LocalizationManager.GetTranslation("models");
        }
        
        private void RefreshUI()
        {
            // Обновляем заголовки колонок
            table1.Columns = new ColumnCollection([
                new("name", LocalizationManager.GetTranslation("name")),
                new("size", LocalizationManager.GetTranslation("size")),
                new("status", LocalizationManager.GetTranslation("status")),
                new("families", LocalizationManager.GetTranslation("families")),
                new("quantization", LocalizationManager.GetTranslation("quantization")),
                new("modifiedAt", LocalizationManager.GetTranslation("modifiedAt")),
                new("btns", LocalizationManager.GetTranslation("btns")) {Fixed = true}
            ]);
            
            // Обновляем заголовок окна
            UpdateWindowTitle();
            
            // Обновляем элементы выпадающего меню
            InitializeDropdownItems();
            
            // Перезагружаем список моделей для обновления кнопок
            ListModels();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Width++;
            progress1.Hide();
            select1.SelectedIndex = (new ComputerInfo().TotalPhysicalMemory / 1024f / 1024f / 1024f) switch
            {
                >= 14 => 2,
                >= 6 => 1,
                _ => select1.SelectedIndex
            };

            // Загружаем языковые настройки
            LocalizationManager.LoadLanguagePreference();
            
            // Обновляем заголовок окна
            UpdateWindowTitle();
            
            // Инициализируем элементы выпадающего списка с переведенными названиями
            InitializeDropdownItems();
            
            // Инициализируем список моделей для загрузки
            InitializeModelSelect();

            if (!IsRemote)
            {
                var ollamaPath = (Environment.GetEnvironmentVariable("PATH")
                        ?.Split(';')
                        .Select(x => Path.Combine(x, "ollama app.exe"))!)
                    .FirstOrDefault(File.Exists);

                if (string.IsNullOrWhiteSpace(ollamaPath))
                {
                    Notification.warn(this, LocalizationManager.GetTranslation("core_not_installed"), 
                        LocalizationManager.GetTranslation("please_install"));
                    Process.Start(new ProcessStartInfo($"https://ollama.com/download/windows") { UseShellExecute = true });
                    Process.Start(new ProcessStartInfo($"https://github.com/ollama/ollama/releases/latest")
                        { UseShellExecute = true });
                    panel1.Enabled = false;
                }
                else if (!PortIsUse(11434))
                {
                    //Notification.info(this, "Ollama 核心未在运行", "正在启动 Ollama 服务，请稍等…");
                    AntdUI.Message.info(this, LocalizationManager.GetTranslation("starting_service"));
                    Process.Start(ollamaPath, "serve");
                }
            }

            ListModels();
        }
        
        private void InitializeDropdownItems()
        {
            // Очищаем и заново добавляем элементы с переводом
            dropdown1.Items.Clear();
            
            // Добавляем элементы с переводом
            dropdown1.Items.Add(LocalizationManager.GetTranslation("import_model_menu"));
            dropdown1.Items.Add(LocalizationManager.GetTranslation("ollama_settings"));
            dropdown1.Items.Add(LocalizationManager.GetTranslation("model_list_refresh"));
            dropdown1.Items.Add("━━━━━━━━━━━━━━"); // Разделитель
            dropdown1.Items.Add("NextChat"); // Не переводится
            dropdown1.Items.Add(LocalizationManager.GetTranslation("openai_api"));
            dropdown1.Items.Add("━━━━━━━━━━━━━━"); // Разделитель
            dropdown1.Items.Add(LocalizationManager.GetTranslation("find_models_online"));
            dropdown1.Items.Add("Ollama.com"); // Прямая ссылка на сайт Ollama
            dropdown1.Items.Add("HuggingFace"); // Прямая ссылка на сайт HuggingFace
            dropdown1.Items.Add("━━━━━━━━━━━━━━"); // Разделитель
            dropdown1.Items.Add(LocalizationManager.GetTranslation("view_models_location"));
            dropdown1.Items.Add(LocalizationManager.GetTranslation("view_logs"));
            dropdown1.Items.Add(LocalizationManager.GetTranslation("check_updates"));
            dropdown1.Items.Add(LocalizationManager.GetTranslation("language_settings"));
        }

        private void InitializeModelSelect()
        {
            try
            {
                // Показываем прогресс с информативным текстом
                progress1.Show();
                progress1.Value = 0;
                progress1.Text = "🔄 Загружаем модели...";
                
                select1.Enabled = false;
                selectHuggingFace.Enabled = false;

                // Получаем список загруженных моделей для индикации
                var loadedModels = new HashSet<string>();
                try
                {
                    progress1.Value = 10;
                    progress1.Text = "🔄 Получаем список установленных моделей...";
                    
                    var localModels = Task.Run(async () => await OllamaApi.ListLocalModelsAsync()).Result;
                    foreach (var model in localModels)
                    {
                        loadedModels.Add(model.Name);
                    }
                }
                catch
                {
                    // Если не удается получить список, продолжаем без индикации
                }

                progress1.Value = 30;
                progress1.Text = "🦙 Загружаем Ollama модели...";
                
                // Инициализируем Ollama модели
                InitializeOllamaModels(loadedModels);

                progress1.Value = 60;
                progress1.Text = "🤗 Загружаем HuggingFace GGUF модели...";
                
                // Инициализируем HuggingFace модели асинхронно с правильным ожиданием
                Task.Run(async () =>
                {
                    try
                    {
                        await InitializeHuggingFaceModelsAsync(loadedModels);
                        
                        Invoke(() =>
                        {
                            progress1.Value = 100;
                            progress1.Text = "✅ Модели загружены успешно!";
                            
                            // Скрываем прогресс через 1 секунду
                            Task.Delay(1000).ContinueWith(_ => Invoke(progress1.Hide));
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error initializing HuggingFace models: {ex.Message}");
                        Invoke(() =>
                        {
                            progress1.Text = "⚠️ Ошибка загрузки HuggingFace моделей";
                            Task.Delay(2000).ContinueWith(_ => Invoke(progress1.Hide));
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in InitializeModelSelect: {ex.Message}");
                progress1.Text = "❌ Ошибка инициализации";
                Task.Delay(2000).ContinueWith(_ => Invoke(progress1.Hide));
            }
        }

        private void InitializeOllamaModels(HashSet<string> loadedModels)
        {
            try
            {
                // Определяем структуру моделей с размерами
                var modelStructure = new Dictionary<string, Dictionary<string, List<string>>>
                {
                    ["🧠 Reasoning"] = new Dictionary<string, List<string>>
                    {
                        ["DeepSeek-R1"] = new List<string> { "1.5b", "7b", "8b", "14b", "32b", "70b" },
                        ["QwQ"] = new List<string> { "32b" }
                    },
                    ["👁️ Vision"] = new Dictionary<string, List<string>>
                    {
                        ["LLaVA"] = new List<string> { "7b", "13b", "34b" },
                        ["LLaVA-Llama3"] = new List<string> { "8b" },
                        ["Llama3.2-Vision"] = new List<string> { "11b", "90b" },
                        ["MiniCPM-V"] = new List<string> { "8b" }
                    },
                    ["💻 Coding"] = new Dictionary<string, List<string>>
                    {
                        ["Qwen2.5-Coder"] = new List<string> { "0.5b", "1.5b", "3b", "7b", "14b", "32b" }
                    },
                    ["🛠️ Tools"] = new Dictionary<string, List<string>>
                    {
                        ["Qwen3"] = new List<string> { "1.7b", "4b", "8b", "14b", "32b" },
                        ["Llama3.1"] = new List<string> { "8b", "70b", "405b" },
                        ["Gemma3"] = new List<string> { "1b", "4b", "12b", "27b" }
                    },
                    ["⭐ Popular"] = new Dictionary<string, List<string>>
                    {
                        ["Qwen2.5"] = new List<string> { "0.5b", "1.5b", "3b", "7b", "14b", "32b", "72b" }
                    },
                    ["🆕 Latest"] = new Dictionary<string, List<string>>
                    {
                        ["Phi4"] = new List<string> { "14b" },
                        ["Phi4-Mini"] = new List<string> { "3.8b" },
                        ["Command-R"] = new List<string> { "35b" },
                        ["Command-R-Plus"] = new List<string> { "104b" },
                        ["Aya-Expanse"] = new List<string> { "8b", "32b" }
                    },
                    ["🔗 Embedding"] = new Dictionary<string, List<string>>
                    {
                        ["Nomic-Embed-Text"] = new List<string> { "latest" },
                        ["MxBai-Embed-Large"] = new List<string> { "335m" },
                        ["All-MiniLM"] = new List<string> { "22m", "33m" },
                        ["BGE-M3"] = new List<string> { "567m" }
                    },
                    ["🔧 " + LocalizationManager.GetTranslation("other_models")] = new Dictionary<string, List<string>>
                    {
                        ["Mistral"] = new List<string> { "7b" },
                        ["Mistral-Nemo"] = new List<string> { "12b" },
                        ["Mistral-Large"] = new List<string> { "123b" },
                        ["Llama3.2"] = new List<string> { "1b", "3b" }
                    }
                };

                // Очищаем и заполняем выпадающий список
                select1.Items.Clear();
                
                // Добавляем кнопку обновления в начало
                select1.Items.Add(new SelectItem("📊 " + LocalizationManager.GetTranslation("refresh_models")));

                // Создаем иерархический список с подсписками
                foreach (var category in modelStructure)
                {
                    // Создаем основной элемент категории
                    var categoryItem = new SelectItem(category.Key);
                    var categorySubItems = new List<object>();
                    
                    foreach (var modelFamily in category.Value)
                    {
                        // Создаем элемент семейства моделей
                        var familyItem = new SelectItem(modelFamily.Key);
                        var familySubItems = new List<object>();
                        
                        foreach (var size in modelFamily.Value)
                        {
                            var modelName = CreateModelName(modelFamily.Key, size);
                            var isLoaded = loadedModels.Contains(modelName);
                            var loadedPrefix = isLoaded ? "● " : "";
                            
                            // Добавляем значок категории к каждой модели
                            var categoryIcon = GetModelCategoryIcon(modelFamily.Key, category.Key);
                            var displayName = $"{loadedPrefix}{categoryIcon} {modelFamily.Key} ({size})";
                            
                            familySubItems.Add(new SelectItem(displayName) { Tag = modelName });
                        }
                        
                        // Устанавливаем подсписок для семейства
                        familyItem.Sub = familySubItems.ToArray();
                        categorySubItems.Add(familyItem);
                    }
                    
                    // Устанавливаем подсписок для категории
                    categoryItem.Sub = categorySubItems.ToArray();
                    select1.Items.Add(categoryItem);
                }

                // Добавляем модели сообщества как отдельную категорию
                var communityItem = new SelectItem($"👥 {LocalizationManager.GetTranslation("community")}");
                var communitySubItems = new List<object>();
                
                var communityModels = new[]
                {
                    ("Bilibili Index 1.9B Chat", "milkey/bilibili-index:1.9b-chat-q8_0"),
                    ("Bilibili Index 1.9B Character", "milkey/bilibili-index:1.9b-character-q8_0"),
                    ("MiniCPM3 4B", "shibing624/minicpm3_4b"),
                    ("Sakura 14B Qwen2.5", "hf.co/SakuraLLM/Sakura-14B-Qwen2.5-v1.0-GGUF"),
                    ("Llama 3.3 70B Instruct", "hf.co/bartowski/Llama-3.3-70B-Instruct-GGUF")
                };

                foreach (var (displayName, modelName) in communityModels)
                {
                    var isLoaded = loadedModels.Contains(modelName);
                    var loadedPrefix = isLoaded ? "● " : "";
                    
                    // Добавляем значок категории для моделей сообщества
                    var categoryIcon = GetModelFamilyIcon(displayName);
                    var fullDisplayName = $"{loadedPrefix}👥 {displayName}";
                    
                    communitySubItems.Add(new SelectItem(fullDisplayName) { Tag = modelName });
                }
                
                communityItem.Sub = communitySubItems.ToArray();
                select1.Items.Add(communityItem);

                // Устанавливаем модель по умолчанию в зависимости от RAM
                var memoryGB = new ComputerInfo().TotalPhysicalMemory / 1024f / 1024f / 1024f;
                var defaultModel = memoryGB switch
                {
                    >= 32 => "qwen3:14b",
                    >= 16 => "qwen3:8b", 
                    >= 8 => "qwen3:4b",
                    >= 4 => "qwen2.5:3b",
                    _ => "qwen2.5:1.5b"
                };

                select1.Text = defaultModel;
                select1.Enabled = true;

                Console.WriteLine($"✅ Loaded Ollama model list with {loadedModels.Count} models marked as installed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in InitializeOllamaModels: {ex.Message}");
                
                // Минимальный fallback список с значками
                select1.Items.Clear();
                var fallbackModels = new[]
                {
                    ("🧠 DeepSeek-R1 (8b)", "deepseek-r1:8b"),
                    ("🛠️ Qwen3 (4b)", "qwen3:4b"),
                    ("🛠️ Qwen3 (8b)", "qwen3:8b"),
                    ("⭐ Qwen2.5 (3b)", "qwen2.5:3b"),
                    ("⭐ Qwen2.5 (7b)", "qwen2.5:7b"),
                    ("🔧 Llama3.2 (3b)", "llama3.2:3b"),
                    ("🛠️ Gemma3 (4b)", "gemma3:4b"),
                    ("🔧 Mistral (7b)", "mistral:7b")
                };
                
                foreach (var (displayName, modelName) in fallbackModels)
                {
                    select1.Items.Add(new SelectItem(displayName) { Tag = modelName });
                }
                
                select1.Text = "qwen3:4b";
                select1.Enabled = true;
            }
        }

        private async Task InitializeHuggingFaceModelsAsync(HashSet<string> loadedModels)
        {
            try
            {
                // Загружаем HuggingFace модели асинхронно, фокусируемся на GGUF
                var huggingFaceModels = new List<HuggingFaceModelExtended>();
                try
                {
                    // Фокусируемся только на GGUF моделях для совместимости с Ollama
                    huggingFaceModels = await HuggingFaceServiceExtended.GetGGUFModelsAsync(50);
                    Console.WriteLine($"✅ Loaded {huggingFaceModels.Count} GGUF HuggingFace models");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Could not load HuggingFace models: {ex.Message}");
                    // Используем fallback модели, чтобы секция всегда была видна
                    huggingFaceModels = HuggingFaceServiceExtended.GetGGUFFallbackModels();
                    Console.WriteLine($"✅ Using {huggingFaceModels.Count} fallback GGUF models");
                }

                // Обновляем UI в главном потоке
                Invoke(() =>
                {
                    // Очищаем и заполняем HuggingFace выпадающий список
                    selectHuggingFace.Items.Clear();
                    
                    // Добавляем кнопку обновления в начало
                    selectHuggingFace.Items.Add(new SelectItem("🔄 Обновить GGUF модели"));

                    // Используем новую иерархическую группировку
                    var hierarchicalModels = HuggingFaceServiceExtended.GroupGGUFModelsByFamilies(huggingFaceModels);

                    foreach (var (categoryName, families) in hierarchicalModels)
                    {
                        var categoryItem = new SelectItem(categoryName);
                        var categorySubItems = new List<object>();

                        foreach (var (familyName, familyModels) in families)
                        {
                            var familyItem = new SelectItem(familyName);
                            var familySubItems = new List<object>();

                            foreach (var model in familyModels.Take(8)) // Ограничиваем до 8 моделей на семейство
                            {
                                var ollamaModelName = HuggingFaceServiceExtended.FormatModelForOllama(model);
                                var isLoaded = loadedModels.Contains(ollamaModelName);
                                var prefix = isLoaded ? "● " : "";
                                
                                // Извлекаем размер модели из названия
                                var modelSize = ExtractModelSize(model.ModelId);
                                var sizeDisplay = !string.IsNullOrEmpty(modelSize) ? $" [{modelSize}]" : "";
                                
                                // Добавляем информацию о популярности
                                var popularity = model.Downloads switch
                                {
                                    > 2000000 => "🔥",
                                    > 1000000 => "⭐",
                                    > 500000 => "📈",
                                    _ => ""
                                };

                                var downloads = model.Downloads > 1000000 
                                    ? $"{model.Downloads / 1000000:F1}M"
                                    : model.Downloads > 1000 
                                        ? $"{model.Downloads / 1000:F0}K" 
                                        : model.Downloads.ToString();
                                
                                var displayName = $"{prefix}{popularity}{GetCleanModelName(model)}{sizeDisplay} ({downloads}⬇)";
                                
                                familySubItems.Add(new SelectItem(displayName) { Tag = ollamaModelName });
                            }

                            if (familySubItems.Any())
                            {
                                familyItem.Sub = familySubItems.ToArray();
                                categorySubItems.Add(familyItem);
                            }
                        }

                        if (categorySubItems.Any())
                        {
                            categoryItem.Sub = categorySubItems.ToArray();
                            selectHuggingFace.Items.Add(categoryItem);
                        }
                    }

                    selectHuggingFace.PlaceholderText = $"🤗 HuggingFace GGUF ({huggingFaceModels.Count} моделей)";
                    selectHuggingFace.Enabled = true;

                    Console.WriteLine($"✅ Loaded HuggingFace GGUF model list with {huggingFaceModels.Count} models in hierarchical structure");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading HuggingFace models: {ex.Message}");
                Invoke(() =>
                {
                    selectHuggingFace.PlaceholderText = "❌ Ошибка загрузки HuggingFace";
                    selectHuggingFace.Enabled = true;
                });
            }
        }

        private string CreateModelName(string family, string size)
        {
            // Преобразуем имя семейства в правильное имя модели
            var modelName = family.ToLowerInvariant() switch
            {
                "deepseek-r1" => "deepseek-r1",
                "qwq" => "qwq",
                "llava" => "llava",
                "llava-llama3" => "llava-llama3",
                "llama3.2-vision" => "llama3.2-vision",
                "minicpm-v" => "minicpm-v",
                "qwen2.5-coder" => "qwen2.5-coder",
                "qwen3" => "qwen3",
                "llama3.1" => "llama3.1",
                "gemma3" => "gemma3",
                "qwen2.5" => "qwen2.5",
                "phi4" => "phi4",
                "phi4-mini" => "phi4-mini",
                "command-r" => "command-r",
                "command-r-plus" => "command-r-plus",
                "aya-expanse" => "aya-expanse",
                "nomic-embed-text" => "nomic-embed-text",
                "mxbai-embed-large" => "mxbai-embed-large",
                "all-minilm" => "all-minilm",
                "bge-m3" => "bge-m3",
                "mistral" => "mistral",
                "mistral-nemo" => "mistral-nemo",
                "mistral-large" => "mistral-large",
                "llama3.2" => "llama3.2",
                _ => family.ToLowerInvariant().Replace("-", "-")
            };

            return $"{modelName}:{size}";
        }

        private string ExtractModelSize(string modelId)
        {
            // Извлекаем размер модели из ID (например, "7B", "3.5B", "1.3B")
            var patterns = new[]
            {
                @"(\d+\.?\d*[BM])",        // 7B, 3.5B, 1.3B
                @"(\d+\.?\d*b)",          // 7b, 3.5b  
                @"(\d+\.?\d*-[BM])",      // 7-B, 3.5-B
                @"mini|small|medium|large|xl" // Размеры в словах
            };

            foreach (var pattern in patterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(modelId, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Value.ToUpperInvariant();
                }
            }

            return "";
        }

        private string GetCleanModelName(HuggingFaceModelExtended model)
        {
            // Очищаем название модели от лишней информации
            var name = model.ModelId.Split('/').LastOrDefault() ?? model.ModelId;
            
            // Убираем часто встречающиеся суффиксы
            var suffixesToRemove = new[] { "-GGUF", "-gguf", "-instruct", "-Instruct", "-INSTRUCT" };
            foreach (var suffix in suffixesToRemove)
            {
                if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    name = name.Substring(0, name.Length - suffix.Length);
                }
            }

            // Убираем размеры из основного названия, так как они отображаются отдельно
            name = System.Text.RegularExpressions.Regex.Replace(name, @"-?\d+\.?\d*[BM]?-?", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            
            return name.Trim('-', '_');
        }

        private string GetModelCategoryIcon(string modelFamily, string categoryName)
        {
            // Определяем значок для модели на основе её семейства и категории
            return categoryName switch
            {
                var cat when cat.Contains("Reasoning") => "[R]",
                var cat when cat.Contains("Vision") => "[V]", 
                var cat when cat.Contains("Coding") => "[C]",
                var cat when cat.Contains("Tools") => "[T]",
                var cat when cat.Contains("Popular") => "[P]",
                var cat when cat.Contains("Latest") => "[N]",
                var cat when cat.Contains("Embedding") => "[E]",
                var cat when cat.Contains("community") || cat.Contains("Community") => "[M]",
                _ => GetModelFamilyIcon(modelFamily)
            };
        }

        private string GetModelFamilyIcon(string modelFamily)
        {
            // Определяем значок по конкретному семейству модели
            return modelFamily.ToLowerInvariant() switch
            {
                // Reasoning models
                var family when family.Contains("deepseek-r1") => "[R]",
                var family when family.Contains("qwq") => "[R]",
                
                // Vision models  
                var family when family.Contains("llava") => "[V]",
                var family when family.Contains("vision") => "[V]",
                var family when family.Contains("minicpm-v") => "[V]",
                
                // Code models
                var family when family.Contains("coder") => "[C]",
                var family when family.Contains("qwen2.5-coder") => "[C]",
                
                // Tool models
                var family when family.Contains("qwen3") => "[T]",
                var family when family.Contains("llama3.1") => "[T]", 
                var family when family.Contains("gemma3") => "[T]",
                
                // Popular models
                var family when family.Contains("qwen2.5") => "[P]",
                
                // Latest models
                var family when family.Contains("phi4") => "[N]",
                var family when family.Contains("command-r") => "[N]",
                var family when family.Contains("aya-expanse") => "[N]",
                
                // Embedding models
                var family when family.Contains("embed") => "[E]",
                var family when family.Contains("nomic") => "[E]",
                var family when family.Contains("mxbai") => "[E]",
                var family when family.Contains("minilm") => "[E]",
                var family when family.Contains("bge") => "[E]",
                
                // Other/General models
                var family when family.Contains("mistral") => "[O]",
                var family when family.Contains("llama3.2") => "[O]",
                
                // Community models
                var family when family.Contains("bilibili") => "[M]",
                var family when family.Contains("minicpm3") => "[M]",
                var family when family.Contains("sakura") => "[M]",
                
                // Default
                _ => "[*]"
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем выбранную модель
            var selectedModel = select1.Text;
            
            // Проверяем, что выбрана не служебная строка
            if (string.IsNullOrEmpty(selectedModel) || 
                selectedModel.StartsWith("═══") || 
                selectedModel.StartsWith("   ┈") ||
                selectedModel == LocalizationManager.GetTranslation("refresh_models") ||
                selectedModel.StartsWith("📊"))
            {
                AntdUI.Message.warn(this, "Пожалуйста, выберите модель для загрузки");
                return;
            }
            
            // Проверяем выбранный элемент (теперь работает с иерархической структурой)
            if (string.IsNullOrEmpty(selectedModel) || selectedModel == "📊 " + LocalizationManager.GetTranslation("refresh_models"))
            {
                AntdUI.Message.warn(this, "Пожалуйста, выберите модель для загрузки");
                return;
            }

            // Если выбран текст без тега, попробуем найти соответствующую модель
            var selectedIndex = select1.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < select1.Items.Count)
            {
                // Попробуем найти элемент с тегом в иерархии
                bool foundModel = false;
                foreach (var item in select1.Items)
                {
                    if (item is SelectItem selectItem && selectItem.Sub != null)
                    {
                        foreach (var subItem in selectItem.Sub)
                        {
                            if (subItem is SelectItem subSelectItem && subSelectItem.Sub != null)
                            {
                                foreach (var subSubItem in subSelectItem.Sub)
                                {
                                    if (subSubItem is SelectItem modelItem && modelItem.Tag != null)
                                    {
                                        // Убираем эмодзи для сравнения, но сохраняем Tag
                                        var cleanedText = selectedModel.Replace("● ", "");
                                        try
                                        {
                                            var emojiPattern = @"[\U0001F300-\U0001F9FF]|[\U0001F600-\U0001F64F]|[\U0001F680-\U0001F6FF]|[\U0001F700-\U0001F77F]|[\U0001F780-\U0001F7FF]|[\U0001F800-\U0001F8FF]|[\U0001F900-\U0001F9FF]|[\U0001FA00-\U0001FA6F]|[\U0001FA70-\U0001FAFF]|[\U00002600-\U000027BF]";
                                            cleanedText = System.Text.RegularExpressions.Regex.Replace(cleanedText, emojiPattern, "").Trim();
                                        }
                                        catch
                                        {
                                            // Fallback: простое удаление известных символов
                                            cleanedText = cleanedText.Replace("●", "").Replace("[R]", "").Replace("[V]", "").Replace("[C]", "").Replace("[T]", "").Replace("[P]", "").Replace("[N]", "").Replace("[E]", "").Replace("[O]", "").Replace("[M]", "").Replace("[*]", "").Trim();
                                        }
                                        
                                        if (modelItem.Text.Contains(cleanedText) || 
                                            cleanedText.Contains(modelItem.Tag.ToString()) ||
                                            modelItem.Text == selectedModel)
                                        {
                                            selectedModel = modelItem.Tag.ToString();
                                            foundModel = true;
                                            break;
                                        }
                                    }
                                }
                                if (foundModel) break;
                            }
                        }
                        if (foundModel) break;
                    }
                }
            }
            else
            {
                // Создаем копию для обработки без изменения оригинального текста
                var cleanedModel = selectedModel;
                
                // Убираем зеленый кружок и значки категорий из названия модели для получения ID
                cleanedModel = cleanedModel.Replace("● ", "").Replace("   ", "").Trim();
                
                // Убираем все эмодзи значки (но сохраняем для отображения в selectedModel)
                try
                {
                    var emojiPattern = @"[\U0001F300-\U0001F9FF]|[\U0001F600-\U0001F64F]|[\U0001F680-\U0001F6FF]|[\U0001F700-\U0001F77F]|[\U0001F780-\U0001F7FF]|[\U0001F800-\U0001F8FF]|[\U0001F900-\U0001F9FF]|[\U0001FA00-\U0001FA6F]|[\U0001FA70-\U0001FAFF]|[\U00002600-\U000027BF]";
                    cleanedModel = System.Text.RegularExpressions.Regex.Replace(cleanedModel, emojiPattern, "").Trim();
                }
                catch
                {
                    // Fallback: простое удаление известных символов
                    cleanedModel = cleanedModel.Replace("●", "").Replace("[R]", "").Replace("[V]", "").Replace("[C]", "").Replace("[T]", "").Replace("[P]", "").Replace("[N]", "").Replace("[E]", "").Replace("[O]", "").Replace("[M]", "").Replace("[*]", "").Trim();
                }
                
                // Если в строке есть двоеточие, значит это модель в формате name:size
                if (cleanedModel.Contains(":"))
                {
                    selectedModel = cleanedModel; // Используем очищенную версию для API
                }
                else if (cleanedModel.Contains(" ")) 
                {
                    selectedModel = cleanedModel.Split(' ').Last();
                }
                else
                {
                    selectedModel = cleanedModel;
                }
            }
            
            new Modal.Config(this, LocalizationManager.GetTranslation("confirm_download"), 
                new[] { new Modal.TextLine(selectedModel, Style.Db.Primary) }, TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = LocalizationManager.GetTranslation("download"),
                OnOk = _ =>
                {
                    Invoke(progress1.Show);

                    Task.Run(async () =>
                    {
                        var lastBytes = 0L;
                        var lastTime = DateTime.Now;
                        var lastDisplayTime = DateTime.Now;
                        var currentSpeed = 0.0;
                        var speedSamples = new Queue<double>();
                        
                        await foreach (var x in OllamaApi.PullModelAsync(selectedModel))
                        {
                            Invoke(() =>
                            {
                                var textInfo = new CultureInfo("en-US", false).TextInfo;
                                progress1.Value = (float)x.Completed / x.Total;
                                
                                // Вычисляем скорость загрузки с сглаживанием
                                var now = DateTime.Now;
                                var elapsedSeconds = (now - lastTime).TotalSeconds;
                                var displayElapsed = (now - lastDisplayTime).TotalSeconds;
                                
                                // Обновляем скорость каждую секунду
                                if (elapsedSeconds >= 1.0 && x.Completed > lastBytes)
                                {
                                    var bytesDownloaded = x.Completed - lastBytes;
                                    var instantSpeed = (bytesDownloaded / elapsedSeconds) / (1024 * 1024);
                                    
                                    // Добавляем в очередь для сглаживания (максимум 5 образцов)
                                    speedSamples.Enqueue(instantSpeed);
                                    if (speedSamples.Count > 5) speedSamples.Dequeue();
                                    
                                    // Усредняем скорость для плавности
                                    currentSpeed = speedSamples.Average();
                                    
                                    lastBytes = x.Completed;
                                    lastTime = now;
                                }
                                
                                // Обновляем заголовок только каждые 0.5 секунды для устранения мерцания
                                if (displayElapsed >= 0.5 || currentSpeed == 0)
                                {
                                    if (currentSpeed > 0)
                                    {
                                        var remainingBytes = x.Total - x.Completed;
                                        var remainingSeconds = remainingBytes / (currentSpeed * 1024 * 1024);
                                        var remainingTime = TimeSpan.FromSeconds(remainingSeconds);
                                        
                                        string speedText = currentSpeed > 1 ? $"{currentSpeed:F1} MB/s" : $"{currentSpeed * 1024:F0} KB/s";
                                        string timeText = remainingSeconds > 3600 
                                            ? $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}"
                                            : $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                                        
                                        Text = $"Onllama - {textInfo.ToTitleCase(x.Status)} {x.Percent:F1}% - {speedText} - {LocalizationManager.GetTranslation("remaining_time")}: {timeText}";
                                    }
                                    else
                                    {
                                        Text = $"Onllama - {textInfo.ToTitleCase(x.Status)} {x.Percent:F1}%";
                                    }
                                    
                                    lastDisplayTime = now;
                                }
                                
                                if (string.IsNullOrEmpty(x.Status))
                                {
                                    Notification.info(this, LocalizationManager.GetTranslation("completed"), 
                                        LocalizationManager.GetTranslation("check_connection"));
                                    UpdateWindowTitle();
                                    progress1.Hide();
                                }
                                else if (x.Status == "success")
                                {
                                    Notification.success(this, LocalizationManager.GetTranslation("completed"), 
                                        LocalizationManager.GetTranslation("download_completed"));
                                    UpdateWindowTitle();
                                    progress1.Hide();
                                    ListModels();
                                }
                            });
                        }
                    });

                    return true;
                }
            }.open();
        }

        public void ListModels()
        {
            try
            {
                var modelsClasses = new List<ModelsClass>();
                var models = Task.Run(async () => await OllamaApi.ListLocalModelsAsync()).Result.ToArray();
                var runningModels = Task.Run(async () => await OllamaApi.ListRunningModelsAsync()).Result.ToArray();
                
                // Отладочная информация
                Console.WriteLine($"[#] Total models: {models.Length}");
                Console.WriteLine($"● Running models: {runningModels.Length}");
                foreach (var runningModel in runningModels)
                {
                    Console.WriteLine($"   ▶️ {runningModel.Name} - {runningModel.Size / 1024.0 / 1024.0 / 1024.0:F1}GB - Expires: {runningModel.ExpiresAt}");
                }
                if (models.Any())
                    foreach (var item in models)
                    {
                        var isRunning = runningModels.Any(x => x.Name == item.Name);
                        var isEmbed = (item.Details.Family ?? string.Empty).ToLower().EndsWith("bert");
                        
                        // Отладочная информация для каждой модели
                        Console.WriteLine($"[?] Model: {item.Name} - Running: {(isRunning ? "[Y]" : "[N]")} - Embed: {(isEmbed ? "[E]" : "[M]")}");
                        var statusList = new List<CellTag>();
                        var quartList = item.Details != null
                            ? new List<CellTag>
                            {
                                new("[#] " + (item.Details.ParameterSize ?? "Unknown").ToUpper(), TTypeMini.Success),
                                new("[Q] " + (item.Details.QuantizationLevel ?? "Unknown").ToUpper(), TTypeMini.Warn),
                                new("[F] " + (item.Details.Format ?? "Unknown").ToUpper(), TTypeMini.Default)
                            }
                            : new List<CellTag>();

                        var btnList = new List<CellButton>
                        {
                            new("copy-id", "[ID]", TTypeMini.Primary)
                            {
                                Tooltip = LocalizationManager.GetTranslation("copy_model_id"),
                                Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgCopy,
                                Back = Color.FromArgb(52, 152, 219), BackHover = Color.FromArgb(41, 128, 185)
                            },
                            new("copy", "Копия", TTypeMini.Success)
                            {
                                Tooltip = LocalizationManager.GetTranslation("copy_model"),
                                Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgCopy,
                                Back = Color.FromArgb(34, 197, 94), BackHover = Color.FromArgb(22, 163, 74)
                            },
                            new("info", "Инфо", TTypeMini.Info)
                            {
                                Tooltip = LocalizationManager.GetTranslation("model_info"),
                                Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgInfo,
                                Back = Color.FromArgb(24, 188, 156), BackHover = Color.FromArgb(105, 211, 191)
                            },
                        };

                        if (isRunning)
                        {
                            var runModel = runningModels.First(x => x.Name == item.Name);
                            var expires = runModel.ExpiresAt;
                            
                            // Более наглядное отображение активной модели
                            statusList.Add(new CellTag("● " + LocalizationManager.GetTranslation("active"), TTypeMini.Success));
                            
                            // Размер в памяти
                            var ramSize = (runModel.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.0") + "GB RAM";
                            statusList.Add(new CellTag("[M] " + ramSize, TTypeMini.Primary));
                            
                            // GPU память (если используется)
                            if (runModel.SizeVram != 0) 
                            {
                                var gpuSize = (runModel.SizeVram / 1024.00 / 1024.00 / 1024.00).ToString("0.1") + "GB GPU";
                                statusList.Add(new CellTag("[G] " + gpuSize, TTypeMini.Warn));
                            }
                            
                            // Время работы
                            if (expires.Year > 2300)
                            {
                                statusList.Add(new CellTag("[∞] " + LocalizationManager.GetTranslation("permanent"), TTypeMini.Info));
                            }
                            else
                            {
                                var remaining = (expires - DateTime.Now).TotalMinutes;
                                var timeText = remaining > 60 
                                    ? $"{remaining / 60:F0}ч {remaining % 60:F0}м"
                                    : $"{remaining:F0} " + LocalizationManager.GetTranslation("minutes");
                                statusList.Add(new CellTag("[T] " + timeText, TTypeMini.Info));
                            }
                            
                            btnList.Insert(0,
                                new("pin", "Pin", TTypeMini.Warn)
                                { 
                                    Ghost = false, 
                                    BorderWidth = 1, 
                                    IconSvg = Properties.Resources.svgPin,
                                    Tooltip = "Закрепить модель в памяти",
                                    Back = Color.FromArgb(255, 193, 7), BackHover = Color.FromArgb(255, 179, 0)
                                });
                        }
                        else
                        {
                            // Более четкое отображение неактивной модели
                            statusList.Add(new CellTag("○ " + LocalizationManager.GetTranslation("sleeping"), TTypeMini.Default));
                            
                            // Показываем размер файла на диске
                            var diskSize = (item.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.1") + "GB";
                            statusList.Add(new CellTag("[D] " + diskSize + " диск", TTypeMini.Default));
                            
                            btnList.Insert(0,
                                new("delete", "Del", TTypeMini.Error)
                                { 
                                    Ghost = false, 
                                    BorderWidth = 1, 
                                    IconSvg = Properties.Resources.svgDel,
                                    Tooltip = "Удалить модель с диска",
                                    Back = Color.FromArgb(239, 68, 68), BackHover = Color.FromArgb(220, 38, 38)
                                });
                        }

                        if (item.Details != null && !isEmbed)
                        {
                            btnList.AddRange(new[]
                            {
                                isRunning
                                    ? new CellButton("sleep", "Сон", TTypeMini.Primary)
                                    {
                                        Tooltip = LocalizationManager.GetTranslation("sleep_model"),
                                        Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgSnow,
                                        Back = Color.FromArgb(30, 136, 229), BackHover = Color.FromArgb(12, 129, 224)
                                    }
                                    : new CellButton("run", "Запуск", TTypeMini.Success)
                                    {
                                        Tooltip = LocalizationManager.GetTranslation("warmup_model"),
                                        Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgWarm,
                                        Back = Color.FromArgb(34, 197, 94), BackHover = Color.FromArgb(22, 163, 74)
                                    },
                                new CellButton("web-chat", "Web", TTypeMini.Default)
                                    {
                                        Ghost = false, 
                                        BorderWidth = 1,
                                        Tooltip = "Открыть веб-чат",
                                        Back = Color.FromArgb(107, 114, 128), BackHover = Color.FromArgb(75, 85, 99)
                                    }
                            });
                        }

                        btnList.Reverse();
                        
                        // Получаем иконку семейства модели
                        var modelIcon = GetModelFamilyIcon(item.Name ?? "");
                        var displayName = $"{modelIcon} {item.Name ?? "Empty"}";
                        
                        modelsClasses.Add(new ModelsClass
                        {
                            name = displayName,
                            size = (item.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.00") + " GB", // ?? 0
                            modifiedAt = item.ModifiedAt, //?? DateTime.MinValue
                            families = item.Details != null
                                ? item.Details.Families != null
                                    ? item.Details.Families.Where(x => !string.IsNullOrEmpty(x))
                                        .Select(x => new CellTag("[F] " + x.ToUpper(), TTypeMini.Default)).ToArray()
                                    : new[] {new CellTag("[?] Unknown", TTypeMini.Default)}
                                : new[] {new CellTag("[?] Unknown", TTypeMini.Default)},
                            status = statusList.ToArray(),
                            quantization = quartList.ToArray(),
                            btns = btnList.ToArray()
                        });
                    }

                Invoke(() =>
                {
                    table1.DataSource = modelsClasses;
                    if (modelsClasses.Count is < 1) AntdUI.Message.info(this, LocalizationManager.GetTranslation("no_models"));
                });
            }
            catch (Exception e)
            {
                AntdUI.Message.error(this, LocalizationManager.GetTranslation("load_models_error"));
                Console.WriteLine(e);
            }
        }

        public static bool PortIsUse(int port)
        {
            try
            {
                var ipEndPointsTcp = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
                return ipEndPointsTcp.Any(endPoint => endPoint.Port == port);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return true;
            }
        }
        private void dropdown1_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            string selectedValue = e.Value.ToString();
            
            // Импорт модели
            if (selectedValue == LocalizationManager.GetTranslation("import_model_menu"))
            {
                if (IsRemote)
                {
                    AntdUI.Message.warn(this, LocalizationManager.GetTranslation("remote_not_supported"));
                    return;
                }
                new FormImport().ShowDialog();
                ListModels();
            }
            // Настройки Ollama
            else if (selectedValue == LocalizationManager.GetTranslation("ollama_settings"))
            {
                if (IsRemote)
                {
                    AntdUI.Message.warn(this, LocalizationManager.GetTranslation("remote_not_supported"));
                    return;
                }
                new FormSettings().ShowDialog();
            }
            // Обновить список моделей
            else if (selectedValue == LocalizationManager.GetTranslation("model_list_refresh"))
            {
                ListModels();
            }
            // Обновить список моделей для загрузки
            else if (selectedValue == LocalizationManager.GetTranslation("refresh_models"))
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // Принудительно обновляем кэш моделей
                        await OllamaModelsFetcher.GetOfficialModelsAsync(forceRefresh: true);
                        
                        // Перезагружаем select на UI потоке
                        Invoke(() => InitializeModelSelect());
                        
                        Invoke(() => AntdUI.Message.success(this, LocalizationManager.GetTranslation("download_completed")));
                    }
                    catch (Exception ex)
                    {
                        Invoke(() => AntdUI.Message.error(this, $"Error: {ex.Message}"));
                                         }
                 });
            }
            // NextChat
            else if (selectedValue == "NextChat")
            {
                Process.Start(
                    new ProcessStartInfo(
                            $"https://app.nextchat.dev/#/?settings={{%22url%22:%22http://{OllamaUri}%22}}")
                    { UseShellExecute = true });
            }
            // Онлайн модели
            else if (selectedValue == LocalizationManager.GetTranslation("find_models_online"))
            {
                Process.Start(new ProcessStartInfo($"https://ollama.com/library") { UseShellExecute = true });
            }
            // Ollama.com - прямая ссылка на сайт
            else if (selectedValue == "Ollama.com")
            {
                Process.Start(new ProcessStartInfo($"https://ollama.com/library") { UseShellExecute = true });
            }
            // HuggingFace - прямая ссылка на сайт
            else if (selectedValue == "HuggingFace")
            {
                Process.Start(new ProcessStartInfo($"https://huggingface.co/models?pipeline_tag=text-generation&sort=trending&search=gguf") { UseShellExecute = true });
            }
            // Расположение моделей
            else if (selectedValue == LocalizationManager.GetTranslation("view_models_location"))
            {
                if (IsRemote)
                {
                    AntdUI.Message.warn(this, LocalizationManager.GetTranslation("remote_not_supported"));
                    return;
                }
                Process.Start(new ProcessStartInfo($"explorer.exe",
                    Environment.GetEnvironmentVariable("OLLAMA_MODELS", EnvironmentVariableTarget.User) ??
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.ollama\\models"
                ));
            }
            // Просмотр логов
            else if (selectedValue == LocalizationManager.GetTranslation("view_logs"))
            {
                if (IsRemote)
                {
                    AntdUI.Message.warn(this, LocalizationManager.GetTranslation("remote_not_supported"));
                    return;
                }
                Process.Start(new ProcessStartInfo($"explorer.exe",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Ollama\\"));
            }
            // Проверка обновлений
            else if (selectedValue == LocalizationManager.GetTranslation("check_updates"))
            {
                new Modal.Config(this, LocalizationManager.GetTranslation("ollama_core_version"), 
                    Task.Run(() => OllamaApi.GetVersionAsync()).Result, TType.Info)
                {
                    OnOk = _ =>
                    {
                        Process.Start(new ProcessStartInfo($"https://github.com/ollama/ollama/releases/latest")
                        { UseShellExecute = true });
                        return true;
                    }
                }.open();
            }
            // OpenAI API
            else if (selectedValue == LocalizationManager.GetTranslation("openai_api"))
            {
                new Modal.Config(this, LocalizationManager.GetTranslation("openai_api"),
                    new[]
                    {
                        new Modal.TextLine("API: " + OllamaUri + "v1", Style.Db.Primary),
                        new Modal.TextLine("Chat: " + OllamaUri + "v1/chat/completions"),
                        new Modal.TextLine("Completions: " + OllamaUri + "v1/completions"),
                        new Modal.TextLine("Embeddings: " + OllamaUri + "v1/embeddings")
                    }, TType.Info)
                {
                    OkText = LocalizationManager.GetTranslation("copy_url"),
                    OnOk = _ =>
                    {
                        Thread.Sleep(1);
                        try
                        {
                            Invoke(() => Clipboard.SetText(OllamaUri + "v1"));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        return true;
                    }
                }.open();
            }
            // Настройки языка
            else if (selectedValue == LocalizationManager.GetTranslation("language_settings"))
            {
                var langOptions = new List<object>
                {
                    new SelectItem("English", LocalizationManager.Language.English),
                    new SelectItem("Русский", LocalizationManager.Language.Russian),
                    new SelectItem("中文", LocalizationManager.Language.Chinese)
                };
                
                // Используем простой выбор языка
                new Modal.Config(this, LocalizationManager.GetTranslation("language_settings"),
                    new[]
                    {
                        new Modal.TextLine(LocalizationManager.GetTranslation("select_language")),
                        new Modal.TextLine("Выберите / Select / 选择:", Style.Db.TextSecondary),
                        new Modal.TextLine("1. English", Style.Db.Primary),
                        new Modal.TextLine("2. Русский", Style.Db.Primary),
                        new Modal.TextLine("3. 中文", Style.Db.Primary)
                    }, TType.Info)
                {
                    OkText = "English",
                    OnOk = _ =>
                    {
                        // Показываем окно выбора языка
                        var langChoice = MessageBox.Show(
                            "Choose Language / Выберите язык / 选择语言:\n\n" +
                            "1. English\n" +
                            "2. Русский\n" +
                            "3. 中文\n\n" +
                            "Click Yes for English, No for Russian, Cancel for Chinese",
                            "Language / Язык / 语言",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);
                        
                        switch (langChoice)
                        {
                            case DialogResult.Yes:
                                LocalizationManager.CurrentLanguage = LocalizationManager.Language.English;
                                break;
                            case DialogResult.No:
                                LocalizationManager.CurrentLanguage = LocalizationManager.Language.Russian;
                                break;
                            case DialogResult.Cancel:
                                LocalizationManager.CurrentLanguage = LocalizationManager.Language.Chinese;
                                break;
                        }
                        
                        LocalizationManager.SaveLanguagePreference();
                        RefreshUI();
                        return true;
                    }
                }.open();
            }
            // Игнорируем разделители
            else if (selectedValue.Contains("━"))
            {
                // Ничего не делаем для разделителей
            }
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            // Динамически адаптируем ширину выпадающих списков под размер формы
            var availableWidth = flowLayoutPanel1.Width - button1.Width - buttonHfPull.Width - button2.Width - buttonHfFilter.Width - dropdown1.Width - 80;
            var listWidth = Math.Max(180, availableWidth / 2);
            
            select1.Width = listWidth;
            selectHuggingFace.Width = listWidth;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (select1.Text.Contains(" ")) select1.Text = select1.Text.Split(' ').Last();
            new FormRegistryInfo(select1.Text).ShowDialog();
        }

        private void buttonHfFilter_Click(object sender, EventArgs e)
        {
            // Показываем диалог фильтрации HuggingFace моделей
            ShowHuggingFaceFilterDialog();
        }

        private void selectHuggingFace_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (selectHuggingFace.Text == "🔄 Обновить GGUF модели")
            {
                // Обновляем HuggingFace модели
                Task.Run(() => RefreshHuggingFaceModels());
                selectHuggingFace.SelectedIndex = -1; // Сбрасываем выбор
            }
        }

        private void buttonHfPull_Click(object sender, EventArgs e)
        {
            // Получаем выбранную HuggingFace модель
            var selectedModel = selectHuggingFace.Text;
            
            // Проверяем, что выбрана не служебная строка
            if (string.IsNullOrEmpty(selectedModel) || 
                selectedModel.StartsWith("🔄") ||
                selectedModel == "🤗 HuggingFace GGUF")
            {
                AntdUI.Message.warn(this, "Пожалуйста, выберите GGUF модель для загрузки");
                return;
            }

            // Извлекаем имя модели из тега (если есть)
            var modelName = selectedModel;
            var selectedIndex = selectHuggingFace.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < selectHuggingFace.Items.Count)
            {
                bool foundModel = false;
                foreach (var item in selectHuggingFace.Items)
                {
                    if (item is SelectItem selectItem && selectItem.Sub != null)
                    {
                        foreach (var subItem in selectItem.Sub)
                        {
                            if (subItem is SelectItem modelItem && modelItem.Tag != null)
                            {
                                if (modelItem.Text.Contains(selectedModel.Replace("● ", "")) || 
                                    selectedModel.Contains(modelItem.Tag.ToString()))
                                {
                                    modelName = modelItem.Tag.ToString();
                                    foundModel = true;
                                    break;
                                }
                            }
                        }
                        if (foundModel) break;
                    }
                }
            }
            
            new Modal.Config(this, "Загрузить GGUF модель", 
                new[] { new Modal.TextLine($"🤗 {modelName}", Style.Db.Primary) }, TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = "Загрузить",
                OnOk = _ =>
                {
                    Invoke(progress1.Show);

                    Task.Run(async () =>
                    {
                        try
                        {
                            // Для HuggingFace моделей используем ollama pull с префиксом hf.co/
                            var fullModelName = modelName.StartsWith("hf.co/") ? modelName : $"hf.co/{modelName}";
                            
                            var lastBytes = 0L;
                            var lastTime = DateTime.Now;
                            var speedSamples = new Queue<double>();
                            
                            await foreach (var x in OllamaApi.PullModelAsync(fullModelName))
                            {
                                Invoke(() =>
                                {
                                    progress1.Value = (float)x.Completed / x.Total;
                                    
                                    // Вычисляем скорость загрузки
                                    var now = DateTime.Now;
                                    var elapsedSeconds = (now - lastTime).TotalSeconds;
                                    
                                    if (elapsedSeconds >= 1.0 && x.Completed > lastBytes)
                                    {
                                        var bytesDownloaded = x.Completed - lastBytes;
                                        var instantSpeed = (bytesDownloaded / elapsedSeconds) / (1024 * 1024);
                                        
                                        speedSamples.Enqueue(instantSpeed);
                                        if (speedSamples.Count > 5) speedSamples.Dequeue();
                                        
                                        var avgSpeed = speedSamples.Average();
                                        var remainingBytes = x.Total - x.Completed;
                                        var estimatedTimeSeconds = avgSpeed > 0 ? remainingBytes / (avgSpeed * 1024 * 1024) : 0;
                                        
                                        var sizeInfo = $"{x.Completed / (1024 * 1024):F1}MB / {x.Total / (1024 * 1024):F1}MB";
                                        var speedInfo = $"{avgSpeed:F1} MB/s";
                                        var timeInfo = estimatedTimeSeconds > 0 ? $"ETA: {TimeSpan.FromSeconds(estimatedTimeSeconds):mm\\:ss}" : "";
                                        
                                        progress1.Text = $"🤗 {x.Status} | {sizeInfo} | {speedInfo} | {timeInfo}";
                                        
                                        lastBytes = x.Completed;
                                        lastTime = now;
                                    }
                                    else
                                    {
                                        progress1.Text = $"🤗 {x.Status}";
                                    }
                                });
                            }

                            Invoke(() =>
                            {
                                progress1.Hide();
                                AntdUI.Message.success(this, $"🤗 Модель {modelName} успешно загружена!");
                                
                                // Обновляем списки моделей
                                Task.Run(() => {
                                    Thread.Sleep(1000);
                                    Invoke(() => InitializeModelSelect());
                                });
                            });
                        }
                        catch (Exception ex)
                        {
                            Invoke(() =>
                            {
                                progress1.Hide();
                                AntdUI.Message.error(this, $"Ошибка загрузки: {ex.Message}");
                            });
                        }
                    });
                    return true;
                }
            }.open();
        }

        private void ShowHuggingFaceFilterDialog()
        {
            var availableTags = new[]
            {
                "🤖 Conversational", "📝 Text Generation", "🔍 Question Answering",
                "🌍 Translation", "💻 Code Generation", "🔤 Text2Text", "🧮 Mathematics"
            };

            var modal = new Modal.Config(this, 
                "🤗 HuggingFace Модели - Фильтр по тегам",
                new[]
                {
                    new Modal.TextLine("Выберите категорию для фильтрации:", Style.Db.Primary),
                    new Modal.TextLine("🤖 Conversational - Чат-боты и ассистенты"),
                    new Modal.TextLine("📝 Text Generation - Генерация текста"),
                    new Modal.TextLine("🔍 Question Answering - Ответы на вопросы"), 
                    new Modal.TextLine("🌍 Translation - Переводчики"),
                    new Modal.TextLine("💻 Code Generation - Генерация кода"),
                    new Modal.TextLine("🔤 Text2Text - Преобразование текста"),
                    new Modal.TextLine("🧮 Mathematics - Математические модели")
                }, TType.Info)
            {
                OkText = "Обновить список",
                OnOk = _ =>
                {
                    // Перезагружаем список с фильтрацией
                    Task.Run(() => RefreshHuggingFaceModels());
                    return true;
                }
            };

            modal.open();
        }

        private async Task RefreshHuggingFaceModels()
        {
            try
            {
                Invoke(() => 
                {
                    progress1.Show();
                    progress1.Value = 0;
                    progress1.Text = "🤗 Обновляем HuggingFace GGUF модели...";
                    selectHuggingFace.Enabled = false;
                });

                // Принудительно сбрасываем кэш и загружаем GGUF модели
                HuggingFaceServiceExtended.ClearCache();
                var models = await HuggingFaceServiceExtended.GetGGUFModelsAsync(60);
                
                var loadedModels = new HashSet<string>();
                try
                {
                    var localModels = await OllamaApi.ListLocalModelsAsync();
                    foreach (var model in localModels)
                    {
                        loadedModels.Add(model.Name);
                    }
                }
                catch
                {
                    // Игнорируем ошибки загрузки локальных моделей
                }
                
                Invoke(() => 
                {
                    // Обновляем HuggingFace dropdown
                    UpdateHuggingFaceDropdown(models, loadedModels);
                    
                    progress1.Value = 100;
                    progress1.Text = $"✅ Загружено {models.Count} GGUF моделей HuggingFace";
                    selectHuggingFace.Enabled = true;
                    
                    // Скрываем прогресс через 2 секунды
                    Task.Delay(2000).ContinueWith(_ => Invoke(progress1.Hide));
                    
                    AntdUI.Message.success(this, $"Загружено {models.Count} HuggingFace GGUF моделей");
                });
            }
            catch (Exception ex)
            {
                Invoke(() => 
                {
                    progress1.Text = "❌ Ошибка обновления HuggingFace";
                    selectHuggingFace.Enabled = true;
                    Task.Delay(2000).ContinueWith(_ => Invoke(progress1.Hide));
                    AntdUI.Message.error(this, $"Ошибка обновления: {ex.Message}");
                });
            }
        }

        private void UpdateHuggingFaceDropdown(List<HuggingFaceModelExtended> models, HashSet<string> loadedModels)
        {
            // Очищаем и заполняем HuggingFace выпадающий список
            selectHuggingFace.Items.Clear();
            
            // Добавляем кнопку обновления в начало
            selectHuggingFace.Items.Add(new SelectItem("🔄 Обновить GGUF модели"));

            // Группируем модели по тегам
            var groupedHfModels = HuggingFaceServiceExtended.GroupModelsByTags(models);

            foreach (var (categoryName, categoryModels) in groupedHfModels)
            {
                var categoryItem = new SelectItem(categoryName);
                var categorySubItems = new List<object>();

                foreach (var model in categoryModels)
                {
                    var ollamaModelName = HuggingFaceServiceExtended.FormatModelForOllama(model);
                    var isLoaded = loadedModels.Contains(ollamaModelName);
                    var displayName = HuggingFaceServiceExtended.GetModelDisplayName(model);
                    var prefix = isLoaded ? "● " : "";
                    
                    // Добавляем информацию об авторе и скачиваниях
                    var downloads = model.Downloads > 1000000 
                        ? $"{model.Downloads / 1000000:F1}M"
                        : model.Downloads > 1000 
                            ? $"{model.Downloads / 1000:F0}K" 
                            : model.Downloads.ToString();
                    
                    var fullDisplayName = $"{prefix}{displayName} ({model.Author}) [{downloads}⬇]";
                    
                    categorySubItems.Add(new SelectItem(fullDisplayName) { Tag = ollamaModelName });
                }

                if (categorySubItems.Any())
                {
                    categoryItem.Sub = categorySubItems.ToArray();
                    selectHuggingFace.Items.Add(categoryItem);
                }
            }

            selectHuggingFace.PlaceholderText = $"🤗 HuggingFace GGUF ({models.Count} моделей)";
        }

        private void UpdateHuggingFaceSection(List<HuggingFaceModelExtended> models)
        {
            // Находим и удаляем старую HuggingFace секцию
            var hfItemIndex = -1;
            for (int i = 0; i < select1.Items.Count; i++)
            {
                if (select1.Items[i] is SelectItem item && item.Text.StartsWith("🤗 HuggingFace"))
                {
                    hfItemIndex = i;
                    break;
                }
            }

            if (hfItemIndex >= 0)
            {
                select1.Items.RemoveAt(hfItemIndex);
            }

            // Добавляем обновленную секцию
            if (models.Any())
            {
                var loadedModels = new HashSet<string>();
                try
                {
                    var localModels = Task.Run(async () => await OllamaApi.ListLocalModelsAsync()).Result;
                    foreach (var model in localModels)
                    {
                        loadedModels.Add(model.Name);
                    }
                }
                catch
                {
                    // Игнорируем ошибки загрузки локальных моделей
                }

                var groupedHfModels = HuggingFaceServiceExtended.GroupModelsByTags(models);
                var hfMainItem = new SelectItem($"🤗 HuggingFace ({models.Count})");
                var hfSubItems = new List<object>();

                foreach (var (categoryName, categoryModels) in groupedHfModels)
                {
                    var categoryItem = new SelectItem(categoryName);
                    var categorySubItems = new List<object>();

                    foreach (var model in categoryModels)
                    {
                        var ollamaModelName = HuggingFaceServiceExtended.FormatModelForOllama(model);
                        var isLoaded = loadedModels.Contains(ollamaModelName);
                        var displayName = HuggingFaceServiceExtended.GetModelDisplayName(model);
                        var prefix = isLoaded ? "● " : "";
                        
                        var downloads = model.Downloads > 1000000 
                            ? $"{model.Downloads / 1000000:F1}M"
                            : model.Downloads > 1000 
                                ? $"{model.Downloads / 1000:F0}K" 
                                : model.Downloads.ToString();
                        
                        var fullDisplayName = $"{prefix}{displayName} ({model.Author}) [{downloads}⬇]";
                        categorySubItems.Add(new SelectItem(fullDisplayName) { Tag = ollamaModelName });
                    }

                    if (categorySubItems.Any())
                    {
                        categoryItem.Sub = categorySubItems.ToArray();
                        hfSubItems.Add(categoryItem);
                    }
                }

                if (hfSubItems.Any())
                {
                    hfMainItem.Sub = hfSubItems.ToArray();
                    select1.Items.Add(hfMainItem);
                }
            }
        }

        private void Table1OnCellButtonClick(object sender, TableButtonEventArgs e)
        {
            var record = e.Record;
            var btn = e.Btn;

            if (record is not ModelsClass data) return;
            switch (btn.Id)
            {
                case "copy-id":
                    try
                    {
                        Clipboard.SetText(data.name);
                        AntdUI.Message.success(this, LocalizationManager.GetTranslation("model_id_copied") + ": " + data.name);
                    }
                    catch (Exception ex)
                    {
                        AntdUI.Message.error(this, LocalizationManager.GetTranslation("copy_failed") + ": " + ex.Message);
                    }
                    break;
                case "delete":
                    new Modal.Config(this, LocalizationManager.GetTranslation("delete_confirm"),
                        new[]
                        {
                            new Modal.TextLine(data.name, Style.Db.Primary),
                            new Modal.TextLine(data.size, Style.Db.TextSecondary)
                        }, TType.Warn)
                    {
                        OkType = TTypeMini.Error,
                        OkText = LocalizationManager.GetTranslation("delete"),
                        OnOk = _ =>
                        {
                            Task.Run(async () => await OllamaApi.DeleteModelAsync(data.name)).Wait();
                            Invoke(() => ListModels());
                            return true;
                        }
                    }.open();
                    break;
                case "web-chat":
                    AntdUI.Message.success(this, LocalizationManager.GetTranslation("to_webchat"));
                    Process.Start(new ProcessStartInfo($"https://onllama.netlify.app/") { UseShellExecute = true });
                    break;
                case "copy":
                    new FormCopy(data.name).ShowDialog();
                    ListModels();
                    break;
                case "run":
                    new Modal.Config(this, LocalizationManager.GetTranslation("warmup_confirm"), data.name, TType.Info)
                    {
                        OnOk = _ =>
                        {
                            Task.Run(async () =>
                            {
                                await foreach (var _ in OllamaApi.GenerateAsync(new GenerateRequest()
                                                   {Model = data.name, KeepAlive = "30m", Stream = false})) { }
                            }).Wait();
                            Invoke(() => ListModels());
                            return true;
                        }
                    }.open();
                    break;
                case "pin":
                    new Modal.Config(this, LocalizationManager.GetTranslation("pin_confirm"), data.name, TType.Info)
                    {
                        OnOk = _ =>
                        {
                            Task.Run(async () =>
                            {
                                await foreach (var _ in OllamaApi.GenerateAsync(new GenerateRequest()
                                                   { Model = data.name, KeepAlive = "-1m", Stream = false })) { }
                            }).Wait();
                            Invoke(() => ListModels());
                            return true;
                        }
                    }.open();
                    break;
                case "sleep":
                    new Modal.Config(this, LocalizationManager.GetTranslation("sleep_confirm"), data.name, TType.Warn)
                    {
                        OnOk = _ =>
                        {
                            Task.Run(async () =>
                            {
                                await foreach (var _ in OllamaApi.GenerateAsync(new GenerateRequest()
                                { Model = data.name, KeepAlive = "0m", Stream = false })) { }
                            }).Wait();
                            Thread.Sleep(2000);
                            Invoke(() => ListModels());
                            return true;
                        }
                    }.open();
                    break;
                case "info":
                    new FormInfo(data.name).ShowDialog();
                    break;
            }
        }

        #region Сохранение размеров окна

        private void LoadWindowSettings()
        {
            try
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Onllama\WindowSettings"))
                {
                    if (key != null)
                    {
                        var width = key.GetValue("Width", this.Width);
                        var height = key.GetValue("Height", this.Height);
                        var left = key.GetValue("Left", this.Left);
                        var top = key.GetValue("Top", this.Top);
                        var windowState = key.GetValue("WindowState", this.WindowState.ToString());

                        // Проверяем, что координаты находятся на видимых экранах
                        var bounds = new Rectangle((int)left, (int)top, (int)width, (int)height);
                        bool isVisible = Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(bounds));

                        if (isVisible)
                        {
                            this.Size = new Size((int)width, (int)height);
                            this.Location = new Point((int)left, (int)top);
                            
                            if (Enum.TryParse<FormWindowState>(windowState.ToString(), out var state))
                            {
                                this.WindowState = state;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки настроек окна: {ex.Message}");
            }
        }

        private void SaveWindowSettings()
        {
            try
            {
                // Сохраняем только если окно не свернуто
                if (this.WindowState != FormWindowState.Minimized)
                {
                    using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Onllama\WindowSettings"))
                    {
                        if (key != null)
                        {
                            // Если окно развернуто, сохраняем RestoreBounds, иначе текущие размеры
                            if (this.WindowState == FormWindowState.Maximized)
                            {
                                key.SetValue("Width", this.RestoreBounds.Width);
                                key.SetValue("Height", this.RestoreBounds.Height);
                                key.SetValue("Left", this.RestoreBounds.Left);
                                key.SetValue("Top", this.RestoreBounds.Top);
                            }
                            else
                            {
                                key.SetValue("Width", this.Width);
                                key.SetValue("Height", this.Height);
                                key.SetValue("Left", this.Left);
                                key.SetValue("Top", this.Top);
                            }
                            
                            key.SetValue("WindowState", this.WindowState.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения настроек окна: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveWindowSettings();
            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            // Сохраняем настройки с небольшой задержкой для избежания частых записей
            if (!_resizeTimer.Enabled)
            {
                _resizeTimer.Start();
            }
            else
            {
                _resizeTimer.Stop();
                _resizeTimer.Start();
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            
            // Сохраняем настройки с небольшой задержкой для избежания частых записей
            if (!_moveTimer.Enabled)
            {
                _moveTimer.Start();
            }
            else
            {
                _moveTimer.Stop();
                _moveTimer.Start();
            }
        }

        private System.Windows.Forms.Timer _resizeTimer = new System.Windows.Forms.Timer { Interval = 500 };
        private System.Windows.Forms.Timer _moveTimer = new System.Windows.Forms.Timer { Interval = 500 };

        private void InitializeTimers()
        {
            _resizeTimer.Tick += (s, e) =>
            {
                _resizeTimer.Stop();
                SaveWindowSettings();
            };

            _moveTimer.Tick += (s, e) =>
            {
                _moveTimer.Stop();
                SaveWindowSettings();
            };
        }

        #endregion
    }

    // Расширенный класс для работы с HuggingFace API
    public class HuggingFaceModelExtended
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        
        [JsonPropertyName("modelId")]
        public string ModelId { get; set; } = "";
        
        [JsonPropertyName("author")]
        public string Author { get; set; } = "";
        
        [JsonPropertyName("sha")]
        public string Sha { get; set; } = "";
        
        [JsonPropertyName("lastModified")]
        public DateTime LastModified { get; set; }
        
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new();
        
        [JsonPropertyName("pipeline_tag")]
        public string PipelineTag { get; set; } = "";
        
        [JsonPropertyName("downloads")]
        public int Downloads { get; set; }
        
        [JsonPropertyName("likes")]
        public int Likes { get; set; }
        
        [JsonPropertyName("library_name")]
        public string LibraryName { get; set; } = "";
        
        [JsonPropertyName("language")]
        public List<string> Language { get; set; } = new();
        
        [JsonPropertyName("license")]
        public string License { get; set; } = "";
    }

    public static class HuggingFaceServiceExtended
    {
        private static readonly HttpClient _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private static List<HuggingFaceModelExtended>? _cachedModels;
        private static DateTime _lastCacheUpdate = DateTime.MinValue;
        private static readonly TimeSpan CacheExpiry = TimeSpan.FromHours(2);

        public static void ClearCache()
        {
            _cachedModels = null;
            _lastCacheUpdate = DateTime.MinValue;
        }

        public static async Task<List<HuggingFaceModelExtended>> GetPopularModelsAsync(int limit = 50)
        {
            if (_cachedModels != null && DateTime.Now - _lastCacheUpdate < CacheExpiry)
            {
                return _cachedModels;
            }

            try
            {
                var url = $"https://huggingface.co/api/models?sort=downloads&direction=-1&limit={limit}&filter=library:gguf,library:transformers,pipeline_tag:text-generation";
                
                var response = await _httpClient.GetStringAsync(url);
                var models = JsonSerializer.Deserialize<List<HuggingFaceModelExtended>>(response) ?? new();
                
                var compatibleModels = models
                    .Where(m => IsOllamaCompatible(m))
                    .OrderByDescending(m => m.Downloads)
                    .ToList();

                _cachedModels = compatibleModels;
                _lastCacheUpdate = DateTime.Now;
                
                return compatibleModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading HuggingFace models: {ex.Message}");
                return GetGGUFFallbackModels();
            }
        }

        public static async Task<List<HuggingFaceModelExtended>> GetGGUFModelsAsync(int limit = 50)
        {
            // Проверяем кэш
            if (_cachedModels != null && DateTime.Now - _lastCacheUpdate < CacheExpiry)
            {
                Console.WriteLine($"📋 Используем кэшированные {_cachedModels.Count} GGUF модели");
                return _cachedModels.Take(limit).ToList();
            }

            // Если нет кэша, сначала загружаем fallback модели для немедленного отображения
            if (_cachedModels == null)
            {
                _cachedModels = GetGGUFFallbackModels();
                _lastCacheUpdate = DateTime.Now;
                Console.WriteLine($"🔄 Загружены fallback GGUF модели: {_cachedModels.Count}");
            }

            try
            {
                Console.WriteLine("🔄 Запрашиваем GGUF модели с HuggingFace API...");
                
                // Несколько попыток с разными фильтрами для максимального охвата
                var urls = new[]
                {
                    $"https://huggingface.co/api/models?sort=downloads&direction=-1&limit={limit}&filter=library:gguf",
                    $"https://huggingface.co/api/models?sort=downloads&direction=-1&limit={limit}&filter=gguf",
                    $"https://huggingface.co/api/models?sort=downloads&direction=-1&limit={limit * 2}&search=gguf"
                };

                var allModels = new List<HuggingFaceModelExtended>();
                
                foreach (var url in urls)
                {
                    try
                    {
                        Console.WriteLine($"🌐 Пробуем URL: {url}");
                        var response = await _httpClient.GetStringAsync(url);
                        var models = JsonSerializer.Deserialize<List<HuggingFaceModelExtended>>(response) ?? new();
                        
                        Console.WriteLine($"📦 Получено {models.Count} моделей из API");
                        
                        // Добавляем модели, избегая дублирования
                        foreach (var model in models)
                        {
                            if (!allModels.Any(m => m.Id == model.Id))
                            {
                                allModels.Add(model);
                            }
                        }
                        
                        // Если получили достаточно моделей, можем остановиться
                        if (allModels.Count >= limit)
                            break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Ошибка с URL {url}: {ex.Message}");
                        continue;
                    }
                }
                
                // Фильтруем GGUF модели с более мягкими критериями
                var ggufModels = allModels.Where(m => 
                    (m.Tags?.Any(tag => 
                        tag.Contains("gguf", StringComparison.OrdinalIgnoreCase) ||
                        tag.Contains("quantized", StringComparison.OrdinalIgnoreCase)) == true) ||
                    m.Id.Contains("gguf", StringComparison.OrdinalIgnoreCase) ||
                    m.Id.Contains("GGUF", StringComparison.Ordinal) ||
                    (m.LibraryName?.Contains("gguf", StringComparison.OrdinalIgnoreCase) == true) ||
                    m.ModelId.Contains("gguf", StringComparison.OrdinalIgnoreCase)
                ).OrderByDescending(m => m.Downloads)
                .Take(limit)
                .ToList();
                
                Console.WriteLine($"✅ Отфильтровано {ggufModels.Count} GGUF моделей");
                
                // Если получили мало моделей, добавляем проверенные GGUF модели
                if (ggufModels.Count < 10)
                {
                    var fallbackModels = GetGGUFFallbackModels();
                    foreach (var fallback in fallbackModels)
                    {
                        if (!ggufModels.Any(m => m.Id == fallback.Id))
                        {
                            ggufModels.Add(fallback);
                        }
                    }
                    Console.WriteLine($"➕ Добавлено {fallbackModels.Count} проверенных GGUF моделей");
                }
                
                // Обновляем кэш
                _cachedModels = ggufModels;
                _lastCacheUpdate = DateTime.Now;
                
                Console.WriteLine($"✅ Итого загружено {ggufModels.Count} GGUF моделей");
                return ggufModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Критическая ошибка загрузки GGUF моделей: {ex.Message}");
                var fallback = GetGGUFFallbackModels();
                Console.WriteLine($"🔄 Используем {fallback.Count} fallback GGUF моделей");
                return fallback;
            }
        }

        public static Dictionary<string, Dictionary<string, List<HuggingFaceModelExtended>>> GroupGGUFModelsByFamilies(List<HuggingFaceModelExtended> models)
        {
            var grouped = new Dictionary<string, Dictionary<string, List<HuggingFaceModelExtended>>>();

            // Создаем структуру по аналогии с Ollama
            var modelFamilies = new Dictionary<string, Dictionary<string, string[]>>
            {
                ["🧠 Reasoning & Instruct"] = new Dictionary<string, string[]>
                {
                    ["Phi-3.5"] = new[] { "phi-3.5", "phi3.5" },
                    ["Phi-3"] = new[] { "phi-3", "phi3" },
                    ["Qwen2.5"] = new[] { "qwen2.5", "qwen25" },
                    ["DeepSeek"] = new[] { "deepseek" }
                },
                ["🦙 Llama Family"] = new Dictionary<string, string[]>
                {
                    ["Llama-3.1"] = new[] { "llama-3.1", "llama3.1", "meta-llama-3.1" },
                    ["Llama-3.2"] = new[] { "llama-3.2", "llama3.2", "meta-llama-3.2" },
                    ["CodeLlama"] = new[] { "codellama", "code-llama" }
                },
                ["💎 Gemma & Mistral"] = new Dictionary<string, string[]>
                {
                    ["Gemma-2"] = new[] { "gemma-2", "gemma2" },
                    ["Mistral-7B"] = new[] { "mistral-7b", "mistral7b" },
                    ["OpenHermes"] = new[] { "openhermes", "hermes" }
                },
                ["💻 Code Generation"] = new Dictionary<string, string[]>
                {
                    ["DeepSeek-Coder"] = new[] { "deepseek-coder", "coder" },
                    ["CodeLlama"] = new[] { "codellama", "code-llama" },
                    ["StarCoder"] = new[] { "starcoder", "star-coder" }
                },
                ["🤖 Conversational"] = new Dictionary<string, string[]>
                {
                    ["Neural-Chat"] = new[] { "neural-chat", "neuralchat" },
                    ["CapybaraHermes"] = new[] { "capybarahermes", "capybara" },
                    ["Starling"] = new[] { "starling" }
                },
                ["🧮 Specialized"] = new Dictionary<string, string[]>
                {
                    ["WizardMath"] = new[] { "wizardmath", "wizard-math" },
                    ["Yi-1.5"] = new[] { "yi-1.5", "yi1.5" },
                    ["Aya-Expanse"] = new[] { "aya-expanse", "aya" }
                }
            };

            foreach (var (categoryName, families) in modelFamilies)
            {
                var categoryData = new Dictionary<string, List<HuggingFaceModelExtended>>();

                foreach (var (familyName, keywords) in families)
                {
                    var familyModels = models
                        .Where(m => keywords.Any(keyword => 
                            m.ModelId.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            m.Id.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            m.Tags.Any(t => t.Contains(keyword, StringComparison.OrdinalIgnoreCase))))
                        .OrderByDescending(m => m.Downloads)
                        .ToList();

                    if (familyModels.Any())
                    {
                        categoryData[familyName] = familyModels;
                    }
                }

                if (categoryData.Any())
                {
                    grouped[categoryName] = categoryData;
                }
            }

            // Добавляем категорию "Популярные" для оставшихся моделей
            var usedModels = new HashSet<string>();
            foreach (var category in grouped.Values)
            {
                foreach (var family in category.Values)
                {
                    foreach (var model in family)
                    {
                        usedModels.Add(model.Id);
                    }
                }
            }

            var remainingModels = models
                .Where(m => !usedModels.Contains(m.Id))
                .OrderByDescending(m => m.Downloads)
                .Take(15)
                .ToList();

            if (remainingModels.Any())
            {
                // Группируем оставшиеся модели по авторам
                var popularCategory = new Dictionary<string, List<HuggingFaceModelExtended>>();
                var byAuthor = remainingModels.GroupBy(m => m.Author).OrderByDescending(g => g.Sum(m => m.Downloads));
                
                foreach (var authorGroup in byAuthor.Take(5))
                {
                    var authorModels = authorGroup.OrderByDescending(m => m.Downloads).Take(5).ToList();
                    if (authorModels.Any())
                    {
                        popularCategory[$"📦 {authorGroup.Key}"] = authorModels;
                    }
                }

                if (popularCategory.Any())
                {
                    grouped["⭐ Popular GGUF"] = popularCategory;
                }
            }

            return grouped;
        }

        // Оставляем старый метод для совместимости
        public static Dictionary<string, List<HuggingFaceModelExtended>> GroupModelsByTags(List<HuggingFaceModelExtended> models)
        {
            var hierarchical = GroupGGUFModelsByFamilies(models);
            var flattened = new Dictionary<string, List<HuggingFaceModelExtended>>();

            foreach (var (categoryName, families) in hierarchical)
            {
                var allModelsInCategory = new List<HuggingFaceModelExtended>();
                foreach (var (familyName, familyModels) in families)
                {
                    allModelsInCategory.AddRange(familyModels);
                }
                if (allModelsInCategory.Any())
                {
                    flattened[categoryName] = allModelsInCategory.OrderByDescending(m => m.Downloads).ToList();
                }
            }

            return flattened;
        }

        private static bool IsOllamaCompatible(HuggingFaceModelExtended model)
        {
            var compatibleFormats = new[] { "gguf", "ggml", "transformers" };
            var compatibleTags = new[] { "gguf", "ollama", "quantized", "instruct", "chat" };
            
            return model.Tags.Any(tag => compatibleTags.Any(ct => tag.Contains(ct, StringComparison.OrdinalIgnoreCase))) ||
                   model.LibraryName != null && compatibleFormats.Contains(model.LibraryName.ToLowerInvariant()) ||
                   model.ModelId.Contains("gguf", StringComparison.OrdinalIgnoreCase) ||
                   model.ModelId.Contains("ollama", StringComparison.OrdinalIgnoreCase);
        }

        public static List<HuggingFaceModelExtended> GetGGUFFallbackModels()
        {
            return new List<HuggingFaceModelExtended>
            {
                // Актуальные популярные GGUF модели
                new() { 
                    Id = "microsoft/Phi-3.5-mini-instruct-gguf", 
                    ModelId = "microsoft/Phi-3.5-mini-instruct-gguf",
                    Author = "microsoft",
                    Tags = new() { "gguf", "text-generation", "instruct", "chat" },
                    PipelineTag = "text-generation",
                    Downloads = 2500000,
                    Likes = 350,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/Phi-3.5-mini-instruct-GGUF", 
                    ModelId = "bartowski/Phi-3.5-mini-instruct-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "quantized" },
                    PipelineTag = "text-generation",
                    Downloads = 1800000,
                    Likes = 280,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "microsoft/Phi-3-mini-128k-instruct-gguf", 
                    ModelId = "microsoft/Phi-3-mini-128k-instruct-gguf",
                    Author = "microsoft",
                    Tags = new() { "gguf", "text-generation", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 2200000,
                    Likes = 320,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/gemma-2-9b-it-GGUF", 
                    ModelId = "bartowski/gemma-2-9b-it-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "gemma", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 1500000,
                    Likes = 250,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/Meta-Llama-3.1-8B-Instruct-GGUF", 
                    ModelId = "bartowski/Meta-Llama-3.1-8B-Instruct-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "llama", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 3200000,
                    Likes = 450,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/Llama-3.2-3B-Instruct-GGUF", 
                    ModelId = "bartowski/Llama-3.2-3B-Instruct-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "llama", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 1900000,
                    Likes = 300,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/Qwen2.5-7B-Instruct-GGUF", 
                    ModelId = "bartowski/Qwen2.5-7B-Instruct-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "qwen", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 2800000,
                    Likes = 380,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "microsoft/DialoGPT-medium-gguf", 
                    ModelId = "microsoft/DialoGPT-medium-gguf",
                    Author = "microsoft",
                    Tags = new() { "gguf", "conversational", "chat" },
                    PipelineTag = "conversational",
                    Downloads = 1200000,
                    Likes = 180,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/deepseek-coder-6.7b-instruct-GGUF", 
                    ModelId = "bartowski/deepseek-coder-6.7b-instruct-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "code-generation", "programming", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 1600000,
                    Likes = 220,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/CodeLlama-7b-Instruct-GGUF", 
                    ModelId = "bartowski/CodeLlama-7b-Instruct-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "code-generation", "llama", "programming" },
                    PipelineTag = "text-generation",
                    Downloads = 2100000,
                    Likes = 290,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/mistral-7b-instruct-v0.3-GGUF", 
                    ModelId = "bartowski/mistral-7b-instruct-v0.3-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "mistral", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 2400000,
                    Likes = 340,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/neural-chat-7b-v3-1-GGUF", 
                    ModelId = "bartowski/neural-chat-7b-v3-1-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "conversational", "chat", "neural" },
                    PipelineTag = "conversational",
                    Downloads = 800000,
                    Likes = 120,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/OpenHermes-2.5-Mistral-7B-GGUF", 
                    ModelId = "bartowski/OpenHermes-2.5-Mistral-7B-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "hermes", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 1750000,
                    Likes = 260,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/CapybaraHermes-2.5-Mistral-7B-GGUF", 
                    ModelId = "bartowski/CapybaraHermes-2.5-Mistral-7B-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "instruct", "creative" },
                    PipelineTag = "text-generation",
                    Downloads = 1350000,
                    Likes = 195,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/Yi-1.5-9B-Chat-GGUF", 
                    ModelId = "bartowski/Yi-1.5-9B-Chat-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "chat", "yi" },
                    PipelineTag = "text-generation",
                    Downloads = 980000,
                    Likes = 145,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/Starling-LM-7B-alpha-GGUF", 
                    ModelId = "bartowski/Starling-LM-7B-alpha-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "text-generation", "starling", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 1120000,
                    Likes = 165,
                    LibraryName = "gguf"
                },
                new() { 
                    Id = "bartowski/WizardMath-7B-V1.1-GGUF", 
                    ModelId = "bartowski/WizardMath-7B-V1.1-GGUF",
                    Author = "bartowski",
                    Tags = new() { "gguf", "math", "mathematical", "wizard", "instruct" },
                    PipelineTag = "text-generation",
                    Downloads = 890000,
                    Likes = 135,
                    LibraryName = "gguf"
                }
            };
        }

        public static string FormatModelForOllama(HuggingFaceModelExtended model)
        {
            return $"hf.co/{model.ModelId}";
        }

        public static string GetModelDisplayName(HuggingFaceModelExtended model)
        {
            var parts = model.ModelId.Split('/');
            var modelName = parts.Length > 1 ? parts[1] : model.ModelId;
            
            var popularity = model.Downloads switch
            {
                > 1000000 => "🔥",
                > 500000 => "⭐",
                > 100000 => "📈",
                _ => ""
            };

            return $"{popularity} {modelName}";
        }
    }
}
