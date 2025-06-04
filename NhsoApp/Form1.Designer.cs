
namespace NhsoApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            lblfcid = new Label();
            label7 = new Label();
            txtMobile = new TextBox();
            label6 = new Label();
            label2 = new Label();
            lblage = new Label();
            lblsex = new Label();
            lblbirthDate = new Label();
            lblnation = new Label();
            lblfname = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            lable2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            label8 = new Label();
            lblDateTime = new Label();
            bntSentData = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(192, 255, 192);
            panel1.Controls.Add(lblfcid);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txtMobile);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(lblage);
            panel1.Controls.Add(lblsex);
            panel1.Controls.Add(lblbirthDate);
            panel1.Controls.Add(lblnation);
            panel1.Controls.Add(lblfname);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(lable2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(646, 376);
            panel1.TabIndex = 3;
            // 
            // lblfcid
            // 
            lblfcid.AutoSize = true;
            lblfcid.Font = new Font("Segoe UI", 15F);
            lblfcid.ForeColor = SystemColors.ControlText;
            lblfcid.Location = new Point(159, 70);
            lblfcid.Name = "lblfcid";
            lblfcid.Size = new Size(24, 28);
            lblfcid.TabIndex = 16;
            lblfcid.Text = "...";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 15F);
            label7.Location = new Point(24, 70);
            label7.Name = "label7";
            label7.Size = new Size(144, 28);
            label7.TabIndex = 15;
            label7.Text = "เลขบัตรประชาชน";
            // 
            // txtMobile
            // 
            txtMobile.BackColor = Color.Cornsilk;
            txtMobile.Font = new Font("Segoe UI", 15F);
            txtMobile.Location = new Point(159, 331);
            txtMobile.Margin = new Padding(3, 2, 3, 2);
            txtMobile.Name = "txtMobile";
            txtMobile.Size = new Size(216, 34);
            txtMobile.TabIndex = 14;
            txtMobile.KeyPress += txtMobile_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15F);
            label6.Location = new Point(26, 331);
            label6.Name = "label6";
            label6.Size = new Size(114, 28);
            label6.TabIndex = 13;
            label6.Text = "เบอร์โทรศัพท์";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F);
            label2.Location = new Point(75, 16);
            label2.Name = "label2";
            label2.Size = new Size(345, 45);
            label2.TabIndex = 12;
            label2.Text = "ระบบตรวจสอบสิทธิ์ NHSO";
            // 
            // lblage
            // 
            lblage.AutoSize = true;
            lblage.Font = new Font("Segoe UI", 15F);
            lblage.Location = new Point(159, 287);
            lblage.Name = "lblage";
            lblage.Size = new Size(24, 28);
            lblage.TabIndex = 10;
            lblage.Text = "...";
            // 
            // lblsex
            // 
            lblsex.AutoSize = true;
            lblsex.Font = new Font("Segoe UI", 15F);
            lblsex.Location = new Point(159, 245);
            lblsex.Name = "lblsex";
            lblsex.Size = new Size(24, 28);
            lblsex.TabIndex = 9;
            lblsex.Text = "...";
            // 
            // lblbirthDate
            // 
            lblbirthDate.AutoSize = true;
            lblbirthDate.Font = new Font("Segoe UI", 15F);
            lblbirthDate.Location = new Point(159, 208);
            lblbirthDate.Name = "lblbirthDate";
            lblbirthDate.Size = new Size(24, 28);
            lblbirthDate.TabIndex = 8;
            lblbirthDate.Text = "...";
            // 
            // lblnation
            // 
            lblnation.AutoSize = true;
            lblnation.Font = new Font("Segoe UI", 15F);
            lblnation.Location = new Point(159, 180);
            lblnation.Name = "lblnation";
            lblnation.Size = new Size(24, 28);
            lblnation.TabIndex = 7;
            lblnation.Text = "...";
            // 
            // lblfname
            // 
            lblfname.AutoSize = true;
            lblfname.Font = new Font("Segoe UI", 15F);
            lblfname.ForeColor = SystemColors.ControlText;
            lblfname.Location = new Point(159, 152);
            lblfname.Name = "lblfname";
            lblfname.Size = new Size(24, 28);
            lblfname.TabIndex = 6;
            lblfname.Text = "...";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15F);
            label5.Location = new Point(26, 287);
            label5.Name = "label5";
            label5.Size = new Size(42, 28);
            label5.TabIndex = 4;
            label5.Text = "อายุ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15F);
            label4.Location = new Point(26, 245);
            label4.Name = "label4";
            label4.Size = new Size(42, 28);
            label4.TabIndex = 3;
            label4.Text = "เพศ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15F);
            label3.Location = new Point(26, 208);
            label3.Name = "label3";
            label3.Size = new Size(112, 28);
            label3.TabIndex = 2;
            label3.Text = "วันเดือนปีเกิด";
            // 
            // lable2
            // 
            lable2.AutoSize = true;
            lable2.Font = new Font("Segoe UI", 15F);
            lable2.Location = new Point(26, 180);
            lable2.Name = "lable2";
            lable2.Size = new Size(73, 28);
            lable2.TabIndex = 1;
            lable2.Text = "สัญชาติ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(24, 152);
            label1.Name = "label1";
            label1.Size = new Size(86, 28);
            label1.TabIndex = 0;
            label1.Text = "ชื่อ - สกุล";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ControlDark;
            pictureBox1.Location = new Point(15, 94);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(164, 142);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.CornflowerBlue;
            panel2.Controls.Add(label8);
            panel2.Controls.Add(lblDateTime);
            panel2.Controls.Add(bntSentData);
            panel2.Controls.Add(pictureBox1);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(450, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(198, 376);
            panel2.TabIndex = 4;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 13F);
            label8.Location = new Point(37, 27);
            label8.Name = "label8";
            label8.Size = new Size(122, 25);
            label8.TabIndex = 13;
            label8.Text = "วันที่มารับบริการ";
            // 
            // lblDateTime
            // 
            lblDateTime.AutoSize = true;
            lblDateTime.Font = new Font("Segoe UI", 10F);
            lblDateTime.Location = new Point(3, 58);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(65, 19);
            lblDateTime.TabIndex = 12;
            lblDateTime.Text = "Datetime";
            // 
            // bntSentData
            // 
            bntSentData.Font = new Font("Segoe UI", 13F);
            bntSentData.Location = new Point(25, 271);
            bntSentData.Margin = new Padding(3, 2, 3, 2);
            bntSentData.Name = "bntSentData";
            bntSentData.Size = new Size(146, 65);
            bntSentData.TabIndex = 1;
            bntSentData.Text = "พิมท์สิทธิ์รับบริการ";
            bntSentData.UseVisualStyleBackColor = true;
            bntSentData.Click += bntSentData_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(648, 376);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lable2;
        private System.Windows.Forms.Label lblage;
        private System.Windows.Forms.Label lblsex;
        private System.Windows.Forms.Label lblbirthDate;
        private System.Windows.Forms.Label lblnation;
        private System.Windows.Forms.Label lblfname;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.Button bntSentData;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label8;
        private Label label7;
        private Label lblfcid;
    }
}
