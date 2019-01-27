namespace VandaModaIntima.View.Produto
{
    partial class CadastrarProduto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CadastrarProduto));
            this.labelCodigoBarra = new System.Windows.Forms.Label();
            this.TxtCodigoBarra = new System.Windows.Forms.TextBox();
            this.TxtPreco = new System.Windows.Forms.TextBox();
            this.labelPreco = new System.Windows.Forms.Label();
            this.TxtDescricao = new System.Windows.Forms.TextBox();
            this.labelDescricao = new System.Windows.Forms.Label();
            this.labelFornecedor = new System.Windows.Forms.Label();
            this.labelMarca = new System.Windows.Forms.Label();
            this.CmbFornecedor = new System.Windows.Forms.ComboBox();
            this.CmbMarca = new System.Windows.Forms.ComboBox();
            this.BtnCadastrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelCodigoBarra
            // 
            this.labelCodigoBarra.AutoSize = true;
            this.labelCodigoBarra.Location = new System.Drawing.Point(44, 29);
            this.labelCodigoBarra.Name = "labelCodigoBarra";
            this.labelCodigoBarra.Size = new System.Drawing.Size(148, 21);
            this.labelCodigoBarra.TabIndex = 0;
            this.labelCodigoBarra.Text = "Código de Barras:";
            // 
            // TxtCodigoBarra
            // 
            this.TxtCodigoBarra.Location = new System.Drawing.Point(198, 26);
            this.TxtCodigoBarra.MaxLength = 15;
            this.TxtCodigoBarra.Name = "TxtCodigoBarra";
            this.TxtCodigoBarra.Size = new System.Drawing.Size(245, 27);
            this.TxtCodigoBarra.TabIndex = 1;
            // 
            // TxtPreco
            // 
            this.TxtPreco.Location = new System.Drawing.Point(198, 92);
            this.TxtPreco.MaxLength = 6;
            this.TxtPreco.Name = "TxtPreco";
            this.TxtPreco.Size = new System.Drawing.Size(245, 27);
            this.TxtPreco.TabIndex = 3;
            // 
            // labelPreco
            // 
            this.labelPreco.AutoSize = true;
            this.labelPreco.Location = new System.Drawing.Point(44, 95);
            this.labelPreco.Name = "labelPreco";
            this.labelPreco.Size = new System.Drawing.Size(58, 21);
            this.labelPreco.TabIndex = 2;
            this.labelPreco.Text = "Preço:";
            // 
            // TxtDescricao
            // 
            this.TxtDescricao.Location = new System.Drawing.Point(198, 59);
            this.TxtDescricao.MaxLength = 100;
            this.TxtDescricao.Name = "TxtDescricao";
            this.TxtDescricao.Size = new System.Drawing.Size(245, 27);
            this.TxtDescricao.TabIndex = 5;
            // 
            // labelDescricao
            // 
            this.labelDescricao.AutoSize = true;
            this.labelDescricao.Location = new System.Drawing.Point(44, 62);
            this.labelDescricao.Name = "labelDescricao";
            this.labelDescricao.Size = new System.Drawing.Size(91, 21);
            this.labelDescricao.TabIndex = 4;
            this.labelDescricao.Text = "Descrição:";
            // 
            // labelFornecedor
            // 
            this.labelFornecedor.AutoSize = true;
            this.labelFornecedor.Location = new System.Drawing.Point(44, 128);
            this.labelFornecedor.Name = "labelFornecedor";
            this.labelFornecedor.Size = new System.Drawing.Size(103, 21);
            this.labelFornecedor.TabIndex = 6;
            this.labelFornecedor.Text = "Fornecedor:";
            // 
            // labelMarca
            // 
            this.labelMarca.AutoSize = true;
            this.labelMarca.Location = new System.Drawing.Point(44, 163);
            this.labelMarca.Name = "labelMarca";
            this.labelMarca.Size = new System.Drawing.Size(66, 21);
            this.labelMarca.TabIndex = 7;
            this.labelMarca.Text = "Marca:";
            // 
            // CmbFornecedor
            // 
            this.CmbFornecedor.FormattingEnabled = true;
            this.CmbFornecedor.Location = new System.Drawing.Point(198, 125);
            this.CmbFornecedor.Name = "CmbFornecedor";
            this.CmbFornecedor.Size = new System.Drawing.Size(245, 29);
            this.CmbFornecedor.TabIndex = 8;
            // 
            // CmbMarca
            // 
            this.CmbMarca.FormattingEnabled = true;
            this.CmbMarca.Location = new System.Drawing.Point(198, 160);
            this.CmbMarca.Name = "CmbMarca";
            this.CmbMarca.Size = new System.Drawing.Size(245, 29);
            this.CmbMarca.TabIndex = 9;
            // 
            // BtnCadastrar
            // 
            this.BtnCadastrar.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCadastrar.Location = new System.Drawing.Point(138, 212);
            this.BtnCadastrar.Name = "BtnCadastrar";
            this.BtnCadastrar.Size = new System.Drawing.Size(210, 60);
            this.BtnCadastrar.TabIndex = 10;
            this.BtnCadastrar.Text = "Cadastrar";
            this.BtnCadastrar.UseVisualStyleBackColor = true;
            this.BtnCadastrar.Click += new System.EventHandler(this.BtnCadastrar_Click);
            // 
            // CadastrarProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 287);
            this.Controls.Add(this.BtnCadastrar);
            this.Controls.Add(this.CmbMarca);
            this.Controls.Add(this.CmbFornecedor);
            this.Controls.Add(this.labelMarca);
            this.Controls.Add(this.labelFornecedor);
            this.Controls.Add(this.TxtDescricao);
            this.Controls.Add(this.labelDescricao);
            this.Controls.Add(this.TxtPreco);
            this.Controls.Add(this.labelPreco);
            this.Controls.Add(this.TxtCodigoBarra);
            this.Controls.Add(this.labelCodigoBarra);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "CadastrarProduto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CadastrarProduto";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCodigoBarra;
        private System.Windows.Forms.TextBox TxtCodigoBarra;
        private System.Windows.Forms.TextBox TxtPreco;
        private System.Windows.Forms.Label labelPreco;
        private System.Windows.Forms.TextBox TxtDescricao;
        private System.Windows.Forms.Label labelDescricao;
        private System.Windows.Forms.Label labelFornecedor;
        private System.Windows.Forms.Label labelMarca;
        private System.Windows.Forms.ComboBox CmbFornecedor;
        private System.Windows.Forms.ComboBox CmbMarca;
        private System.Windows.Forms.Button BtnCadastrar;
    }
}