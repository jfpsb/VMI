using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class PesquisarProdutoVM : APesquisarViewModel<ProdutoModel>
    {
        private int pesquisarPor;
        public ICommand ListarMargensDeLucroComando { get; set; }

        private enum OpcoesPesquisa
        {
            Descricao,
            CodBarra,
            Fornecedor,
            Marca
        }
        public PesquisarProdutoVM()
        {
            daoEntidade = new DAOProduto(_session);
            excelStrategy = new ProdutoExcelStrategy(_session);
            pesquisarViewModelStrategy = new PesqProdutoMsgVMStrategy();
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;

            ListarMargensDeLucroComando = new RelayCommand(ListarMargensDeLucro);
        }

        private void ListarMargensDeLucro(object obj)
        {
            VisualizarMargensDeLucroVM viewModel = new VisualizarMargensDeLucroVM(_session);
            VisualizarMargensDeLucro view = new VisualizarMargensDeLucro
            {
                DataContext = viewModel
            };
            view.ShowDialog();
        }

        public int PesquisarPor
        {
            get { return pesquisarPor; }
            set
            {
                pesquisarPor = value;
                OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
            }
        }
        public override async Task PesquisaItens(string termo)
        {
            if (termo == null)
                return;

            IList<EntidadeComCampo<ProdutoModel>> ents = new List<EntidadeComCampo<ProdutoModel>>();

            DAOProduto daoProduto = (DAOProduto)daoEntidade;

            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Descricao:
                    ents = EntidadeComCampo<ProdutoModel>.CriarListaEntidadeComCampo(await daoProduto.ListarPorDescricao(termo));
                    break;
                case (int)OpcoesPesquisa.CodBarra:
                    ents = EntidadeComCampo<ProdutoModel>.CriarListaEntidadeComCampo(await daoProduto.ListarPorCodigoDeBarra(termo));
                    break;
                case (int)OpcoesPesquisa.Fornecedor:
                    ents = EntidadeComCampo<ProdutoModel>.CriarListaEntidadeComCampo(await daoProduto.ListarPorFornecedor(termo));
                    break;
                case (int)OpcoesPesquisa.Marca:
                    ents = EntidadeComCampo<ProdutoModel>.CriarListaEntidadeComCampo(await daoProduto.ListarPorMarca(termo));
                    break;
            }

            Entidades = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(ents);
        }
        public override bool Editavel(object parameter)
        {
            return true;
        }

        protected override WorksheetContainer<ProdutoModel>[] GetWorksheetContainers()
        {
            var worksheets = new WorksheetContainer<ProdutoModel>[1];
            worksheets[0] = new WorksheetContainer<ProdutoModel>()
            {
                Nome = "Produtos",
                Lista = Entidades.Select(s => s.Entidade).ToList()
            };

            return worksheets;
        }
    }
}
