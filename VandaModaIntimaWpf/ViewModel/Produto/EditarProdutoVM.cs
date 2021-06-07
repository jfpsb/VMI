using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVM : CadastrarProdutoVM
    {
        private ProdutoModel produtoOriginal;
        private HistoricoProduto historicoProduto;
        public EditarProdutoVM(ISession session, ProdutoModel produto, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarProdutoVMStrategy();
            Entidade = produto;
            produtoOriginal = produto.Clone() as ProdutoModel;
            Entidade.PropertyChanged += HistoricoProdutoChanged;
            ProdutoGrade.Produto = Entidade;
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(Entidade.Grades);
            historicoProduto = new HistoricoProduto()
            {
                Produto = Entidade
            };

            AntesDeInserirNoBancoDeDados += InsereHistoricoProduto;
        }

        private void InsereHistoricoProduto()
        {
            Entidade.Historico.Add(historicoProduto);
        }

        private void HistoricoProdutoChanged(object sender, PropertyChangedEventArgs e)
        {
            string descricao = "";

            if (!Entidade.Descricao.Equals(produtoOriginal.Descricao))
            {
                descricao += $"{produtoOriginal.CodBarra} - {produtoOriginal.Descricao} MUDOU DE NOME PARA {produtoOriginal.CodBarra} - {Entidade.Descricao}";
            }

            if (Entidade.Preco != produtoOriginal.Preco)
            {
                if (descricao.Length != 0)
                    descricao += "\n";

                descricao += $"{produtoOriginal.CodBarra} - {produtoOriginal.Descricao}";

                if (Entidade.Preco < produtoOriginal.Preco)
                {
                    descricao += " DIMINUIU PARA";
                }
                else
                {
                    descricao += " AUMENTOU PARA";
                }

                descricao += $" {Entidade.Preco.ToString("C", CultureInfo.CurrentCulture)}";
            }

            historicoProduto.DataAlteracao = DateTime.Now;
            historicoProduto.Descricao = descricao;
        }
    }
}
