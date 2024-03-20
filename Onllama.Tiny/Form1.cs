using System.Diagnostics;
using System.Net.NetworkInformation;
using AntdUI;
using OllamaSharp;

namespace Onllama.Tiny
{
    public partial class Form1 : BaseForm
    {
        public static OllamaApiClient OllamaApi = new("http://127.0.0.1:11434");

        public Form1()
        {
            InitializeComponent();
            table1.Columns = new Column[]
            {
                new("name", "模型名称"),
                new("size", "模型大小"),
                new("modifiedAt", "上次修改"),
                new("families", "家族"),
                new("quantization", "格式与规模"),
                new("btns", "操作") {Fixed = true},
            };
            dropdown1.Items.Add(new SelectItem("Ollama") {Sub = new List<object> {"NextChat", "查看日志", "查找模型", "检查更新"}});
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var ollamaPath = (Environment.GetEnvironmentVariable("PATH")
                    ?.Split(';')
                    .Select(x => Path.Combine(x, "ollama app.exe"))!)
                .FirstOrDefault(File.Exists);

            try
            {
                Environment.SetEnvironmentVariable("OLLAMA_ORIGINS", "*", EnvironmentVariableTarget.User);
                progress1.Hide();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            if (string.IsNullOrWhiteSpace(ollamaPath))
            {
                Notification.warn(this, "Ollama 核心未安装", "请先安装 Ollama 服务，并稍候重试。");
                Process.Start(new ProcessStartInfo($"https://ollama.com/download/windows") {UseShellExecute = true});
                Process.Start(new ProcessStartInfo($"https://github.com/ollama/ollama/releases/latest")
                    {UseShellExecute = true});
                panel1.Enabled = false;
            }
            else if (!PortIsUse(11434))
            {
                //Notification.info(this, "Ollama 核心未在运行", "正在启动 Ollama 服务，请稍等…");
                AntdUI.Message.info(this, "正在启动 Ollama 服务…");
                Process.Start(ollamaPath, "serve");
            }

            ListModels();
        }

        private void table1_CellButtonClick(object sender, CellLink btn, MouseEventArgs args, object record,
            int rowIndex, int columnIndex)
        {
            if (record is ModelsClass data)
            {
                if (btn.Id == "delete")
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(this, "您确定要删除模型吗？",
                        new[]
                        {
                            new Modal.TextLine(data.name, Style.Db.Primary),
                            new Modal.TextLine(data.size, Style.Db.TextSecondary)
                        }, TType.Warn)
                    {
                        OkType = TTypeMini.Error,
                        OkText = "删除",
                        OnOk = _ =>
                        {
                            Task.Run(async () => await OllamaApi.DeleteModel(data.name)).Wait();
                            Invoke(ListModels);
                            return true;
                        }
                    });
                }
                else if (btn.Id == "web-chat")
                {
                    AntdUI.Message.success(this, "已带您前往 Ollama GUI");
                    Process.Start(new ProcessStartInfo($"https://ollama-gui.vercel.app") {UseShellExecute = true});
                }
                else if (btn.Id == "copy")
                {
                    new FormCopy(data.name).ShowDialog();
                    ListModels();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (select1.Text.Contains(" ")) select1.Text = select1.Text.Split(' ').Last();
            AntdUI.Modal.open(new AntdUI.Modal.Config(this, "您确定要下载模型吗？",
                new[]
                {
                    new Modal.TextLine(select1.Text, Style.Db.Primary)
                }, TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = "下载",
                OnOk = _ =>
                {
                    Invoke(progress1.Show);
                    Task.Run(() => OllamaApi.PullModel(select1.Text, (x) =>
                    {
                        Invoke(() =>
                        {
                            progress1.Value = (float) x.Completed / x.Total;
                            Text = "Onllama - " + x.Status;
                            if (string.IsNullOrEmpty(x.Status))
                            {
                                Notification.info(this, "已完成", "模型下载任务无响应，请检查与 ollama.com 的连接。");
                                Text = "Onllama - Models";
                                progress1.Hide();
                            }
                            else if (x.Status == "success")
                            {
                                Notification.success(this, "已完成", "模型已下载完成！");
                                Text = "Onllama - Models";
                                progress1.Hide();
                                ListModels();
                            }
                        });
                    }));
                    return true;
                }
            });
        }

        public void ListModels()
        {
            try
            {
                var modelsClasses = new List<ModelsClass>();
                var models = Task.Run(async () => await OllamaApi.ListLocalModels()).Result;
                foreach (var item in models)
                {
                    var quartList = new List<CellTag>()
                    {
                        new(item.Details.Format.ToUpper(), TTypeMini.Default),
                        new(item.Details.ParameterSize.ToUpper(), TTypeMini.Success),
                        new(item.Details.QuantizationLevel.ToUpper(), TTypeMini.Warn)
                    };
                    var btnList = new List<CellButton>
                    {
                        new("delete", "删除", TTypeMini.Error)
                            {Ghost = true, BorderWidth = 1},
                        new("copy", "复制", TTypeMini.Success)
                            {Ghost = true, BorderWidth = 1}
                    };
                    if (item.Details.Family != "bert")
                    {
                        btnList.AddRange(new[]
                        {
                            new CellButton("web-chat", "WebUI", TTypeMini.Default)
                                {Ghost = true, BorderWidth = 1},
                        });
                    }

                    btnList.Reverse();

                    modelsClasses.Add(new ModelsClass()
                    {
                        name = item.Name,
                        size = (item.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.00") + "G",
                        modifiedAt = item.ModifiedAt,
                        families = item.Details.Families.Select(x => new CellTag(x.ToUpper(), TTypeMini.Info)).ToArray(),
                        quantization = quartList.ToArray(),
                        btns = btnList.ToArray()
                    });
                }

                table1.DataSource = modelsClasses;
            }
            catch (Exception e)
            {
                AntdUI.Message.error(this, "刷新模型列表失败：" + e.Message);
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

            DateTime _modifiedAt;

            public DateTime modifiedAt
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

        private void dropdown1_SelectedValueChanged(object sender, object value)
        {
            if (value.ToString() == "导入模型")
            {
                new FormImport().ShowDialog();
                ListModels();
            }
            else if (value.ToString() == "Ollama 设置")
            {
                new FormSettings().ShowDialog();
            }
            else if (value.ToString() == "刷新模型列表")
            {
                ListModels();
                AntdUI.Message.success(this, "刷新模型列表完成");
            }
            else if (value.ToString() == "NextChat")
            {
                Process.Start(
                    new ProcessStartInfo(
                            $"https://app.nextchat.dev/#/?settings={{%22url%22:%22http://127.0.0.1:11434%22}}")
                        {UseShellExecute = true});
            }
            else if (value.ToString() == "查找模型")
            {
                Process.Start(new ProcessStartInfo($"https://ollama.com/library") {UseShellExecute = true});
            }
            else if (value.ToString() == "查看日志")
            {
                Process.Start(new ProcessStartInfo($"explorer.exe",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Ollama\\"));
            }
            else if (value.ToString() == "检查更新")
            {
                var process = Process.Start(new ProcessStartInfo("ollama.exe", "-v")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                });
                process.WaitForExit();
                var version = process.StandardOutput.ReadToEnd();
                AntdUI.Modal.open(new AntdUI.Modal.Config(this, "Ollama 核心版本", version, TType.Info)
                {
                    OnOk = _ =>
                    {
                        Process.Start(new ProcessStartInfo($"https://github.com/ollama/ollama/releases/latest")
                            {UseShellExecute = true});
                        return true;
                    }
                });
            }
        }
    }
}
