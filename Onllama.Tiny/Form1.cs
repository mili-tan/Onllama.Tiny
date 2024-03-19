using System.Diagnostics;
using System.Xml.Linq;
using AntdUI;
using OllamaSharp;

namespace Onllama.Tiny
{
    public partial class Form1 : AntdUI.BaseForm
    {
        public static OllamaApiClient OllamaApi = new OllamaApiClient("http://127.0.0.1:11434");

        public Form1()
        {
            InitializeComponent();
            table1.Columns = new AntdUI.Column[]
            {
                new AntdUI.Column("name", "模型名称"),
                new AntdUI.Column("size", "模型大小"),
                new AntdUI.Column("modifiedAt", "上次修改"),
                new AntdUI.Column("families", "系列"),
                new AntdUI.Column("quantization", "格式与规模"),
                new AntdUI.Column("btns", "操作") {Fixed = true},
            };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                progress1.Hide();
                Environment.SetEnvironmentVariable("OLLAMA_ORIGINS", "*", EnvironmentVariableTarget.User);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            var modelsClasses = new List<ModelsClass>();
            var models = Task.Run(async () => await OllamaApi.ListLocalModels()).Result;
            foreach (var item in models)
            {
                var q = new List<CellTag>()
                {
                    new(item.Details.Format.ToUpper(),TTypeMini.Default),
                    new(item.Details.ParameterSize.ToUpper(),TTypeMini.Success),
                    new(item.Details.QuantizationLevel.ToUpper(),TTypeMini.Warn)
                };
                modelsClasses.Add(new ModelsClass()
                {
                    name = item.Name,
                    size = (item.Size / 1024.00 / 1024.00 / 1024.00).ToString("0.00") + "G",
                    modifiedAt = item.ModifiedAt,
                    families = item.Details.Families.Select(x => new CellTag(x.ToUpper(), TTypeMini.Info)).ToArray(),
                    quantization = q.ToArray(),
                    btns = new AntdUI.CellLink[]
                    {
                        new AntdUI.CellButton("chat", "NextChat", AntdUI.TTypeMini.Default)
                            {Ghost = true, BorderWidth = 1},
                        new AntdUI.CellButton("delete", "删除", AntdUI.TTypeMini.Error)
                            {Ghost = true, BorderWidth = 1}
                    }
                });
            }
            table1.DataSource = modelsClasses;
        }

        public class ModelsClass : AntdUI.NotifyProperty
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
                            return true;
                        }
                    });
                }
                else if (btn.Id == "chat")
                {
                    Process.Start(new ProcessStartInfo($"https://app.nextchat.dev/#/?settings={{%22url%22:%22http://127.0.0.1:11434%22}}") { UseShellExecute = true });
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                            Text = x.Status + ":" + x.Digest;
                            if (x.Status == "success" || string.IsNullOrEmpty(x.Status)) progress1.Hide();
                        });
                    }));
                    return true;
                }
            });
        }
    }
}
