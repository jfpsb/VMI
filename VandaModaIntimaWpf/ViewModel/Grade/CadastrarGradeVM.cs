using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Grade
{
    public class CadastrarGradeVM : ACadastrarViewModel<Model.Grade>
    {
        private Model.Grade _gradePesquisa;
        private DAOTipoGrade daoTipoGrade;
        private Model.TipoGrade _tipoGrade;
        public IList<Model.TipoGrade> TipoGrades { get; set; }
        public CadastrarGradeVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            daoEntidade = new DAOGrade(session);
            daoTipoGrade = new DAOTipoGrade(session);
            Entidade = new Model.Grade();
            Entidade.PropertyChanged += Entidade_PropertyChanged;
            PropertyChanged += CadastrarViewModel_PropertyChanged;
            GetTipoGrades();
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoGrade":
                    Entidade.TipoGrade = TipoGrade;
                    break;
            }
        }

        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DAOGrade daoGrade = (DAOGrade)daoEntidade;
            switch (e.PropertyName)
            {
                case "Nome":
                    GradePesquisa = await daoGrade.ListarPorNome(Entidade.Nome);
                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Model.Grade();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Nome))
                return false;

            if (GradePesquisa != null)
                return false;

            return true;
        }

        protected override void ExecutarAntesCriarDocumento()
        {

        }
        private async void GetTipoGrades()
        {
            TipoGrades = await daoTipoGrade.Listar<Model.TipoGrade>();
        }

        public Model.Grade GradePesquisa
        {
            get => _gradePesquisa; set
            {
                _gradePesquisa = value;
                OnPropertyChanged("GradePesquisa");
            }
        }

        public Model.TipoGrade TipoGrade
        {
            get => _tipoGrade;
            set
            {
                _tipoGrade = value;
                OnPropertyChanged("TipoGrade");
            }
        }
    }
}
