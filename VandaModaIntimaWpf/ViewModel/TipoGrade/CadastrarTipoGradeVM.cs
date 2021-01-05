using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.TipoGrade
{
    public class CadastrarTipoGradeVM : ACadastrarViewModel<Model.TipoGrade>
    {
        private ObservableCollection<Model.TipoGrade> _tipoGrades;
        private Model.TipoGrade _tipoGradePesquisa;
        public CadastrarTipoGradeVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            Entidade = new Model.TipoGrade();
            daoEntidade = new DAOTipoGrade(session);
            viewModelStrategy = new CadastrarTipoGradeVMStrategy();
            GetTipoGrades();
            PropertyChanged += CadastrarTipoGrade_PropertyChanged;
            Entidade.PropertyChanged += ChecaPropriedadesTipoGrade;
        }

        public void CadastrarTipoGrade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entidade":
                    GetTipoGrades();
                    break;
            }
        }

        public async void ChecaPropriedadesTipoGrade(object sender, PropertyChangedEventArgs e)
        {
            DAOTipoGrade daoTipoGrade = (DAOTipoGrade)daoEntidade;
            switch (e.PropertyName)
            {
                case "Nome":
                    TipoGradePesquisa = await daoTipoGrade.ListarPorNome(Entidade.Nome);
                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.TipoGrade();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
            {
                SetStatusBarErro("Nome Não Pode Ser Vazio");
                return false;
            }

            if (TipoGradePesquisa != null)
            {
                SetStatusBarErro("Já Existe Um Tipo De Grade Com Esse Nome");
                return false;
            }

            SetStatusBarAguardando();
            return true;
        }
        private async void GetTipoGrades()
        {
            TipoGrades = new ObservableCollection<Model.TipoGrade>(await daoEntidade.Listar<Model.TipoGrade>());
        }
        public ObservableCollection<Model.TipoGrade> TipoGrades
        {
            get => _tipoGrades;
            set
            {
                _tipoGrades = value;
                OnPropertyChanged("TipoGrades");
            }
        }
        public Model.TipoGrade TipoGradePesquisa
        {
            get => _tipoGradePesquisa;
            set
            {
                _tipoGradePesquisa = value;
                OnPropertyChanged("TipoGradePesquisa");
            }
        }
    }
}
