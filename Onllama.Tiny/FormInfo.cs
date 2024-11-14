using Newtonsoft.Json;
using OllamaSharp.Models;

namespace Onllama.Tiny
{
    public partial class FormInfo : Form
    {
        public FormInfo(string model)
        {
            InitializeComponent();
            try
            {
                this.Text += @" - " + model;

                var show = Task.Run(() => Form1.OllamaApi.ShowModelAsync(new ShowModelRequest {Model = model})).Result;
                var info = show.Info;

                inputLicense.Text = string.Join(Environment.NewLine,
                    (show.License ?? string.Empty).Trim().Split('\n').ToList().Select(x => x.Trim()));
                inputParameters.Text = show.Parameters ?? string.Empty;
                inputTemplate.Text = show.Template ?? string.Empty;

                if (show.Template != null && show.Template.Contains("{{- if or .System .Tools }}"))
                    toolTag.Visible = true;

                if (show.Projector?.ExtraInfo != null)
                {
                    visionTag.Visible = true;
                    var hasVision = show.Projector.ExtraInfo.FirstOrDefault(x => x.Key.EndsWith("has_vision_encoder"));
                    if (hasVision.Value != null && bool.Parse(hasVision.Value.ToString() ?? "0"))
                    {
                        if (!toolTag.Visible) visionTag.Location = new Point(toolTag.Location.X, toolTag.Location.Y);
                    }
                    else
                    {
                        if (!toolTag.Visible) visionTag.Location = new Point(toolTag.Location.X, toolTag.Location.Y);
                        visionTag.Text = show.Projector.ExtraInfo["general.architecture"].ToString();
                    }
                }

                badgeContext.Text = info.ExtraInfo[$"{info.Architecture}.context_length"].ToString();
                badgeEmbedding.Text = info.ExtraInfo[$"{info.Architecture}.embedding_length"].ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            
        }
    }
}
