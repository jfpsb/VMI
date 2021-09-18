using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using RecebimentoCartaoModel = VandaModaIntimaWpf.Model.RecebimentoCartao;

namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    public class PesquisarRecebimentoVM : APesquisarViewModel<RecebimentoCartaoModel>
    {
        private DAOLoja daoLoja;
        private LojaModel matriz;
        private DateTime dataEscolhida = DateTime.Now;
        private int matrizComboBoxIndex;
        private double _recebido;
        private double _totalOperadora;

        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public ICommand AbrirCadastrarOperadoraComando { get; set; }
        public ICommand MaisDetalhesComando { get; set; }
        public PesquisarRecebimentoVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<RecebimentoCartaoModel> abrePelaTelaPesquisaService)
            : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAORecebimentoCartao(_session);
            daoLoja = new DAOLoja(_session);
            pesquisarViewModelStrategy = new PesquisarRecebMsgVMStrategy();
            GetMatrizes();
            MatrizComboBoxIndex = 0;

            MaisDetalhesComando = new RelayCommand(MaisDetalhes);

            DataEscolhida = DateTime.Now;
        }

        private void MaisDetalhes(object obj)
        {
            MaisDetalhesVM viewModel = new MaisDetalhesVM(_session, EntidadeSelecionada.Entidade);
            View.RecebimentoCartao.MaisDetalhes view = new View.RecebimentoCartao.MaisDetalhes()
            {
                DataContext = viewModel
            };
            view.ShowDialog();
        }

        private void Entidades_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculaTotais();
        }

        public override async Task PesquisaItens(string termo)
        {
            DAORecebimentoCartao daoRecebimento = (DAORecebimentoCartao)daoEntidade;
            if (MatrizComboBoxIndex != 0)
            {
                Entidades = new ObservableCollection<EntidadeComCampo<RecebimentoCartaoModel>>(EntidadeComCampo<RecebimentoCartaoModel>.CriarListaEntidadeComCampo(await daoRecebimento.ListarPorMesAnoLojaGroupByLoja(DataEscolhida.Month, DataEscolhida.Year, Matriz)));
            }
            else
            {
                Entidades = new ObservableCollection<EntidadeComCampo<RecebimentoCartaoModel>>(EntidadeComCampo<RecebimentoCartaoModel>.CriarListaEntidadeComCampo(await daoRecebimento.ListarPorMesAnoGroupByLoja(DataEscolhida.Month, DataEscolhida.Year)));
            }

            Entidades.CollectionChanged += Entidades_CollectionChanged;

            CalculaTotais();
        }
        public async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(GetResource.GetString("matriz_nao_selecionada")));
        }
        public override bool Editavel(object parameter)
        {
            return false;
        }
        private void CalculaTotais()
        {
            double totalRecebido = 0;
            double totalOperadora = 0;

            foreach (RecebimentoCartaoModel recebimento in Entidades.Select(s => s.Entidade).ToList())
            {
                totalRecebido += recebimento.Recebido;
                totalOperadora += recebimento.ValorOperadora;
            }

            TotalOperadora = totalOperadora;
            Recebido = totalRecebido;
        }
        public LojaModel Matriz
        {
            get { return matriz; }
            set
            {
                matriz = value;
                OnPropertyChanged("Matriz");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public int MatrizComboBoxIndex
        {
            get { return matrizComboBoxIndex; }
            set
            {
                matrizComboBoxIndex = value;
                OnPropertyChanged("MatrizComboBoxIndex");
            }
        }
        public DateTime DataEscolhida
        {
            get { return dataEscolhida; }
            set
            {
                dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public double Recebido
        {
            get => _recebido;
            set
            {
                _recebido = value;
                OnPropertyChanged("Recebido");
            }
        }
        public double TotalOperadora
        {
            get => _totalOperadora;
            set
            {
                _totalOperadora = value;
                OnPropertyChanged("TotalOperadora");
            }
        }
    }
}
