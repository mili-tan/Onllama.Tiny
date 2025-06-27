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
                this.Text = LocalizationManager.GetTranslation("model_info") + @" - " + model;

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
                        if (show.Projector.ExtraInfo.ContainsKey("general.architecture"))
                        {
                            visionTag.Text = show.Projector.ExtraInfo["general.architecture"]?.ToString() ?? LocalizationManager.GetTranslation("unknown");
                        }
                        else
                        {
                            visionTag.Text = LocalizationManager.GetTranslation("unknown");
                        }
                    }
                }

                if (info?.ExtraInfo != null && !string.IsNullOrEmpty(info.Architecture))
                {
                    var contextKey = $"{info.Architecture}.context_length";
                    var embeddingKey = $"{info.Architecture}.embedding_length";
                    
                    if (info.ExtraInfo.ContainsKey(contextKey))
                    {
                        badgeContext.Text = info.ExtraInfo[contextKey]?.ToString() ?? LocalizationManager.GetTranslation("unknown");
                    }
                    else
                    {
                        badgeContext.Text = LocalizationManager.GetTranslation("unknown");
                    }
                    
                    if (info.ExtraInfo.ContainsKey(embeddingKey))
                    {
                        badgeEmbedding.Text = info.ExtraInfo[embeddingKey]?.ToString() ?? LocalizationManager.GetTranslation("unknown");
                    }
                    else
                    {
                        badgeEmbedding.Text = LocalizationManager.GetTranslation("unknown");
                    }
                }
                else
                {
                    badgeContext.Text = LocalizationManager.GetTranslation("unknown");
                    badgeEmbedding.Text = LocalizationManager.GetTranslation("unknown");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in FormInfo: {e}");
                MessageBox.Show($"{LocalizationManager.GetTranslation("model_info_error")}: {e.Message}", 
                    LocalizationManager.GetTranslation("error"), 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            LocalizationManager.ApplyTranslations(this);
        }
    }
}
