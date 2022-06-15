using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
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
        private Model.ContaBancaria _contaBancaria;
        private Model.Banco _bancoContaBancaria;
        private Model.Banco _bancoPix;
        private Model.ChavePix _chavePix;
        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ICommand AdicionarChavePixComando { get; set; }
        public ICommand AdicionarContaBancariaComando { get; set; }
        public ICommand DeletarChavePixComando { get; set; }
        public ICommand DeletarContaBancariaComando { get; set; }

        public CadastrarFuncionarioVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
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
            DeletarChavePixComando = new RelayCommand(DeletarChavePix);
            DeletarContaBancariaComando = new RelayCommand(DeletarContaBancaria);
        }

        private void DeletarContaBancaria(object obj)
        {
            var contaBancaria = obj as Model.ContaBancaria;
            if (contaBancaria != null)
            {
                contaBancaria.Deletado = true;
                ContasBancarias.Remove(contaBancaria);
                Entidade.ContasBancarias.Remove(contaBancaria);
            }
        }

        private void DeletarChavePix(object obj)
        {
            var chavePix = obj as Model.ChavePix;
            if (chavePix != null)
            {
                chavePix.Deletado = true;
                ChavesPix.Remove(chavePix);
                Entidade.ChavesPix.Remove(chavePix);
            }
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
            //Atribui banco selecionado
            ContaBancaria.Banco = BancoContaBancaria;
            //Adiciona em coleção
            Entidade.AddContaBancaria(ContaBancaria);
            //Atualiza coleção com binding na view
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>(Entidade.ContasBancarias);
            //Reseta objeto ContaBancaria para nova conta
            ContaBancaria = new Model.ContaBancaria();
        }

        private void AdicionarChavePix(object obj)
        {
            //Atribui banco selecionado
            ChavePix.Banco = BancoPix;
            //Adiciona em coleção
            Entidade.AddChavePix(ChavePix);
            //Atualiza coleção com binding na view
            ChavesPix = new ObservableCollection<Model.ChavePix>(Entidade.ChavesPix);
            //Reseta objeto ContaBancaria para nova conta
            ChavePix = new Model.ChavePix();
        }

        private void CadastrarFuncionarioVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void ConfiguraFuncionarioAntesDeInserir()
        {
            if (Entidade.Loja.Cnpj == null)
                Entidade.Loja = null;
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
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.ListarSomenteLojas());
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

        public Model.ChavePix ChavePix
        {
            get
            {
                return _chavePix;
            }

            set
            {
                _chavePix = value;
                OnPropertyChanged("ChavePix");
            }
        }
    }
}
