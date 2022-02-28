using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    class PesquisarEntradaVM : APesquisarViewModel<Model.EntradaDeMercadoria>
    {
        private DateTime _dataEscolhida;
        private DAOLoja daoLoja;
        private ObservableCollection<Model.Loja> _lojas = new ObservableCollection<Model.Loja>();
        private Model.Loja _loja;

        public ICommand AbrirRelatorioFornecedorComando { get; set; }

        public PesquisarEntradaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.EntradaDeMercadoria> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAOEntradaDeMercadoria(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarEntradaVMStrategy();

            AbrirRelatorioFornecedorComando = new RelayCommand(AbrirRelatorioFornecedor);

            GetLojas();

            DataEscolhida = DateTime.Now;
        }

        private void AbrirRelatorioFornecedor(object obj)
        {
            PesquisarEntradasPorFornecedorVM viewModel = new PesquisarEntradasPorFornecedorVM(new MessageBoxService(), null);
            PesquisarEntradasPorFornecedor view = new PesquisarEntradasPorFornecedor() { DataContext = viewModel };
            view.ShowDialog();
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarSomenteLojas());
            Lojas.Insert(0, new Model.Loja("TODAS AS LOJAS"));
            Loja = Lojas[0];
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public async override Task PesquisaItens(string termo)
        {
            if (DataEscolhida.Year == 1)
                return;

            var dao = daoEntidade as DAOEntradaDeMercadoria;

            Entidades = new ObservableCollection<EntidadeComCampo<Model.EntradaDeMercadoria>>(EntidadeComCampo<Model.EntradaDeMercadoria>.CriarListaEntidadeComCampo(await dao.ListarPorMesLoja(DataEscolhida, Loja)));
        }

        protected override WorksheetContainer<Model.EntradaDeMercadoria>[] GetWorksheetContainers()
        {
            return null;
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

        public ObservableCollection<Model.Loja> Lojas
        {
            get => _lojas;
            set
            {
                _lojas = value;
                OnPropertyChanged("Lojas");
            }
        }
        public Model.Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
