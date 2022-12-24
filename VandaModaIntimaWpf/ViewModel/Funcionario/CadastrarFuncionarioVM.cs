using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Ferias;
using FuncionarioModel = VandaModaIntimaWpf.Model.Funcionario;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class CadastrarFuncionarioVM : ACadastrarViewModel<FuncionarioModel>
    {
        private DAOLoja daoLoja;
        private DAOFaltas daoFaltas;
        private DAO<Banco> daoBanco;
        private DAO<Funcao> daoFuncao;
        private ObservableCollection<ChavePix> _chavesPix;
        private ObservableCollection<ContaBancaria> _contasBancarias;
        private ObservableCollection<Banco> _bancos;
        private ObservableCollection<Faltas> _faltas;
        private ObservableCollection<Funcao> _funcoes;
        private ContaBancaria _contaBancaria;
        private Banco _bancoContaBancaria;
        private Banco _bancoPix;
        private ChavePix _chavePix;
        private int _indexAba;
        private DateTime _dataEscolhida;
        private string _totalHorasFaltas;

        #region "Usado em EditarFuncionarioVM"
        private DateTime _inicioAquisitivo;
        private DateTime _inicioFerias;
        private ObservableCollection<Model.Ferias> _feriasRegistradas;
        private string _observacao;
        #endregion

        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ICommand AdicionarChavePixComando { get; set; }
        public ICommand AdicionarContaBancariaComando { get; set; }
        public ICommand DeletarChavePixComando { get; set; }
        public ICommand DeletarContaBancariaComando { get; set; }
        public ICommand DeletarFeriasRegistradaComando { get; set; }
        public ICommand SalvarFeriasComando { get; set; }
        public ICommand ImprimirComunicacaoComando { get; set; }

        public CadastrarFuncionarioVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            viewModelStrategy = new CadastrarFuncionarioVMStrategy();
            daoEntidade = new DAOFuncionario(_session);
            daoLoja = new DAOLoja(_session);
            daoBanco = new DAO<Model.Banco>(_session);
            daoFaltas = new DAOFaltas(session);
            daoFuncao = new DAO<Funcao>(session);

            DataEscolhida = DateTime.Now;

            var task1 = GetLojas();
            var task2 = GetBancos();
            var task3 = GetFuncoes();

            task1.Wait();
            task2.Wait();
            task3.Wait();

            Entidade = new FuncionarioModel
            {
                Cpf = "0",
                Loja = Lojas[0],
                Admissao = DateTime.Now
            };

            ChavePix = new Model.ChavePix();
            ContaBancaria = new Model.ContaBancaria();

            ContaBancaria = new Model.ContaBancaria();
            BancoContaBancaria = Bancos[0];
            BancoPix = Bancos[0];

            ChavesPix = new ObservableCollection<Model.ChavePix>();
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>();
            FeriasRegistradas = new ObservableCollection<Model.Ferias>();

            Entidade.PropertyChanged += Entidade_PropertyChanged;
            PropertyChanged += CadastrarFuncionarioVM_PropertyChanged;
            AntesDeInserirNoBancoDeDados += ConfiguraFuncionarioAntesDeInserir;

            AdicionarChavePixComando = new RelayCommand(AdicionarChavePix, ValidaChavePix);
            AdicionarContaBancariaComando = new RelayCommand(AdicionarContaBancaria, ValidaContaBancaria);
            DeletarChavePixComando = new RelayCommand(DeletarChavePix);
            DeletarContaBancariaComando = new RelayCommand(DeletarContaBancaria);
            DeletarFeriasRegistradaComando = new RelayCommand(DeletarFeriasRegistrada);
            SalvarFeriasComando = new RelayCommand(SalvarFerias);
            ImprimirComunicacaoComando = new RelayCommand(ImprimirComunicacao);
        }

        private void ImprimirComunicacao(object obj)
        {
            TelaComunicacaoDeFerias tela = new TelaComunicacaoDeFerias(obj as Model.Ferias);
            tela.ShowDialog();
        }

        private void SalvarFerias(object obj)
        {
            if (InicioFerias.DayOfWeek == DayOfWeek.Sunday)
            {
                _messageBoxService.Show("Período de férias não pode iniciar em dia de domingo.", viewModelStrategy.MessageBoxCaption(), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var feriados = FeriadoJsonUtil.RetornaListagemDeFeriados(InicioFerias.Year);
            var feriado = feriados.FirstOrDefault(s => s.Date.Day == InicioFerias.Day && s.Date.Month == InicioFerias.Month);

            if (feriado != null && InicioFerias.DayOfWeek != DayOfWeek.Sunday)
            {
                if (feriado.Type.ToLower().Equals("feriado nacional")
                    || feriado.Type.ToLower().Equals("feriado estadual")
                    || feriado.Type.ToLower().Equals("feriado municipal")
                    || feriado.Type.ToLower().Equals("dia não útil"))
                {
                    _messageBoxService.Show("Período de férias não pode iniciar em dia de feriado.", viewModelStrategy.MessageBoxCaption(), MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            if (InicioFerias < InicioConcessivo || InicioFerias > FimConcessivo)
            {
                _messageBoxService.Show("Início de período de férias não pode estar fora do período concessivo.", viewModelStrategy.MessageBoxCaption(), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Model.Ferias ferias = new Model.Ferias
            {
                Funcionario = Entidade,
                InicioAquisitivo = InicioAquisitivo,
                Inicio = InicioFerias,
                Fim = FimFerias,
                Observacao = Observacao
            };

            Entidade.Ferias.Add(ferias);
            FeriasRegistradas.Add(ferias);
        }

        private void DeletarFeriasRegistrada(object obj)
        {
            Model.Ferias ferias = obj as Model.Ferias;
            if (obj != null)
            {
                ferias.Deletado = true;
                Entidade.Ferias.Remove(ferias);
                FeriasRegistradas.Remove(ferias);
            }
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

        private async void CadastrarFuncionarioVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "InicioAquisitivo":
                    InicioFerias = InicioConcessivo;
                    break;
                case "DataEscolhida":
                    await GetFaltas();
                    break;
            }
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
        private async Task GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.ListarExcetoDeposito());
        }
        private async Task GetBancos()
        {
            Bancos = new ObservableCollection<Model.Banco>(await daoBanco.Listar());
        }
        private async Task GetFuncoes()
        {
            Funcoes = new ObservableCollection<Funcao>(await daoFuncao.Listar());
        }
        protected async Task GetFaltas()
        {
            if (Faltas != null)
            {
                foreach (var falta in Faltas)
                {
                    falta.PropertyChanged -= FaltaPropertyChanged;
                }
            }

            Faltas = new ObservableCollection<Faltas>(await daoFaltas.ListarFaltasPorMesFuncionario(DataEscolhida.Year, DataEscolhida.Month, Entidade));

            int horas = 0, minutos = 0;

            foreach (var falta in Faltas)
            {
                falta.PropertyChanged += FaltaPropertyChanged;
                if (!falta.Justificado)
                {
                    horas += falta.Horas;
                    minutos += falta.Minutos;
                }
            }

            if (minutos > 60)
            {
                int resto = minutos % 60;
                horas += resto;
            }

            TotalHorasFaltas = $"{horas:00}:{minutos:00}";
        }

        private async void FaltaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Faltas falta = sender as Model.Faltas;
            switch (e.PropertyName)
            {
                case "Justificativa":
                    if (falta.Justificativa.Trim().Length > 0)
                    {
                        falta.Justificado = true;
                    }
                    else
                    {
                        falta.Justificado = false;
                    }
                    try
                    {
                        await daoFaltas.Atualizar(falta);
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.Show($"Erro ao atualizar faltas.\n\n{ex.Message}");
                    }
                    break;
                case "Justificado":
                    try
                    {
                        await daoFaltas.Atualizar(falta);
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.Show($"Erro ao atualizar faltas.\n\n{ex.Message}");
                    }
                    break;
            }
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            if (e.Sucesso)
            {
                IssoEUmUpdate = true;
                viewModelStrategy = new EditarFuncionarioVMStrategy();
            }
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

            if (!CPF.IsValid(Entidade.Cpf))
            {
                BtnSalvarToolTip += "CPF informado é inválido!\n";
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

        public DateTime InicioAquisitivo
        {
            get
            {
                return _inicioAquisitivo;
            }

            set
            {
                _inicioAquisitivo = value;
                OnPropertyChanged("InicioAquisitivo");
                OnPropertyChanged("FimAquisitivo");
                OnPropertyChanged("InicioConcessivo");
                OnPropertyChanged("FimConcessivo");
            }
        }

        public DateTime FimAquisitivo
        {
            get => InicioAquisitivo.AddYears(1).AddDays(-1);
        }

        public DateTime InicioConcessivo
        {
            get => FimAquisitivo.AddDays(1);
        }

        public DateTime FimConcessivo
        {
            get => InicioConcessivo.AddYears(1).AddDays(-1);
        }

        public DateTime InicioFerias
        {
            get
            {
                return _inicioFerias;
            }

            set
            {
                _inicioFerias = value;
                OnPropertyChanged("InicioFerias");
                OnPropertyChanged("FimFerias");
            }
        }

        public DateTime FimFerias
        {
            //O dia de início das férias conta como o primeiro dia
            //então só preciso somar 29 para achar o último dia das férias
            get => InicioFerias.AddDays(30).AddDays(-1);
        }

        public ObservableCollection<Model.Ferias> FeriasRegistradas
        {
            get
            {
                return _feriasRegistradas;
            }

            set
            {
                _feriasRegistradas = value;
                OnPropertyChanged("FeriasRegistradas");
            }
        }

        public string Observacao
        {
            get
            {
                return _observacao;
            }

            set
            {
                _observacao = value;
                OnPropertyChanged("Observacao");
            }
        }

        public int IndexAba
        {
            get
            {
                return _indexAba;
            }

            set
            {
                _indexAba = value;
                OnPropertyChanged("IndexAba");
            }
        }

        public ObservableCollection<Faltas> Faltas
        {
            get
            {
                return _faltas;
            }

            set
            {
                _faltas = value;
                OnPropertyChanged("Faltas");
            }
        }

        public DateTime DataEscolhida
        {
            get
            {
                return _dataEscolhida;
            }

            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }

        public string TotalHorasFaltas
        {
            get
            {
                return _totalHorasFaltas;
            }

            set
            {
                _totalHorasFaltas = value;
                OnPropertyChanged("TotalHorasFaltas");
            }
        }

        public ObservableCollection<Funcao> Funcoes
        {
            get
            {
                return _funcoes;
            }

            set
            {
                _funcoes = value;
                OnPropertyChanged("Funcoes");
            }
        }
    }
}
