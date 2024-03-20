namespace Onllama.Tiny
{
    partial class FormSettings
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
            panel1 = new AntdUI.Panel();
            divider1 = new AntdUI.Divider();
            panel2 = new AntdUI.Panel();
            input1 = new AntdUI.Input();
            button1 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            checkbox1 = new AntdUI.Checkbox();
            checkbox2 = new AntdUI.Checkbox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(checkbox2);
            panel1.Controls.Add(checkbox1);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Shadow = 5;
            panel1.Size = new Size(310, 337);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // divider1
            // 
            divider1.BackColor = Color.Transparent;
            divider1.Location = new Point(8, 8);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(294, 23);
            divider1.TabIndex = 0;
            divider1.Text = "模型位置";
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(input1);
            panel2.Controls.Add(button1);
            panel2.Location = new Point(11, 29);
            panel2.Name = "panel2";
            panel2.Size = new Size(288, 41);
            panel2.TabIndex = 2;
            // 
            // input1
            // 
            input1.Dock = DockStyle.Fill;
            input1.JoinRight = true;
            input1.Location = new Point(0, 0);
            input1.Margin = new Padding(5);
            input1.Margins = 6;
            input1.Name = "input1";
            input1.PlaceholderText = ".ollama";
            input1.Size = new Size(239, 41);
            input1.TabIndex = 0;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.BorderWidth = 1F;
            button1.Dock = DockStyle.Right;
            button1.Ghost = true;
            button1.JoinLeft = true;
            button1.Location = new Point(239, 0);
            button1.Margins = 6;
            button1.Name = "button1";
            button1.Size = new Size(49, 41);
            button1.TabIndex = 1;
            button1.Text = "更改";
            // 
            // divider2
            // 
            divider2.BackColor = Color.Transparent;
            divider2.Location = new Point(8, 70);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(294, 23);
            divider2.TabIndex = 3;
            divider2.Text = "设置";
            // 
            // checkbox1
            // 
            checkbox1.BackColor = Color.Transparent;
            checkbox1.Location = new Point(11, 99);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(288, 23);
            checkbox1.TabIndex = 4;
            checkbox1.Text = "允许局域网访问";
            // 
            // checkbox2
            // 
            checkbox2.BackColor = Color.Transparent;
            checkbox2.Location = new Point(11, 128);
            checkbox2.Name = "checkbox2";
            checkbox2.Size = new Size(288, 23);
            checkbox2.TabIndex = 5;
            checkbox2.Text = "禁用 NVIDIA CUDA、AMD ROCm 显卡加速 ";
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 361);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "设置";
            Load += FormSettings_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Panel panel2;
        private AntdUI.Input input1;
        private AntdUI.Button button1;
        private AntdUI.Checkbox checkbox1;
        private AntdUI.Divider divider2;
        private AntdUI.Checkbox checkbox2;
    }
}