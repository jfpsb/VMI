using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVM : ACadastrarViewModel<Model.HoraExtra>
    {
        private Model.FolhaPagamento _folha;
        private DAOTipoHoraExtra daoTipoHoraExtra;

        public ObservableCollection<TipoHoraExtra> TiposHoraExtra { get; set; }
        public int CmbDescricaoIndex { get; set; }

        public AdicionarHoraExtraVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOHoraExtra(session);
            daoTipoHoraExtra = new DAOTipoHoraExtra(session);
            Folha = folha;

            GetTiposHoraExtra();
            CmbDescricaoIndex = 0;

            Entidade = new Model.HoraExtra()
            {
                Ano = Folha.Ano,
                Mes = Folha.Mes,
                Funcionario = Folha.Funcionario,
                TipoHoraExtra = TiposHoraExtra[0]
            };
        }
        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.HoraExtra()
            {
                Ano = Folha.Ano,
                Mes = Folha.Mes,
                Funcionario = Folha.Funcionario,
                TipoHoraExtra = TiposHoraExtra[0],
                Horas = 0,
                Minutos = 0
            };

            CmbDescricaoIndex = 0;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Horas == 0 && Entidade.Minutos == 0)
            {
                BtnSalvarToolTip += "Horas e Minutos Não Podem Ser Iguais a Zero!\n";
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
            TiposHoraExtra = new ObservableCollection<Model.TipoHoraExtra>(await daoTipoHoraExtra.Listar<Model.TipoHoraExtra>());
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
    }
}
