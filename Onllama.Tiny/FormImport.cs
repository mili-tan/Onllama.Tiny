﻿using System.Globalization;
using AntdUI;
using OllamaSharp.Models;
using Onllama.Tiny.Properties;

namespace Onllama.Tiny
{
    public partial class FormImport : Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var f = new OpenFileDialog();
                if (f.ShowDialog() == DialogResult.OK) input1.Text = f.FileName;
                inputName.Text = f.SafeFileNames.Last().Split('.', '-').First();
                foreach (var item in select1.Items)
                    if (input1.Text.Contains(item.ToString() ?? string.Empty))
                        select1.SelectedValue = item.ToString();
                if (select1.Items.Contains("embed") || select1.Items.Contains("bge") || select1.Items.Contains("m3e"))
                    select1.SelectedValue = "none";
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(input1.Text) || string.IsNullOrEmpty(inputName.Text) ||
                string.IsNullOrEmpty(inputMf.Text))
            {
                new Modal.Config(this, "模板不应为空！", new[] {new Modal.TextLine(inputName.Text, Style.Db.Error)},
                    TType.Error)
                {
                    OkType = TTypeMini.Error,
                }.open();
                return;
            }
            new Modal.Config(this, "您确定要导入模型吗？", new[] {new Modal.TextLine(inputName.Text, Style.Db.Primary)},
                TType.Success)
            {
                OkType = TTypeMini.Success,
                OkText = "导入",
                OnOk = _ =>
                {
                    try
                    {
                        var req = new CreateModelRequest
                            {ModelFileContent = inputMf.Text, Model = inputName.Text, Stream = true};
                        var textInfo = new CultureInfo("en-US", false).TextInfo;
                        if (string.IsNullOrWhiteSpace(select2.Text) || select2.Text != "不量化")
                            req.Quantize = select2.Text;

                        Task.Run(async () =>
                        {
                            await foreach (var x in Form1.OllamaApi.CreateModelAsync(req))
                                Invoke(() => Text = textInfo.ToTitleCase(x.Status));
                        }).Wait();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                    Invoke(Close);
                    return true;
                }
            }.open();
        }

        private void select1_SelectedValueChanged(object sender, ObjectNEventArgs value)
        {
            inputMf.Text = @"FROM " + input1.Text + Environment.NewLine;
            switch (value.Value.ToString())
            {
                case "qwen2":
                    inputMf.Text += Resources.qwen2Tmp;
                    break;
                case "qwen1.5":
                    inputMf.Text += Resources.qwenTmp;
                    break;
                case "yi-1.5":
                    inputMf.Text += Resources.yi15Tmp;
                    break;
                case "yi":
                    inputMf.Text += Resources.yiTmp;
                    break;
                case "gemma":
                    inputMf.Text += Resources.gemmaTmp;
                    break;
                case "mistral":
                    inputMf.Text += Resources.mistralTmp;
                    break;
                case "deepseek-v2":
                    inputMf.Text += Resources.deekseekv2Tmp;
                    break;
                case "deepseek":
                    inputMf.Text += Resources.deekseekTmp;
                    break;
            }
        }

        private void FormImport_Load(object sender, EventArgs e)
        {

        }
    }
}
