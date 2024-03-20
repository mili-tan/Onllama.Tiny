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
            Parallel.Invoke(() => Environment.SetEnvironmentVariable("OLLAMA_MODELS",
                    !string.IsNullOrWhiteSpace(input1.Text) ? input1.Text : null, EnvironmentVariableTarget.User),
                () => Environment.SetEnvironmentVariable("OLLAMA_HOST",
                    checkboxAny.Checked ? "0.0.0.0" : null, EnvironmentVariableTarget.User),
                () => Environment.SetEnvironmentVariable("CUDA_VISIBLE_DEVICES",
                    checkboxNoGpu.Checked ? "-1" : null, EnvironmentVariableTarget.User),
                () => Environment.SetEnvironmentVariable("HIP_VISIBLE_DEVICES",
                    checkboxNoGpu.Checked ? "-1" : null, EnvironmentVariableTarget.User)
                //() => Kill("ollama app"),
                //() => Kill("ollama")
            );
            new Modal.Config(this, "设置已更改", "请手动重启 Ollama 核心以使配置生效。", TType.Info)
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
