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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PesquisarProduto));
            this.MenuProduto = new System.Windows.Forms.MenuStrip();
            this.cadastrarNovoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelPesquisarPor = new System.Windows.Forms.Label();
            this.CmbPesquisarPor = new System.Windows.Forms.ComboBox();
            this.dgvProduto = new System.Windows.Forms.DataGridView();
            this.labelPesquisa = new System.Windows.Forms.Label();
            this.TxtPesquisa = new System.Windows.Forms.TextBox();
            this.marcar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cod_barra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fornecedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.marca = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.MenuProduto.Size = new System.Drawing.Size(684, 25);
            this.MenuProduto.TabIndex = 0;
            this.MenuProduto.Text = "Opções de Produto";
            // 
            // cadastrarNovoToolStripMenuItem
            // 
            this.cadastrarNovoToolStripMenuItem.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cadastrarNovoToolStripMenuItem.Name = "cadastrarNovoToolStripMenuItem";
            this.cadastrarNovoToolStripMenuItem.Size = new System.Drawing.Size(125, 21);
            this.cadastrarNovoToolStripMenuItem.Text = "Cadastrar Novo";
            this.cadastrarNovoToolStripMenuItem.Click += new System.EventHandler(this.CadastrarNovoToolStripMenuItem_Click);
            // 
            // labelPesquisarPor
            // 
            this.labelPesquisarPor.AutoSize = true;
            this.labelPesquisarPor.Location = new System.Drawing.Point(13, 36);
            this.labelPesquisarPor.Name = "labelPesquisarPor";
            this.labelPesquisarPor.Size = new System.Drawing.Size(113, 21);
            this.labelPesquisarPor.TabIndex = 1;
            this.labelPesquisarPor.Text = "Pesquisar Por:";
            // 
            // CmbPesquisarPor
            // 
            this.CmbPesquisarPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbPesquisarPor.FormattingEnabled = true;
            this.CmbPesquisarPor.Items.AddRange(new object[] {
            "Descrição",
            "Código De Barras",
            "Fornecedor",
            "Marca"});
            this.CmbPesquisarPor.Location = new System.Drawing.Point(132, 33);
            this.CmbPesquisarPor.Name = "CmbPesquisarPor";
            this.CmbPesquisarPor.Size = new System.Drawing.Size(230, 29);
            this.CmbPesquisarPor.TabIndex = 2;
            this.CmbPesquisarPor.SelectedIndexChanged += new System.EventHandler(this.CmbPesquisarPor_SelectedIndexChanged);
            // 
            // dgvProduto
            // 
            this.dgvProduto.AllowUserToAddRows = false;
            this.dgvProduto.AllowUserToDeleteRows = false;
            this.dgvProduto.AllowUserToOrderColumns = true;
            this.dgvProduto.AllowUserToResizeRows = false;
            this.dgvProduto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProduto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProduto.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProduto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.marcar,
            this.cod_barra,
            this.descricao,
            this.preco,
            this.fornecedor,
            this.marca});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProduto.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProduto.Location = new System.Drawing.Point(13, 106);
            this.dgvProduto.Name = "dgvProduto";
            this.dgvProduto.RowHeadersVisible = false;
            this.dgvProduto.Size = new System.Drawing.Size(658, 293);
            this.dgvProduto.TabIndex = 3;
            // 
            // labelPesquisa
            // 
            this.labelPesquisa.AutoSize = true;
            this.labelPesquisa.Location = new System.Drawing.Point(13, 76);
            this.labelPesquisa.Name = "labelPesquisa";
            this.labelPesquisa.Size = new System.Drawing.Size(80, 21);
            this.labelPesquisa.TabIndex = 4;
            this.labelPesquisa.Text = "Pesquisa:";
            // 
            // TxtPesquisa
            // 
            this.TxtPesquisa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtPesquisa.Location = new System.Drawing.Point(132, 73);
            this.TxtPesquisa.Name = "TxtPesquisa";
            this.TxtPesquisa.Size = new System.Drawing.Size(539, 27);
            this.TxtPesquisa.TabIndex = 5;
            this.TxtPesquisa.TextChanged += new System.EventHandler(this.TxtPesquisa_TextChanged);
            // 
            // marcar
            // 
            this.marcar.HeaderText = "      ";
            this.marcar.MinimumWidth = 25;
            this.marcar.Name = "marcar";
            this.marcar.Width = 36;
            // 
            // cod_barra
            // 
            this.cod_barra.DataPropertyName = "Cod_Barra";
            this.cod_barra.HeaderText = "Cód. De Barras";
            this.cod_barra.Name = "cod_barra";
            this.cod_barra.ReadOnly = true;
            this.cod_barra.Width = 150;
            // 
            // descricao
            // 
            this.descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricao.DataPropertyName = "Descricao";
            this.descricao.HeaderText = "Descrição";
            this.descricao.Name = "descricao";
            // 
            // preco
            // 
            this.preco.DataPropertyName = "Preco";
            this.preco.HeaderText = "Preço";
            this.preco.Name = "preco";
            this.preco.Width = 79;
            // 
            // fornecedor
            // 
            this.fornecedor.DataPropertyName = "FornecedorNome";
            this.fornecedor.HeaderText = "Fornecedor";
            this.fornecedor.Name = "fornecedor";
            this.fornecedor.ReadOnly = true;
            this.fornecedor.Width = 124;
            // 
            // marca
            // 
            this.marca.DataPropertyName = "MarcaNome";
            this.marca.HeaderText = "Marca";
            this.marca.Name = "marca";
            this.marca.ReadOnly = true;
            this.marca.Width = 87;
            // 
            // PesquisarProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.TxtPesquisa);
            this.Controls.Add(this.labelPesquisa);
            this.Controls.Add(this.dgvProduto);
            this.Controls.Add(this.CmbPesquisarPor);
            this.Controls.Add(this.labelPesquisarPor);
            this.Controls.Add(this.MenuProduto);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "PesquisarProduto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PesquisarProduto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PesquisarProduto_FormClosing);
            this.Load += new System.EventHandler(this.PesquisarProduto_Load);
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
        private System.Windows.Forms.ComboBox CmbPesquisarPor;
        private System.Windows.Forms.DataGridView dgvProduto;
        private System.Windows.Forms.Label labelPesquisa;
        private System.Windows.Forms.TextBox TxtPesquisa;
        private System.Windows.Forms.DataGridViewCheckBoxColumn marcar;
        private System.Windows.Forms.DataGridViewTextBoxColumn cod_barra;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn preco;
        private System.Windows.Forms.DataGridViewTextBoxColumn fornecedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn marca;
    }
}