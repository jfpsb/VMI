using NHibernate;
using System.Collections.ObjectModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVM : CadastrarProdutoVM
    {
        public EditarProdutoVM(ISession session, ProdutoModel produto, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            viewModelStrategy = new EditarProdutoVMStrategy();
            Entidade = produto;
            ProdutoGrade.Produto = Entidade;
            ProdutoGrades = new ObservableCollection<ProdutoGrade>(Entidade.Grades);
            issoEUmUpdate = true;
        }
        public new ProdutoModel Entidade
        {
            get { return _entidade; }
            set
            {
                _entidade = value;
                FornecedorComboBox = value.Fornecedor;
                MarcaComboBox = value.Marca;
                OnPropertyChanged("Entidade");
            }
        }
        public FornecedorModel FornecedorComboBox
        {
            get
            {
                if (Entidade.Fornecedor == null)
                {
                    Entidade.Fornecedor = Fornecedores[0];
                }

                return Entidade.Fornecedor;
            }

            set
            {
                Entidade.Fornecedor = value;
                OnPropertyChanged("FornecedorComboBox");
            }
        }
        public MarcaModel MarcaComboBox
        {
            get
            {
                if (Entidade.Marca == null)
                {
                    Entidade.Marca = Marcas[0];
                }

                return Entidade.Marca;
            }

            set
            {
                Entidade.Marca = value;
                OnPropertyChanged("MarcaComboBox");
            }
        }
    }
}
