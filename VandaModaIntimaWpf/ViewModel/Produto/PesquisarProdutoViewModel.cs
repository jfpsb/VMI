using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class PesquisarProdutoViewModel : APesquisarViewModel<ProdutoModel>
    {        
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Descricao,
            CodBarra,
            Fornecedor,
            Marca
        }
        public PesquisarProdutoViewModel()
        {
            daoEntidade = new DAOProduto(_session);
            excelStrategy = new ExcelStrategy(new ProdutoExcelStrategy(_session));
            pesquisarViewModelStrategy = new PesquisarProdutoViewModelStrategy();
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
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
        public override async void GetItems(string termo)
        {
            DAOProduto daoProduto = (DAOProduto)daoEntidade;

            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Descricao:
                    Entidades = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorDescricao(termo)));
                    break;
                case (int)OpcoesPesquisa.CodBarra:
                    Entidades = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorCodigoDeBarra(termo)));
                    break;
                case (int)OpcoesPesquisa.Fornecedor:
                    Entidades = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorFornecedor(termo)));
                    break;
                case (int)OpcoesPesquisa.Marca:
                    Entidades = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorMarca(termo)));
                    break;
            }
        }
        public override bool IsEditable(object parameter)
        {
            return true;
        }
    }
}
