using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class PesquisarContagemVM : APesquisarViewModel<ContagemModel>
    {
        private DAOLoja daoLoja;
        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ICommand AbrirVisualizarContagemProdutoComando { get; set; }
        public ICommand AbrirCadastrarTipoContagemComando { get; set; }
        public ICommand AbrirPesquisarTipoContagemComando { get; set; }

        private DateTime _dataInicial;
        private DateTime _dataFinal;
        private LojaModel _loja;

        public PesquisarContagemVM(IMessageBoxService messageBoxService) : base(messageBoxService)
        {
            daoLoja = new DAOLoja(_session);
            daoEntidade = new DAOContagem(_session);
            pesquisarViewModelStrategy = new PesquisarContMsgVMStrategy();
            DataInicial = DateTime.Now;
            DataFinal = DateTime.Now;
            GetLojas();
            AbrirVisualizarContagemProdutoComando = new RelayCommand(AbrirVisualizarContagemProduto);
            AbrirCadastrarTipoContagemComando = new RelayCommand(AbrirCadastrarTipoContagem);
        }
        public override async Task PesquisaItens(string termo)
        {
            DAOContagem daoContagem = (DAOContagem)daoEntidade;
            Entidades = new ObservableCollection<EntidadeComCampo<ContagemModel>>(EntidadeComCampo<ContagemModel>.CriarListaEntidadeComCampo(await daoContagem.ListarPorLojaEPeriodo(Loja, DataInicial, DataFinal)));
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.Listar());
        }

        private void AbrirVisualizarContagemProduto(object parameter)
        {
            openView.ShowDialog(new VisualizarContagemProdutoVM(EntidadeSelecionada.Entidade));
        }

        private void AbrirCadastrarTipoContagem(object parameter)
        {
            //TODO: implementar viewmodel
        }

        protected override WorksheetContainer<ContagemModel>[] GetWorksheetContainers()
        {
            throw new NotImplementedException();
        }

        public override ACadastrarViewModel<ContagemModel> GetCadastrarViewModel()
        {
            return new CadastrarContagemVM(_session, MessageBoxService, false);
        }

        public override ACadastrarViewModel<ContagemModel> GetEditarViewModel()
        {
            return new EditarContagemVM(_session, MessageBoxService);
        }

        public override AAjudarVM GetAjudaVM()
        {
            throw new NotImplementedException();
        }

        public override ExportarSQLViewModel<ContagemModel> GetExportaSQLVM()
        {
            throw new NotImplementedException();
        }

        public override ATelaRelatorio GetTelaRelatorioVM()
        {
            throw new NotImplementedException();
        }

        public DateTime DataInicial
        {
            get
            {
                return _dataInicial;
            }

            set
            {
                _dataInicial = value;
                OnPropertyChanged("DataInicial");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public DateTime DataFinal
        {
            get
            {
                return _dataFinal;
            }

            set
            {
                _dataFinal = value.AddDays(1).AddSeconds(-1);
                OnPropertyChanged("DataFinal");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public LojaModel Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
