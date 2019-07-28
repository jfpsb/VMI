using System;
using System.Collections.ObjectModel;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorViewModel : APesquisarViewModel
    {
        private FornecedorModel fornecedor;
        private EntidadeComCampo<FornecedorModel> fornecedorSelecionado;
        private ObservableCollection<EntidadeComCampo<FornecedorModel>> fornecedores;
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome,
            NomeFantasia,
            Email
        }
        public PesquisarFornecedorViewModel() : base()
        {
            fornecedor = new FornecedorModel();
            PropertyChanged += PesquisarViewModel_PropertyChanged;
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public override void AbrirCadastrarNovo(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void AbrirApagarMsgBox(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void AbrirEditar(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void ChecarItensMarcados(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void ApagarMarcados(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void ExportarExcel(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void GetItems(string termo)
        {
            throw new NotImplementedException();
        }
        public int PesquisarPor
        {
            get { return pesquisarPor; }
            set
            {
                pesquisarPor = value;
                OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
            }
        }
        public ObservableCollection<EntidadeComCampo<FornecedorModel>> Fornecedores
        {
            get { return fornecedores; }
            set
            {
                fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }

        public FornecedorModel Fornecedor
        {
            get { return fornecedor; }
            set
            {
                fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }

        public EntidadeComCampo<FornecedorModel> FornecedorSelecionado
        {
            get { return fornecedorSelecionado; }
            set
            {
                fornecedorSelecionado = value;
                if (fornecedorSelecionado != null)
                {
                    OnPropertyChanged("FornecedorSelecionado");
                    OnPropertyChanged("FornecedorSelecionadoNome");
                }
            }
        }

        public string FornecedorSelecionadoNome
        {
            get { return fornecedorSelecionado.Entidade.Nome.ToUpper(); }
        }
    }
}
