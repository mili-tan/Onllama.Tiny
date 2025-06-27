using OllamaSharp.Models;

namespace Onllama.Tiny
{
    public partial class FormCopy : Form
    {
        private string sourceName = string.Empty;
        public FormCopy(string model)
        {
            InitializeComponent();
            sourceName = model;
            Text = LocalizationManager.GetTranslation("copy") + " " + model;
            input1.Text = model + @"-copy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() =>
                        Form1.OllamaApi.CopyModelAsync(new CopyModelRequest { Destination = input1.Text, Source = sourceName }))
                    .Wait();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Close();
        }

        private void FormCopy_Load(object sender, EventArgs e)
        {
            LocalizationManager.ApplyTranslations(this);
        }
    }
}
