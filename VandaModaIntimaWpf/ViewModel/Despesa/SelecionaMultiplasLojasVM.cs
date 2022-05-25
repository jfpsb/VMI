using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class SelecionaMultiplasLojasVM : ObservableObject, IRequestClose
    {
        private ObservableCollection<EntidadeComCampo<Model.Loja>> _entidades;
        private DAOLoja daoLoja;

        public event EventHandler<EventArgs> RequestClose;

        public ICommand FinalizarComando { get; set; }

        public SelecionaMultiplasLojasVM(ISession session, IList<EntidadeComCampo<Model.Loja>> lojasSelecionadas)
        {
            daoLoja = new DAOLoja(session);
            FinalizarComando = new RelayCommand(Finalizar);
            if (lojasSelecionadas != null && lojasSelecionadas.Count > 0)
            {
                Entidades = new ObservableCollection<EntidadeComCampo<Model.Loja>>(lojasSelecionadas);
            }
            else
            {
                GetLojas();
            }
        }

        private void Finalizar(object obj)
        {
            RequestClose?.Invoke(null, null);
        }

        private async void GetLojas()
        {
            Entidades = new ObservableCollection<EntidadeComCampo<Model.Loja>>(EntidadeComCampo<Model.Loja>.CriarListaEntidadeComCampo(await daoLoja.ListarExcetoDeposito()));
        }

        public ObservableCollection<EntidadeComCampo<Model.Loja>> Entidades
        {
            get
            {
                return _entidades;
            }

            set
            {
                _entidades = value;
            }
        }
    }
}
