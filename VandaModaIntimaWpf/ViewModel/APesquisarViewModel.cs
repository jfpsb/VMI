using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Arquivo;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel<E> : ObservableObject, IPesquisarViewModel where E : class, IModel, ICloneable
    {
        protected ISession _session;
        protected ExcelStrategy excelStrategy;
        protected IPesquisarViewModelStrategy<E> pesquisarViewModelStrategy;
        private string mensagemStatusBar;
        private string termoPesquisa;
        private bool _threadLocked;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        private string formId;
        private string imagemStatusBar;
        private DataGridCellInfo celulaSelecionada;
        private EntidadeComCampo<E> entidadeSelecionada;
        private ObservableCollection<EntidadeComCampo<E>> entidades;
        protected DAO daoEntidade;

        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        protected static readonly string IMAGEMDELETADO = "/Resources/Delete.png";
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand AbrirAjudaComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public ICommand ApagarMarcadosComando { get; set; }
        public ICommand ExportarExcelComando { get; set; }
        public ICommand ImportarExcelComando { get; set; }
        public ICommand CopiarValorCelulaComando { get; set; }
        public ICommand ExportarSQLComando { get; set; }
        public APesquisarViewModel(string formId)
        {
            AbrirCadastrarComando = new RelayCommand(AbrirCadastrar);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox);
            AbrirEditarComando = new RelayCommand(AbrirEditar, IsEditable);
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados);
            ApagarMarcadosComando = new RelayCommand(ApagarMarcados);
            ExportarExcelComando = new RelayCommand(ExportarExcel);
            ImportarExcelComando = new RelayCommand(ImportarExcel);
            AbrirAjudaComando = new RelayCommand(AbrirAjuda);
            CopiarValorCelulaComando = new RelayCommand(CopiarValorCelula);
            ExportarSQLComando = new RelayCommand(AbrirExportarSQL);

            this.formId = formId;
            _session = SessionProvider.GetSession(formId);

            PropertyChanged += PesquisarViewModel_PropertyChanged;

            SetStatusBarAguardando();
        }
        public abstract void GetItems(string termo);
        public abstract bool IsEditable(object parameter);
        public void AbrirCadastrar(object parameter)
        {
            pesquisarViewModelStrategy.AbrirCadastrar(parameter);
            OnPropertyChanged("TermoPesquisa");
        }
        public async void AbrirApagarMsgBox(object parameter)
        {
            TelaApagarDialog telaApagarDialog = new TelaApagarDialog(pesquisarViewModelStrategy.MensagemApagarEntidadeCerteza(EntidadeSelecionada.Entidade), pesquisarViewModelStrategy.TelaApagarCaption());
            bool? result = telaApagarDialog.ShowDialog();

            if (result == true)
            {
                bool deletado = await daoEntidade.Deletar(EntidadeSelecionada.Entidade);

                if (deletado)
                {
                    SetStatusBarItemDeletado(pesquisarViewModelStrategy.MensagemEntidadeDeletada(EntidadeSelecionada.Entidade));
                    OnPropertyChanged("TermoPesquisa");
                    await ResetarStatusBar();
                }
                else
                {
                    MensagemStatusBar = pesquisarViewModelStrategy.MensagemEntidadeNaoDeletada();
                }
            }
        }
        public void AbrirEditar(object parameter)
        {
            E backup = (E)EntidadeSelecionada.Entidade.Clone();

            var result = pesquisarViewModelStrategy.AbrirEditar(backup);

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                pesquisarViewModelStrategy.RestauraEntidade(EntidadeSelecionada.Entidade, backup);
            }
        }
        public void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<E> em in entidades)
            {
                if (em.IsChecked)
                    marcados++;
            }

            if (marcados > 1)
                VisibilidadeBotaoApagarSelecionado = Visibility.Visible;
            else
                VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        }
        public async void ApagarMarcados(object parameter)
        {
            TelaApagarDialog telaApagar = new TelaApagarDialog(pesquisarViewModelStrategy.MensagemApagarMarcados(), pesquisarViewModelStrategy.TelaApagarCaption());
            bool? result = telaApagar.ShowDialog();

            if (result == true)
            {
                IList<E> AApagar = new List<E>();

                foreach (EntidadeComCampo<E> em in entidades)
                {
                    if (em.IsChecked)
                        AApagar.Add(em.Entidade);
                }

                bool resultDeletar = await daoEntidade.Deletar(AApagar);

                if (resultDeletar)
                {
                    SetStatusBarItemDeletado(pesquisarViewModelStrategy.MensagemEntidadesDeletadas());
                }
                else
                {
                    SetStatusBarErro(pesquisarViewModelStrategy.MensagemEntidadesNaoDeletadas());
                }
            }
        }
        public void AbrirAjuda(object parameter)
        {
            pesquisarViewModelStrategy.AbrirAjuda(parameter);
        }
        public async void ImportarExcel(object parameter)
        {
            var OpenFileDialog = (IOpenFileDialog)parameter;

            string path = OpenFileDialog.OpenFileDialog();

            if (path != null)
            {
                IsThreadLocked = true;
                await new Excel<E>(excelStrategy, path).Importar();
                IsThreadLocked = false;
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public void CopiarValorCelula(object parameter)
        {
            string valorCelula = (CelulaSelecionada.Column.GetCellContent(CelulaSelecionada.Item) as TextBlock).Text;
            Clipboard.SetText(valorCelula);
        }
        public async void ExportarExcel(object parameter)
        {
            SetStatusBarAguardandoExcel();
            IsThreadLocked = true;
            await new Excel<E>(excelStrategy).Salvar(EntidadeComCampo<E>.ConverterIList(Entidades));
            IsThreadLocked = false;
            SetStatusBarExportadoComSucesso();
        }
        public void SetStatusBarItemDeletado(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMDELETADO;
            OnPropertyChanged("TermoPesquisa");
        }
        public async Task ResetarStatusBar()
        {
            await Task.Delay(7000); //Espera 7 segundos para resetar StatusBar
            SetStatusBarAguardando();
        }
        public void SetStatusBarAguardandoExcel()
        {
            MensagemStatusBar = "O Arquivo Excel Está Sendo Gerado. Aguarde a Tela Para Salvar";
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        public void SetStatusBarAguardando()
        {
            MensagemStatusBar = "Aguardando Usuário";
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        public async void SetStatusBarExportadoComSucesso()
        {
            MensagemStatusBar = "Exportação em Excel Realizada Com Sucesso";
            ImagemStatusBar = IMAGEMSUCESSO;
            await Task.Delay(7000);
            SetStatusBarAguardando();
        }
        protected void PesquisarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    GetItems(termoPesquisa);
                    break;
            }
        }
        public string TermoPesquisa
        {
            get { return termoPesquisa; }
            set
            {
                termoPesquisa = value;
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public bool IsThreadLocked
        {
            get { return _threadLocked; }
            set
            {
                _threadLocked = value;
                OnPropertyChanged("IsThreadLocked");
            }
        }
        public string MensagemStatusBar
        {
            get { return mensagemStatusBar; }
            set
            {
                mensagemStatusBar = value;
                OnPropertyChanged("MensagemStatusBar");
            }
        }
        public Visibility VisibilidadeBotaoApagarSelecionado
        {
            get { return visibilidadeBotaoApagarSelecionado; }
            set
            {
                visibilidadeBotaoApagarSelecionado = value;
                OnPropertyChanged("VisibilidadeBotaoApagarSelecionado");
            }
        }
        public string ImagemStatusBar
        {
            get { return imagemStatusBar; }
            set
            {
                imagemStatusBar = value;
                OnPropertyChanged("ImagemStatusBar");
            }
        }
        public DataGridCellInfo CelulaSelecionada
        {
            get { return celulaSelecionada; }
            set
            {
                celulaSelecionada = value;
                OnPropertyChanged("CelulaSelecionada");
            }
        }
        public EntidadeComCampo<E> EntidadeSelecionada
        {
            get { return entidadeSelecionada; }
            set
            {
                entidadeSelecionada = value;
                OnPropertyChanged("EntidadeSelecionada");
            }
        }
        public ObservableCollection<EntidadeComCampo<E>> Entidades
        {
            get { return entidades; }
            set
            {
                entidades = value;
                OnPropertyChanged("Entidades");
            }
        }
        public void DisposeSession()
        {
            SessionProvider.FechaSession(formId);
        }

        bool IPesquisarViewModel.IsThreadLocked()
        {
            return _threadLocked;
        }

        public async void SetStatusBarErro(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMERRO;
            await Task.Delay(7000);
            SetStatusBarAguardando();
        }

        public async void AbrirExportarSQL(object parameter)
        {
            pesquisarViewModelStrategy.AbrirExportarSQL(parameter, await daoEntidade.Listar<E>());
        }
    }
}
