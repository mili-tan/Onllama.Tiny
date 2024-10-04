using OllamaSharp.Models;

namespace Onllama.Tiny
{
    public partial class FormInfo : Form
    {
        public FormInfo(string model)
        {
            InitializeComponent();
            this.Text += @" - " + model;

            var show = Task.Run(() => Form1.OllamaApi.ShowModelInformation(model)).Result;
            inputLicense.Text = string.Join(Environment.NewLine,
                (show.License ?? string.Empty).Trim().Split('\n').ToList().Select(x => x.Trim()));
            inputParameters.Text = show.Parameters ?? string.Empty;
            inputTemplate.Text = show.Template ?? string.Empty;

            if (show.Template != null && show.Template.Contains("{{- if or .System .Tools }}")) toolTag.Visible = true;
            if (show.Details.Families != null && show.Details.Families.Contains("clip"))
            {
                if (!toolTag.Visible) visionTag.Location = new Point(toolTag.Location.X, toolTag.Location.Y);
                visionTag.Visible = true;
            }

            var info = show.Info;
            badgeContext.Text = info.ExtraInfo[$"{info.Architecture}.context_length"].ToString();
            badgeEmbedding.Text = info.ExtraInfo[$"{info.Architecture}.embedding_length"].ToString();
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            
        }
    }
}
