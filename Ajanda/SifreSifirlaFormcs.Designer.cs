namespace Ajanda
{
    partial class SifreSifirlaForm
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
            this.txtEposta = new System.Windows.Forms.TextBox();
            this.txtYeniSifre = new System.Windows.Forms.TextBox();
            this.txtYeniSifreTekrar = new System.Windows.Forms.TextBox();
            this.btnSifreSifirla = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtEposta
            // 
            this.txtEposta.Location = new System.Drawing.Point(244, 100);
            this.txtEposta.Name = "txtEposta";
            this.txtEposta.Size = new System.Drawing.Size(100, 22);
            this.txtEposta.TabIndex = 0;
            // 
            // txtYeniSifre
            // 
            this.txtYeniSifre.Location = new System.Drawing.Point(244, 150);
            this.txtYeniSifre.Name = "txtYeniSifre";
            this.txtYeniSifre.Size = new System.Drawing.Size(100, 22);
            this.txtYeniSifre.TabIndex = 1;
            // 
            // txtYeniSifreTekrar
            // 
            this.txtYeniSifreTekrar.Location = new System.Drawing.Point(244, 195);
            this.txtYeniSifreTekrar.Name = "txtYeniSifreTekrar";
            this.txtYeniSifreTekrar.Size = new System.Drawing.Size(100, 22);
            this.txtYeniSifreTekrar.TabIndex = 2;
            // 
            // btnSifreSifirla
            // 
            this.btnSifreSifirla.Location = new System.Drawing.Point(244, 257);
            this.btnSifreSifirla.Name = "btnSifreSifirla";
            this.btnSifreSifirla.Size = new System.Drawing.Size(100, 23);
            this.btnSifreSifirla.TabIndex = 3;
            this.btnSifreSifirla.Text = "Sıfırla";
            this.btnSifreSifirla.UseVisualStyleBackColor = true;
            this.btnSifreSifirla.Click += new System.EventHandler(this.btnSifreSifirla_Click);
            // 
            // SifreSifirlaFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSifreSifirla);
            this.Controls.Add(this.txtYeniSifreTekrar);
            this.Controls.Add(this.txtYeniSifre);
            this.Controls.Add(this.txtEposta);
            this.Name = "SifreSifirlaFormcs";
            this.Text = "SifreSifirlaFormcs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEposta;
        private System.Windows.Forms.TextBox txtYeniSifre;
        private System.Windows.Forms.TextBox txtYeniSifreTekrar;
        private System.Windows.Forms.Button btnSifreSifirla;
    }
}