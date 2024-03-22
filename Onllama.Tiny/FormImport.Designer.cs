namespace Onllama.Tiny
{
    partial class FormImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImport));
            panel1 = new AntdUI.Panel();
            inputName = new AntdUI.Input();
            inputMf = new AntdUI.Input();
            select1 = new AntdUI.Select();
            buttonSave = new AntdUI.Button();
            panel2 = new AntdUI.Panel();
            input1 = new AntdUI.Input();
            buttonOpen = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(inputName);
            panel1.Controls.Add(inputMf);
            panel1.Controls.Add(select1);
            panel1.Controls.Add(buttonSave);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(divider1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Shadow = 5;
            panel1.Size = new Size(560, 387);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // inputName
            // 
            inputName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inputName.BackColor = Color.Transparent;
            inputName.Location = new Point(11, 75);
            inputName.Name = "inputName";
            inputName.PlaceholderText = "模型名称";
            inputName.Size = new Size(538, 30);
            inputName.TabIndex = 9;
            // 
            // inputMf
            // 
            inputMf.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inputMf.BackColor = Color.Transparent;
            inputMf.Location = new Point(11, 176);
            inputMf.Multiline = true;
            inputMf.Name = "inputMf";
            inputMf.Size = new Size(538, 167);
            inputMf.TabIndex = 8;
            // 
            // select1
            // 
            select1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            select1.BackColor = Color.Transparent;
            select1.Items.AddRange(new object[] { "qwen", "yi", "gemma", "mistral", "deepseek", "none" });
            select1.Location = new Point(11, 140);
            select1.Name = "select1";
            select1.Size = new Size(538, 30);
            select1.TabIndex = 7;
            select1.SelectedValueChanged += select1_SelectedValueChanged;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonSave.BackColor = Color.Transparent;
            buttonSave.BorderWidth = 1F;
            buttonSave.Location = new Point(11, 349);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(538, 30);
            buttonSave.TabIndex = 6;
            buttonSave.Text = "导入";
            buttonSave.Click += buttonSave_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(input1);
            panel2.Controls.Add(buttonOpen);
            panel2.Location = new Point(11, 32);
            panel2.Name = "panel2";
            panel2.Size = new Size(538, 35);
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
            input1.PlaceholderText = "qwen1_5-14b-chat-q4_0.gguf";
            input1.Size = new Size(489, 35);
            input1.TabIndex = 0;
            // 
            // buttonOpen
            // 
            buttonOpen.AutoSizeMode = AntdUI.TAutoSize.Width;
            buttonOpen.BorderWidth = 1F;
            buttonOpen.Dock = DockStyle.Right;
            buttonOpen.Ghost = true;
            buttonOpen.JoinLeft = true;
            buttonOpen.Location = new Point(489, 0);
            buttonOpen.Margins = 6;
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(49, 35);
            buttonOpen.TabIndex = 1;
            buttonOpen.Text = "选择";
            buttonOpen.Click += buttonOpen_Click;
            // 
            // divider2
            // 
            divider2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            divider2.BackColor = Color.Transparent;
            divider2.Location = new Point(8, 111);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(544, 23);
            divider2.TabIndex = 3;
            divider2.Text = "Modelfile";
            // 
            // divider1
            // 
            divider1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            divider1.BackColor = Color.Transparent;
            divider1.Location = new Point(8, 8);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(544, 23);
            divider1.TabIndex = 0;
            divider1.Text = "模型位置";
            // 
            // FormImport
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 411);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormImport";
            ShowIcon = false;
            Text = "导入";
            Load += FormImport_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Select select1;
        private AntdUI.Button buttonSave;
        private AntdUI.Panel panel2;
        private AntdUI.Input input1;
        private AntdUI.Button buttonOpen;
        private AntdUI.Divider divider2;
        private AntdUI.Divider divider1;
        private AntdUI.Input inputMf;
        private AntdUI.Input inputName;
    }
}