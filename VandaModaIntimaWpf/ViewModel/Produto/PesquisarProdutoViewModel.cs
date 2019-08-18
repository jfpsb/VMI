using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class PesquisarProdutoViewModel : APesquisarViewModel
    {
        private DAOProduto daoProduto;
        private EntidadeComCampo<ProdutoModel> produtoSelecionado;
        private ObservableCollection<EntidadeComCampo<ProdutoModel>> produtos;
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Descricao,
            CodBarra,
            Fornecedor,
            Marca
        }
        public PesquisarProdutoViewModel() : base()
        {
            daoProduto = new DAOProduto(_session);
            excelStrategy = new ExcelStrategy(new ProdutoExcelStrategy());
            PropertyChanged += PesquisarViewModel_PropertyChanged;
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public override void AbrirCadastrar(object parameter)
        {
            CadastrarProduto cadastrar = new CadastrarProduto();
            cadastrar.ShowDialog();
            OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
        }

        public override async void AbrirApagarMsgBox(object parameter)
        {
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog("Tem Certeza Que Deseja Apagar o Produto " + produtoSelecionado.Entidade.Descricao + "?", "Apagar Produto");
            bool? result = telaApagarDialog.ShowDialog();

            if (result == true)
            {
                bool deletado = await daoProduto.Deletar(produtoSelecionado.Entidade);

                if (deletado)
                {
                    SetStatusBarApagado();
                    OnPropertyChanged("TermoPesquisa");
                    await ResetarStatusBar();
                }
                else
                {
                    StatusBarText = "Produto Não Foi Deletado";
                }
            }
        }

        public override void AbrirEditar(object parameter)
        {
            ProdutoModel produtoBkp = (ProdutoModel)produtoSelecionado.Entidade.Clone();

            EditarProduto editar = new EditarProduto(produtoSelecionado.Entidade.Cod_Barra);

            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                produtoSelecionado.Entidade.Descricao = produtoBkp.Descricao;
                produtoSelecionado.Entidade.Preco = produtoBkp.Preco;
                produtoSelecionado.Entidade.Fornecedor = produtoBkp.Fornecedor;
                produtoSelecionado.Entidade.Marca = produtoBkp.Marca;
                produtoSelecionado.Entidade.Codigos = produtoBkp.Codigos;
            }
        }

        public override void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<ProdutoModel> pm in produtos)
            {
                if (pm.IsChecked)
                    marcados++;
            }

            if (marcados > 1)
                VisibilidadeBotaoApagarSelecionado = Visibility.Visible;
            else
                VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        }

        public override async void ApagarMarcados(object parameter)
        {
            var Mensagem = (IMessageable)parameter;
            var resultMsgBox = Mensagem.MensagemSimOuNao("Desejar Apagar os Produtos Marcados?", "Apagar Produtos");

            if (resultMsgBox == MessageBoxResult.Yes)
            {
                IList<ProdutoModel> AApagar = new List<ProdutoModel>();

                foreach (EntidadeComCampo<ProdutoModel> pm in produtos)
                {
                    if (pm.IsChecked)
                        AApagar.Add(pm.Entidade);
                }

                bool result = await daoProduto.Deletar(AApagar);

                if (result)
                {
                    Mensagem.MensagemDeAviso("Produtos Apagados Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Erro ao Apagar Produtos");
                }
            }
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

        public ObservableCollection<EntidadeComCampo<ProdutoModel>> Produtos
        {
            get { return produtos; }
            set
            {
                produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public EntidadeComCampo<ProdutoModel> ProdutoSelecionado
        {
            get { return produtoSelecionado; }
            set
            {
                produtoSelecionado = value;
                if (produtoSelecionado != null)
                {
                    OnPropertyChanged("ProdutoSelecionado");
                    OnPropertyChanged("ProdutoSelecionadoDescricao");
                }
            }
        }

        public string ProdutoSelecionadoDescricao
        {
            get
            {
                if (produtoSelecionado != null)
                {
                    return produtoSelecionado.Entidade.Descricao.ToUpper();
                }

                return string.Empty;
            }
        }

        public override async void GetItems(string termo)
        {
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Descricao:
                    Produtos = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorDescricao(termo)));
                    break;
                case (int)OpcoesPesquisa.CodBarra:
                    Produtos = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorCodigoDeBarra(termo)));
                    break;
                case (int)OpcoesPesquisa.Fornecedor:
                    Produtos = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorFornecedor(termo)));
                    break;
                case (int)OpcoesPesquisa.Marca:
                    Produtos = new ObservableCollection<EntidadeComCampo<ProdutoModel>>(EntidadeComCampo<ProdutoModel>.ConverterIList(await daoProduto.ListarPorMarca(termo)));
                    break;
            }
        }

        public override async void ExportarExcel(object parameter)
        {
            IsThreadLocked = true;
            await new Excel<ProdutoModel>(excelStrategy).Salvar(EntidadeComCampo<ProdutoModel>.ConverterIList(Produtos));
            IsThreadLocked = false;
        }
        public override async void ImportarExcel(object parameter)
        {
            var OpenFileDialog = (IOpenFileDialog)parameter;

            string path = OpenFileDialog.OpenFileDialog();

            if (path != null)
            {
                IsThreadLocked = true;
                await new Excel<ProdutoModel>(excelStrategy, path).Importar();
                IsThreadLocked = false;
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public override void SetStatusBarApagado()
        {
            StatusBarText = "Produto " + ProdutoSelecionado.Entidade.Descricao + " Foi Deletado Com Sucesso";
        }

        public override void AbrirAjuda(object parameter)
        {
            AjudaProduto ajudaProduto = new AjudaProduto();
            ajudaProduto.ShowDialog();
        }
    }
}
