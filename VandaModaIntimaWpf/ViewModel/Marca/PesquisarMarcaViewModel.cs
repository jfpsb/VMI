using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class PesquisarMarcaViewModel : APesquisarViewModel
    {
        private MarcaModel marca;
        private EntidadeComCampo<MarcaModel> marcaSelecionada;
        private ObservableCollection<EntidadeComCampo<MarcaModel>> marcas;
        public PesquisarMarcaViewModel() : base()
        {
            marca = new MarcaModel();
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

        public override void AbrirApagarMsgBox(object parameter)
        {
            var Mensagem = (IMessageable)parameter;

            var result = Mensagem.MensagemSimOuNao("Tem Certeza Que Deseja Apagar a Marca?", "Apagar " + marcaSelecionada.Entidade.Nome + "?");

            if (result == MessageBoxResult.Yes)
            {
                bool deletado = marcaSelecionada.Entidade.Deletar();

                if (deletado)
                {
                    Mensagem.MensagemDeAviso("Marca " + marcaSelecionada.Entidade.Nome + " Foi Deletada Com Sucesso");
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    Mensagem.MensagemDeErro("Marca Não Foi Deletada");
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

        public override void ApagarMarcados(object parameter)
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

                bool result = marca.Deletar(AApagar);

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

        public override void ExportarExcel(object parameter)
        {
            ExportarExcelStrategy exportaExcelStrategy = new ExportarExcelStrategy(new ExportarMarcaExcelStrategy());
            new Excel<MarcaModel>(exportaExcelStrategy).Salvar(EntidadeComCampo<MarcaModel>.ConverterIList(Marcas));
        }

        public override void GetItems(string termo)
        {
            Marcas = new ObservableCollection<EntidadeComCampo<MarcaModel>>(EntidadeComCampo<MarcaModel>.ConverterIList(marca.ListarPorNome(termo)));
        }

        public override void ImportarExcel(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
