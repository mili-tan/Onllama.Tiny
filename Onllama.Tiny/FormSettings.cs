using System.Diagnostics;
using AntdUI;

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
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            var f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK) input1.Text = f.SelectedPath;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            new Modal.Config(this, "您确定要保存配置吗？","这需要一些时间，请稍等…",
                TType.Info)
            {
                OnOk = _ =>
                {
                    try
                    {
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
            new Modal.Config(this, "设置已更改", "Ollama 核心已退出，请手动重启 Ollama 核心以使配置生效。", TType.Success)
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
