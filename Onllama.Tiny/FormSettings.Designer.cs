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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            panel1 = new AntdUI.Panel();
            checkboxNoHistory = new AntdUI.Checkbox();
            checkboxModels = new AntdUI.Checkbox();
            checkboxPara = new AntdUI.Checkbox();
            buttonSave = new AntdUI.Button();
            panel2 = new AntdUI.Panel();
            input1 = new AntdUI.Input();
            buttonOpen = new AntdUI.Button();
            checkboxNoGpu = new AntdUI.Checkbox();
            checkboxAny = new AntdUI.Checkbox();
            divider2 = new AntdUI.Divider();
            divider1 = new AntdUI.Divider();
            checkboxFlushAttention = new AntdUI.Checkbox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(checkboxFlushAttention);
            panel1.Controls.Add(checkboxNoHistory);
            panel1.Controls.Add(checkboxModels);
            panel1.Controls.Add(checkboxPara);
            panel1.Controls.Add(buttonSave);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(checkboxNoGpu);
            panel1.Controls.Add(checkboxAny);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(divider1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Shadow = 5;
            panel1.Size = new Size(310, 360);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // checkboxNoHistory
            // 
            checkboxNoHistory.BackColor = Color.Transparent;
            checkboxNoHistory.Location = new Point(11, 215);
            checkboxNoHistory.Name = "checkboxNoHistory";
            checkboxNoHistory.Size = new Size(288, 23);
            checkboxNoHistory.TabIndex = 9;
            checkboxNoHistory.Text = "禁用注意力";
            // 
            // checkboxModels
            // 
            checkboxModels.BackColor = Color.Transparent;
            checkboxModels.Location = new Point(11, 186);
            checkboxModels.Name = "checkboxModels";
            checkboxModels.Size = new Size(288, 23);
            checkboxModels.TabIndex = 8;
            checkboxModels.Text = "允许同时加载多个模型";
            // 
            // checkboxPara
            // 
            checkboxPara.BackColor = Color.Transparent;
            checkboxPara.Location = new Point(11, 157);
            checkboxPara.Name = "checkboxPara";
            checkboxPara.Size = new Size(288, 23);
            checkboxPara.TabIndex = 7;
            checkboxPara.Text = "允许对话并发请求";
            // 
            // buttonSave
            // 
            buttonSave.BackColor = Color.Transparent;
            buttonSave.BorderWidth = 1F;
            buttonSave.Location = new Point(11, 327);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(288, 25);
            buttonSave.TabIndex = 6;
            buttonSave.Text = "保存";
            buttonSave.Click += buttonSave_Click;
            // 
            // panel2
            // 
            panel2.Back = Color.White;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(input1);
            panel2.Controls.Add(buttonOpen);
            panel2.Location = new Point(11, 32);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new Size(288, 35);
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
            input1.Size = new Size(239, 35);
            input1.TabIndex = 0;
            // 
            // buttonOpen
            // 
            buttonOpen.AutoSizeMode = AntdUI.TAutoSize.Width;
            buttonOpen.BorderWidth = 1F;
            buttonOpen.Dock = DockStyle.Right;
            buttonOpen.Ghost = true;
            buttonOpen.JoinLeft = true;
            buttonOpen.Location = new Point(239, 0);
            buttonOpen.Margins = 6;
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(49, 35);
            buttonOpen.TabIndex = 1;
            buttonOpen.Text = "更改";
            buttonOpen.Click += buttonOpen_Click;
            // 
            // checkboxNoGpu
            // 
            checkboxNoGpu.BackColor = Color.Transparent;
            checkboxNoGpu.Location = new Point(11, 128);
            checkboxNoGpu.Name = "checkboxNoGpu";
            checkboxNoGpu.Size = new Size(288, 23);
            checkboxNoGpu.TabIndex = 5;
            checkboxNoGpu.Text = "禁用 NVIDIA CUDA、AMD ROCm 显卡加速 ";
            // 
            // checkboxAny
            // 
            checkboxAny.BackColor = Color.Transparent;
            checkboxAny.Location = new Point(11, 99);
            checkboxAny.Name = "checkboxAny";
            checkboxAny.Size = new Size(288, 23);
            checkboxAny.TabIndex = 4;
            checkboxAny.Text = "允许局域网和外部访问";
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
            // checkboxFlushAttention
            // 
            checkboxFlushAttention.BackColor = Color.Transparent;
            checkboxFlushAttention.Location = new Point(11, 244);
            checkboxFlushAttention.Name = "checkboxFlushAttention";
            checkboxFlushAttention.Size = new Size(288, 23);
            checkboxFlushAttention.TabIndex = 10;
            checkboxFlushAttention.Text = "启用 Flush Attention";
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 384);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            ShowIcon = false;
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
        private AntdUI.Button buttonOpen;
        private AntdUI.Checkbox checkboxAny;
        private AntdUI.Divider divider2;
        private AntdUI.Checkbox checkboxNoGpu;
        private AntdUI.Button buttonSave;
        private AntdUI.Checkbox checkboxModels;
        private AntdUI.Checkbox checkboxPara;
        private AntdUI.Checkbox checkboxNoHistory;
        private AntdUI.Checkbox checkboxFlushAttention;
    }
}