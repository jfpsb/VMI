using NHibernate;
using System;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVM : ACadastrarViewModel<Model.HoraExtra>
    {
        private Model.FolhaPagamento _folha;
        private Model.TipoHoraExtra tipoHoraExtra;
        private int _horas;
        private int _minutos;

        public AdicionarHoraExtraVM(ISession session, Model.FolhaPagamento folha, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOHoraExtra(session);
            _folha = folha;

            Entidade = new Model.HoraExtra()
            {
                Ano = _folha.Ano,
                Mes = _folha.Mes,
                Funcionario = _folha.Funcionario,
                TipoHoraExtra = TipoHoraExtra
            };
        }
        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.HoraExtra()
            {
                Ano = _folha.Ano,
                Mes = _folha.Mes,
                Funcionario = _folha.Funcionario,
                TipoHoraExtra = TipoHoraExtra,
                Horas = 0,
                Minutos = 0
            };
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (Entidade.Horas == 0 && Entidade.Minutos == 0)
            {
                return false;
            }

            return true;
        }

        public TipoHoraExtra TipoHoraExtra
        {
            get => tipoHoraExtra;
            set
            {
                tipoHoraExtra = value;
                OnPropertyChanged("TipoHoraExtra");
            }
        }
        public int Horas
        {
            get => _horas;
            set
            {
                _horas = value;
                OnPropertyChanged("Horas");
            }
        }
        public int Minutos
        {
            get => _minutos;
            set
            {
                _minutos = value;
                OnPropertyChanged("Minutos");
            }
        }
    }
}
