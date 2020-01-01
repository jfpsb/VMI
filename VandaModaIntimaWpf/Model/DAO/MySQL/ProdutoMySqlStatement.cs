using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class ProdutoMySqlStatement : IMySqlStatementStrategy
    {
        private readonly string TABELA = "produto";
        private readonly string APPDATAPATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public void Delete(IModel model)
        {
            Produto produto = (Produto)model;
            string statement = $"DELETE FROM {TABELA} WHERE cod_barra = '{produto.Cod_Barra}';";
        }

        public void Delete(IList<IModel> models)
        {
            foreach (IModel model in models)
            {
                Delete(model);
            }
        }

        public void Insert(IModel model)
        {
            Produto produto = (Produto)model;

            var fornecedor = produto.Fornecedor == null ? "null" : $"'{produto.Fornecedor.Cnpj}'";
            var marca = produto.Marca == null ? "null" : $"'{produto.Marca.Nome}'";

            string statement = $"INSERT INTO {TABELA} (cod_barra, fornecedor, marca, descricao, preco) VALUES ('{produto.Cod_Barra}',{fornecedor},{marca},'{produto.Descricao}',{produto.Preco});";
        }

        public void Insert(IList<IModel> models)
        {
            foreach(IModel m in models)
            {
                Produto produto = (Produto)m;

                var fornecedor = produto.Fornecedor == null ? "null" : $"'{produto.Fornecedor.Cnpj}'";
                var marca = produto.Marca == null ? "null" : $"'{produto.Marca.Nome}'";

                string statement = $"INSERT INTO {TABELA} (cod_barra, fornecedor, marca, descricao, preco) VALUES ('{produto.Cod_Barra}',{fornecedor},{marca},'{produto.Descricao}',{produto.Preco});";
            }
        }

        public void Update(IModel model)
        {
            throw new NotImplementedException();
        }

        public void Update(IList<IModel> models)
        {
            throw new NotImplementedException();
        }
    }
}
