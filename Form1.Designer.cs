namespace N64_RSP_DISASSEMBLER
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.button_loadROM = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ROM_ADDRESS = new System.Windows.Forms.TextBox();
            this.textBox_DATA_LENGTH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_RAM_ADDRESS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_showAddresses = new System.Windows.Forms.CheckBox();
            this.cb_showBinary = new System.Windows.Forms.CheckBox();
            this.cb_regNames = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(-1, 50);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(562, 365);
            this.textBox.TabIndex = 0;
            this.textBox.Text = resources.GetString("textBox.Text");
            // 
            // button_loadROM
            // 
            this.button_loadROM.AutoSize = true;
            this.button_loadROM.Location = new System.Drawing.Point(5, 4);
            this.button_loadROM.Name = "button_loadROM";
            this.button_loadROM.Size = new System.Drawing.Size(75, 42);
            this.button_loadROM.TabIndex = 1;
            this.button_loadROM.Text = "Load BIN\r\nor ROM file";
            this.button_loadROM.UseVisualStyleBackColor = true;
            this.button_loadROM.Click += new System.EventHandler(this.button_loadROM_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "File offset: ";
            // 
            // textBox_ROM_ADDRESS
            // 
            this.textBox_ROM_ADDRESS.Location = new System.Drawing.Point(288, 4);
            this.textBox_ROM_ADDRESS.Name = "textBox_ROM_ADDRESS";
            this.textBox_ROM_ADDRESS.Size = new System.Drawing.Size(75, 20);
            this.textBox_ROM_ADDRESS.TabIndex = 5;
            this.textBox_ROM_ADDRESS.Text = "0";
            this.textBox_ROM_ADDRESS.TextChanged += new System.EventHandler(this.textBox_DATA_LENGTH_TextChanged);
            // 
            // textBox_DATA_LENGTH
            // 
            this.textBox_DATA_LENGTH.Location = new System.Drawing.Point(413, 4);
            this.textBox_DATA_LENGTH.Name = "textBox_DATA_LENGTH";
            this.textBox_DATA_LENGTH.Size = new System.Drawing.Size(58, 20);
            this.textBox_DATA_LENGTH.TabIndex = 7;
            this.textBox_DATA_LENGTH.Text = "0";
            this.textBox_DATA_LENGTH.TextChanged += new System.EventHandler(this.textBox_DATA_LENGTH_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(369, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Length: ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(486, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_RAM_ADDRESS
            // 
            this.textBox_RAM_ADDRESS.Location = new System.Drawing.Point(164, 4);
            this.textBox_RAM_ADDRESS.Name = "textBox_RAM_ADDRESS";
            this.textBox_RAM_ADDRESS.Size = new System.Drawing.Size(58, 20);
            this.textBox_RAM_ADDRESS.TabIndex = 10;
            this.textBox_RAM_ADDRESS.Text = "04001000";
            this.textBox_RAM_ADDRESS.TextChanged += new System.EventHandler(this.textBox_DATA_LENGTH_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "RAM address: ";
            // 
            // cb_showAddresses
            // 
            this.cb_showAddresses.AutoSize = true;
            this.cb_showAddresses.Checked = true;
            this.cb_showAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_showAddresses.Location = new System.Drawing.Point(128, 28);
            this.cb_showAddresses.Name = "cb_showAddresses";
            this.cb_showAddresses.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_showAddresses.Size = new System.Drawing.Size(105, 17);
            this.cb_showAddresses.TabIndex = 11;
            this.cb_showAddresses.Text = "Show Addresses";
            this.cb_showAddresses.UseVisualStyleBackColor = true;
            this.cb_showAddresses.CheckedChanged += new System.EventHandler(this.cb_showBinary_CheckedChanged);
            // 
            // cb_showBinary
            // 
            this.cb_showBinary.AutoSize = true;
            this.cb_showBinary.Checked = true;
            this.cb_showBinary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_showBinary.Location = new System.Drawing.Point(235, 28);
            this.cb_showBinary.Name = "cb_showBinary";
            this.cb_showBinary.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_showBinary.Size = new System.Drawing.Size(107, 17);
            this.cb_showBinary.TabIndex = 12;
            this.cb_showBinary.Text = "Show Binary Hex";
            this.cb_showBinary.UseVisualStyleBackColor = true;
            this.cb_showBinary.CheckedChanged += new System.EventHandler(this.cb_showBinary_CheckedChanged);
            // 
            // cb_regNames
            // 
            this.cb_regNames.AutoSize = true;
            this.cb_regNames.Location = new System.Drawing.Point(345, 28);
            this.cb_regNames.Name = "cb_regNames";
            this.cb_regNames.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_regNames.Size = new System.Drawing.Size(170, 17);
            this.cb_regNames.TabIndex = 13;
            this.cb_regNames.Text = "Use MIPS GP Register Names";
            this.cb_regNames.UseVisualStyleBackColor = true;
            this.cb_regNames.CheckedChanged += new System.EventHandler(this.cb_showBinary_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 411);
            this.Controls.Add(this.cb_regNames);
            this.Controls.Add(this.cb_showBinary);
            this.Controls.Add(this.cb_showAddresses);
            this.Controls.Add(this.textBox_RAM_ADDRESS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_DATA_LENGTH);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_ROM_ADDRESS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_loadROM);
            this.Controls.Add(this.textBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(578, 450);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "David\'s N64 RSP disassembler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.Button button_loadROM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ROM_ADDRESS;
        private System.Windows.Forms.TextBox textBox_DATA_LENGTH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_RAM_ADDRESS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_showAddresses;
        private System.Windows.Forms.CheckBox cb_showBinary;
        private System.Windows.Forms.CheckBox cb_regNames;
    }
}

