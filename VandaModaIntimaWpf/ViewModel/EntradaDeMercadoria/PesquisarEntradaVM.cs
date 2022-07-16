using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    class PesquisarEntradaVM : APesquisarViewModel<Model.EntradaDeMercadoria>
    {
        private DateTime _dataEscolhida;
        private DAOLoja daoLoja;
        private ObservableCollection<Model.Loja> _lojas = new ObservableCollection<Model.Loja>();
        private Model.Loja _loja;

        public ICommand AbrirRelatorioFornecedorComando { get; set; }
        public ICommand ImprimirComando { get; set; }
        public ICommand ImprimirRelacaoComando { get; set; }

        public PesquisarEntradaVM()
        {
            daoEntidade = new DAOEntradaDeMercadoria(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarEntradaVMStrategy();

            AbrirRelatorioFornecedorComando = new RelayCommand(AbrirRelatorioFornecedor);
            ImprimirComando = new RelayCommand(Imprimir);
            ImprimirRelacaoComando = new RelayCommand(ImprimirRelacao);

            GetLojas();

            DataEscolhida = DateTime.Now;
        }

        private void ImprimirRelacao(object obj)
        {
            TelaRelatorioRelacaoMercadoria telaRelatorioRelacaoMercadoria = new TelaRelatorioRelacaoMercadoria(EntidadeSelecionada.Entidade);
            telaRelatorioRelacaoMercadoria.ShowDialog();
        }

        private void Imprimir(object obj)
        {
            TelaRelatorioEntradaMercadoria telaRelatorioEntradaMercadoria = new TelaRelatorioEntradaMercadoria(EntidadeSelecionada.Entidade);
            telaRelatorioEntradaMercadoria.ShowDialog();
        }

        private void AbrirRelatorioFornecedor(object obj)
        {
            _windowService.ShowDialog(new PesquisarEntradasPorFornecedorVM(), null);
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
            var worksheets = new WorksheetContainer<Model.EntradaDeMercadoria>[1];
            worksheets[0] = new WorksheetContainer<Model.EntradaDeMercadoria>()
            {
                Nome = "Entrada De Mercadoria",
                Lista = Entidades.Select(s => s.Entidade).ToList()
            };

            return worksheets;
        }

        public override object GetCadastrarViewModel()
        {
            return new CadastrarEntradaDeMercadoriaVM(_session);
        }

        public override object GetEditarViewModel()
        {
            return new EditarEntradaDeMercadoriaVM(_session, EntidadeSelecionada.Entidade);
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
