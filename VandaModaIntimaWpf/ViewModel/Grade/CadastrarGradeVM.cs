using NHibernate;
using System.Collections.Generic;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.Grade
{
    public class CadastrarGradeVM : ACadastrarViewModel<Model.Grade>
    {
        private Model.Grade _gradePesquisa;
        private DAOTipoGrade daoTipoGrade;
        private Model.TipoGrade _tipoGrade;
        public IList<Model.TipoGrade> TipoGrades { get; set; }
        public CadastrarGradeVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            daoEntidade = new DAOGrade(_session);
            daoTipoGrade = new DAOTipoGrade(_session);
            Entidade = new Model.Grade();
            PropertyChanged += CadastrarGrade_PropertyChanged;
            GetTipoGrades();
        }

        private void CadastrarGrade_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {
            Entidade = new Model.Grade();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Nome))
            {
                BtnSalvarToolTip += "O Nome Da Grade Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (GradePesquisa != null)
            {
                BtnSalvarToolTip += "Essa Grade Já Está Cadastrada!\n";
                valido = false;
            }

            return valido;
        }
        private async void GetTipoGrades()
        {
            TipoGrades = await daoTipoGrade.Listar();
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
