using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class CadastrarFuncionarioVM : ACadastrarViewModel<FuncionarioModel>
    {
        private DAOLoja daoLoja;
        private DAO<Model.Banco> daoBanco;
        private ObservableCollection<Model.ChavePix> _chavesPix;
        private ObservableCollection<Model.ContaBancaria> _contasBancarias;
        private ObservableCollection<Model.Banco> _bancos;
        private Model.ChavePix _chavePix;
        private Model.ContaBancaria _contaBancaria;
        private Model.Banco _bancoContaBancaria;
        private Model.Banco _bancoPix;
        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ICommand AdicionarChavePixComando { get; set; }
        public ICommand AdicionarContaBancariaComando { get; set; }

        public CadastrarFuncionarioVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarFuncionarioVMStrategy();
            daoEntidade = new DAOFuncionario(_session);
            daoLoja = new DAOLoja(_session);
            daoBanco = new DAO<Model.Banco>(_session);

            GetLojas();
            GetBancos();

            Entidade = new FuncionarioModel
            {
                Cpf = "0",
                Loja = Lojas[0],
                Admissao = DateTime.Now
            };

            ChavePix = new Model.ChavePix();
            ContaBancaria = new Model.ContaBancaria();
            BancoContaBancaria = Bancos[0];
            BancoPix = Bancos[0];

            ChavesPix = new ObservableCollection<Model.ChavePix>();
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>();

            Entidade.PropertyChanged += Entidade_PropertyChanged;
            PropertyChanged += CadastrarFuncionarioVM_PropertyChanged;
            AntesDeInserirNoBancoDeDados += ConfiguraFuncionarioAntesDeInserir;

            AdicionarChavePixComando = new RelayCommand(AdicionarChavePix, ValidaChavePix);
            AdicionarContaBancariaComando = new RelayCommand(AdicionarContaBancaria, ValidaContaBancaria);
        }

        private bool ValidaContaBancaria(object arg)
        {
            if (!string.IsNullOrEmpty(ContaBancaria.Agencia) && !string.IsNullOrEmpty(ContaBancaria.Conta))
                return true;

            return false;
        }

        private bool ValidaChavePix(object arg)
        {
            if (!string.IsNullOrEmpty(ChavePix.Chave))
                return true;

            return false;
        }

        private void AdicionarContaBancaria(object obj)
        {
            ContaBancaria.Banco = BancoContaBancaria;
            ContasBancarias.Add(ContaBancaria);
            ContaBancaria = new Model.ContaBancaria();
        }

        private void AdicionarChavePix(object obj)
        {
            ChavePix.Banco = BancoPix;
            ChavesPix.Add(ChavePix);
            ChavePix = new Model.ChavePix();
        }

        private void CadastrarFuncionarioVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void ConfiguraFuncionarioAntesDeInserir()
        {
            if (Entidade.Loja.Cnpj == null)
                Entidade.Loja = null;

            Entidade.ChavesPix.Clear();
            foreach (var cp in ChavesPix)
            {
                cp.Funcionario = Entidade;
                Entidade.ChavesPix.Add(cp);
            }

            Entidade.ContasBancarias.Clear();
            foreach (var cb in ContasBancarias)
            {
                cb.Funcionario = Entidade;
                Entidade.ContasBancarias.Add(cb);
            }
        }

        override public async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cpf":
                    var result = await daoEntidade.ListarPorId(Entidade.Cpf);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.ListarExcetoDeposito());
        }
        private async void GetBancos()
        {
            Bancos = new ObservableCollection<Model.Banco>(await daoBanco.Listar());
        }
        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new FuncionarioModel
            {
                Loja = Lojas[0]
            };
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Cpf) || string.IsNullOrEmpty(Entidade.Nome))
            {
                BtnSalvarToolTip += "CPF Ou Nome Não Podem Ser Vazios!\n";
                valido = false;
            }

            return valido;
        }

        public ObservableCollection<Model.ChavePix> ChavesPix
        {
            get => _chavesPix;
            set
            {
                _chavesPix = value;
                OnPropertyChanged("ChavesPix");
            }
        }

        public ObservableCollection<Model.ContaBancaria> ContasBancarias
        {
            get => _contasBancarias;
            set
            {
                _contasBancarias = value;
                OnPropertyChanged("ContasBancarias");
            }
        }

        public Model.ChavePix ChavePix
        {
            get => _chavePix;
            set
            {
                _chavePix = value;
                OnPropertyChanged("ChavePix");
            }
        }
        public Model.ContaBancaria ContaBancaria
        {
            get => _contaBancaria;
            set
            {
                _contaBancaria = value;
                OnPropertyChanged("ContaBancaria");
            }
        }

        public ObservableCollection<Model.Banco> Bancos
        {
            get => _bancos;
            set
            {
                _bancos = value;
                OnPropertyChanged("Bancos");
            }
        }

        public Model.Banco BancoContaBancaria
        {
            get => _bancoContaBancaria;
            set
            {
                _bancoContaBancaria = value;
                OnPropertyChanged("BancoContaBancaria");
            }
        }

        public Model.Banco BancoPix
        {
            get => _bancoPix;
            set
            {
                _bancoPix = value;
                OnPropertyChanged("BancoPix");
            }
        }
    }
}
