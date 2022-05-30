using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.SQL;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    public class PesquisarEntradasPorFornecedorVM : APesquisarViewModel<Model.EntradaMercadoriaProdutoGrade>
    {
        private DAOFornecedor daoFornecedor;
        private DateTime _dataEscolhida;
        private Model.Fornecedor _fornecedor;
        private ObservableCollection<EntradaMercadoriaProdutoGrade> _entradas = new ObservableCollection<EntradaMercadoriaProdutoGrade>();
        private ObservableCollection<Model.Fornecedor> _fornecedores = new ObservableCollection<Model.Fornecedor>();
        public PesquisarEntradasPorFornecedorVM()
        {
            daoEntidade = new DAOEntradaMercadoriaProdutoGrade(_session);
            daoFornecedor = new DAOFornecedor(_session);
            pesquisarViewModelStrategy = new PesquisarEntradasPorFornecedorVMStrategy();
            excelStrategy = new EntradaMercadoriaPorFornecedorExcelStrategy();

            GetFornecedores();

            DataEscolhida = DateTime.Now;
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<Model.Fornecedor>(await daoFornecedor.Listar());
            Fornecedor = Fornecedores[0];
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public async override Task PesquisaItens(string termo)
        {
            if (DataEscolhida.Year == 1)
                return;

            var dao = daoEntidade as DAOEntradaMercadoriaProdutoGrade;
            Entradas = new ObservableCollection<EntradaMercadoriaProdutoGrade>(await dao.ListarPorPeriodoFornecedor(DataEscolhida, Fornecedor));
        }

        protected override WorksheetContainer<EntradaMercadoriaProdutoGrade>[] GetWorksheetContainers()
        {
            if (Entradas.Count > 0)
            {
                var worksheets = new WorksheetContainer<Model.EntradaMercadoriaProdutoGrade>[1];
                worksheets[0] = new WorksheetContainer<Model.EntradaMercadoriaProdutoGrade>()
                {
                    Nome = "Entradas Por Fornecedor",
                    Lista = Entradas
                };

                return worksheets;
            }
            else
            {
                MessageBoxService.Show("Não Há Itens Listados!", "Exportar Para Excel", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

        public override ACadastrarViewModel<EntradaMercadoriaProdutoGrade> GetCadastrarViewModel()
        {
            throw new NotImplementedException();
        }

        public override ACadastrarViewModel<EntradaMercadoriaProdutoGrade> GetEditarViewModel()
        {
            throw new NotImplementedException();
        }

        public override AAjudarVM GetAjudaVM()
        {
            throw new NotImplementedException();
        }

        public override ExportarSQLViewModel<EntradaMercadoriaProdutoGrade> GetExportaSQLVM()
        {
            throw new NotImplementedException();
        }

        public override ATelaRelatorio GetTelaRelatorioVM()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<EntradaMercadoriaProdutoGrade> Entradas
        {
            get => _entradas;
            set
            {
                _entradas = value;
                OnPropertyChanged("Entradas");
            }
        }

        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public Model.Fornecedor Fornecedor
        {
            get => _fornecedor;
            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public ObservableCollection<Model.Fornecedor> Fornecedores
        {
            get => _fornecedores;
            set
            {
                _fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }
    }
}
