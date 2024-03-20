using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            Text = "Onllama - 复制 " + model;
            input1.Text = model + "-copy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() =>
                        Form1.OllamaApi.CopyModel(new CopyModelRequest { Destination = input1.Text, Source = sourceName }))
                    .Wait();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Close();
        }
    }
}
