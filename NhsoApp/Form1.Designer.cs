
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblage = new System.Windows.Forms.Label();
            this.lblsex = new System.Windows.Forms.Label();
            this.lblbirthDate = new System.Windows.Forms.Label();
            this.lblnation = new System.Windows.Forms.Label();
            this.lblfname = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lable2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.bntSentData = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.txtMobile);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblage);
            this.panel1.Controls.Add(this.lblsex);
            this.panel1.Controls.Add(this.lblbirthDate);
            this.panel1.Controls.Add(this.lblnation);
            this.panel1.Controls.Add(this.lblfname);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lable2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(738, 501);
            this.panel1.TabIndex = 3;
            // 
            // txtMobile
            // 
            this.txtMobile.BackColor = System.Drawing.Color.Cornsilk;
            this.txtMobile.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMobile.Location = new System.Drawing.Point(182, 408);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(246, 41);
            this.txtMobile.TabIndex = 14;
            this.txtMobile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMobile_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(28, 411);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 35);
            this.label6.TabIndex = 13;
            this.label6.Text = "เบอร์โทรศัพท์";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(86, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(430, 54);
            this.label2.TabIndex = 12;
            this.label2.Text = "ระบบตรวจสอบสิทธิ์ NHSO";
            // 
            // lblage
            // 
            this.lblage.AutoSize = true;
            this.lblage.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblage.Location = new System.Drawing.Point(182, 343);
            this.lblage.Name = "lblage";
            this.lblage.Size = new System.Drawing.Size(30, 35);
            this.lblage.TabIndex = 10;
            this.lblage.Text = "...";
            // 
            // lblsex
            // 
            this.lblsex.AutoSize = true;
            this.lblsex.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblsex.Location = new System.Drawing.Point(182, 285);
            this.lblsex.Name = "lblsex";
            this.lblsex.Size = new System.Drawing.Size(30, 35);
            this.lblsex.TabIndex = 9;
            this.lblsex.Text = "...";
            // 
            // lblbirthDate
            // 
            this.lblbirthDate.AutoSize = true;
            this.lblbirthDate.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblbirthDate.Location = new System.Drawing.Point(182, 236);
            this.lblbirthDate.Name = "lblbirthDate";
            this.lblbirthDate.Size = new System.Drawing.Size(30, 35);
            this.lblbirthDate.TabIndex = 8;
            this.lblbirthDate.Text = "...";
            // 
            // lblnation
            // 
            this.lblnation.AutoSize = true;
            this.lblnation.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblnation.Location = new System.Drawing.Point(182, 179);
            this.lblnation.Name = "lblnation";
            this.lblnation.Size = new System.Drawing.Size(30, 35);
            this.lblnation.TabIndex = 7;
            this.lblnation.Text = "...";
            // 
            // lblfname
            // 
            this.lblfname.AutoSize = true;
            this.lblfname.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblfname.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblfname.Location = new System.Drawing.Point(182, 130);
            this.lblfname.Name = "lblfname";
            this.lblfname.Size = new System.Drawing.Size(30, 35);
            this.lblfname.TabIndex = 6;
            this.lblfname.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(30, 343);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 35);
            this.label5.TabIndex = 4;
            this.label5.Text = "อายุ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(30, 285);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 35);
            this.label4.TabIndex = 3;
            this.label4.Text = "เพศ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(30, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 35);
            this.label3.TabIndex = 2;
            this.label3.Text = "วันเดือนปีเกิด";
            // 
            // lable2
            // 
            this.lable2.AutoSize = true;
            this.lable2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lable2.Location = new System.Drawing.Point(30, 179);
            this.lable2.Name = "lable2";
            this.lable2.Size = new System.Drawing.Size(89, 35);
            this.lable2.TabIndex = 1;
            this.lable2.Text = "สัญชาติ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(30, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "ชื่อ - สกุล";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            //this.pictureBox1.Image = global::NhsoApp.Properties.Resources._777;
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(17, 126);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(187, 189);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.lblDateTime);
            this.panel2.Controls.Add(this.bntSentData);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(515, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 501);
            this.panel2.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(42, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(147, 30);
            this.label8.TabIndex = 13;
            this.label8.Text = "วันที่มารับบริการ";
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDateTime.Location = new System.Drawing.Point(3, 78);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(80, 23);
            this.lblDateTime.TabIndex = 12;
            this.lblDateTime.Text = "Datetime";
            // 
            // bntSentData
            // 
            this.bntSentData.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bntSentData.Location = new System.Drawing.Point(29, 361);
            this.bntSentData.Name = "bntSentData";
            this.bntSentData.Size = new System.Drawing.Size(167, 87);
            this.bntSentData.TabIndex = 1;
            this.bntSentData.Text = "พิมท์สิทธิ์รับบริการ";
            this.bntSentData.UseVisualStyleBackColor = true;
            this.bntSentData.Click += new System.EventHandler(this.bntSentData_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(741, 501);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

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
    }
}
