using NHibernate;
using System.Collections.ObjectModel;
using System.Linq;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class VisualizarMargensDeLucroVM : ObservableObject
    {
        private DAOProdutoGrade daoProdutoGrade;
        private double _aliquotaLucro;
        private ObservableCollection<ProdutoGrade> _produtoGrades;
        public VisualizarMargensDeLucroVM(ISession session)
        {
            daoProdutoGrade = new DAOProdutoGrade(session);
            PropertyChanged += VisualizarMargensDeLucroVM_PropertyChanged;
            GetProdutoGrades();

            AliquotaLucro = 40;
        }

        private async void GetProdutoGrades()
        {
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(await daoProdutoGrade.ListarComLucroValido());
        }

        private void VisualizarMargensDeLucroVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AliquotaLucro"))
            {
                OnPropertyChanged("ProdutoGrades");
            }
        }

        public double AliquotaLucro
        {
            get => _aliquotaLucro;
            set
            {
                _aliquotaLucro = value / 100;
                OnPropertyChanged("AliquotaLucro");
            }
        }
        public ObservableCollection<ProdutoGrade> ProdutoGrades
        {
            get => _produtoGrades;
            set
            {
                _produtoGrades = new ObservableCollection<ProdutoGrade>(value.OrderBy(o => o.MargemDeLucro));
                OnPropertyChanged("ProdutoGrades");
            }
        }
    }
}
