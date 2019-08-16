using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class PesquisarFornecedorViewModel : APesquisarViewModel
    {
        private DAOFornecedor daoFornecedor;
        private FornecedorModel fornecedor;
        private EntidadeComCampo<FornecedorModel> fornecedorSelecionado;
        private ObservableCollection<EntidadeComCampo<FornecedorModel>> fornecedores;
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome,
            Email
        }
        public PesquisarFornecedorViewModel() : base()
        {
            excelStrategy = new ExcelStrategy(new FornecedorExcelStrategy());
            daoFornecedor = new DAOFornecedor(_session);
            fornecedor = new FornecedorModel();
            PropertyChanged += PesquisarViewModel_PropertyChanged;
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public override void AbrirCadastrar(object parameter)
        {
            CadastrarFornecedor cadastrar = new CadastrarFornecedor();
            cadastrar.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        public override void AbrirApagarMsgBox(object parameter)
        {
            var Mensagem = ((IMessageable)parameter);
            var result = Mensagem.MensagemSimOuNao("Tem Certeza Que Deseja Apagar o Fornecedor?", "Apagar " + FornecedorSelecionado.Entidade.Nome + "?");

            if (result == MessageBoxResult.Yes)
            {
                bool deletado = daoFornecedor.Deletar(FornecedorSelecionado.Entidade);

                if (deletado)
                {
                    Mensagem.MensagemDeAviso("Fornecedor " + FornecedorSelecionado.Entidade.Nome + " Foi Deletado Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Fornecedor Não Foi Deletado");
                }
            }
        }

        public override void AbrirEditar(object parameter)
        {
            FornecedorModel fornecedorBkp = (FornecedorModel)FornecedorSelecionado.Entidade.Clone();

            EditarFornecedor editar = new EditarFornecedor(FornecedorSelecionado.Entidade.Cnpj);
            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                FornecedorSelecionado.Entidade.Cnpj = fornecedorBkp.Cnpj;
                FornecedorSelecionado.Entidade.Nome = fornecedorBkp.Nome;
                FornecedorSelecionado.Entidade.NomeFantasia = fornecedorBkp.NomeFantasia;
                FornecedorSelecionado.Entidade.Email = fornecedorBkp.Email;
            }
        }

        public override void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<FornecedorModel> em in Fornecedores)
            {
                if (em.IsChecked)
                    marcados++;
            }

            if (marcados > 1)
                VisibilidadeBotaoApagarSelecionado = Visibility.Visible;
            else
                VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        }

        public override void ApagarMarcados(object parameter)
        {
            var Mensagem = (IMessageable)parameter;
            var resultMsgBox = Mensagem.MensagemSimOuNao("Desejar Apagar os Fornecedores Marcados?", "Apagar Fornecedores");

            if (resultMsgBox == MessageBoxResult.Yes)
            {
                IList<FornecedorModel> AApagar = new List<FornecedorModel>();

                foreach (EntidadeComCampo<FornecedorModel> em in Fornecedores)
                {
                    if (em.IsChecked)
                        AApagar.Add(em.Entidade);
                }

                bool result = daoFornecedor.Deletar(AApagar);

                if (result)
                {
                    Mensagem.MensagemDeAviso("Fornecedores Apagados Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Erro ao Apagar Fornecedores");
                }
            }
        }

        public override void ExportarExcel(object parameter)
        {
            new Excel<FornecedorModel>(excelStrategy).Salvar(EntidadeComCampo<FornecedorModel>.ConverterIList(Fornecedores));
        }

        public override void GetItems(string termo)
        {
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cnpj:
                    Fornecedores = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.ConverterIList(daoFornecedor.ListarPorCnpj(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Fornecedores = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.ConverterIList(daoFornecedor.ListarPorNome(termo)));
                    break;
                case (int)OpcoesPesquisa.Email:
                    Fornecedores = new ObservableCollection<EntidadeComCampo<FornecedorModel>>(EntidadeComCampo<FornecedorModel>.ConverterIList(daoFornecedor.ListarPorEmail(termo)));
                    break;
            }
        }

        public override void ImportarExcel(object parameter)
        {
            var OpenFileDialog = (IOpenFileDialog)parameter;

            string path = OpenFileDialog.OpenFileDialog();

            if (path != null)
                new Excel<FornecedorModel>(excelStrategy, path).Importar();
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
            get
            {
                if (fornecedorSelecionado != null)
                {
                    return fornecedorSelecionado.Entidade.Nome.ToUpper();
                }

                return string.Empty;
            }
        }
    }
}
