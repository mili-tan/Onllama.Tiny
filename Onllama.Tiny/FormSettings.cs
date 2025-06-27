using System.Diagnostics;
using AntdUI;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace Onllama.Tiny
{
    public partial class FormSettings : BaseForm
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            // Загружаем языковые настройки
            LocalizationManager.LoadLanguagePreference();
            
            // UI элементы для выбора языка
            label1.Text = LocalizationManager.GetTranslation("settings_language");
            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.Add(LocalizationManager.GetTranslation("language_english"));
            comboBoxLanguage.Items.Add(LocalizationManager.GetTranslation("language_russian"));
            comboBoxLanguage.Items.Add(LocalizationManager.GetTranslation("language_chinese"));
            comboBoxLanguage.SelectedIndex = (int)LocalizationManager.CurrentLanguage;

            input1.PlaceholderText =
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.ollama\\models";
            input1.Text = Environment.GetEnvironmentVariable("OLLAMA_MODELS", EnvironmentVariableTarget.User) ?? "";
            checkboxAny.Checked = (Environment.GetEnvironmentVariable("OLLAMA_HOST", EnvironmentVariableTarget.User) ?? "").Equals("0.0.0.0");
            checkboxNoHistory.Checked = (Environment.GetEnvironmentVariable("OLLAMA_NOHISTORY", EnvironmentVariableTarget.User) ?? "").Equals("1");
            checkboxFlashAttention.Checked = (Environment.GetEnvironmentVariable("OLLAMA_FLASH_ATTENTION", EnvironmentVariableTarget.User) ?? "").Equals("1");
            checkboxPara.Checked = (Environment.GetEnvironmentVariable("OLLAMA_NUM_PARALLEL", EnvironmentVariableTarget.User) ?? "1") != "1";
            checkboxModels.Checked = (Environment.GetEnvironmentVariable("OLLAMA_MAX_LOADED_MODELS", EnvironmentVariableTarget.User) ?? "1") != "1";
            checkboxNoGpu.Checked = (Environment.GetEnvironmentVariable("CUDA_VISIBLE_DEVICES", EnvironmentVariableTarget.User) ?? "").Equals("-1") ||
                                    (Environment.GetEnvironmentVariable("HIP_VISIBLE_DEVICES", EnvironmentVariableTarget.User) ?? "").Equals("-1");
            
            // Применяем переводы к элементам управления
            UpdateUITexts();
        }

        private void UpdateUITexts()
        {
            // Обновляем тексты на форме в соответствии с выбранным языком
            this.Text = LocalizationManager.GetTranslation("settings");
            buttonSave.Text = LocalizationManager.GetTranslation("save");
            buttonOpen.Text = LocalizationManager.GetTranslation("open");
            
            // Другие элементы управления...
            checkboxAny.Text = LocalizationManager.GetTranslation("allow_remote_connections");
            checkboxNoHistory.Text = LocalizationManager.GetTranslation("disable_history");
            checkboxFlashAttention.Text = LocalizationManager.GetTranslation("enable_flash_attention");
            checkboxPara.Text = LocalizationManager.GetTranslation("enable_parallel_processing");
            checkboxModels.Text = LocalizationManager.GetTranslation("increase_loaded_models_limit");
            checkboxNoGpu.Text = LocalizationManager.GetTranslation("disable_gpu");
            label2.Text = LocalizationManager.GetTranslation("models_directory");
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Обновляем выбранный язык и применяем переводы
            LocalizationManager.CurrentLanguage = (LocalizationManager.Language)comboBoxLanguage.SelectedIndex;
            UpdateUITexts();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            var f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK) input1.Text = f.SelectedPath;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            new Modal.Config(this, LocalizationManager.GetTranslation("save_settings"), 
                    LocalizationManager.GetTranslation("please_wait"),
                    TType.Info)
            {
                OnOk = _ =>
                {
                    try
                    {
                        // Сохраняем языковые настройки
                        LocalizationManager.SaveLanguagePreference();
                        
                        Parallel.Invoke(() => Environment.SetEnvironmentVariable("OLLAMA_MODELS",
                                !string.IsNullOrWhiteSpace(input1.Text) ? input1.Text : null,
                                EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("OLLAMA_HOST",
                                checkboxAny.Checked ? "0.0.0.0" : null, EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("OLLAMA_NOHISTORY",
                                checkboxNoHistory.Checked ? "1" : null, EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("OLLAMA_FLASH_ATTENTION",
                                checkboxFlashAttention.Checked ? "1" : null, EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("CUDA_VISIBLE_DEVICES",
                                checkboxNoGpu.Checked ? " " : null, EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("HIP_VISIBLE_DEVICES",
                                checkboxNoGpu.Checked ? " " : null, EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("OLLAMA_NUM_PARALLEL",
                                checkboxPara.Checked ? "32" : "1", EnvironmentVariableTarget.User),
                            () => Environment.SetEnvironmentVariable("OLLAMA_MAX_LOADED_MODELS",
                                checkboxModels.Checked ? "8" : "1", EnvironmentVariableTarget.User),
                            () => Kill("ollama app"),
                            () => Kill("ollama")
                        );
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                    return true;
                }
            }.open();
            new Modal.Config(this, LocalizationManager.GetTranslation("settings_changed"), 
                LocalizationManager.GetTranslation("restart_core"), TType.Success)
            {
                OnOk = _ =>
                {
                    Invoke(Close);
                    return true;
                }
            }.open();
        }

        private void Kill(string name)
        {
            foreach (var process in Process.GetProcessesByName(name)) process.Kill();
        }
    }
}
