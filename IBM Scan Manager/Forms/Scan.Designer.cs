
namespace IBM_Scan_Manager.Forms
{
    partial class frmScan
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
            this.lblProject = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cmbScan = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dtpScan = new System.Windows.Forms.DateTimePicker();
            this.nmbScan = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbNewScan = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbScan)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.Location = new System.Drawing.Point(12, 21);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(117, 15);
            this.lblProject.TabIndex = 0;
            this.lblProject.Text = "Project and Module :";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(358, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "New Assessment";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Scans :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.cmbScan);
            this.groupBox1.Location = new System.Drawing.Point(24, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(820, 95);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Existing Scans";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(584, 30);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(106, 39);
            this.button4.TabIndex = 13;
            this.button4.Text = "Open Assessment";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(472, 30);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(106, 39);
            this.button3.TabIndex = 7;
            this.button3.Text = "New Excel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cmbScan
            // 
            this.cmbScan.FormattingEnabled = true;
            this.cmbScan.Location = new System.Drawing.Point(123, 30);
            this.cmbScan.Name = "cmbScan";
            this.cmbScan.Size = new System.Drawing.Size(227, 23);
            this.cmbScan.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.dtpScan);
            this.groupBox2.Controls.Add(this.nmbScan);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbNewScan);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(24, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(820, 160);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "New Scan";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(696, 79);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 40);
            this.button2.TabIndex = 7;
            this.button2.Text = "Create new";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dtpScan
            // 
            this.dtpScan.Location = new System.Drawing.Point(145, 73);
            this.dtpScan.Name = "dtpScan";
            this.dtpScan.Size = new System.Drawing.Size(227, 23);
            this.dtpScan.TabIndex = 12;
            // 
            // nmbScan
            // 
            this.nmbScan.Location = new System.Drawing.Point(575, 30);
            this.nmbScan.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nmbScan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmbScan.Name = "nmbScan";
            this.nmbScan.Size = new System.Drawing.Size(227, 23);
            this.nmbScan.TabIndex = 11;
            this.nmbScan.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(74, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Scan Date :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(484, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Scan Number :";
            // 
            // cmbNewScan
            // 
            this.cmbNewScan.FormattingEnabled = true;
            this.cmbNewScan.Items.AddRange(new object[] {
            "DOTNET",
            "JS",
            "Angular",
            "PHP",
            "Java"});
            this.cmbNewScan.Location = new System.Drawing.Point(145, 29);
            this.cmbNewScan.Name = "cmbNewScan";
            this.cmbNewScan.Size = new System.Drawing.Size(227, 23);
            this.cmbNewScan.TabIndex = 7;
            this.cmbNewScan.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Scan Type :";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(696, 30);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(106, 39);
            this.button5.TabIndex = 14;
            this.button5.Text = "Assessment Details";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // frmScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 353);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmScan";
            this.Text = "Scan";
            this.Load += new System.EventHandler(this.Scan_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbScan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbScan;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker dtpScan;
        private System.Windows.Forms.NumericUpDown nmbScan;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbNewScan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}