namespace Guldkortet
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
            this.components = new System.ComponentModel.Container();
            this.lblNos = new System.Windows.Forms.Label();
            this.lblKortnummer = new System.Windows.Forms.Label();
            this.lblResultat = new System.Windows.Forms.Label();
            this.lblSvarResultat = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtKortnummer = new System.Windows.Forms.TextBox();
            this.lstKoder = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSparaFil = new System.Windows.Forms.Button();
            this.btnAvsluta = new System.Windows.Forms.Button();
            this.btnValideraKod = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNos
            // 
            this.lblNos.AutoSize = true;
            this.lblNos.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.lblNos.Location = new System.Drawing.Point(42, 144);
            this.lblNos.Name = "lblNos";
            this.lblNos.Size = new System.Drawing.Size(254, 20);
            this.lblNos.TabIndex = 0;
            this.lblNos.Text = "Mottagna koder från NOS_Export: ";
            // 
            // lblKortnummer
            // 
            this.lblKortnummer.AutoSize = true;
            this.lblKortnummer.Location = new System.Drawing.Point(42, 35);
            this.lblKortnummer.Name = "lblKortnummer";
            this.lblKortnummer.Size = new System.Drawing.Size(104, 20);
            this.lblKortnummer.TabIndex = 1;
            this.lblKortnummer.Text = "Kortnummer: ";
            // 
            // lblResultat
            // 
            this.lblResultat.AutoSize = true;
            this.lblResultat.Location = new System.Drawing.Point(42, 90);
            this.lblResultat.Name = "lblResultat";
            this.lblResultat.Size = new System.Drawing.Size(77, 20);
            this.lblResultat.TabIndex = 2;
            this.lblResultat.Text = "Resultat: ";
            // 
            // lblSvarResultat
            // 
            this.lblSvarResultat.AutoSize = true;
            this.lblSvarResultat.Location = new System.Drawing.Point(166, 89);
            this.lblSvarResultat.Name = "lblSvarResultat";
            this.lblSvarResultat.Size = new System.Drawing.Size(0, 20);
            this.lblSvarResultat.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtKortnummer
            // 
            this.txtKortnummer.Location = new System.Drawing.Point(170, 29);
            this.txtKortnummer.Name = "txtKortnummer";
            this.txtKortnummer.Size = new System.Drawing.Size(423, 26);
            this.txtKortnummer.TabIndex = 6;
            // 
            // lstKoder
            // 
            this.lstKoder.FormattingEnabled = true;
            this.lstKoder.ItemHeight = 20;
            this.lstKoder.Location = new System.Drawing.Point(46, 177);
            this.lstKoder.Name = "lstKoder";
            this.lstKoder.Size = new System.Drawing.Size(691, 204);
            this.lstKoder.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(42, 471);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(143, 20);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status: Ej ansluten";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label1.Location = new System.Drawing.Point(42, 442);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(693, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "____________________________________________________________________________";
            // 
            // btnSparaFil
            // 
            this.btnSparaFil.Location = new System.Drawing.Point(226, 404);
            this.btnSparaFil.Name = "btnSparaFil";
            this.btnSparaFil.Size = new System.Drawing.Size(146, 35);
            this.btnSparaFil.TabIndex = 10;
            this.btnSparaFil.Text = "Spara till textfil";
            this.btnSparaFil.UseVisualStyleBackColor = true;
            this.btnSparaFil.Click += new System.EventHandler(this.btnSparaFil_Click);
            // 
            // btnAvsluta
            // 
            this.btnAvsluta.Location = new System.Drawing.Point(418, 404);
            this.btnAvsluta.Name = "btnAvsluta";
            this.btnAvsluta.Size = new System.Drawing.Size(146, 35);
            this.btnAvsluta.TabIndex = 11;
            this.btnAvsluta.Text = "Avsluta";
            this.btnAvsluta.UseVisualStyleBackColor = true;
            this.btnAvsluta.Click += new System.EventHandler(this.btnAvsluta_Click);
            // 
            // btnValideraKod
            // 
            this.btnValideraKod.BackColor = System.Drawing.Color.White;
            this.btnValideraKod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnValideraKod.ForeColor = System.Drawing.Color.Black;
            this.btnValideraKod.Location = new System.Drawing.Point(618, 26);
            this.btnValideraKod.Name = "btnValideraKod";
            this.btnValideraKod.Size = new System.Drawing.Size(119, 32);
            this.btnValideraKod.TabIndex = 12;
            this.btnValideraKod.Text = "Validera kod";
            this.btnValideraKod.UseVisualStyleBackColor = false;
            this.btnValideraKod.Click += new System.EventHandler(this.btnValideraKort_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 512);
            this.Controls.Add(this.btnValideraKod);
            this.Controls.Add(this.btnAvsluta);
            this.Controls.Add(this.btnSparaFil);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lstKoder);
            this.Controls.Add(this.txtKortnummer);
            this.Controls.Add(this.lblSvarResultat);
            this.Controls.Add(this.lblResultat);
            this.Controls.Add(this.lblKortnummer);
            this.Controls.Add(this.lblNos);
            this.Name = "Form1";
            this.Text = "Guldkortet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNos;
        private System.Windows.Forms.Label lblKortnummer;
        private System.Windows.Forms.Label lblResultat;
        private System.Windows.Forms.Label lblSvarResultat;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtKortnummer;
        private System.Windows.Forms.ListBox lstKoder;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSparaFil;
        private System.Windows.Forms.Button btnAvsluta;
        private System.Windows.Forms.Button btnValideraKod;
    }
}

