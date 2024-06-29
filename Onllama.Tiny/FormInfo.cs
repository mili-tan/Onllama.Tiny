using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            var info = show.Info;
            badgeContext.Text = info.ExtraInfo[$"{info.Architecture}.context_length"].ToString();
            badgeEmbedding.Text = info.ExtraInfo[$"{info.Architecture}.embedding_length"].ToString();
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
        }
    }
}
