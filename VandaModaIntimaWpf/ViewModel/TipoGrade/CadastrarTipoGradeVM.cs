using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.TipoGrade
{
    public class CadastrarTipoGradeVM : ACadastrarViewModel<Model.TipoGrade>
    {
        private ObservableCollection<Model.TipoGrade> _tipoGrades;
        private Model.TipoGrade _tipoGradePesquisa;
        public CadastrarTipoGradeVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            Entidade = new Model.TipoGrade();
            daoEntidade = new DAOTipoGrade(_session);
            viewModelStrategy = new CadastrarTipoGradeVMStrategy();
            GetTipoGrades();
            PropertyChanged += CadastrarTipoGrade_PropertyChanged;
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

        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {
            Entidade = new Model.TipoGrade();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Nome?.Trim()))
            {
                BtnSalvarToolTip += "Nome Não Pode Ser Vazio!\n";
                valido = false;
            }

            if (TipoGradePesquisa != null)
            {
                BtnSalvarToolTip += "Já Existe Um Tipo De Grade Com Esse Nome!\n";
                valido = false;
            }

            return valido;
        }
        private async void GetTipoGrades()
        {
            TipoGrades = new ObservableCollection<Model.TipoGrade>(await daoEntidade.Listar());
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
