using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.Ferias
{
    public class VisualizadorDeFeriasVM : APesquisarViewModel<Model.Ferias>
    {
        private DAOFuncionario daoFuncionario;
        private ObservableCollection<Model.Funcionario> _funcionarios;
        private DateTime _anoEscolhido;

        public VisualizadorDeFeriasVM()
        {
            daoFuncionario = new DAOFuncionario(_session);
            GetFuncionarios();
            AnoEscolhido = DateTime.Now;
        }

        private async void GetFuncionarios()
        {
            Funcionarios = new ObservableCollection<Model.Funcionario>(await daoFuncionario.ListarNaoDemitidos());
        }

        public ObservableCollection<Model.Funcionario> Funcionarios
        {
            get
            {
                return _funcionarios;
            }

            set
            {
                _funcionarios = value;
                OnPropertyChanged("Funcionarios");
            }
        }

        public DateTime AnoEscolhido
        {
            get
            {
                return _anoEscolhido;
            }

            set
            {
                _anoEscolhido = value;
                OnPropertyChanged("AnoEscolhido");
            }
        }

        public override bool Editavel(object parameter)
        {
            return false;
        }

        public override object GetCadastrarViewModel()
        {
            throw new System.NotImplementedException();
        }

        public override object GetEditarViewModel()
        {
            throw new System.NotImplementedException();
        }

        public override Task PesquisaItens(string termo)
        {
            throw new System.NotImplementedException();
        }

        protected override WorksheetContainer<Model.Ferias>[] GetWorksheetContainers()
        {
            throw new System.NotImplementedException();
        }
    }
}
