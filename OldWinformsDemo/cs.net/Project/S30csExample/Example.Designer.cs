namespace S30csExample
{
    partial class Example
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.log_tb = new System.Windows.Forms.TextBox();
            this.close_btn = new System.Windows.Forms.Button();
            this.comport_combo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.f48_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.baud_combo = new System.Windows.Forms.ComboBox();
            this.adress_ud = new System.Windows.Forms.NumericUpDown();
            this.f73_btn = new System.Windows.Forms.Button();
            this.f69_btn = new System.Windows.Forms.Button();
            this.channel_ud = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.adress_ud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel_ud)).BeginInit();
            this.SuspendLayout();
            // 
            // log_tb
            // 
            this.log_tb.AcceptsTab = true;
            this.log_tb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.log_tb.Location = new System.Drawing.Point(12, 105);
            this.log_tb.Multiline = true;
            this.log_tb.Name = "log_tb";
            this.log_tb.ReadOnly = true;
            this.log_tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log_tb.Size = new System.Drawing.Size(552, 150);
            this.log_tb.TabIndex = 0;
            this.log_tb.TextChanged += new System.EventHandler(this.log_tb_TextChanged);
            // 
            // close_btn
            // 
            this.close_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_btn.Location = new System.Drawing.Point(489, 261);
            this.close_btn.Name = "close_btn";
            this.close_btn.Size = new System.Drawing.Size(75, 23);
            this.close_btn.TabIndex = 1;
            this.close_btn.Text = "Beenden";
            this.close_btn.UseVisualStyleBackColor = true;
            this.close_btn.Click += new System.EventHandler(this.close_btn_Click);
            // 
            // comport_combo
            // 
            this.comport_combo.FormattingEnabled = true;
            this.comport_combo.Location = new System.Drawing.Point(67, 23);
            this.comport_combo.Name = "comport_combo";
            this.comport_combo.Size = new System.Drawing.Size(144, 21);
            this.comport_combo.TabIndex = 3;
            this.comport_combo.SelectedIndexChanged += new System.EventHandler(this.comport_combo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Comport:";
            // 
            // f48_btn
            // 
            this.f48_btn.Location = new System.Drawing.Point(12, 76);
            this.f48_btn.Name = "f48_btn";
            this.f48_btn.Size = new System.Drawing.Size(75, 23);
            this.f48_btn.TabIndex = 5;
            this.f48_btn.Text = "F48 (Init)";
            this.f48_btn.UseVisualStyleBackColor = true;
            this.f48_btn.Click += new System.EventHandler(this.f48_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(410, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Adresse:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Baudrate:";
            // 
            // baud_combo
            // 
            this.baud_combo.FormattingEnabled = true;
            this.baud_combo.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.baud_combo.Location = new System.Drawing.Point(296, 23);
            this.baud_combo.Name = "baud_combo";
            this.baud_combo.Size = new System.Drawing.Size(97, 21);
            this.baud_combo.TabIndex = 9;
            // 
            // adress_ud
            // 
            this.adress_ud.Location = new System.Drawing.Point(464, 24);
            this.adress_ud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.adress_ud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.adress_ud.Name = "adress_ud";
            this.adress_ud.Size = new System.Drawing.Size(100, 20);
            this.adress_ud.TabIndex = 10;
            this.adress_ud.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // f73_btn
            // 
            this.f73_btn.Location = new System.Drawing.Point(344, 76);
            this.f73_btn.Name = "f73_btn";
            this.f73_btn.Size = new System.Drawing.Size(75, 23);
            this.f73_btn.TabIndex = 11;
            this.f73_btn.Text = "F73 (Wert)";
            this.f73_btn.UseVisualStyleBackColor = true;
            this.f73_btn.Click += new System.EventHandler(this.f73_btn_Click);
            // 
            // f69_btn
            // 
            this.f69_btn.Location = new System.Drawing.Point(93, 76);
            this.f69_btn.Name = "f69_btn";
            this.f69_btn.Size = new System.Drawing.Size(75, 23);
            this.f69_btn.TabIndex = 12;
            this.f69_btn.Text = "F69 (SN)";
            this.f69_btn.UseVisualStyleBackColor = true;
            this.f69_btn.Click += new System.EventHandler(this.f69_btn_Click);
            // 
            // channel_ud
            // 
            this.channel_ud.Location = new System.Drawing.Point(296, 79);
            this.channel_ud.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.channel_ud.Name = "channel_ud";
            this.channel_ud.Size = new System.Drawing.Size(42, 20);
            this.channel_ud.TabIndex = 13;
            this.channel_ud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(253, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Kanal:";
            // 
            // Example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 296);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.channel_ud);
            this.Controls.Add(this.f69_btn);
            this.Controls.Add(this.f73_btn);
            this.Controls.Add(this.adress_ud);
            this.Controls.Add(this.baud_combo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.f48_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comport_combo);
            this.Controls.Add(this.close_btn);
            this.Controls.Add(this.log_tb);
            this.Name = "Example";
            this.Text = "C# Example";
            ((System.ComponentModel.ISupportInitialize)(this.adress_ud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel_ud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox log_tb;
        private System.Windows.Forms.Button close_btn;
        private System.Windows.Forms.ComboBox comport_combo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button f48_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox baud_combo;
        private System.Windows.Forms.NumericUpDown adress_ud;
        private System.Windows.Forms.Button f73_btn;
        private System.Windows.Forms.Button f69_btn;
        private System.Windows.Forms.NumericUpDown channel_ud;
        private System.Windows.Forms.Label label4;
    }
}

