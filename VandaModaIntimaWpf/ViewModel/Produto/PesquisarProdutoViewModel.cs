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
    class PesquisarProdutoViewModel : APesquisarViewModel<ProdutoModel>
    {
        private DAOProduto daoProduto;
        private ObservableCollection<EntidadeComCampo<ProdutoModel>> produtos;
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Descricao,
            CodBarra,
            Fornecedor,
            Marca
        }
        public PesquisarProdutoViewModel() : base("Produto")
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
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog("Tem Certeza Que Deseja Apagar o Produto " + EntidadeSelecionada.Entidade.Descricao + "?", "Apagar Produto");
            bool? result = telaApagarDialog.ShowDialog();

            if (result == true)
            {
                bool deletado = await daoProduto.Deletar(EntidadeSelecionada.Entidade);

                if (deletado)
                {
                    SetStatusBarItemDeletado("Produto " + EntidadeSelecionada.Entidade.Descricao + " Foi Deletado Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                    await ResetarStatusBar();
                }
                else
                {
                    MensagemStatusBar = "Produto Não Foi Deletado";
                }
            }
        }

        public override void AbrirEditar(object parameter)
        {
            ProdutoModel produtoBkp = (ProdutoModel)EntidadeSelecionada.Entidade.Clone();

            EditarProduto editar = new EditarProduto(EntidadeSelecionada.Entidade.Cod_Barra);

            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                EntidadeSelecionada.Entidade.Descricao = produtoBkp.Descricao;
                EntidadeSelecionada.Entidade.Preco = produtoBkp.Preco;
                EntidadeSelecionada.Entidade.Fornecedor = produtoBkp.Fornecedor;
                EntidadeSelecionada.Entidade.Marca = produtoBkp.Marca;
                EntidadeSelecionada.Entidade.Codigos = produtoBkp.Codigos;
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
            base.ExportarExcel(parameter);
            IsThreadLocked = true;
            await new Excel<ProdutoModel>(excelStrategy).Salvar(EntidadeComCampo<ProdutoModel>.ConverterIList(Produtos));
            IsThreadLocked = false;
            SetStatusBarExportadoComSucesso();
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
        public override void AbrirAjuda(object parameter)
        {
            AjudaProduto ajudaProduto = new AjudaProduto();
            ajudaProduto.ShowDialog();
        }
    }
}
