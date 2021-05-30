using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class CadastrarRepresentanteVM : ACadastrarViewModel<Model.Representante>
    {
        private DAOFornecedor daoFornecedor;
        private Model.Fornecedor _fornecedor; //Guarda fornecedor selecionado atualmente na ComboBox
        private ObservableCollection<Model.Fornecedor> _fornecedores; //Salva os fornecedores selecionados pelo usuário
        private ObservableCollection<Model.Fornecedor> _comboBoxFornecedores; //Salva os fornecedores listados na ComboBox

        public ICommand AdicionarFornecedorComando { get; set; }

        public CadastrarRepresentanteVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAO<Model.Representante>(session);
            daoFornecedor = new DAOFornecedor(session);
            MessageBoxService = messageBoxService;

            viewModelStrategy = new CadastrarRepresentanteVMStrategy();
            Entidade = new Model.Representante();
            Fornecedores = new ObservableCollection<Model.Fornecedor>();

            GetComboBoxFornecedores();

            AdicionarFornecedorComando = new RelayCommand(AdicionarFornecedor);
            AntesDeInserirNoBancoDeDados += ColocaFornecedoresEmEntidade;
        }

        private void ColocaFornecedoresEmEntidade()
        {
            Entidade.Fornecedores.Clear();

            foreach (var f in Fornecedores)
            {
                f.Representante = Entidade;
                Entidade.Fornecedores.Add(f);
            }
        }

        private void AdicionarFornecedor(object obj)
        {
            Fornecedores.Add(Fornecedor);
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.Representante();
            Fornecedores.Clear();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Nome != null && Entidade.Nome.Trim().Equals(string.Empty))
            {
                BtnSalvarToolTip += "Nome de Representante Não Pode Ser Vazio!\n".ToUpper();
                valido = false;
            }

            if (Fornecedores.Count == 0)
            {
                BtnSalvarToolTip += "Selecione Ao Menos Um Fornecedor Na Lista Para Prosseguir!".ToUpper();
                valido = false;
            }

            return valido;
        }

        private async void GetComboBoxFornecedores()
        {
            ComboBoxFornecedores = new ObservableCollection<Model.Fornecedor>(await daoFornecedor.Listar());
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

        public ObservableCollection<Model.Fornecedor> ComboBoxFornecedores
        {
            get => _comboBoxFornecedores;
            set
            {
                _comboBoxFornecedores = value;
                OnPropertyChanged("ComboBoxFornecedores");
            }
        }

        public Model.Fornecedor Fornecedor
        {
            get => _fornecedor;
            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }
    }
}
