using NHibernate;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class ConsolidarPontosEletronicosVM : ObservableObject
    {
        private ObservableCollection<Model.Funcionario> _funcionarios;
        private Model.Funcionario _funcionario;
        private DAOFuncionario daoFuncionario;
        public ConsolidarPontosEletronicosVM(ISession session)
        {
            daoFuncionario = new DAOFuncionario(session);

            var task = GetFuncionarios();
            task.Wait();
        }

        private async Task GetFuncionarios()
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

        public Model.Funcionario Funcionario
        {
            get
            {
                return _funcionario;
            }

            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
    }
}
