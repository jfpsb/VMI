using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class ConsolidarPontosEletronicosVM : ObservableObject
    {
        private ObservableCollection<Model.Funcionario> _funcionarios;
        private Model.Funcionario _funcionario;
        private DAOFuncionario daoFuncionario;
        private DAOPontoEletronico daoPonto;
        private DAO<Model.TipoHoraExtra> daoTipoHoraExtra;
        private DateTime _dataEscolhida;
        private ObservableCollection<Model.PontoEletronico> _pontosEletronicos;
        private Model.PontoEletronico _pontoEletronico;
        private ISession _session;
        private IWindowService windowService;

        public ICommand ConsolidarPontosComando { get; set; }

        public ConsolidarPontosEletronicosVM(ISession session)
        {
            _session = session;
            daoFuncionario = new DAOFuncionario(session);
            daoPonto = new DAOPontoEletronico(session);
            daoTipoHoraExtra = new DAO<Model.TipoHoraExtra>(_session);
            DataEscolhida = DateTime.Now;
            PontosEletronicos = new ObservableCollection<Model.PontoEletronico>();
            windowService = new WindowService();

            var task = GetFuncionarios();
            task.Wait();
            var task2 = GetPontosEletronicos();
            task2.Wait();

            PropertyChanged += ConsolidarPontosEletronicosVM_PropertyChanged;

            Funcionario = Funcionarios[0];

            ConsolidarPontosComando = new RelayCommand(ConsolidarPontos);
        }

        private async void ConsolidarPontos(object obj)
        {
            IList<object> listaConsolidacao = new List<object>();

            foreach (var ponto in PontosEletronicos)
            {
                DataFeriado dataFeriado = FeriadoJsonUtil.IsDataFeriado(ponto.Dia.Day, ponto.Dia.Month, ponto.Dia.Year);
                TimeSpan limiteAtraso = new TimeSpan(0, 10, 0);
                TimeSpan zeroCargaHoraria = new TimeSpan(0, 0, 0);

                //Determina carga horária do dia do ponto
                TimeSpan cargaHoraria = ponto.Dia.DayOfWeek == DayOfWeek.Saturday ? new TimeSpan(4, 0, 0) : new TimeSpan(8, 0, 0);

                //Data não é feriado
                if (dataFeriado == null)
                {
                    if (ponto.Dia.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (ponto.Jornada != zeroCargaHoraria)
                        {
                            var horaExtra100 = await daoTipoHoraExtra.ListarPorId(1);
                            CriaHoraExtraParaConsolidar(listaConsolidacao, ponto, ponto.Jornada, horaExtra100);
                            continue;
                        }
                    }

                    if (ponto.Jornada > cargaHoraria)
                    {
                        var dif = ponto.Jornada - cargaHoraria;
                        if (dif > limiteAtraso)
                        {
                            var horaExtraNormal = await daoTipoHoraExtra.ListarPorId(2);
                            CriaHoraExtraParaConsolidar(listaConsolidacao, ponto, dif, horaExtraNormal);
                            continue;
                        }
                    }
                    else if (ponto.Jornada < cargaHoraria)
                    {
                        var dif = cargaHoraria - ponto.Jornada;
                        if (dif > limiteAtraso)
                        {
                            CriaFaltaParaConsolidar(listaConsolidacao, ponto, dif);
                            continue;
                        }
                    }
                }
                else
                {
                    if (dataFeriado.Type.ToLower().Equals("feriado nacional")
                        || dataFeriado.Type.ToLower().Equals("feriado estadual")
                        || dataFeriado.Type.ToLower().Equals("feriado municipal"))
                    {
                        //Trabalho em feriado
                        if (ponto.Jornada > zeroCargaHoraria)
                        {
                            var tipoHoraExtra = await daoTipoHoraExtra.ListarPorId(1);

                            //Caso este dia seja feriado mas o dia de trabalho foi normal, mudo as horas extras para normais
                            if (ponto.IsDiaUtil)
                                tipoHoraExtra = await daoTipoHoraExtra.ListarPorId(2);

                            CriaHoraExtraParaConsolidar(listaConsolidacao, ponto, ponto.Jornada, tipoHoraExtra);
                            continue;
                        }
                    }
                }
            }

            windowService.ShowDialog(new ConfirmarConsolidacaoPontosEletronicosVM(_session, listaConsolidacao), null);
        }

        private static void CriaFaltaParaConsolidar(IList<object> listaConsolidacao, Model.PontoEletronico ponto, TimeSpan dif)
        {
            var falta = new Model.Faltas
            {
                Funcionario = ponto.Funcionario,
                Data = ponto.Dia,
                Horas = dif.Hours,
                Minutos = dif.Minutes
            };

            listaConsolidacao.Add(falta);
        }

        private static void CriaHoraExtraParaConsolidar(IList<object> listaConsolidacao, Model.PontoEletronico ponto, TimeSpan diferenca, TipoHoraExtra tipoHoraExtra)
        {
            var horaExtra = new Model.HoraExtra
            {
                Funcionario = ponto.Funcionario,
                TipoHoraExtra = tipoHoraExtra,
                LojaTrabalho = ponto.Funcionario.LojaTrabalho,
                Data = ponto.Dia,
                Horas = diferenca.Hours,
                Minutos = diferenca.Minutes
            };

            listaConsolidacao.Add(horaExtra);
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
                var dataFeriado = FeriadoJsonUtil.IsDataFeriado(dia.Day, dia.Month, dia.Year);

                if (ponto == null)
                {
                    ponto = new Model.PontoEletronico
                    {
                        Funcionario = Funcionario,
                        Dia = dia
                    };
                }

                if (dataFeriado == null)
                {
                    if (ponto.Dia.DayOfWeek != DayOfWeek.Sunday)
                    {
                        ponto.IsDiaUtil = true;
                    }
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
