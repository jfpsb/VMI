using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaViewModel : APesquisarViewModel
    {
        private DAOMarca daoMarca;
        private EntidadeComCampo<MarcaModel> marcaSelecionada;
        private ObservableCollection<EntidadeComCampo<MarcaModel>> marcas;
        public PesquisarMarcaViewModel() : base("Marca")
        {
            daoMarca = new DAOMarca(_session);
            excelStrategy = new ExcelStrategy(new MarcaExcelStrategy());
            PropertyChanged += PesquisarViewModel_PropertyChanged;
            OnPropertyChanged("TermoPesquisa");
        }

        public ObservableCollection<EntidadeComCampo<MarcaModel>> Marcas
        {
            get { return marcas; }
            set
            {
                marcas = value;
                OnPropertyChanged("Marcas");
            }
        }

        public EntidadeComCampo<MarcaModel> MarcaSelecionada
        {
            get { return marcaSelecionada; }
            set
            {
                marcaSelecionada = value;
                OnPropertyChanged("MarcaSelecionada");
            }
        }

        public override void AbrirAjuda(object parameter)
        {
            throw new System.NotImplementedException();
        }

        public override async void AbrirApagarMsgBox(object parameter)
        {
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog("Tem Certeza Que Deseja Apagar a Marca " + MarcaSelecionada.Entidade.Nome + "?", "Apagar Marca");
            bool? result = telaApagarDialog.ShowDialog();

            if (result == true)
            {
                bool deletado = await daoMarca.Deletar(MarcaSelecionada.Entidade);

                if (deletado)
                {
                    SetStatusBarItemDeletado("Marca " + MarcaSelecionada.Entidade.Nome + " Foi Deletada Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                    await ResetarStatusBar();
                }
                else
                {
                    MensagemStatusBar = "Marca Não Foi Deletada";
                }
            }


        }

        public override void AbrirCadastrar(object parameter)
        {
            CadastrarMarca cadastrarMarca = new CadastrarMarca();
            cadastrarMarca.ShowDialog();
            OnPropertyChanged("TermoPesquisa");
        }

        public override void AbrirEditar(object parameter)
        {
            MarcaModel marcaBkp = (MarcaModel)marcaSelecionada.Entidade.Clone();

            EditarMarca editar = new EditarMarca(marcaSelecionada.Entidade.Nome);
            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                marcaSelecionada.Entidade.Nome = marcaBkp.Nome;
            }
        }

        public override async void ApagarMarcados(object parameter)
        {
            var Mensagem = (IMessageable)parameter;
            var resultMsgBox = Mensagem.MensagemSimOuNao("Desejar Apagar as Marcas Selecionadas?", "Apagar Marcas");

            if (resultMsgBox == MessageBoxResult.Yes)
            {
                IList<MarcaModel> AApagar = new List<MarcaModel>();

                foreach (EntidadeComCampo<MarcaModel> mm in marcas)
                {
                    if (mm.IsChecked)
                        AApagar.Add(mm.Entidade);
                }

                bool result = await daoMarca.Deletar(AApagar);

                if (result)
                {
                    Mensagem.MensagemDeAviso("Marcas Apagadas Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Erro ao Apagar Marcas");
                }
            }
        }

        public override void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<MarcaModel> mm in marcas)
            {
                if (mm.IsChecked)
                    marcados++;
            }

            if (marcados > 1)
                VisibilidadeBotaoApagarSelecionado = Visibility.Visible;
            else
                VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        }

        public override async void ExportarExcel(object parameter)
        {
            base.ExportarExcel(parameter);
            IsThreadLocked = true;
            await new Excel<MarcaModel>(excelStrategy).Salvar(EntidadeComCampo<MarcaModel>.ConverterIList(Marcas));
            IsThreadLocked = false;
            SetStatusBarExportadoComSucesso();
        }

        public override async void GetItems(string termo)
        {
            Marcas = new ObservableCollection<EntidadeComCampo<MarcaModel>>(EntidadeComCampo<MarcaModel>.ConverterIList(await daoMarca.ListarPorNome(termo)));
        }

        public override async void ImportarExcel(object parameter)
        {
            var OpenFileDialog = (IOpenFileDialog)parameter;

            string path = OpenFileDialog.OpenFileDialog();

            if (path != null)
            {
                IsThreadLocked = true;
                await new Excel<MarcaModel>(excelStrategy, path).Importar();
                IsThreadLocked = false;
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
