using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVM : ACadastrarViewModel<Model.HoraExtra>
    {
        private Model.FolhaPagamento _folha;
        private DAO<Model.TipoHoraExtra> daoTipoHoraExtra;
        private TipoHoraExtra _tipoHoraExtra;
        private ObservableCollection<HoraExtra> _horasExtras;
        private int _totalEmHoras;
        private int _totalEmMinutos;

        public ObservableCollection<TipoHoraExtra> TiposHoraExtra { get; set; }

        public AdicionarHoraExtraVM(ISession session, Model.FolhaPagamento folha) : base(session, false)
        {
            daoEntidade = new DAOHoraExtra(_session);
            daoTipoHoraExtra = new DAO<Model.TipoHoraExtra>(_session);
            viewModelStrategy = new AdicionarHoraExtraVMStrategy();
            Folha = folha;

            GetTiposHoraExtra();

            PropertyChanged += AoEscolherTipoHoraExtra;

            HorasExtras = new ObservableCollection<HoraExtra>();
            PopulaHorasExtras();
        }

        private async void PopulaHorasExtras()
        {
            var dao = daoEntidade as DAOHoraExtra;
            HorasExtras.Clear();
            foreach (var dia in DateTimeUtil.RetornaDiasEmMes(Folha.Ano, Folha.Mes))
            {
                var horaExtra = await dao.ListarPorDiaFuncionarioTipoHoraExtra(dia, Folha.Funcionario, TipoHoraExtra);

                if (horaExtra == null)
                {
                    horaExtra = new HoraExtra
                    {
                        Data = dia,
                        Funcionario = Folha.Funcionario,
                        TipoHoraExtra = TipoHoraExtra,
                        LojaTrabalho = Folha.Funcionario.LojaTrabalho
                    };
                }

                horaExtra.PropertyChanged += HoraExtra_PropertyChanged;
                HorasExtras.Add(horaExtra);
            }

            CalculaTotalHoras();
        }

        private void HoraExtra_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Horas":
                    CalculaTotalHoras();
                    break;
                case "Minutos":
                    CalculaTotalHoras();
                    break;
            }
        }

        private void CalculaTotalHoras()
        {
            TotalEmHoras = HorasExtras.Sum(s => s.Horas);
            TotalEmMinutos = HorasExtras.Sum(s => s.Minutos);

            if (TotalEmMinutos >= 60)
            {
                int minutosParaHoras = TotalEmMinutos / 60;
                TotalEmHoras += minutosParaHoras;
                TotalEmMinutos %= 60;
            }
        }

        private void AoEscolherTipoHoraExtra(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TipoHoraExtra"))
            {
                PopulaHorasExtras();
            }
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            return true;
        }

        protected async override Task<AposInserirBDEventArgs> ExecutarSalvar(object parametro)
        {
            _result = false;
            try
            {
                var horasExtrasValidas = HorasExtras.Where(w => (w.Uuid != null && w.Uuid != Guid.Empty) || (w.Horas != 0 || w.Minutos != 0)).ToList();
                await daoEntidade.InserirOuAtualizar(horasExtrasValidas);
                _result = true;
                MessageBoxService.Show("Horas extras foram salvas com sucesso.", viewModelStrategy.MessageBoxCaption(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show($"Erro ao salvar horas extras.\n\n{ex.Message}\n\n{ex.InnerException?.Message}", viewModelStrategy.MessageBoxCaption(),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IssoEUmUpdate = IssoEUmUpdate,
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }

        private async void GetTiposHoraExtra()
        {
            TiposHoraExtra = new ObservableCollection<Model.TipoHoraExtra>(await daoTipoHoraExtra.Listar());
            TipoHoraExtra = TiposHoraExtra[0];
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public Model.FolhaPagamento Folha
        {
            get => _folha;
            set
            {
                _folha = value;
                OnPropertyChanged("Folha");
            }
        }

        public TipoHoraExtra TipoHoraExtra
        {
            get => _tipoHoraExtra;
            set
            {
                _tipoHoraExtra = value;
                OnPropertyChanged("TipoHoraExtra");
            }
        }

        public ObservableCollection<HoraExtra> HorasExtras
        {
            get
            {
                return _horasExtras;
            }

            set
            {
                _horasExtras = value;
                OnPropertyChanged("HorasExtras");
            }
        }

        public int TotalEmHoras
        {
            get
            {
                return _totalEmHoras;
            }

            set
            {
                _totalEmHoras = value;
                OnPropertyChanged("TotalEmHoras");
            }
        }

        public int TotalEmMinutos
        {
            get
            {
                return _totalEmMinutos;
            }

            set
            {
                _totalEmMinutos = value;
                OnPropertyChanged("TotalEmMinutos");
            }
        }
    }
}
