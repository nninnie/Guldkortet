namespace Guldkortet2
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
            this.lstMottagnaKoder = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
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
            this.txtKortnummer.Size = new System.Drawing.Size(441, 26);
            this.txtKortnummer.TabIndex = 6;
            // 
            // lstMottagnaKoder
            // 
            this.lstMottagnaKoder.FormattingEnabled = true;
            this.lstMottagnaKoder.ItemHeight = 20;
            this.lstMottagnaKoder.Location = new System.Drawing.Point(46, 177);
            this.lstMottagnaKoder.Name = "lstMottagnaKoder";
            this.lstMottagnaKoder.Size = new System.Drawing.Size(691, 204);
            this.lstMottagnaKoder.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(46, 400);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(143, 20);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status: Ej ansluten";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lstMottagnaKoder);
            this.Controls.Add(this.txtKortnummer);
            this.Controls.Add(this.lblSvarResultat);
            this.Controls.Add(this.lblResultat);
            this.Controls.Add(this.lblKortnummer);
            this.Controls.Add(this.lblNos);
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.ListBox lstMottagnaKoder;
        private System.Windows.Forms.Label lblStatus;
    }
}

