namespace VandaModaIntima.View.Produto
{
    partial class PesquisarProduto
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
            this.MenuProduto = new System.Windows.Forms.MenuStrip();
            this.cadastrarNovoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelPesquisarPor = new System.Windows.Forms.Label();
            this.cmbPesquisarPor = new System.Windows.Forms.ComboBox();
            this.dgvProduto = new System.Windows.Forms.DataGridView();
            this.labelPesquisa = new System.Windows.Forms.Label();
            this.txtPesquisa = new System.Windows.Forms.TextBox();
            this.MenuProduto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduto)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuProduto
            // 
            this.MenuProduto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cadastrarNovoToolStripMenuItem});
            this.MenuProduto.Location = new System.Drawing.Point(0, 0);
            this.MenuProduto.Name = "MenuProduto";
            this.MenuProduto.Size = new System.Drawing.Size(683, 25);
            this.MenuProduto.TabIndex = 0;
            this.MenuProduto.Text = "Opções de Produto";
            // 
            // cadastrarNovoToolStripMenuItem
            // 
            this.cadastrarNovoToolStripMenuItem.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cadastrarNovoToolStripMenuItem.Name = "cadastrarNovoToolStripMenuItem";
            this.cadastrarNovoToolStripMenuItem.Size = new System.Drawing.Size(125, 21);
            this.cadastrarNovoToolStripMenuItem.Text = "Cadastrar Novo";
            this.cadastrarNovoToolStripMenuItem.Click += new System.EventHandler(this.cadastrarNovoToolStripMenuItem_Click);
            // 
            // labelPesquisarPor
            // 
            this.labelPesquisarPor.AutoSize = true;
            this.labelPesquisarPor.Location = new System.Drawing.Point(12, 36);
            this.labelPesquisarPor.Name = "labelPesquisarPor";
            this.labelPesquisarPor.Size = new System.Drawing.Size(113, 21);
            this.labelPesquisarPor.TabIndex = 1;
            this.labelPesquisarPor.Text = "Pesquisar Por:";
            // 
            // cmbPesquisarPor
            // 
            this.cmbPesquisarPor.FormattingEnabled = true;
            this.cmbPesquisarPor.Items.AddRange(new object[] {
            "Descrição",
            "Código De Barras",
            "Fornecedor",
            "Marca"});
            this.cmbPesquisarPor.Location = new System.Drawing.Point(131, 33);
            this.cmbPesquisarPor.Name = "cmbPesquisarPor";
            this.cmbPesquisarPor.Size = new System.Drawing.Size(216, 29);
            this.cmbPesquisarPor.TabIndex = 2;
            // 
            // dgvProduto
            // 
            this.dgvProduto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduto.Location = new System.Drawing.Point(12, 106);
            this.dgvProduto.Name = "dgvProduto";
            this.dgvProduto.Size = new System.Drawing.Size(658, 292);
            this.dgvProduto.TabIndex = 3;
            // 
            // labelPesquisa
            // 
            this.labelPesquisa.AutoSize = true;
            this.labelPesquisa.Location = new System.Drawing.Point(12, 76);
            this.labelPesquisa.Name = "labelPesquisa";
            this.labelPesquisa.Size = new System.Drawing.Size(80, 21);
            this.labelPesquisa.TabIndex = 4;
            this.labelPesquisa.Text = "Pesquisa:";
            // 
            // txtPesquisa
            // 
            this.txtPesquisa.Location = new System.Drawing.Point(131, 73);
            this.txtPesquisa.Name = "txtPesquisa";
            this.txtPesquisa.Size = new System.Drawing.Size(539, 27);
            this.txtPesquisa.TabIndex = 5;
            // 
            // PesquisarProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 410);
            this.Controls.Add(this.txtPesquisa);
            this.Controls.Add(this.labelPesquisa);
            this.Controls.Add(this.dgvProduto);
            this.Controls.Add(this.cmbPesquisarPor);
            this.Controls.Add(this.labelPesquisarPor);
            this.Controls.Add(this.MenuProduto);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PesquisarProduto";
            this.Text = "PesquisarProduto";
            this.MenuProduto.ResumeLayout(false);
            this.MenuProduto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuProduto;
        private System.Windows.Forms.ToolStripMenuItem cadastrarNovoToolStripMenuItem;
        private System.Windows.Forms.Label labelPesquisarPor;
        private System.Windows.Forms.ComboBox cmbPesquisarPor;
        private System.Windows.Forms.DataGridView dgvProduto;
        private System.Windows.Forms.Label labelPesquisa;
        private System.Windows.Forms.TextBox txtPesquisa;
    }
}