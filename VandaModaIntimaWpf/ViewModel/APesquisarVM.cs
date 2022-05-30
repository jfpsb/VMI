using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;
using VandaModaIntimaWpf.ViewModel.Produto;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.SQL;
using VandaModaIntimaWpf.ViewModel.Util;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel<E> : ObservableObject, IPesquisarVM where E : AModel, IModel
    {
        protected ISession _session;
        protected AExcelStrategy<E> excelStrategy;
        protected IPesquisarMsgVMStrategy<E> pesquisarViewModelStrategy;
        protected ObservableCollection<EntidadeComCampo<E>> _entidadesOriginal;
        protected IMessageBoxService MessageBoxService;
        protected DAO<E> daoEntidade;
        protected IOpenViewService openView;

        private string termoPesquisa;
        private bool _threadLocked;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        private DataGridCellInfo celulaSelecionada;
        private EntidadeComCampo<E> entidadeSelecionada;
        private ObservableCollection<EntidadeComCampo<E>> _entidades;
        private Visibility _visibilidadeStatusBar = Visibility.Collapsed;
        private double _valorBarraProgresso;
        private string _descricaoBarraProgresso;
        private bool _isIndefinidaBarraProgresso;

        protected CancellationTokenSource cancellationTokenSource;
        protected IProgress<double> setValorBarraProgresso;
        protected IProgress<string> setDescricaoBarraProgresso;
        protected IProgress<bool> setIsIndefinidaBarraProgresso;

        public delegate void AposDeletarDoBDEventHandler(AposDeletarDoBDEventArgs e);
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
        public ICommand CancelaTaskComando { get; set; }
        public APesquisarViewModel()
        {
            MessageBoxService = new MessageBoxService();
            openView = new OpenView();

            cancellationTokenSource = new CancellationTokenSource();

            setValorBarraProgresso = new Progress<double>(valor =>
            {
                if (valor < 0.0)
                {
                    ValorBarraProgresso = 0;
                    return;
                }

                ValorBarraProgresso += valor;
            });
            setDescricaoBarraProgresso = new Progress<string>(descricao => { DescricaoBarraProgresso = descricao; });
            setIsIndefinidaBarraProgresso = new Progress<bool>(isindefinida => { IsIndefinidaBarraProgresso = isindefinida; });

            AbrirCadastrarComando = new RelayCommand(AbrirCadastrar);
            AbrirImprimirComando = new RelayCommand(AbrirImprimir);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox);
            AbrirEditarComando = new RelayCommand(AbrirEditar, Editavel);
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados);
            ApagarMarcadosComando = new RelayCommand(ApagarMarcados);
            ExportarExcelComando = new RelayCommand(ExportarExcel);
            AbrirAjudaComando = new RelayCommand(AbrirAjuda);
            CopiarValorCelulaComando = new RelayCommand(CopiarValorCelula);
            ExportarSQLComando = new RelayCommand(AbrirExportarSQL);
            CancelaTaskComando = new RelayCommand(CancelaTask);

            Entidades = new ObservableCollection<EntidadeComCampo<E>>();

            _session = SessionProvider.GetSession();

            PropertyChanged += PesquisarViewModel_PropertyChanged;
        }

        private void CancelaTask(object obj)
        {
            cancellationTokenSource.Cancel();
        }

        public abstract Task PesquisaItens(string termo);
        public void AbrirImprimir(object parameter)
        {
            //openView.ShowDialog(GetTelaRelatorioVM());
        }
        public abstract bool Editavel(object parameter);
        protected virtual void ChamaAposDeletarDoBD(AposDeletarDoBDEventArgs e)
        {
            AposDeletarDoBD?.Invoke(e);
        }
        public void AbrirCadastrar(object parameter)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("NHibernateSession", _session);
            keyValuePairs.Add("IssoEUmUpdate", false);
            ViewModelParameterHandler.Instance.AdicionaParametros(GetType(), keyValuePairs);

            //var v = GetCadastrarViewModel();

            //var result = openView.ShowDialog(v);
            ////Se houve cadastro
            //if (result.HasValue && result == true)
            //{
            //    OnPropertyChanged("TermoPesquisa");
            //}
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
                bool deletado = false;

                try
                {
                    await daoEntidade.Deletar(EntidadeSelecionada.Entidade);
                    deletado = true;
                    MessageBoxService.Show(pesquisarViewModelStrategy.MensagemEntidadeDeletada(EntidadeSelecionada.Entidade), pesquisarViewModelStrategy.TelaApagarCaption(),
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPropertyChanged("TermoPesquisa");
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"{pesquisarViewModelStrategy.MensagemEntidadeNaoDeletada()}\n\n{ex.Message}\n\n{ex.InnerException.Message}",
                        pesquisarViewModelStrategy.TelaApagarCaption(), MessageBoxButton.OK, MessageBoxImage.Error);
                }

                AposDeletarDoBDEventArgs e2 = new AposDeletarDoBDEventArgs()
                {
                    DeletadoComSucesso = deletado
                };

                ChamaAposDeletarDoBD(e2);
            }
        }
        public void AbrirEditar(object parameter)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("NHibernateSession", _session);
            keyValuePairs.Add("IssoEUmUpdate", true);
            keyValuePairs.Add("Entidade", EntidadeSelecionada.Entidade);
            ViewModelParameterHandler.Instance.AdicionaParametros(GetType(), keyValuePairs);

            //try
            //{
            //    EntidadeSelecionada.Entidade.InicializaLazyLoad();
            //}
            //catch (NotImplementedException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //openView.ShowDialog(GetEditarViewModel());
            //_session.Evict(EntidadeSelecionada.Entidade);
            //OnPropertyChanged("TermoPesquisa");
        }
        public void ChecarItensMarcados(object parameter)
        {
            int marcados = Entidades.Where(w => w.IsChecked).Count();

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
                var AApagar = Entidades.Where(w => w.IsChecked).Select(s => s.Entidade).ToList();

                try
                {
                    await daoEntidade.Deletar(AApagar);
                    MessageBoxService.Show(pesquisarViewModelStrategy.MensagemEntidadesDeletadas(), pesquisarViewModelStrategy.PesquisarEntidadeCaption());
                    VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
                    OnPropertyChanged("TermoPesquisa");
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"{pesquisarViewModelStrategy.MensagemEntidadesNaoDeletadas()}\n\n{ex.Message}\n\n{ex.InnerException.Message}",
                        pesquisarViewModelStrategy.PesquisarEntidadeCaption(),
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void AbrirAjuda(object parameter)
        {
            //TODO: abrir telas de ajuda
        }
        public void CopiarValorCelula(object parameter)
        {
            string valorCelula = (CelulaSelecionada.Column.GetCellContent(CelulaSelecionada.Item) as TextBlock).Text;
            Clipboard.SetDataObject(valorCelula);
        }
        public virtual async void ExportarExcel(object parameter)
        {
            try
            {
                var containers = GetWorksheetContainers();

                if (parameter == null)
                    throw new Exception($"Parâmetro de comando não configurado para ExportarExcel em {typeof(E).Name}.");

                if (containers == null)
                    throw new Exception($"Containers de planilha Excel não configurados para ExportarExcel em {typeof(E).Name}.");

                var saveFileDialog = parameter as ISaveFileDialog;
                string caminhoPlanilha = saveFileDialog.OpenSaveFileDialog("", "Planilha Excel (*.xlsx)|*.xlsx");

                if (caminhoPlanilha != null)
                {
                    IsThreadLocked = true;
                    VisibilidadeStatusBar = Visibility.Visible;
                    CancellationToken token = cancellationTokenSource.Token;
                    Task excelTask = new Excel<E>(excelStrategy, caminhoPlanilha).Salvar(token, setDescricaoBarraProgresso, setValorBarraProgresso,
                        setIsIndefinidaBarraProgresso, containers);

                    try
                    {
                        await excelTask;
                    }
                    catch (OperationCanceledException)
                    {
                        MessageBoxService.Show("Exportação para Excel foi cancelada pelo usuário");
                        cancellationTokenSource.Dispose();
                        cancellationTokenSource = new CancellationTokenSource();
                    }

                    await excelTask.ContinueWith(task =>
                    {
                        VisibilidadeStatusBar = Visibility.Collapsed;
                        IsThreadLocked = false;
                        if (!excelTask.IsCanceled)
                            MessageBoxService.Show("Exportado para Excel com sucesso");
                    });
                }
            }
            catch (OperationCanceledException)
            {
                MessageBoxService.Show("Exportação para Excel foi cancelada pela usuário");
                cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message);
            }
        }
        /// <summary>
        /// Retorna a configuração de cada aba da planilha em um container contendo nome e lista com dados
        /// para exportação para arquivo em Excel.
        /// </summary>
        protected abstract WorksheetContainer<E>[] GetWorksheetContainers();
        protected async void PesquisarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    try
                    {
                        await PesquisaItens(termoPesquisa);
                    }
                    catch (Exception ex)
                    {
                        MessageBoxService.Show($"Erro ao realizar pesquisa de itens. Acesse {Log.LogBanco} para mais detalhes.\n\n{ex.Message}");
                    }
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
        public Visibility VisibilidadeBotaoApagarSelecionado
        {
            get { return visibilidadeBotaoApagarSelecionado; }
            set
            {
                visibilidadeBotaoApagarSelecionado = value;
                OnPropertyChanged("VisibilidadeBotaoApagarSelecionado");
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

        public Visibility VisibilidadeStatusBar
        {
            get
            {
                return _visibilidadeStatusBar;
            }

            set
            {
                _visibilidadeStatusBar = value;
                OnPropertyChanged("VisibilidadeStatusBar");
            }
        }

        public double ValorBarraProgresso
        {
            get
            {
                return _valorBarraProgresso;
            }

            set
            {
                _valorBarraProgresso = value;
                OnPropertyChanged("ValorBarraProgresso");
            }
        }

        public string DescricaoBarraProgresso
        {
            get
            {
                return _descricaoBarraProgresso;
            }

            set
            {
                _descricaoBarraProgresso = value;
                OnPropertyChanged("DescricaoBarraProgresso");
            }
        }

        public bool IsIndefinidaBarraProgresso
        {
            get
            {
                return _isIndefinidaBarraProgresso;
            }

            set
            {
                _isIndefinidaBarraProgresso = value;
                OnPropertyChanged("IsIndefinidaBarraProgresso");
            }
        }
        public ISession Session
        {
            get => _session;
        }

        public void DisposeSession()
        {
            SessionProvider.FechaSession(_session);
        }
        bool IPesquisarVM.IsThreadLocked()
        {
            return _threadLocked;
        }
        public void AbrirExportarSQL(object parameter)
        {
            //openView.ShowDialog(GetExportaSQLVM());
        }
    }
}
