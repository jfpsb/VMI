using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class RegistrarPontoVM : ACadastrarViewModel<Model.PontoEletronico>
    {
        private DateTime _horaAtual;
        private ObservableCollection<Model.PontoEletronico> _pontosEletronicos;
        private Model.PontoEletronico _pontoEletronico;
        private Timer horaAtualTimer;
        private DAOFuncionario daoFuncionario;
        private IList<Model.Funcionario> funcionarios;
        private IMessageBoxService messageBoxService;

        public ICommand RegistrarEntradaComando { get; set; }
        public ICommand RegistrarSaidaComando { get; set; }
        public ICommand RegistrarEntradaAlmocoComando { get; set; }
        public ICommand RegistrarSaidaAlmocoComando { get; set; }

        public RegistrarPontoVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            HoraAtual = DateTime.Now;
            horaAtualTimer = new Timer
            {
                Interval = 500.0
            };
            horaAtualTimer.Elapsed += HoraAtualTimer_Elapsed;
            horaAtualTimer.Start();

            daoEntidade = new DAOPontoEletronico(_session);
            daoFuncionario = new DAOFuncionario(_session);
            PontosEletronicos = new ObservableCollection<Model.PontoEletronico>();
            messageBoxService = new MessageBoxService();

            var task = GetFuncionarios();
            task.Wait();

            var task2 = PopulaListaDePontos();
            task2.Wait();

            PontoEletronico = PontosEletronicos[0];

            RegistrarEntradaComando = new RelayCommand(RegistrarEntrada, RegistrarEntradaValidacao);
            RegistrarSaidaComando = new RelayCommand(RegistrarSaida, RegistrarSaidaValidacao);
            RegistrarEntradaAlmocoComando = new RelayCommand(RegistrarEntradaAlmoco, RegistrarEntradaAlmocoValidacao);
            RegistrarSaidaAlmocoComando = new RelayCommand(RegistrarSaidaAlmoco, RegistrarSaidaAlmocoValidacao);
        }

        private bool RegistrarSaidaAlmocoValidacao(object arg)
        {
            if (PontoEletronico.SaidaAlmoco != null)
                return false;

            return true;
        }

        private void RegistrarSaidaAlmoco(object obj)
        {
            throw new NotImplementedException();
        }

        private bool RegistrarEntradaAlmocoValidacao(object arg)
        {
            if (PontoEletronico.EntradaAlmoco != null)
                return false;

            return true;
        }

        private void RegistrarEntradaAlmoco(object obj)
        {
            throw new NotImplementedException();
        }

        private bool RegistrarSaidaValidacao(object arg)
        {
            if (PontoEletronico.Saida != null)
                return false;

            return true;
        }

        private void RegistrarSaida(object obj)
        {
            throw new NotImplementedException();
        }

        private bool RegistrarEntradaValidacao(object arg)
        {
            if (PontoEletronico.Entrada != null)
                return false;

            return true;
        }

        private void RegistrarEntrada(object obj)
        {
            throw new NotImplementedException();
        }

        private async Task PopulaListaDePontos()
        {
            var dao = daoEntidade as DAOPontoEletronico;

            foreach (var f in funcionarios)
            {
                var ponto = await dao.ListarPorDiaFuncionario(DateTime.Now, f);

                if (ponto == null)
                {
                    ponto = new Model.PontoEletronico
                    {
                        Funcionario = f,
                        Dia = DateTime.Now
                    };
                }

                PontosEletronicos.Add(ponto);
            }
        }

        private async Task GetFuncionarios()
        {
            funcionarios = await daoFuncionario.ListarNaoDemitidos();
        }

        private void HoraAtualTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            HoraAtual = e.SignalTime;
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {

        }

        public override bool ValidacaoSalvar(object parameter)
        {
            return false;
        }

        public DateTime HoraAtual
        {
            get
            {
                return _horaAtual;
            }

            set
            {
                _horaAtual = value;
                OnPropertyChanged("HoraAtual");
            }
        }

        public ObservableCollection<Model.PontoEletronico> PontosEletronicos
        {
            get
            {
                return _pontosEletronicos;
            }

            set
            {
                _pontosEletronicos = value;
                OnPropertyChanged("PontosEletronicos");
            }
        }

        public Model.PontoEletronico PontoEletronico
        {
            get
            {
                return _pontoEletronico;
            }

            set
            {
                _pontoEletronico = value;
                OnPropertyChanged("PontoEletronico");
            }
        }
    }
}
