using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class ConsolidarPontosEletronicosVM : ObservableObject
    {
        private ObservableCollection<Model.Funcionario> _funcionarios;
        private Model.Funcionario _funcionario;
        private DAOFuncionario daoFuncionario;
        private DAOPontoEletronico daoPonto;
        private DateTime _dataEscolhida;
        private ObservableCollection<Model.PontoEletronico> _pontosEletronicos;
        private Model.PontoEletronico _pontoEletronico;

        public ICommand ConsolidarPontosComando { get; set; }

        public ConsolidarPontosEletronicosVM(ISession session)
        {
            daoFuncionario = new DAOFuncionario(session);
            daoPonto = new DAOPontoEletronico(session);
            DataEscolhida = DateTime.Now;
            PontosEletronicos = new ObservableCollection<Model.PontoEletronico>();

            var task = GetFuncionarios();
            task.Wait();
            var task2 = GetPontosEletronicos();
            task2.Wait();

            PropertyChanged += ConsolidarPontosEletronicosVM_PropertyChanged;

            Funcionario = Funcionarios[0];

            ConsolidarPontosComando = new RelayCommand(ConsolidarPontos);
        }

        private void ConsolidarPontos(object obj)
        {
            throw new NotImplementedException();
        }

        private async void ConsolidarPontosEletronicosVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DataEscolhida":
                    await GetPontosEletronicos();
                    break;
                case "Funcionario":
                    await GetPontosEletronicos();
                    break;
            }
        }

        private async Task GetPontosEletronicos()
        {
            PontosEletronicos.Clear();

            var dias = DateTimeUtil.RetornaDiasEmMes(DataEscolhida.Year, DataEscolhida.Month);

            foreach (var dia in dias)
            {
                var ponto = await daoPonto.ListarPorDiaFuncionario(dia, Funcionario);

                if (ponto == null)
                {
                    ponto = new Model.PontoEletronico
                    {
                        Funcionario = Funcionario,
                        Dia = dia
                    };
                }

                PontosEletronicos.Add(ponto);
            }
        }

        private async Task GetFuncionarios()
        {
            Funcionarios = new ObservableCollection<Model.Funcionario>(await daoFuncionario.ListarNaoDemitidos());
        }

        public ObservableCollection<Model.Funcionario> Funcionarios
        {
            get
            {
                return _funcionarios;
            }

            set
            {
                _funcionarios = value;
                OnPropertyChanged("Funcionarios");
            }
        }

        public Model.Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
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
