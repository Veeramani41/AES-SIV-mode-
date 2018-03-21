namespace BCKeyGeneration
{
    partial class EncyptionProcess
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
            this.lblSampleText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReceiverPublic = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GenerateSender = new System.Windows.Forms.Button();
            this.Decrypt = new System.Windows.Forms.Button();
            this.IsSenderComp = new System.Windows.Forms.CheckBox();
            this.txtSenderPublic = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPlainData = new System.Windows.Forms.TextBox();
            this.txtSenderSecret = new System.Windows.Forms.TextBox();
            this.txtSenderDerived = new System.Windows.Forms.TextBox();
            this.Encrypt = new System.Windows.Forms.Button();
            this.IsReceiverComp = new System.Windows.Forms.CheckBox();
            this.GenerateReceiver = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtReceiverDerived = new System.Windows.Forms.TextBox();
            this.txtReceiverSecret = new System.Windows.Forms.TextBox();
            this.txtChiperData = new System.Windows.Forms.TextBox();
            this.txtSenderPub = new System.Windows.Forms.TextBox();
            this.txtReceiverPub = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDecryptedData = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSampleText
            // 
            this.lblSampleText.AutoSize = true;
            this.lblSampleText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleText.Location = new System.Drawing.Point(12, 71);
            this.lblSampleText.Name = "lblSampleText";
            this.lblSampleText.Size = new System.Drawing.Size(147, 17);
            this.lblSampleText.TabIndex = 0;
            this.lblSampleText.Text = "Create EC Key Pair";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "EncyptionProcess";
            // 
            // txtReceiverPublic
            // 
            this.txtReceiverPublic.Location = new System.Drawing.Point(12, 176);
            this.txtReceiverPublic.Multiline = true;
            this.txtReceiverPublic.Name = "txtReceiverPublic";
            this.txtReceiverPublic.Size = new System.Drawing.Size(656, 31);
            this.txtReceiverPublic.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 354);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Receiver(Mobile App)";
            // 
            // GenerateSender
            // 
            this.GenerateSender.Location = new System.Drawing.Point(316, 67);
            this.GenerateSender.Name = "GenerateSender";
            this.GenerateSender.Size = new System.Drawing.Size(119, 27);
            this.GenerateSender.TabIndex = 6;
            this.GenerateSender.Text = "Generate";
            this.GenerateSender.UseVisualStyleBackColor = true;
            this.GenerateSender.Click += new System.EventHandler(this.GenerateSender_Click);
            // 
            // Decrypt
            // 
            this.Decrypt.Location = new System.Drawing.Point(549, 562);
            this.Decrypt.Name = "Decrypt";
            this.Decrypt.Size = new System.Drawing.Size(119, 27);
            this.Decrypt.TabIndex = 9;
            this.Decrypt.Text = "Decrypt Data";
            this.Decrypt.UseVisualStyleBackColor = true;
            this.Decrypt.Click += new System.EventHandler(this.Decrypt_Click);
            // 
            // IsSenderComp
            // 
            this.IsSenderComp.AutoSize = true;
            this.IsSenderComp.Location = new System.Drawing.Point(165, 71);
            this.IsSenderComp.Name = "IsSenderComp";
            this.IsSenderComp.Size = new System.Drawing.Size(119, 21);
            this.IsSenderComp.TabIndex = 10;
            this.IsSenderComp.Text = "IsCompressed";
            this.IsSenderComp.UseVisualStyleBackColor = true;
            // 
            // txtSenderPublic
            // 
            this.txtSenderPublic.Location = new System.Drawing.Point(15, 98);
            this.txtSenderPublic.Multiline = true;
            this.txtSenderPublic.Name = "txtSenderPublic";
            this.txtSenderPublic.Size = new System.Drawing.Size(656, 51);
            this.txtSenderPublic.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Sender(OnPremise)";
            // 
            // txtPlainData
            // 
            this.txtPlainData.Location = new System.Drawing.Point(12, 213);
            this.txtPlainData.Multiline = true;
            this.txtPlainData.Name = "txtPlainData";
            this.txtPlainData.Size = new System.Drawing.Size(656, 31);
            this.txtPlainData.TabIndex = 13;
            // 
            // txtSenderSecret
            // 
            this.txtSenderSecret.Location = new System.Drawing.Point(12, 279);
            this.txtSenderSecret.Multiline = true;
            this.txtSenderSecret.Name = "txtSenderSecret";
            this.txtSenderSecret.Size = new System.Drawing.Size(656, 31);
            this.txtSenderSecret.TabIndex = 14;
            // 
            // txtSenderDerived
            // 
            this.txtSenderDerived.Location = new System.Drawing.Point(12, 316);
            this.txtSenderDerived.Multiline = true;
            this.txtSenderDerived.Name = "txtSenderDerived";
            this.txtSenderDerived.Size = new System.Drawing.Size(656, 31);
            this.txtSenderDerived.TabIndex = 15;
            // 
            // Encrypt
            // 
            this.Encrypt.Location = new System.Drawing.Point(549, 250);
            this.Encrypt.Name = "Encrypt";
            this.Encrypt.Size = new System.Drawing.Size(119, 27);
            this.Encrypt.TabIndex = 16;
            this.Encrypt.Text = "Encrypt Data";
            this.Encrypt.UseVisualStyleBackColor = true;
            this.Encrypt.Click += new System.EventHandler(this.Encrypt_Click);
            // 
            // IsReceiverComp
            // 
            this.IsReceiverComp.AutoSize = true;
            this.IsReceiverComp.Location = new System.Drawing.Point(169, 382);
            this.IsReceiverComp.Name = "IsReceiverComp";
            this.IsReceiverComp.Size = new System.Drawing.Size(119, 21);
            this.IsReceiverComp.TabIndex = 19;
            this.IsReceiverComp.Text = "IsCompressed";
            this.IsReceiverComp.UseVisualStyleBackColor = true;
            // 
            // GenerateReceiver
            // 
            this.GenerateReceiver.Location = new System.Drawing.Point(320, 378);
            this.GenerateReceiver.Name = "GenerateReceiver";
            this.GenerateReceiver.Size = new System.Drawing.Size(119, 27);
            this.GenerateReceiver.TabIndex = 18;
            this.GenerateReceiver.Text = "Generate";
            this.GenerateReceiver.UseVisualStyleBackColor = true;
            this.GenerateReceiver.Click += new System.EventHandler(this.GenerateReceiver_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "Create EC Key Pair";
            // 
            // txtReceiverDerived
            // 
            this.txtReceiverDerived.Location = new System.Drawing.Point(12, 630);
            this.txtReceiverDerived.Multiline = true;
            this.txtReceiverDerived.Name = "txtReceiverDerived";
            this.txtReceiverDerived.Size = new System.Drawing.Size(656, 31);
            this.txtReceiverDerived.TabIndex = 23;
            // 
            // txtReceiverSecret
            // 
            this.txtReceiverSecret.Location = new System.Drawing.Point(12, 593);
            this.txtReceiverSecret.Multiline = true;
            this.txtReceiverSecret.Name = "txtReceiverSecret";
            this.txtReceiverSecret.Size = new System.Drawing.Size(656, 31);
            this.txtReceiverSecret.TabIndex = 22;
            // 
            // txtChiperData
            // 
            this.txtChiperData.Location = new System.Drawing.Point(12, 525);
            this.txtChiperData.Multiline = true;
            this.txtChiperData.Name = "txtChiperData";
            this.txtChiperData.Size = new System.Drawing.Size(656, 31);
            this.txtChiperData.TabIndex = 21;
            // 
            // txtSenderPub
            // 
            this.txtSenderPub.Location = new System.Drawing.Point(12, 488);
            this.txtSenderPub.Multiline = true;
            this.txtSenderPub.Name = "txtSenderPub";
            this.txtSenderPub.Size = new System.Drawing.Size(656, 31);
            this.txtSenderPub.TabIndex = 20;
            // 
            // txtReceiverPub
            // 
            this.txtReceiverPub.Location = new System.Drawing.Point(15, 411);
            this.txtReceiverPub.Multiline = true;
            this.txtReceiverPub.Name = "txtReceiverPub";
            this.txtReceiverPub.Size = new System.Drawing.Size(656, 57);
            this.txtReceiverPub.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 468);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 17);
            this.label3.TabIndex = 25;
            this.label3.Text = "DecryptionProcess";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(671, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 17);
            this.label6.TabIndex = 26;
            this.label6.Text = "Sample Data(Plain Text)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(671, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(226, 17);
            this.label7.TabIndex = 27;
            this.label7.Text = "SharedSecret Key(Hex String)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(671, 330);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(257, 17);
            this.label8.TabIndex = 28;
            this.label8.Text = "DerivedSymmetric Key(Hex String)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(671, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(209, 17);
            this.label9.TabIndex = 29;
            this.label9.Text = "Reciver PublicKey (base64)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(675, 116);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(206, 17);
            this.label10.TabIndex = 30;
            this.label10.Text = "Sender PublicKey (base64)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(675, 424);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(218, 17);
            this.label11.TabIndex = 35;
            this.label11.Text = "Receiver PublicKey (base64)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(671, 502);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(206, 17);
            this.label12.TabIndex = 34;
            this.label12.Text = "Sender PublicKey (base64)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(668, 644);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(257, 17);
            this.label13.TabIndex = 33;
            this.label13.Text = "DerivedSymmetric Key(Hex String)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(671, 607);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(226, 17);
            this.label14.TabIndex = 32;
            this.label14.Text = "SharedSecret Key(Hex String)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(671, 539);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(186, 17);
            this.label15.TabIndex = 31;
            this.label15.Text = "Chiper Data (Hex String)";
            // 
            // txtDecryptedData
            // 
            this.txtDecryptedData.Location = new System.Drawing.Point(12, 682);
            this.txtDecryptedData.Multiline = true;
            this.txtDecryptedData.Name = "txtDecryptedData";
            this.txtDecryptedData.Size = new System.Drawing.Size(656, 31);
            this.txtDecryptedData.TabIndex = 36;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(671, 696);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(121, 17);
            this.label16.TabIndex = 37;
            this.label16.Text = "Decrypted Data";
            // 
            // EncyptionProcess
            // 
            this.ClientSize = new System.Drawing.Size(925, 722);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtDecryptedData);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtReceiverPub);
            this.Controls.Add(this.txtReceiverDerived);
            this.Controls.Add(this.txtReceiverSecret);
            this.Controls.Add(this.txtChiperData);
            this.Controls.Add(this.txtSenderPub);
            this.Controls.Add(this.IsReceiverComp);
            this.Controls.Add(this.GenerateReceiver);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Encrypt);
            this.Controls.Add(this.txtSenderDerived);
            this.Controls.Add(this.txtSenderSecret);
            this.Controls.Add(this.txtPlainData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSenderPublic);
            this.Controls.Add(this.IsSenderComp);
            this.Controls.Add(this.Decrypt);
            this.Controls.Add(this.GenerateSender);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReceiverPublic);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSampleText);
            this.Name = "EncyptionProcess";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblSampleText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReceiverPublic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GenerateSender;
        private System.Windows.Forms.Button Decrypt;
        private System.Windows.Forms.CheckBox IsSenderComp;
        private System.Windows.Forms.TextBox txtSenderPublic;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPlainData;
        private System.Windows.Forms.TextBox txtSenderSecret;
        private System.Windows.Forms.TextBox txtSenderDerived;
        private System.Windows.Forms.Button Encrypt;
        private System.Windows.Forms.CheckBox IsReceiverComp;
        private System.Windows.Forms.Button GenerateReceiver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtReceiverDerived;
        private System.Windows.Forms.TextBox txtReceiverSecret;
        private System.Windows.Forms.TextBox txtChiperData;
        private System.Windows.Forms.TextBox txtSenderPub;
        private System.Windows.Forms.TextBox txtReceiverPub;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDecryptedData;
        private System.Windows.Forms.Label label16;
    }
}