using AntdUI;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Onllama.Tiny
{
    // Класс для официальных моделей Ollama
    public class OfficialOllamaModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Capabilities { get; set; } = new List<string>();
        public string PullCount { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
        
        public string GetDisplayName()
        {
            if (Sizes.Count > 0)
            {
                return $"{Name}:{Sizes[0]}";
            }
            return Name;
        }
        
        public string GetFullSizesList()
        {
            return Sizes.Count > 0 ? string.Join(", ", Sizes.Select(s => $"{Name}:{s}")) : Name;
        }
    }

    // Статический класс для загрузки моделей Ollama
    public static class OllamaModelsFetcher
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static List<OfficialOllamaModel>? cachedModels = null;
        private static DateTime lastFetchTime = DateTime.MinValue;
        private static readonly TimeSpan cacheValidTime = TimeSpan.FromHours(1); // Кэш на 1 час

        public static async Task<List<OfficialOllamaModel>> GetOfficialModelsAsync(bool forceRefresh = false)
        {
            // Проверяем кэш
            if (!forceRefresh && cachedModels != null && DateTime.Now - lastFetchTime < cacheValidTime)
            {
                return cachedModels;
            }

            try
            {
                var response = await httpClient.GetStringAsync("https://ollama.com/models");
                var models = ParseModelsFromHtml(response);
                
                cachedModels = models;
                lastFetchTime = DateTime.Now;
                
                return models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Ollama models: {ex.Message}");
                
                // Возвращаем кэш если есть, иначе fallback список
                if (cachedModels != null)
                {
                    return cachedModels;
                }
                
                return GetFallbackModels();
            }
        }

        private static List<OfficialOllamaModel> ParseModelsFromHtml(string html)
        {
            var models = new List<OfficialOllamaModel>();
            
            try
            {
                // Извлекаем информацию о моделях из HTML
                var modelPattern = @"<a[^>]*href=""/([\w\.-]+)""[^>]*>.*?<span[^>]*>(.*?)</span>.*?(?:<span[^>]*>(.*?)</span>)?.*?(?:(\d+[\w\s]*)\s*Pulls)?";
                var matches = Regex.Matches(html, modelPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                
                foreach (Match match in matches)
                {
                    var name = match.Groups[1].Value.Trim();
                    var description = match.Groups[2].Value.Trim();
                    var pullCount = match.Groups[4].Success ? match.Groups[4].Value.Trim() : "";
                    
                    if (!string.IsNullOrEmpty(name) && !name.Contains("/") && name != "models")
                    {
                        // Извлекаем размеры модели из HTML
                        var sizes = ExtractModelSizes(html, name);
                        
                        // Определяем возможности модели по ключевым словам
                        var capabilities = new List<string>();
                        var tags = new List<string>();
                        
                        if (description.ToLower().Contains("vision") || name.Contains("vision") || name.Contains("llava"))
                        {
                            capabilities.Add("vision");
                            tags.Add("Vision");
                        }
                        if (description.ToLower().Contains("embedding") || name.Contains("embed"))
                        {
                            capabilities.Add("embedding");
                            tags.Add("Embedding");
                        }
                        if (description.ToLower().Contains("tool") || description.ToLower().Contains("function"))
                        {
                            capabilities.Add("tools");
                            tags.Add("Tools");
                        }
                        if (description.ToLower().Contains("reasoning") || name.Contains("r1") || name.Contains("qwq"))
                        {
                            capabilities.Add("thinking");
                            tags.Add("Thinking");
                        }
                        if (description.ToLower().Contains("code") || name.Contains("code"))
                        {
                            tags.Add("Code");
                        }
                        
                        models.Add(new OfficialOllamaModel
                        {
                            Name = name,
                            Description = description,
                            Tags = tags,
                            Capabilities = capabilities,
                            PullCount = pullCount,
                            UpdatedAt = DateTime.Now,
                            Sizes = sizes
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing models: {ex.Message}");
            }
            
            return models.OrderBy(m => m.Name).ToList();
        }

        private static List<string> ExtractModelSizes(string html, string modelName)
        {
            var sizes = new List<string>();
            
            // Ищем размеры в описании модели
            var sizePattern = @$"{Regex.Escape(modelName)}.*?(\d+[bm]|\d+\.?\d*[bm])";
            var matches = Regex.Matches(html, sizePattern, RegexOptions.IgnoreCase);
            
            foreach (Match match in matches)
            {
                var size = match.Groups[1].Value.ToLower();
                if (!sizes.Contains(size) && (size.EndsWith("b") || size.EndsWith("m")))
                {
                    sizes.Add(size);
                }
            }
            
            // Если размеры не найдены, добавляем стандартные
            if (sizes.Count == 0)
            {
                sizes.Add("latest");
            }
            
            return sizes.OrderBy(s => s).ToList();
        }

        private static List<OfficialOllamaModel> GetFallbackModels()
        {
            // Fallback список основных моделей если не удалось загрузить с сайта
            return new List<OfficialOllamaModel>
            {
                new() { Name = "deepseek-r1", Description = "DeepSeek-R1 reasoning model", Sizes = new List<string>{"1.5b", "7b", "8b", "14b", "32b", "70b", "671b"}, Capabilities = new List<string>{"tools", "thinking"} },
                new() { Name = "gemma3", Description = "Google Gemma 3 efficient model", Sizes = new List<string>{"1b", "4b", "12b", "27b"}, Capabilities = new List<string>{"vision"} },
                new() { Name = "qwen3", Description = "Qwen3 latest generation models", Sizes = new List<string>{"0.6b", "1.7b", "4b", "8b", "14b", "30b", "32b", "235b"}, Capabilities = new List<string>{"tools", "thinking"} },
                new() { Name = "llama3.1", Description = "Meta Llama 3.1 state-of-the-art", Sizes = new List<string>{"8b", "70b", "405b"}, Capabilities = new List<string>{"tools"} },
                new() { Name = "llama3.2", Description = "Meta Llama 3.2 small models", Sizes = new List<string>{"1b", "3b"}, Capabilities = new List<string>{"tools"} },
                new() { Name = "qwen2.5", Description = "Qwen2.5 multilingual models", Sizes = new List<string>{"0.5b", "1.5b", "3b", "7b", "14b", "32b", "72b"}, Capabilities = new List<string>{"tools"} },
                new() { Name = "qwen2.5-coder", Description = "Code-specific Qwen models", Sizes = new List<string>{"0.5b", "1.5b", "3b", "7b", "14b", "32b"}, Capabilities = new List<string>{"tools"} },
                new() { Name = "mistral", Description = "Mistral 7B model", Sizes = new List<string>{"7b"}, Capabilities = new List<string>{"tools"} },
                new() { Name = "gemma2", Description = "Google Gemma 2 efficient", Sizes = new List<string>{"2b", "9b", "27b"} },
                new() { Name = "llava", Description = "Vision and language understanding", Sizes = new List<string>{"7b", "13b", "34b"}, Capabilities = new List<string>{"vision"} },
                new() { Name = "nomic-embed-text", Description = "High-performing embedding model", Sizes = new List<string>{"latest"}, Capabilities = new List<string>{"embedding"} },
                new() { Name = "mxbai-embed-large", Description = "Large embedding model", Sizes = new List<string>{"335m"}, Capabilities = new List<string>{"embedding"} }
            };
        }

        public enum ModelSortType
        {
            Name,
            Popularity,
            Updated,
            Size
        }

        public static List<OfficialOllamaModel> SortModels(List<OfficialOllamaModel> models, ModelSortType sortType, bool ascending = true)
        {
            IEnumerable<OfficialOllamaModel> sorted = sortType switch
            {
                ModelSortType.Name => models.OrderBy(m => m.Name),
                ModelSortType.Popularity => models.OrderByDescending(m => ParsePullCount(m.PullCount)),
                ModelSortType.Updated => models.OrderByDescending(m => m.UpdatedAt),
                ModelSortType.Size => models.OrderBy(m => GetModelSizeOrder(m.Sizes.FirstOrDefault() ?? "")),
                _ => models.OrderBy(m => m.Name)
            };

            if (!ascending && sortType != ModelSortType.Popularity && sortType != ModelSortType.Updated)
            {
                sorted = sorted.Reverse();
            }

            return sorted.ToList();
        }

        private static long ParsePullCount(string pullCount)
        {
            if (string.IsNullOrEmpty(pullCount)) return 0;
            
            var number = Regex.Match(pullCount, @"([\d.]+)").Value;
            if (!double.TryParse(number, out var value)) return 0;
            
            if (pullCount.Contains("M"))
                return (long)(value * 1_000_000);
            if (pullCount.Contains("K"))
                return (long)(value * 1_000);
            
            return (long)value;
        }

        private static int GetModelSizeOrder(string size)
        {
            if (string.IsNullOrEmpty(size)) return int.MaxValue;
            
            var number = Regex.Match(size, @"([\d.]+)").Value;
            if (!double.TryParse(number, out var value)) return int.MaxValue;
            
            return size.ToLower() switch
            {
                var s when s.Contains("m") => (int)value, // мегабайты
                var s when s.Contains("b") => (int)(value * 1000), // гигабайты в мегабайты
                _ => int.MaxValue
            };
        }
    }

    public class ModelsClass : NotifyProperty
    {
        string _name;

        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        string _size;

        public string size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged("size");
            }
        }

        DateTime? _modifiedAt;

        public DateTime? modifiedAt
        {
            get => _modifiedAt;
            set
            {
                _modifiedAt = value;
                OnPropertyChanged("modifiedAt");
            }
        }

        CellTag[]? _families;

        public CellTag[]? families
        {
            get => _families;
            set
            {
                _families = value;
                OnPropertyChanged("families");
            }
        }

        CellTag[]? _status;

        public CellTag[]? status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        CellTag[]? _quantization;

        public CellTag[]? quantization
        {
            get => _quantization;
            set
            {
                _quantization = value;
                OnPropertyChanged("quantization");
            }
        }

        CellLink[]? _btns;

        public CellLink[]? btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged("btns");
            }
        }
    }

    public static class LocalizationManager
    {
        public enum Language
        {
            English,
            Russian,
            Chinese
        }

        public static Language CurrentLanguage { get; set; } = Language.Russian;

        private static Dictionary<string, Dictionary<Language, string>> translations = new Dictionary<string, Dictionary<Language, string>>
        {
            // Колонки таблицы
            {"name", new Dictionary<Language, string> { 
                {Language.English, "Name"}, 
                {Language.Russian, "Название"}, 
                {Language.Chinese, "名称"} 
            }},
            {"size", new Dictionary<Language, string> { 
                {Language.English, "Size"}, 
                {Language.Russian, "Размер"}, 
                {Language.Chinese, "容量"} 
            }},
            {"status", new Dictionary<Language, string> { 
                {Language.English, "Status"}, 
                {Language.Russian, "Статус"}, 
                {Language.Chinese, "状态"} 
            }},
            {"families", new Dictionary<Language, string> { 
                {Language.English, "Families"}, 
                {Language.Russian, "Семейство"}, 
                {Language.Chinese, "家族"} 
            }},
            {"quantization", new Dictionary<Language, string> { 
                {Language.English, "Format & Scale"}, 
                {Language.Russian, "Формат и масштаб"}, 
                {Language.Chinese, "格式与规模"} 
            }},
            {"modifiedAt", new Dictionary<Language, string> { 
                {Language.English, "Last modified"}, 
                {Language.Russian, "Последнее изменение"}, 
                {Language.Chinese, "上次修改"} 
            }},
            {"btns", new Dictionary<Language, string> { 
                {Language.English, "Actions"}, 
                {Language.Russian, "Действия"}, 
                {Language.Chinese, "操作"} 
            }},

            // Уведомления и сообщения
            {"core_not_installed", new Dictionary<Language, string> { 
                {Language.English, "Ollama core not installed"}, 
                {Language.Russian, "Ядро Ollama не установлено"}, 
                {Language.Chinese, "Ollama 核心未安装"} 
            }},
            {"please_install", new Dictionary<Language, string> { 
                {Language.English, "Please install Ollama service and try again."}, 
                {Language.Russian, "Пожалуйста, установите службу Ollama и повторите попытку."}, 
                {Language.Chinese, "请先安装 Ollama 服务，并稍候重试。"} 
            }},
            {"starting_service", new Dictionary<Language, string> { 
                {Language.English, "Starting Ollama service..."}, 
                {Language.Russian, "Запуск службы Ollama..."}, 
                {Language.Chinese, "正在启动 Ollama 服务…"} 
            }},
            {"confirm_download", new Dictionary<Language, string> { 
                {Language.English, "Are you sure you want to download the model?"}, 
                {Language.Russian, "Вы уверены, что хотите скачать модель?"}, 
                {Language.Chinese, "您确定要下载模型吗？"} 
            }},
            {"download", new Dictionary<Language, string> { 
                {Language.English, "Download"}, 
                {Language.Russian, "Скачать"}, 
                {Language.Chinese, "下载"} 
            }},
            {"completed", new Dictionary<Language, string> { 
                {Language.English, "Completed"}, 
                {Language.Russian, "Завершено"}, 
                {Language.Chinese, "已完成"} 
            }},
            {"check_connection", new Dictionary<Language, string> { 
                {Language.English, "Model download task unresponsive, check connection to ollama.com."}, 
                {Language.Russian, "Задача загрузки модели не отвечает, проверьте соединение с ollama.com."}, 
                {Language.Chinese, "模型下载任务无响应，请检查与 ollama.com 的连接。"} 
            }},
            {"download_completed", new Dictionary<Language, string> { 
                {Language.English, "Model download completed!"}, 
                {Language.Russian, "Загрузка модели завершена!"}, 
                {Language.Chinese, "模型已下载完成！"} 
            }},

            // Кнопки и действия
            {"copy_model", new Dictionary<Language, string> { 
                {Language.English, "Copy model"}, 
                {Language.Russian, "Копировать модель"}, 
                {Language.Chinese, "复制模型"} 
            }},
            {"copy", new Dictionary<Language, string> { 
                {Language.English, "Copy"}, 
                {Language.Russian, "Копирование"}, 
                {Language.Chinese, "复制"} 
            }},
            {"model_info", new Dictionary<Language, string> { 
                {Language.English, "Model info"}, 
                {Language.Russian, "Информация о модели"}, 
                {Language.Chinese, "模型信息"} 
            }},
            {"sleeping", new Dictionary<Language, string> { 
                {Language.English, "Sleeping"}, 
                {Language.Russian, "Неактивна"}, 
                {Language.Chinese, "休眠"} 
            }},
            {"active", new Dictionary<Language, string> { 
                {Language.English, "Active"}, 
                {Language.Russian, "Активна"}, 
                {Language.Chinese, "活动"} 
            }},
            {"permanent", new Dictionary<Language, string> { 
                {Language.English, "Permanent"}, 
                {Language.Russian, "Постоянно"}, 
                {Language.Chinese, "永久"} 
            }},
            {"minutes", new Dictionary<Language, string> { 
                {Language.English, "minutes"}, 
                {Language.Russian, "минут"}, 
                {Language.Chinese, "分钟"} 
            }},
            {"sleep_model", new Dictionary<Language, string> { 
                {Language.English, "Sleep model"}, 
                {Language.Russian, "Деактивировать модель"}, 
                {Language.Chinese, "休眠模型"} 
            }},
            {"warmup_model", new Dictionary<Language, string> { 
                {Language.English, "Warmup model"}, 
                {Language.Russian, "Активировать модель"}, 
                {Language.Chinese, "预热模型"} 
            }},

            // Настройки
            {"settings_language", new Dictionary<Language, string> { 
                {Language.English, "Interface language:"}, 
                {Language.Russian, "Язык интерфейса:"}, 
                {Language.Chinese, "界面语言:"} 
            }},
            {"language_english", new Dictionary<Language, string> { 
                {Language.English, "English"}, 
                {Language.Russian, "Английский"}, 
                {Language.Chinese, "英语"} 
            }},
            {"language_russian", new Dictionary<Language, string> { 
                {Language.English, "Russian"}, 
                {Language.Russian, "Русский"}, 
                {Language.Chinese, "俄语"} 
            }},
            {"language_chinese", new Dictionary<Language, string> { 
                {Language.English, "Chinese"}, 
                {Language.Russian, "Китайский"}, 
                {Language.Chinese, "中文"} 
            }},
            {"save_settings", new Dictionary<Language, string> { 
                {Language.English, "Are you sure you want to save settings?"}, 
                {Language.Russian, "Вы уверены, что хотите сохранить настройки?"}, 
                {Language.Chinese, "您确定要保存配置吗？"} 
            }},
            {"please_wait", new Dictionary<Language, string> { 
                {Language.English, "This will take some time, please wait..."}, 
                {Language.Russian, "Это займет некоторое время, пожалуйста, подождите..."}, 
                {Language.Chinese, "这需要一些时间，请稍等…"} 
            }},
            {"settings_changed", new Dictionary<Language, string> { 
                {Language.English, "Settings changed"}, 
                {Language.Russian, "Настройки изменены"}, 
                {Language.Chinese, "设置已更改"} 
            }},
            {"restart_core", new Dictionary<Language, string> { 
                {Language.English, "Ollama core has been exited, please restart it manually for changes to take effect."}, 
                {Language.Russian, "Ядро Ollama было закрыто, перезапустите его вручную для применения изменений."}, 
                {Language.Chinese, "Ollama 核心已退出，请手动重启 Ollama 核心以使配置生效。"} 
            }},

            // HuggingFace интеграция
            {"hf_import", new Dictionary<Language, string> { 
                {Language.English, "Import from HuggingFace"}, 
                {Language.Russian, "Импорт из HuggingFace"}, 
                {Language.Chinese, "从 HuggingFace 导入"} 
            }},
            {"hf_model_id", new Dictionary<Language, string> { 
                {Language.English, "HuggingFace Model ID:"}, 
                {Language.Russian, "ID модели HuggingFace:"}, 
                {Language.Chinese, "HuggingFace 模型 ID:"} 
            }},
            {"hf_search", new Dictionary<Language, string> { 
                {Language.English, "Search"}, 
                {Language.Russian, "Найти"}, 
                {Language.Chinese, "搜索"} 
            }},
            {"hf_download", new Dictionary<Language, string> { 
                {Language.English, "Download"}, 
                {Language.Russian, "Загрузить"}, 
                {Language.Chinese, "下载"} 
            }},
            {"please_select_file", new Dictionary<Language, string> { 
                {Language.English, "Please select a model file"}, 
                {Language.Russian, "Пожалуйста, выберите файл модели"}, 
                {Language.Chinese, "请选择模型文件"} 
            }},
            {"file_not_found", new Dictionary<Language, string> { 
                {Language.English, "File not found"}, 
                {Language.Russian, "Файл не найден"}, 
                {Language.Chinese, "文件未找到"} 
            }},
            {"importing_model", new Dictionary<Language, string> { 
                {Language.English, "Importing model"}, 
                {Language.Russian, "Импорт модели"}, 
                {Language.Chinese, "导入模型"} 
            }},
            {"import_complete", new Dictionary<Language, string> { 
                {Language.English, "Import completed successfully!"}, 
                {Language.Russian, "Импорт успешно завершен!"}, 
                {Language.Chinese, "导入成功完成！"} 
            }},
            {"import_error", new Dictionary<Language, string> { 
                {Language.English, "Import error"}, 
                {Language.Russian, "Ошибка импорта"}, 
                {Language.Chinese, "导入错误"} 
            }},
            {"select_model_file", new Dictionary<Language, string> { 
                {Language.English, "Select model file"}, 
                {Language.Russian, "Выберите файл модели"}, 
                {Language.Chinese, "选择模型文件"} 
            }},
            {"no_gguf_files", new Dictionary<Language, string> { 
                {Language.English, "No GGUF files found in this repository."}, 
                {Language.Russian, "GGUF файлы не найдены в этом репозитории."}, 
                {Language.Chinese, "在此存储库中未找到GGUF文件。"} 
            }},
            {"download_failed", new Dictionary<Language, string> { 
                {Language.English, "Download failed"}, 
                {Language.Russian, "Ошибка загрузки"}, 
                {Language.Chinese, "下载失败"} 
            }},
            {"invalid_gguf_format", new Dictionary<Language, string> { 
                {Language.English, "The selected file is not in GGUF format or is corrupted."}, 
                {Language.Russian, "Выбранный файл не в формате GGUF или поврежден."}, 
                {Language.Chinese, "所选文件不是GGUF格式或已损坏。"} 
            }},
            {"file_saved_to", new Dictionary<Language, string> { 
                {Language.English, "File saved to"}, 
                {Language.Russian, "Файл сохранен в"}, 
                {Language.Chinese, "文件保存到"} 
            }},
            {"copy_model_id", new Dictionary<Language, string> { 
                {Language.English, "Copy Model ID"}, 
                {Language.Russian, "Копировать ID модели"}, 
                {Language.Chinese, "复制模型ID"} 
            }},
            {"model_id_copied", new Dictionary<Language, string> { 
                {Language.English, "Model ID copied"}, 
                {Language.Russian, "ID модели скопирован"}, 
                {Language.Chinese, "模型ID已复制"} 
            }},
            {"copy_failed", new Dictionary<Language, string> { 
                {Language.English, "Copy failed"}, 
                {Language.Russian, "Ошибка копирования"}, 
                {Language.Chinese, "复制失败"} 
            }},
            {"download_speed", new Dictionary<Language, string> { 
                {Language.English, "Download speed"}, 
                {Language.Russian, "Скорость загрузки"}, 
                {Language.Chinese, "下载速度"} 
            }},
            {"remaining_time", new Dictionary<Language, string> { 
                {Language.English, "Remaining time"}, 
                {Language.Russian, "Осталось времени"}, 
                {Language.Chinese, "剩余时间"} 
            }},
            {"language_settings", new Dictionary<Language, string> { 
                {Language.English, "Language Settings"}, 
                {Language.Russian, "Настройки языка"}, 
                {Language.Chinese, "语言设置"} 
            }},
            {"select_language", new Dictionary<Language, string> { 
                {Language.English, "Select your preferred language:"}, 
                {Language.Russian, "Выберите предпочитаемый язык:"}, 
                {Language.Chinese, "选择您的首选语言："} 
            }},
            {"other_models", new Dictionary<Language, string> { 
                {Language.English, "Other Models"}, 
                {Language.Russian, "Другие модели"}, 
                {Language.Chinese, "其他模型"} 
            }},
            {"official_models", new Dictionary<Language, string> { 
                {Language.English, "Official Models"}, 
                {Language.Russian, "Официальные модели"}, 
                {Language.Chinese, "官方模型"} 
            }},
            {"sort_by_name", new Dictionary<Language, string> { 
                {Language.English, "Sort by Name"}, 
                {Language.Russian, "Сортировать по названию"}, 
                {Language.Chinese, "按名称排序"} 
            }},
            {"sort_by_popularity", new Dictionary<Language, string> { 
                {Language.English, "Sort by Popularity"}, 
                {Language.Russian, "Сортировать по популярности"}, 
                {Language.Chinese, "按热度排序"} 
            }},
            {"sort_by_updated", new Dictionary<Language, string> { 
                {Language.English, "Sort by Updated"}, 
                {Language.Russian, "Сортировать по обновлению"}, 
                {Language.Chinese, "按更新时间排序"} 
            }},
            {"sort_by_size", new Dictionary<Language, string> { 
                {Language.English, "Sort by Size"}, 
                {Language.Russian, "Сортировать по размеру"}, 
                {Language.Chinese, "按大小排序"} 
            }},
            {"loading_models", new Dictionary<Language, string> { 
                {Language.English, "Loading models..."}, 
                {Language.Russian, "Загрузка моделей..."}, 
                {Language.Chinese, "加载模型中..."} 
            }},
            {"refresh_models", new Dictionary<Language, string> { 
                {Language.English, "Refresh Models"}, 
                {Language.Russian, "Обновить модели"}, 
                {Language.Chinese, "刷新模型"} 
            }},
            {"ssl_error", new Dictionary<Language, string> { 
                {Language.English, "SSL connection error. Please check your internet connection or try again later."}, 
                {Language.Russian, "Ошибка SSL соединения. Проверьте интернет-соединение или попробуйте позже."}, 
                {Language.Chinese, "SSL连接错误。请检查您的互联网连接或稍后再试。"} 
            }},
            {"ssl_error_with_recommendation", new Dictionary<Language, string> { 
                {Language.English, "SSL connection error to HuggingFace.\n\nRecommendations:\n1. Check your internet connection\n2. Try downloading the model manually from HuggingFace website\n3. Use 'Import from file' tab to import downloaded model"}, 
                {Language.Russian, "Ошибка SSL соединения с HuggingFace.\n\nРекомендации:\n1. Проверьте интернет-соединение\n2. Попробуйте скачать модель вручную с сайта HuggingFace\n3. Используйте вкладку 'Файл модели' для импорта скачанной модели"}, 
                {Language.Chinese, "与HuggingFace的SSL连接错误。\n\n建议：\n1. 检查您的互联网连接\n2. 尝试从HuggingFace网站手动下载模型\n3. 使用'模型文件'标签导入下载的模型"} 
            }},

            // Обновленные и дополнительные переводы
            {"models", new Dictionary<Language, string> { 
                {Language.English, "Models"}, 
                {Language.Russian, "Модели"}, 
                {Language.Chinese, "模型"} 
            }},
            {"community", new Dictionary<Language, string> { 
                {Language.English, "Community"}, 
                {Language.Russian, "Сообщество"}, 
                {Language.Chinese, "社区"} 
            }},
            {"settings", new Dictionary<Language, string> { 
                {Language.English, "Settings"}, 
                {Language.Russian, "Настройки"}, 
                {Language.Chinese, "设置"} 
            }},
            {"save", new Dictionary<Language, string> { 
                {Language.English, "Save"}, 
                {Language.Russian, "Сохранить"}, 
                {Language.Chinese, "保存"} 
            }},
            {"open", new Dictionary<Language, string> { 
                {Language.English, "Open"}, 
                {Language.Russian, "Открыть"}, 
                {Language.Chinese, "更改"} 
            }},
            {"allow_remote_connections", new Dictionary<Language, string> { 
                {Language.English, "Allow LAN and external access"}, 
                {Language.Russian, "Разрешить доступ из локальной сети и извне"}, 
                {Language.Chinese, "允许局域网和外部访问"} 
            }},
            {"disable_history", new Dictionary<Language, string> { 
                {Language.English, "Disable conversation history"}, 
                {Language.Russian, "Отключить историю диалогов"}, 
                {Language.Chinese, "禁用历史对话"} 
            }},
            {"enable_flash_attention", new Dictionary<Language, string> { 
                {Language.English, "Enable Flash Attention"}, 
                {Language.Russian, "Включить Flash Attention"}, 
                {Language.Chinese, "启用 Flash Attention"} 
            }},
            {"enable_parallel_processing", new Dictionary<Language, string> { 
                {Language.English, "Allow more concurrent requests"}, 
                {Language.Russian, "Разрешить больше параллельных запросов"}, 
                {Language.Chinese, "允许更多对话并发请求"} 
            }},
            {"increase_loaded_models_limit", new Dictionary<Language, string> { 
                {Language.English, "Allow loading more models simultaneously"}, 
                {Language.Russian, "Разрешить загрузку большего количества моделей одновременно"}, 
                {Language.Chinese, "允许同时加载更多模型"} 
            }},
            {"disable_gpu", new Dictionary<Language, string> { 
                {Language.English, "Disable NVIDIA CUDA, AMD ROCm GPU acceleration"}, 
                {Language.Russian, "Отключить ускорение NVIDIA CUDA, AMD ROCm GPU"}, 
                {Language.Chinese, "禁用 NVIDIA CUDA、AMD ROCm 显卡加速"} 
            }},
            {"models_directory", new Dictionary<Language, string> { 
                {Language.English, "Models location"}, 
                {Language.Russian, "Расположение моделей"}, 
                {Language.Chinese, "模型位置"} 
            }},
            {"find_models_online", new Dictionary<Language, string> { 
                {Language.English, "Find models online"}, 
                {Language.Russian, "Найти модели онлайн"}, 
                {Language.Chinese, "在线查找模型"} 
            }},
            {"view_models_location", new Dictionary<Language, string> { 
                {Language.English, "View models location"}, 
                {Language.Russian, "Показать расположение моделей"}, 
                {Language.Chinese, "查看模型位置"} 
            }},
            {"view_logs", new Dictionary<Language, string> { 
                {Language.English, "View logs"}, 
                {Language.Russian, "Показать логи"}, 
                {Language.Chinese, "查看日志"} 
            }},
            {"check_updates", new Dictionary<Language, string> { 
                {Language.English, "Check for updates"}, 
                {Language.Russian, "Проверить обновления"}, 
                {Language.Chinese, "检查更新"} 
            }},
            {"no_models", new Dictionary<Language, string> { 
                {Language.English, "No models found"}, 
                {Language.Russian, "Модели не найдены"}, 
                {Language.Chinese, "未找到模型"} 
            }},
            {"load_models_error", new Dictionary<Language, string> { 
                {Language.English, "Failed to load model list"}, 
                {Language.Russian, "Ошибка загрузки списка моделей"}, 
                {Language.Chinese, "刷新模型列表失败"} 
            }},

            // HuggingFace специфичные
            {"huggingface", new Dictionary<Language, string> { 
                {Language.English, "HuggingFace"}, 
                {Language.Russian, "HuggingFace"}, 
                {Language.Chinese, "HuggingFace"} 
            }},
            {"huggingface_search", new Dictionary<Language, string> { 
                {Language.English, "Search on HuggingFace"}, 
                {Language.Russian, "Поиск на HuggingFace"}, 
                {Language.Chinese, "在 HuggingFace 上搜索"} 
            }},
            {"import_model", new Dictionary<Language, string> { 
                {Language.English, "Import Model"}, 
                {Language.Russian, "Импорт модели"}, 
                {Language.Chinese, "导入模型"} 
            }},
            {"model_file", new Dictionary<Language, string> { 
                {Language.English, "Model file"}, 
                {Language.Russian, "Файл модели"}, 
                {Language.Chinese, "模型文件"} 
            }},
            {"model_name", new Dictionary<Language, string> { 
                {Language.English, "Model name"}, 
                {Language.Russian, "Имя модели"}, 
                {Language.Chinese, "模型名称"} 
            }},
            {"model_type", new Dictionary<Language, string> { 
                {Language.English, "Model type"}, 
                {Language.Russian, "Тип модели"}, 
                {Language.Chinese, "模型类型"} 
            }},
            {"import", new Dictionary<Language, string> { 
                {Language.English, "Import"}, 
                {Language.Russian, "Импорт"}, 
                {Language.Chinese, "导入"} 
            }},
            {"change", new Dictionary<Language, string> { 
                {Language.English, "Change"}, 
                {Language.Russian, "Изменить"}, 
                {Language.Chinese, "更改"} 
            }},
            {"empty_fields", new Dictionary<Language, string> { 
                {Language.English, "Model file name and model name should not be empty!"}, 
                {Language.Russian, "Имя файла модели и имя модели не должны быть пустыми!"}, 
                {Language.Chinese, "模型文件名与模型名称不应为空！"} 
            }},
            {"confirm_import", new Dictionary<Language, string> { 
                {Language.English, "Are you sure you want to import the model?"}, 
                {Language.Russian, "Вы уверены, что хотите импортировать модель?"}, 
                {Language.Chinese, "您确定要导入模型吗？"} 
            }},
            {"no_quantization", new Dictionary<Language, string> { 
                {Language.English, "No quantization"}, 
                {Language.Russian, "Без квантизации"}, 
                {Language.Chinese, "不量化"} 
            }},
            {"downloading_from_hf", new Dictionary<Language, string> { 
                {Language.English, "Downloading from HuggingFace..."}, 
                {Language.Russian, "Загрузка с HuggingFace..."}, 
                {Language.Chinese, "从 HuggingFace 下载中..."} 
            }},
            {"download_complete", new Dictionary<Language, string> { 
                {Language.English, "Download complete"}, 
                {Language.Russian, "Загрузка завершена"}, 
                {Language.Chinese, "下载完成"} 
            }},
            {"preparing_model", new Dictionary<Language, string> { 
                {Language.English, "Preparing model..."}, 
                {Language.Russian, "Подготовка модели..."}, 
                {Language.Chinese, "准备模型中..."} 
            }},
            // Дополнительные переводы для элементов UI
            {"delete_confirm", new Dictionary<Language, string> { 
                {Language.English, "Are you sure you want to delete the model?"}, 
                {Language.Russian, "Вы уверены, что хотите удалить модель?"}, 
                {Language.Chinese, "您确定要删除模型吗？"} 
            }},
            {"delete", new Dictionary<Language, string> { 
                {Language.English, "Delete"}, 
                {Language.Russian, "Удалить"}, 
                {Language.Chinese, "删除"} 
            }},
            {"warmup_confirm", new Dictionary<Language, string> { 
                {Language.English, "Do you want to warm up the model?"}, 
                {Language.Russian, "Вы хотите активировать модель?"}, 
                {Language.Chinese, "要预热模型吗？"} 
            }},
            {"pin_confirm", new Dictionary<Language, string> { 
                {Language.English, "Do you want to pin the model?"}, 
                {Language.Russian, "Вы хотите закрепить модель?"}, 
                {Language.Chinese, "要固定模型吗？"} 
            }},
            {"sleep_confirm", new Dictionary<Language, string> { 
                {Language.English, "Do you want to sleep the model?"}, 
                {Language.Russian, "Вы хотите деактивировать модель?"}, 
                {Language.Chinese, "要休眠模型吗？"} 
            }},
            {"model_list_refresh", new Dictionary<Language, string> { 
                {Language.English, "Refresh model list"}, 
                {Language.Russian, "Обновить список моделей"}, 
                {Language.Chinese, "刷新模型列表"} 
            }},
            {"model_list_refreshed", new Dictionary<Language, string> { 
                {Language.English, "Model list refreshed"}, 
                {Language.Russian, "Список моделей обновлен"}, 
                {Language.Chinese, "刷新模型列表完成"} 
            }},
            {"remote_not_supported", new Dictionary<Language, string> { 
                {Language.English, "Remote management does not support this feature"}, 
                {Language.Russian, "Удаленное управление не поддерживает эту функцию"}, 
                {Language.Chinese, "远程管理暂不支持该功能"} 
            }},
            {"ollama_core_version", new Dictionary<Language, string> { 
                {Language.English, "Ollama core version"}, 
                {Language.Russian, "Версия ядра Ollama"}, 
                {Language.Chinese, "Ollama 核心版本"} 
            }},
            {"copy_url", new Dictionary<Language, string> { 
                {Language.English, "Copy URL"}, 
                {Language.Russian, "Копировать URL"}, 
                {Language.Chinese, "复制 URL"} 
            }},
            {"to_webchat", new Dictionary<Language, string> { 
                {Language.English, "Redirecting to Onllama WebChat"}, 
                {Language.Russian, "Перенаправление в Onllama WebChat"}, 
                {Language.Chinese, "已带您前往 Onllama WebChat"} 
            }},
            {"import_model_menu", new Dictionary<Language, string> { 
                {Language.English, "Import model"}, 
                {Language.Russian, "Импортировать модель"}, 
                {Language.Chinese, "导入模型"} 
            }},
            {"ollama_settings", new Dictionary<Language, string> { 
                {Language.English, "Ollama settings"}, 
                {Language.Russian, "Настройки Ollama"}, 
                {Language.Chinese, "Ollama 设置"} 
            }},
            {"openai_api", new Dictionary<Language, string> { 
                {Language.English, "OpenAI API"}, 
                {Language.Russian, "OpenAI API"}, 
                {Language.Chinese, "OpenAI API"} 
            }},
            {"error", new Dictionary<Language, string> { 
                {Language.English, "Error"}, 
                {Language.Russian, "Ошибка"}, 
                {Language.Chinese, "错误"} 
            }},
            {"downloads_count", new Dictionary<Language, string> { 
                {Language.English, "downloads"}, 
                {Language.Russian, "загрузок"}, 
                {Language.Chinese, "下载次数"} 
            }},
            {"cancel", new Dictionary<Language, string> { 
                {Language.English, "Cancel"}, 
                {Language.Russian, "Отмена"}, 
                {Language.Chinese, "取消"} 
            }},
            {"select_file", new Dictionary<Language, string> { 
                {Language.English, "Select file"}, 
                {Language.Russian, "Выбор файла"}, 
                {Language.Chinese, "选择文件"} 
            }},

            // Диалог информации о модели
            {"context_length", new Dictionary<Language, string> { 
                {Language.English, "Context length:"}, 
                {Language.Russian, "Длина контекста:"}, 
                {Language.Chinese, "上下文长度："} 
            }},
            {"embedding_length", new Dictionary<Language, string> { 
                {Language.English, "Embedding length:"}, 
                {Language.Russian, "Длина эмбеддинга:"}, 
                {Language.Chinese, "Embedding 长度："} 
            }},
            {"license", new Dictionary<Language, string> { 
                {Language.English, "License:"}, 
                {Language.Russian, "Лицензия:"}, 
                {Language.Chinese, "许可证："} 
            }},
            {"inference_params", new Dictionary<Language, string> { 
                {Language.English, "Inference parameters:"}, 
                {Language.Russian, "Параметры вывода:"}, 
                {Language.Chinese, "推理参数："} 
            }},
            {"template", new Dictionary<Language, string> { 
                {Language.English, "Template:"}, 
                {Language.Russian, "Шаблон:"}, 
                {Language.Chinese, "模板："} 
            }},
            
            // Диалог копирования
            {"new_model_name", new Dictionary<Language, string> { 
                {Language.English, "New model name"}, 
                {Language.Russian, "Новое имя модели"}, 
                {Language.Chinese, "新模型名称"} 
            }},
            {"ok", new Dictionary<Language, string> { 
                {Language.English, "OK"}, 
                {Language.Russian, "ОК"}, 
                {Language.Chinese, "确定"} 
            }},
            
            // Дополнительные переводы для сообщений об ошибках
            {"model_info_error", new Dictionary<Language, string> { 
                {Language.English, "Error loading model information"}, 
                {Language.Russian, "Ошибка при загрузке информации о модели"}, 
                {Language.Chinese, "加载模型信息时出错"} 
            }},
            {"unknown", new Dictionary<Language, string> { 
                {Language.English, "Unknown"}, 
                {Language.Russian, "Неизвестно"}, 
                {Language.Chinese, "未知"} 
            }},
            {"model_not_found", new Dictionary<Language, string> { 
                {Language.English, "Model not found"}, 
                {Language.Russian, "Модель не найдена"}, 
                {Language.Chinese, "模型未找到"} 
            }},
            {"connection_error", new Dictionary<Language, string> { 
                {Language.English, "Connection error"}, 
                {Language.Russian, "Ошибка соединения"}, 
                {Language.Chinese, "连接错误"} 
            }},

            // Новые строки для обновленного интерфейса
            {"app_started_log_message", new Dictionary<Language, string> {
                {Language.English, "Application started."},
                {Language.Russian, "Приложение запущено."},
                {Language.Chinese, "应用程序已启动。"}
            }},
            {"status_ready", new Dictionary<Language, string> {
                {Language.English, "Ready"},
                {Language.Russian, "Готов"},
                {Language.Chinese, "准备就绪"}
            }},
            {"settings_tooltip", new Dictionary<Language, string> {
                {Language.English, "Settings"},
                {Language.Russian, "Настройки"},
                {Language.Chinese, "设置"}
            }},
            {"tab_local_models", new Dictionary<Language, string> {
                {Language.English, "Installed"},
                {Language.Russian, "Установленные"},
                {Language.Chinese, "已安装"}
            }},
            {"tab_online_models", new Dictionary<Language, string> {
                {Language.English, "Online"},
                {Language.Russian, "Онлайн"},
                {Language.Chinese, "在线模型"}
            }},
            {"search_online_models_placeholder", new Dictionary<Language, string> {
                {Language.English, "Search online models..."},
                {Language.Russian, "Поиск онлайн моделей..."},
                {Language.Chinese, "搜索在线模型..."}
            }},
            {"button_pull_model", new Dictionary<Language, string> {
                {Language.English, "Download"},
                {Language.Russian, "Загрузить"},
                {Language.Chinese, "下载"}
            }},
            {"button_copy_log", new Dictionary<Language, string> {
                {Language.English, "Copy"},
                {Language.Russian, "Копировать"},
                {Language.Chinese, "复制"}
            }},
            {"tooltip_copy_log", new Dictionary<Language, string> {
                {Language.English, "Copy entire log"},
                {Language.Russian, "Копировать весь лог"},
                {Language.Chinese, "复制全部日志"}
            }},
            {"button_clear_log", new Dictionary<Language, string> {
                {Language.English, "Clear"},
                {Language.Russian, "Очистить"},
                {Language.Chinese, "清除"}
            }},
            {"tooltip_clear_log", new Dictionary<Language, string> {
                {Language.English, "Clear log"},
                {Language.Russian, "Очистить лог"},
                {Language.Chinese, "清除日志"}
            }},
            {"app_title_suffix", new Dictionary<Language, string> {
                {Language.English, "Model Manager"},
                {Language.Russian, "Менеджер моделей"},
                {Language.Chinese, "模型管理器"}
            }},
             {"core_not_installed_log", new Dictionary<Language, string> {
                {Language.English, "Ollama core not found or not in PATH."},
                {Language.Russian, "Ядро Ollama не найдено или отсутствует в PATH."},
                {Language.Chinese, "未找到Ollama核心或不在PATH中。"}
            }},
            {"starting_service_log", new Dictionary<Language, string> {
                {Language.English, "Ollama core not running. Attempting to start service..."},
                {Language.Russian, "Ядро Ollama не запущено. Попытка запуска службы..."},
                {Language.Chinese, "Ollama核心未运行。正在尝试启动服务..."}
            }},
            {"ollama_service_running_log", new Dictionary<Language, string> {
                {Language.English, "Ollama service is running."},
                {Language.Russian, "Служба Ollama запущена."},
                {Language.Chinese, "Ollama服务正在运行。"}
            }},
            {"remote_mode_active_log", new Dictionary<Language, string> {
                {Language.English, "Running in remote mode. Ollama service status on remote host is not checked locally."},
                {Language.Russian, "Работа в удаленном режиме. Статус службы Ollama на удаленном хосте локально не проверяется."},
                {Language.Chinese, "以远程模式运行。远程主机上的Ollama服务状态未在本地检查。"}
            }},
            {"log_copied_message", new Dictionary<Language, string> {
                {Language.English, "Log copied to clipboard."},
                {Language.Russian, "Лог скопирован в буфер обмена."},
                {Language.Chinese, "日志已复制到剪افظ板。"}
            }},
            {"log_cleared_message", new Dictionary<Language, string> {
                {Language.English, "Log cleared."},
                {Language.Russian, "Лог очищен."},
                {Language.Chinese, "日志已清除。"}
            }},
            {"source_ollama", new Dictionary<Language, string> {
                {Language.English, "Ollama"},
                {Language.Russian, "Ollama"},
                {Language.Chinese, "Ollama"}
            }},
            {"source_huggingface", new Dictionary<Language, string> {
                {Language.English, "HuggingFace"},
                {Language.Russian, "HuggingFace"},
                {Language.Chinese, "HuggingFace"}
            }},
            {"loading_models_placeholder", new Dictionary<Language, string> {
                {Language.English, "Loading models..."},
                {Language.Russian, "Загрузка моделей..."},
                {Language.Chinese, "正在加载模型..."}
            }},
            {"select_ollama_model_placeholder", new Dictionary<Language, string> {
                {Language.English, "Select Ollama model"},
                {Language.Russian, "Выберите модель Ollama"},
                {Language.Chinese, "选择Ollama模型"}
            }},
            {"select_hf_model_placeholder", new Dictionary<Language, string> {
                {Language.English, "Select HuggingFace GGUF model"},
                {Language.Russian, "Выберите модель HuggingFace GGUF"},
                {Language.Chinese, "选择HuggingFace GGUF模型"}
            }},
            {"load_models_error_placeholder", new Dictionary<Language, string> {
                {Language.English, "Error loading models"},
                {Language.Russian, "Ошибка загрузки моделей"},
                {Language.Chinese, "加载模型出错"}
            }},
            {"refresh_gguf_models", new Dictionary<Language, string> {
                {Language.English, "Refresh GGUF models"},
                {Language.Russian, "Обновить GGUF модели"},
                {Language.Chinese, "刷新GGUF模型"}
            }},
            {"please_select_model_to_download", new Dictionary<Language, string> {
                {Language.English, "Please select a model to download"},
                {Language.Russian, "Пожалуйста, выберите модель для загрузки"},
                {Language.Chinese, "请选择要下载的模型"}
            }},
            {"download_in_progress_warning", new Dictionary<Language, string> {
                {Language.English, "Another model is already downloading."},
                {Language.Russian, "Другая модель уже загружается."},
                {Language.Chinese, "另一个模型已在下载中。"}
            }},
            {"status_downloading", new Dictionary<Language, string> {
                {Language.English, "Downloading..."},
                {Language.Russian, "Загрузка..."},
                {Language.Chinese, "下载中..."}
            }},
            {"download_completed_check_list", new Dictionary<Language, string> {
                {Language.English, "Download complete, check the list."},
                {Language.Russian, "Загрузка завершена, проверьте список."},
                {Language.Chinese, "下载完成，请检查列表。"}
            }},
            {"download_cancelled_message", new Dictionary<Language, string> {
                {Language.English, "Download cancelled."},
                {Language.Russian, "Загрузка отменена."},
                {Language.Chinese, "下载已取消。"}
            }},
            {"download_error_title", new Dictionary<Language, string> {
                {Language.English, "Download Error"},
                {Language.Russian, "Ошибка загрузки"},
                {Language.Chinese, "下载错误"}
            }},
            {"url_copied_message", new Dictionary<Language, string> {
                {Language.English, "URL copied to clipboard!"},
                {Language.Russian, "URL скопирован в буфер обмена!"},
                {Language.Chinese, "URL已复制到剪افظ板！"}
            }},
            {"unknown_version", new Dictionary<Language, string> {
                {Language.English, "Unknown"},
                {Language.Russian, "Неизвестно"},
                {Language.Chinese, "未知"}
            }},
            {"open_releases_page", new Dictionary<Language, string> {
                {Language.English, "Open releases page"},
                {Language.Russian, "Открыть страницу релизов"},
                {Language.Chinese, "打开发布页面"}
            }},
            {"error_checking_updates", new Dictionary<Language, string> {
                {Language.English, "Error checking for updates."},
                {Language.Russian, "Ошибка проверки обновлений."},
                {Language.Chinese, "检查更新时出错。"}
            }},
            {"lang_prompt_english", new Dictionary<Language, string> {
                {Language.English, "English"},
                {Language.Russian, "Английский"},
                {Language.Chinese, "英语"}
            }},
            {"lang_prompt_russian", new Dictionary<Language, string> {
                {Language.English, "Russian"},
                {Language.Russian, "Русский"},
                {Language.Chinese, "俄语"}
            }},
            {"lang_prompt_chinese", new Dictionary<Language, string> {
                {Language.English, "Chinese"},
                {Language.Russian, "Китайский"},
                {Language.Chinese, "中文"}
            }},
            {"lang_prompt_current", new Dictionary<Language, string> {
                {Language.English, "Current language"},
                {Language.Russian, "Текущий язык"},
                {Language.Chinese, "当前语言"}
            }},
            {"language_changed_message", new Dictionary<Language, string> {
                {Language.English, "Language changed to"},
                {Language.Russian, "Язык изменен на"},
                {Language.Chinese, "语言已更改为"}
            }},
        };

        public static string GetTranslation(string key)
        {
            if (translations.ContainsKey(key) && translations[key].ContainsKey(CurrentLanguage))
            {
                return translations[key][CurrentLanguage];
            }
            
            // Если перевод не найден, возвращаем ключ
            return key;
        }

        public static void ApplyTranslations(Form form)
        {
            // Применяем переводы к форме и её элементам
            form.Text = GetTranslation(form.Text) ?? form.Text;

            foreach (Control control in form.Controls)
            {
                ApplyTranslationToControl(control);
            }
        }

        private static void ApplyTranslationToControl(Control control)
        {
            // Применяем переводы к тексту контрола
            if (!string.IsNullOrEmpty(control.Text))
            {
                string translation = GetTranslation(control.Text);
                if (translation != control.Text)
                {
                    control.Text = translation;
                }
            }

            // Для кнопок AntdUI
            if (control is AntdUI.Button button)
            {
                string translation = GetTranslation(button.Text);
                if (translation != button.Text)
                {
                    button.Text = translation;
                }
            }
            
            // Для меток AntdUI
            if (control is AntdUI.Label label)
            {
                string translation = GetTranslation(label.Text);
                if (translation != label.Text)
                {
                    label.Text = translation;
                }
            }
            
            // Для чекбоксов AntdUI
            if (control is AntdUI.Checkbox checkbox)
            {
                string translation = GetTranslation(checkbox.Text);
                if (translation != checkbox.Text)
                {
                    checkbox.Text = translation;
                }
            }

            // Рекурсивно обрабатываем дочерние контролы
            foreach (Control child in control.Controls)
            {
                ApplyTranslationToControl(child);
            }
        }

        public static void SaveLanguagePreference()
        {
            Environment.SetEnvironmentVariable("OLLAMA_LANGUAGE", 
                CurrentLanguage.ToString(), 
                EnvironmentVariableTarget.User);
        }

        public static void LoadLanguagePreference()
        {
            string savedLanguage = Environment.GetEnvironmentVariable("OLLAMA_LANGUAGE", EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(savedLanguage) && Enum.TryParse<Language>(savedLanguage, out var language))
            {
                CurrentLanguage = language;
            }
        }
    }
}
