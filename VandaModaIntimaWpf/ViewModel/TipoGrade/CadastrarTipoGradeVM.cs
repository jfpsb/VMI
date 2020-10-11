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
        public CadastrarTipoGradeVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            Entidade = new Model.TipoGrade();
            daoEntidade = new DAOTipoGrade(session);
            cadastrarViewModelStrategy = new CadastrarTipoGradeVMStrategy();
            GetTipoGrades();
            PropertyChanged += CadastrarViewModel_PropertyChanged;
            Entidade.PropertyChanged += Entidade_PropertyChanged;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entidade":
                    GetTipoGrades();
                    break;
            }
        }

        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            if (string.IsNullOrEmpty(Entidade.Nome))
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

        protected override void ExecutarAntesCriarDocumento()
        {

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
