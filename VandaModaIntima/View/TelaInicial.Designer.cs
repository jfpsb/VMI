namespace VandaModaIntima
{
    partial class TelaInicial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TelaInicial));
            this.BtnProduto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnProduto
            // 
            this.BtnProduto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnProduto.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnProduto.Image = ((System.Drawing.Image)(resources.GetObject("BtnProduto.Image")));
            this.BtnProduto.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnProduto.Location = new System.Drawing.Point(12, 12);
            this.BtnProduto.Name = "BtnProduto";
            this.BtnProduto.Size = new System.Drawing.Size(320, 300);
            this.BtnProduto.TabIndex = 0;
            this.BtnProduto.Text = "Produtos";
            this.BtnProduto.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnProduto.UseVisualStyleBackColor = true;
            this.BtnProduto.Click += new System.EventHandler(this.BtnProduto_Click);
            // 
            // TelaInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 476);
            this.Controls.Add(this.BtnProduto);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TelaInicial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vanda Moda Íntima";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TelaInicial_FormClosing);
            this.Load += new System.EventHandler(this.TelaInicial_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnProduto;
    }
}

