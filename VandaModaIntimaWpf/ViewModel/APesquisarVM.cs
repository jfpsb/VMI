using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel<E> : ObservableObject, IPesquisarVM where E : AModel, IModel
    {
        protected ISession _session;
        protected AExcelStrategy excelStrategy;
        protected IPesquisarMsgVMStrategy<E> pesquisarViewModelStrategy;
        private string termoPesquisa;
        private bool _threadLocked;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        private DataGridCellInfo celulaSelecionada;
        private EntidadeComCampo<E> entidadeSelecionada;
        private ObservableCollection<EntidadeComCampo<E>> _entidades;
        protected ObservableCollection<EntidadeComCampo<E>> _entidadesOriginal;

        protected IMessageBoxService MessageBoxService;
        protected IAbrePelaTelaPesquisaService<E> AbrePelaTelaPesquisaService;

        protected DAO<E> daoEntidade;

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

            Entidades = new ObservableCollection<EntidadeComCampo<E>>();

            _session = SessionProvider.GetSession();

            PropertyChanged += PesquisarViewModel_PropertyChanged;
        }

        public abstract Task PesquisaItens(string termo);
        public void AbrirImprimir(object parameter)
        {
            AbrePelaTelaPesquisaService.AbrirImprimir(Entidades.Select(s => s.Entidade).ToList());
        }
        public abstract bool Editavel(object parameter);
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
            AbrePelaTelaPesquisaService.AbrirAjuda();
        }
        public async void ImportarExcel(object parameter)
        {
            var OpenFileDialog = (IOpenFileDialog)parameter;

            string path = OpenFileDialog.OpenFileDialog();

            if (path != null)
            {
                IsThreadLocked = true;
                try
                {
                    await new Excel<E>(excelStrategy, path).Importar();
                    OnPropertyChanged("TermoPesquisa");
                }
                catch (Exception ex)
                {
                    Log.EscreveLogExcel(ex, "importar de excel");
                    MessageBoxService.Show($"Erro ao importar dados de planilha Excel.\n\n{ex.InnerException.Message}", "Importar De Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                IsThreadLocked = false;
            }
        }
        public void CopiarValorCelula(object parameter)
        {
            string valorCelula = (CelulaSelecionada.Column.GetCellContent(CelulaSelecionada.Item) as TextBlock).Text;
            Clipboard.SetDataObject(valorCelula);
        }
        public virtual async void ExportarExcel(object parameter)
        {
            var containers = GetWorksheetContainers();
            if (containers != null)
            {
                MessageBoxService.Show(GetResource.GetString("arquivo_excel_sendo_gerado"), pesquisarViewModelStrategy.PesquisarEntidadeCaption());
                IsThreadLocked = true;
                await new Excel<E>(excelStrategy).Salvar(containers);
                IsThreadLocked = false;
                MessageBoxService.Show(GetResource.GetString("exportacao_excel_realizada_com_sucesso"), pesquisarViewModelStrategy.PesquisarEntidadeCaption());
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
            AbrePelaTelaPesquisaService.AbrirExportarSQL(Entidades.Select(s => s.Entidade).ToList(), _session);
        }
    }
}
