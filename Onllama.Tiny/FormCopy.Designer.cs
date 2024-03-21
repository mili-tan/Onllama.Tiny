using AntdUI;
using static AntdUI.Modal;

namespace Onllama.Tiny
{
    partial class FormCopy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCopy));
            panel1 = new AntdUI.Panel();
            input1 = new Input();
            button1 = new AntdUI.Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(input1);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(0, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(349, 35);
            panel1.TabIndex = 1;
            // 
            // input1
            // 
            input1.Dock = DockStyle.Fill;
            input1.JoinRight = true;
            input1.Location = new Point(0, 0);
            input1.Margin = new Padding(5);
            input1.Margins = 6;
            input1.Name = "input1";
            input1.PlaceholderText = "新模型名称";
            input1.Size = new Size(300, 35);
            input1.TabIndex = 0;
            // 
            // button1
            // 
            button1.AutoSizeMode = TAutoSize.Width;
            button1.BorderWidth = 1F;
            button1.Dock = DockStyle.Right;
            button1.Ghost = true;
            button1.JoinLeft = true;
            button1.Location = new Point(300, 0);
            button1.Margins = 6;
            button1.Name = "button1";
            button1.Size = new Size(49, 35);
            button1.TabIndex = 1;
            button1.Text = "确定";
            button1.Click += button1_Click;
            // 
            // FormCopy
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(349, 41);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCopy";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "复制模型";
            Load += FormCopy_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Input input1;
        private AntdUI.Button button1;
    }
}