using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using AntdUI;
using Microsoft.VisualBasic.Devices;
using OllamaSharp;
using OllamaSharp.Models;

namespace Onllama.Tiny
{
    public partial class Form1 : BaseForm
    {
        public static Uri OllamaUri = new Uri("http://127.0.0.1:11434");
        public static OllamaApiClient OllamaApi = new(OllamaUri);

        public Form1()
        {
            InitializeComponent();

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
                OllamaUri = new Uri(Environment.GetEnvironmentVariable("OLLAMA_API") ?? OllamaUri.ToString());
                OllamaApi = new OllamaApiClient(OllamaUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            table1.Columns = new ColumnCollection([
                new("name", "名称"),
                new("size", "容量"),
                new("status", "状态"),
                new("families", "家族"),
                new("quantization", "格式与规模"),
                new("modifiedAt", "上次修改"),
                new("btns", "操作") {Fixed = true}
            ]);
            dropdown1.Items.Add(new SelectItem("Ollama")
            { Sub = new List<object> { "NextChat", "OpenAI 兼容 API", "在线查找模型", "查看模型位置", "查看日志", "检查更新" } });
            select1.Items.Add(new SelectItem("社区")
            {
                Sub = new List<object>
                {
                    "milkey/bilibili-index:1.9b-chat-q8_0", "milkey/bilibili-index:1.9b-character-q8_0",
                    "roger/minicpm:latest", "onekuma/sakura-13b-lnovel-v0.9b-q2_k"
                }
            });
            select1.Items.Add(new SelectItem("Embed")
            {
                Sub = new List<object>
                {
                    "znbang/bge:large-zh-v1.5-f16", "znbang/bge:large-en-v1.5-f16",
                    "milkey/m3e:large-f16", "milkey/gte:large-zh-f16",
                    "nomic-embed-text", "mxbai-embed-large"
                }
            });
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

            var ollamaPath = (Environment.GetEnvironmentVariable("PATH")
                    ?.Split(';')
                    .Select(x => Path.Combine(x, "ollama app.exe"))!)
                .FirstOrDefault(File.Exists);

            if (string.IsNullOrWhiteSpace(ollamaPath))
            {
                Notification.warn(this, "Ollama 核心未安装", "请先安装 Ollama 服务，并稍候重试。");
                Process.Start(new ProcessStartInfo($"https://ollama.com/download/windows") { UseShellExecute = true });
                Process.Start(new ProcessStartInfo($"https://github.com/ollama/ollama/releases/latest")
                { UseShellExecute = true });
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (select1.Text.Contains(" ")) select1.Text = select1.Text.Split(' ').Last();
            new Modal.Config(this, "您确定要下载模型吗？", new[] { new Modal.TextLine(select1.Text, Style.Db.Primary) }, TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = "下载",
                OnOk = _ =>
                {
                    Invoke(progress1.Show);

                    Task.Run(async () =>
                    {
                        await foreach (var x in OllamaApi.PullModelAsync(select1.Text))
                        {
                            Invoke(() =>
                            {
                                var textInfo = new CultureInfo("en-US", false).TextInfo;
                                progress1.Value = (float)x.Completed / x.Total;
                                Text = "Onllama - " + textInfo.ToTitleCase(x.Status) + " " + x.Percent.ToString("0.00") + "%";
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
                if (models.Any())
                    foreach (var item in models)
                    {
                        var isRunning = runningModels.Any(x => x.Name == item.Name);
                        var isEmbed = (item.Details.Family ?? string.Empty).ToLower().EndsWith("bert");
                        var statusList = new List<CellTag>();
                        var quartList = item.Details != null
                            ? new List<CellTag>
                            {
                                new((item.Details.ParameterSize ?? "Unknown").ToUpper(), TTypeMini.Success),
                                new((item.Details.QuantizationLevel ?? "Unknown").ToUpper(), TTypeMini.Warn),
                                new((item.Details.Format ?? "Unknown").ToUpper(), TTypeMini.Default)
                            }
                            : new List<CellTag>();

                        var btnList = new List<CellButton>
                        {
                            new("copy", null, TTypeMini.Success)
                                {Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgCopy},
                            new("info", null, TTypeMini.Success)
                            {
                                Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgInfo,
                                Back = Color.FromArgb(24, 188, 156), BackHover = Color.FromArgb(105, 211, 191)
                            },
                        };

                        if (isRunning)
                        {
                            //statusList.Add(new CellTag("活动", TTypeMini.Primary));

                            var runModel = runningModels.First(x => x.Name == item.Name);
                            var expires = runModel.ExpiresAt;
                            statusList.Add(new CellTag((runModel.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.0") + "G", TTypeMini.Primary));
                            if (runModel.SizeVram != 0) statusList.Add(new CellTag((runModel.SizeVram / 1024.00 / 1024.00 / 1024.00).ToString("0.0") + "G (GPU)", TTypeMini.Warn));
                            statusList.Add(new CellTag(
                                expires.Year > 2300
                                    ? "永久"
                                    : (expires - DateTime.Now).TotalMinutes.ToString("0.0") + " 分钟",
                                TTypeMini.Success));
                            btnList.Insert(0,
                                new("pin", null, TTypeMini.Warn)
                                { Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgPin });
                        }
                        else
                        {
                            statusList.Add(new CellTag("休眠", TTypeMini.Default));
                            btnList.Insert(0,
                                new("delete", null, TTypeMini.Error)
                                { Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgDel });
                        }

                        if (item.Details != null && !isEmbed)
                        {
                            btnList.AddRange(new[]
                            {
                                isRunning
                                    ? new CellButton("sleep", null, TTypeMini.Primary)
                                    {
                                        Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgSnow,
                                        //Back = Color.FromArgb(30, 136, 229), BackHover = Color.FromArgb(12, 129, 224)
                                    }
                                    : new CellButton("run", null, TTypeMini.Warn)
                                    {
                                        Ghost = false, BorderWidth = 1, IconSvg = Properties.Resources.svgWarm,
                                        //Back = Color.FromArgb(255, 152, 0), BackHover = Color.FromArgb(230, 147, 0)
                                    },
                                new CellButton("web-chat", "Web", TTypeMini.Default)
                                    {Ghost = false, BorderWidth = 1}
                            });
                        }

                        btnList.Reverse();
                        modelsClasses.Add(new ModelsClass
                        {
                            name = item.Name ?? "Empty",
                            size = (item.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.00") + "G", // ?? 0
                            modifiedAt = item.ModifiedAt, //?? DateTime.MinValue
                            families = item.Details != null
                                ? item.Details.Families != null
                                    ? item.Details.Families.Distinct()
                                        .Select(x => new CellTag(x.ToUpper(), TTypeMini.Info)).ToArray()
                                    : new CellTag[] { new(item.Details.Family?.ToUpper()!, TTypeMini.Info) }
                                : new CellTag[] { },
                            status = statusList.ToArray(),
                            quantization = quartList.ToArray(),
                            btns = btnList.ToArray()
                        });
                    }
                else
                {
                    modelsClasses.Add(new ModelsClass
                    {
                        name = "❓ 未找到模型…"
                    });
                }

                table1.DataSource = modelsClasses;
            }
            catch (Exception e)
            {
                AntdUI.Message.error(this, "刷新模型列表失败：" + e.Message);
                table1.DataSource = new[]
                {
                    new ModelsClass
                    {
                        name = "❌ 加载模型列表失败"
                    }
                };
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
            switch (e.Value.ToString())
            {
                case "导入模型":
                    new FormImport().ShowDialog();
                    ListModels();
                    break;
                case "Ollama 设置":
                    new FormSettings().ShowDialog();
                    break;
                case "刷新模型列表":
                    ListModels();
                    AntdUI.Message.success(this, "刷新模型列表完成");
                    break;
                case "NextChat":
                    Process.Start(
                        new ProcessStartInfo(
                                $"https://app.nextchat.dev/#/?settings={{%22url%22:%22http://127.0.0.1:11434%22}}")
                        { UseShellExecute = true });
                    break;
                case "在线查找模型":
                    Process.Start(new ProcessStartInfo($"https://ollama.com/library") { UseShellExecute = true });
                    break;
                case "查看模型位置":
                    Process.Start(new ProcessStartInfo($"explorer.exe",
                        Environment.GetEnvironmentVariable("OLLAMA_MODELS", EnvironmentVariableTarget.User) ??
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.ollama\\models"
                    ));
                    break;
                case "查看日志":
                    Process.Start(new ProcessStartInfo($"explorer.exe",
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Ollama\\"));
                    break;
                case "检查更新":
                    {
                        new Modal.Config(this, "Ollama 核心版本", Task.Run(() => OllamaApi.GetVersionAsync()).Result, TType.Info)
                        {
                            OnOk = _ =>
                            {
                                Process.Start(new ProcessStartInfo($"https://github.com/ollama/ollama/releases/latest")
                                { UseShellExecute = true });
                                return true;
                            }
                        }.open();
                        break;
                    }
                case "OpenAI 兼容 API":
                    {
                        new Modal.Config(this, "OpenAI 兼容 API",
                            new[]
                            {
                            new Modal.TextLine("API: " + OllamaUri + "v1", Style.Db.Primary),
                            new Modal.TextLine("Chat: " + OllamaUri + "v1/chat/completions"),
                            new Modal.TextLine("Completions: " + OllamaUri + "v1/completions"),
                            new Modal.TextLine("Embeddings: " + OllamaUri + "v1/embeddings")
                            }, TType.Info)
                        {
                            OkText = "复制 URL",
                            OnOk = _ =>
                            {
                                Thread.Sleep(1);
                                try
                                {
                                    Invoke(() => Clipboard.SetText(OllamaUri + "v1"));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                return true;
                            }
                        }.open();
                        break;
                    }
            }
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            select1.Width += (flowLayoutPanel1.Width - select1.Width - button1.Width - button2.Width - dropdown1.Width -
                              50);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (select1.Text.Contains(" ")) select1.Text = select1.Text.Split(' ').Last();
            new FormRegistryInfo(select1.Text).ShowDialog();
        }

        private void Table1OnCellButtonClick(object sender, TableButtonEventArgs e)
        {
            var record = e.Record;
            var btn = e.Btn;

            if (record is not ModelsClass data) return;
            switch (btn.Id)
            {
                case "delete":
                    new Modal.Config(this, "您确定要删除模型吗？",
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
                            Task.Run(async () => await OllamaApi.DeleteModelAsync(data.name)).Wait();
                            Invoke(() => ListModels());
                            return true;
                        }
                    }.open();
                    break;
                case "web-chat":
                    AntdUI.Message.success(this, "已带您前往 Onllama WebChat");
                    Process.Start(new ProcessStartInfo($"https://onllama.netlify.app/") { UseShellExecute = true });
                    break;
                case "copy":
                    new FormCopy(data.name).ShowDialog();
                    ListModels();
                    break;
                case "run":
                    new Modal.Config(this, "要预热模型吗？", data.name, TType.Info)
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
                    new Modal.Config(this, "要固定模型吗？", data.name, TType.Info)
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
                    new Modal.Config(this, "要休眠模型吗？", data.name, TType.Warn)
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
    }
}
