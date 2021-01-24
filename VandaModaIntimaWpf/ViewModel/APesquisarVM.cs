using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel<E> : ObservableObject, IPesquisarVM where E : class, IModel
    {
        protected ISession _session;
        protected ExcelStrategy excelStrategy;
        protected IPesquisarMsgVMStrategy<E> pesquisarViewModelStrategy;
        private string mensagemStatusBar;
        private string termoPesquisa;
        private bool _threadLocked;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        private BitmapImage imagemStatusBar;
        private DataGridCellInfo celulaSelecionada;
        private EntidadeComCampo<E> entidadeSelecionada;
        private ObservableCollection<EntidadeComCampo<E>> _entidades;

        protected IMessageBoxService MessageBoxService;
        protected IAbrePelaTelaPesquisaService<E> AbrePelaTelaPesquisaService;

        protected CouchDbClient couchDbClient;
        protected DAO daoEntidade;

        public delegate void AposDeletarDocumentoEventHandler(AposDeletarDocumentoEventArgs e);
        public delegate void AposDeletarDoBDEventHandler(AposDeletarDoBDEventArgs e);
        public event AposDeletarDocumentoEventHandler AposDeletarDocumento;
        public event AposDeletarDoBDEventHandler AposDeletarDoBD;
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirImprimirComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand AbrirAjudaComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public ICommand ApagarMarcadosComando { get; set; }
        public ICommand ExportarExcelComando { get; set; }
        public ICommand ImportarExcelComando { get; set; }
        public ICommand CopiarValorCelulaComando { get; set; }
        public ICommand ExportarSQLComando { get; set; }
        public APesquisarViewModel(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<E> abrePelaTelaPesquisaService)
        {
            MessageBoxService = messageBoxService;
            AbrePelaTelaPesquisaService = abrePelaTelaPesquisaService;

            AbrirCadastrarComando = new RelayCommand(AbrirCadastrar);
            AbrirImprimirComando = new RelayCommand(AbrirImprimir);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox);
            AbrirEditarComando = new RelayCommand(AbrirEditar, Editavel);
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados);
            ApagarMarcadosComando = new RelayCommand(ApagarMarcados);
            ExportarExcelComando = new RelayCommand(ExportarExcel);
            ImportarExcelComando = new RelayCommand(ImportarExcel);
            AbrirAjudaComando = new RelayCommand(AbrirAjuda);
            CopiarValorCelulaComando = new RelayCommand(CopiarValorCelula);
            ExportarSQLComando = new RelayCommand(AbrirExportarSQL);
            couchDbClient = CouchDbClient.Instancia;

            _session = SessionProvider.GetSession();

            PropertyChanged += PesquisarViewModel_PropertyChanged;

            AposDeletarDocumento += DeletarEntidadeDoBD;
            AposDeletarDoBD += InformaResultadoDeletarBD;

            SetStatusBarAguardando();
        }

        public abstract void PesquisaItens(string termo);
        public void AbrirImprimir(object parameter)
        {
            AbrePelaTelaPesquisaService.AbrirImprimir(EntidadeComCampo<E>.ConverterIList(Entidades));
        }
        public abstract bool Editavel(object parameter);
        protected virtual void ChamaAposDeletarDocumento(AposDeletarDocumentoEventArgs e)
        {
            AposDeletarDocumento?.Invoke(e);
        }
        protected virtual void ChamaAposDeletarDoBD(AposDeletarDoBDEventArgs e)
        {
            AposDeletarDoBD?.Invoke(e);
        }
        public void AbrirCadastrar(object parameter)
        {
            var result = AbrePelaTelaPesquisaService.AbrirCadastrar(_session);
            //Se houve cadastro
            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public async void AbrirApagarMsgBox(object parameter)
        {
            MessageBoxResult telaApagarDialog = MessageBoxService.Show(pesquisarViewModelStrategy.MensagemApagarEntidadeCerteza(EntidadeSelecionada.Entidade),
                pesquisarViewModelStrategy.TelaApagarCaption(),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagarDialog.Equals(MessageBoxResult.Yes))
            {
                CouchDbLog couchDbLog = await couchDbClient.FindById(EntidadeSelecionada.Entidade.ToString());
                CouchDbResponse couchDbResponse;

                if (couchDbLog != null)
                {
                    couchDbResponse = await couchDbClient.DeleteDocument(couchDbLog.Id, couchDbLog.Rev);
                }
                else
                {
                    //Se não existe log
                    couchDbResponse = new CouchDbResponse() { Ok = true };
                }

                AposDeletarDocumentoEventArgs e = new AposDeletarDocumentoEventArgs()
                {
                    CouchDbResponse = couchDbResponse,
                    MensagemSucesso = pesquisarViewModelStrategy.MensagemDocumentoDeletado(),
                    MensagemErro = pesquisarViewModelStrategy.MensagemDocumentoNaoDeletado()
                };

                ChamaAposDeletarDocumento(e);
            }
        }
        private async void DeletarEntidadeDoBD(AposDeletarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                bool deletado = await daoEntidade.Deletar(EntidadeSelecionada.Entidade);

                AposDeletarDoBDEventArgs e2 = new AposDeletarDoBDEventArgs()
                {
                    DeletadoComSucesso = deletado,
                    MensagemSucesso = pesquisarViewModelStrategy.MensagemEntidadeDeletada(EntidadeSelecionada.Entidade),
                    MensagemErro = pesquisarViewModelStrategy.MensagemEntidadeNaoDeletada()
                };

                ChamaAposDeletarDoBD(e2);
            }
            else
            {
                SetStatusBarErro(e.MensagemErro);
            }
        }
        private async void InformaResultadoDeletarBD(AposDeletarDoBDEventArgs e)
        {
            if (e.DeletadoComSucesso)
            {
                SetStatusBarItemDeletado(e.MensagemSucesso);
                OnPropertyChanged("TermoPesquisa");
                await ResetarStatusBar();
            }
            else
            {
                SetStatusBarErro(e.MensagemErro);
            }
        }
        public void AbrirEditar(object parameter)
        {
            try
            {
                EntidadeSelecionada.Entidade.InicializaLazyLoad();
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
            }


            AbrePelaTelaPesquisaService.AbrirEditar(EntidadeSelecionada.Entidade, _session);

            _session.Evict(EntidadeSelecionada.Entidade);
            OnPropertyChanged("TermoPesquisa");
        }
        public void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (EntidadeComCampo<E> em in _entidades)
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
            MessageBoxResult telaApagar = MessageBoxService.Show(pesquisarViewModelStrategy.MensagemApagarMarcados(),
                pesquisarViewModelStrategy.TelaApagarCaption(),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
                IList<E> AApagar = new List<E>();

                foreach (EntidadeComCampo<E> em in _entidades)
                {
                    if (em.IsChecked)
                        AApagar.Add(em.Entidade);
                }

                bool resultDeletar = await daoEntidade.Deletar(AApagar);

                if (resultDeletar)
                {
                    SetStatusBarItemDeletado(pesquisarViewModelStrategy.MensagemEntidadesDeletadas());
                    VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
                }
                else
                {
                    SetStatusBarErro(pesquisarViewModelStrategy.MensagemEntidadesNaoDeletadas());
                }
            }
        }
        public void AbrirAjuda(object parameter)
        {
            AbrePelaTelaPesquisaService.AbrirAjuda();
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
        public virtual async void ExportarExcel(object parameter)
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
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemDeletado");
            OnPropertyChanged("TermoPesquisa");
        }
        public async Task ResetarStatusBar()
        {
            await Task.Delay(7000); //Espera 7 segundos para resetar StatusBar
            SetStatusBarAguardando();
        }
        public void SetStatusBarAguardandoExcel()
        {
            MensagemStatusBar = GetResource.GetString("arquivo_excel_sendo_gerado");
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemAguardando");
        }
        public void SetStatusBarAguardando()
        {
            MensagemStatusBar = GetResource.GetString("aguardando_usuario");
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemAguardando");
        }
        public async void SetStatusBarExportadoComSucesso()
        {
            MensagemStatusBar = GetResource.GetString("exportacao_excel_realizada_com_sucesso");
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemSucesso");
            await Task.Delay(7000);
            SetStatusBarAguardando();
        }
        protected void PesquisarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    PesquisaItens(termoPesquisa);
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
        public BitmapImage ImagemStatusBar
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
            get { return _entidades; }
            set
            {
                _entidades = value;
                OnPropertyChanged("Entidades");
            }
        }
        public void DisposeSession()
        {
            SessionProvider.FechaSession(_session);
        }

        bool IPesquisarVM.IsThreadLocked()
        {
            return _threadLocked;
        }

        public async void SetStatusBarErro(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = GetResource.GetBitmapImage("ImagemErro");
            await Task.Delay(7000);
            SetStatusBarAguardando();
        }

        public async void AbrirExportarSQL(object parameter)
        {
            AbrePelaTelaPesquisaService.AbrirExportarSQL(await daoEntidade.Listar<E>());
        }
    }
}
