using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVM : ACadastrarViewModel<Model.HoraExtra>
    {
        private Model.FolhaPagamento _folha;
        private DAO<Model.TipoHoraExtra> daoTipoHoraExtra;
        private TipoHoraExtra _tipoHoraExtra;

        public ObservableCollection<TipoHoraExtra> TiposHoraExtra { get; set; }
        public int CmbDescricaoIndex { get; set; }

        public AdicionarHoraExtraVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOHoraExtra(session);
            daoTipoHoraExtra = new DAO<Model.TipoHoraExtra>(session);
            viewModelStrategy = new AdicionarHoraExtraVMStrategy();
            Folha = folha;

            GetTiposHoraExtra();

            PropertyChanged += AoEscolherTipoHoraExtra;

            ResetaPropriedades(null);
        }

        private async void AoEscolherTipoHoraExtra(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TipoHoraExtra"))
            {
                HoraExtra horaExtra = await (daoEntidade as DAOHoraExtra).ListarPorAnoMesFuncionarioTipo(Folha.Ano, Folha.Mes, Folha.Funcionario, TipoHoraExtra);

                if (horaExtra != null)
                {
                    Entidade = horaExtra;
                }
                else
                {
                    Entidade = new Model.HoraExtra()
                    {
                        Ano = Folha.Ano,
                        Mes = Folha.Mes,
                        Funcionario = Folha.Funcionario,
                        LojaTrabalho = Folha.Funcionario.LojaTrabalho
                    };

                    Entidade.TipoHoraExtra = TipoHoraExtra;
                }
            }
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            CmbDescricaoIndex = 0;

            Entidade = new Model.HoraExtra()
            {
                Ano = Folha.Ano,
                Mes = Folha.Mes,
                Funcionario = Folha.Funcionario,
                TipoHoraExtra = TiposHoraExtra[0],
                LojaTrabalho = Folha.Funcionario.LojaTrabalho
            };

            TipoHoraExtra = TiposHoraExtra[0];
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Horas < 0 && Entidade.Minutos < 0)
            {
                BtnSalvarToolTip += "Horas e Minutos Não Podem Ser Menores Que Zero!\n";
                valido = false;
            }

            if (Entidade.Horas < 0)
            {
                BtnSalvarToolTip += "Valor de Horas Não Pode Ser Menor Que Zero!\n";
                valido = false;
            }

            if (Entidade.Horas > 99)
            {
                BtnSalvarToolTip += "Valor de Horas É Alto Demais!\n";
                valido = false;
            }

            if (Entidade.Minutos >= 60)
            {
                BtnSalvarToolTip += "Valor de Minutos Não Pode Ser Maior Ou Igual a 60!\n";
                valido = false;
            }

            if (Entidade.Minutos < 0)
            {
                BtnSalvarToolTip += "Valor de Minutos Não Pode Ser Menor Que Zero!\n";
                valido = false;
            }

            return valido;
        }

        private async void GetTiposHoraExtra()
        {
            TiposHoraExtra = new ObservableCollection<Model.TipoHoraExtra>(await daoTipoHoraExtra.Listar());
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
    }
}
