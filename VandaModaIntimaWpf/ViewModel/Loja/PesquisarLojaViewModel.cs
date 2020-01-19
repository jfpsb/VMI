using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class PesquisarLojaViewModel : APesquisarViewModel
    {
        private DAOLoja daoLoja;
        private EntidadeComCampo<LojaModel> lojaSelecionada;
        private ObservableCollection<EntidadeComCampo<LojaModel>> lojas;
        private int pesquisarPor;
        private enum OpcoesPesquisa
        {
            Cnpj,
            Nome
        }
        public PesquisarLojaViewModel() : base("Loja")
        {
            daoLoja = new DAOLoja(_session);
            excelStrategy = new ExcelStrategy(new LojaExcelStrategy());
            PropertyChanged += PesquisarViewModel_PropertyChanged;
            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public override void AbrirCadastrar(object parameter)
        {
            CadastrarLoja cadastrar = new CadastrarLoja();
            cadastrar.ShowDialog();
            OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
        }

        public override async void AbrirApagarMsgBox(object parameter)
        {
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog("Tem Certeza Que Deseja Apagar a Loja " + lojaSelecionada.Entidade.Nome + "?", "Apagar Loja");
            bool? result = telaApagarDialog.ShowDialog();

            if (result == true)
            {
                bool deletado = await daoLoja.Deletar(lojaSelecionada.Entidade);

                if (deletado)
                {
                    SetStatusBarItemDeletado("Loja " + LojaSelecionada.Entidade.Nome + " Foi Deletada Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                    await ResetarStatusBar();
                }
                else
                {
                    MensagemStatusBar = "Loja Não Foi Deletada";
                }
            }
        }

        public override void AbrirEditar(object parameter)
        {
            LojaModel lojaBkp = (LojaModel)lojaSelecionada.Entidade.Clone();

            EditarLoja editar = new EditarLoja(lojaSelecionada.Entidade.Cnpj);

            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                lojaSelecionada.Entidade.Cnpj = lojaBkp.Cnpj;
                lojaSelecionada.Entidade.Nome = lojaBkp.Nome;
                lojaSelecionada.Entidade.Telefone = lojaBkp.Telefone;
                lojaSelecionada.Entidade.Endereco = lojaBkp.Endereco;
                lojaSelecionada.Entidade.InscricaoEstadual = lojaBkp.InscricaoEstadual;
                lojaSelecionada.Entidade.Matriz = lojaBkp.Matriz;
            }
        }

        public override void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<LojaModel> lm in lojas)
            {
                if (lm.IsChecked)
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
            var resultMsgBox = Mensagem.MensagemSimOuNao("Desejar Apagar as Lojas Marcadas?", "Apagar Lojas");

            if (resultMsgBox == MessageBoxResult.Yes)
            {
                IList<LojaModel> AApagar = new List<LojaModel>();

                foreach (EntidadeComCampo<LojaModel> lm in lojas)
                {
                    if (lm.IsChecked)
                        AApagar.Add(lm.Entidade);
                }

                bool result = await daoLoja.Deletar(AApagar);

                if (result)
                {
                    Mensagem.MensagemDeAviso("Lojas Apagadas Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Erro ao Apagar Lojas");
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
        public ObservableCollection<EntidadeComCampo<LojaModel>> Lojas
        {
            get { return lojas; }
            set
            {
                lojas = value;
                OnPropertyChanged("Lojas");
            }
        }
        public EntidadeComCampo<LojaModel> LojaSelecionada
        {
            get { return lojaSelecionada; }
            set
            {
                lojaSelecionada = value;
                if (lojaSelecionada != null)
                {
                    OnPropertyChanged("LojaSelecionada");
                }
            }
        }
        public override async void GetItems(string termo)
        {
            switch (pesquisarPor)
            {
                case (int)OpcoesPesquisa.Cnpj:
                    Lojas = new ObservableCollection<EntidadeComCampo<LojaModel>>(EntidadeComCampo<LojaModel>.ConverterIList(await daoLoja.ListarPorCnpj(termo)));
                    break;
                case (int)OpcoesPesquisa.Nome:
                    Lojas = new ObservableCollection<EntidadeComCampo<LojaModel>>(EntidadeComCampo<LojaModel>.ConverterIList(await daoLoja.ListarPorNome(termo)));
                    break;
            }
        }
        public override async void ExportarExcel(object parameter)
        {
            base.ExportarExcel(parameter);
            IsThreadLocked = true;
            await new Excel<LojaModel>(excelStrategy).Salvar(EntidadeComCampo<LojaModel>.ConverterIList(Lojas));
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
                await new Excel<LojaModel>(excelStrategy, path).Importar();
                IsThreadLocked = false;
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public override void AbrirAjuda(object parameter)
        {
            AjudaLoja ajudaLoja = new AjudaLoja();
            ajudaLoja.ShowDialog();
        }
    }
}
