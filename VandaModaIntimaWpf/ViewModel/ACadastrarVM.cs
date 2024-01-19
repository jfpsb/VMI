using NHibernate;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel<E> : ObservableObject, ICadastrarVM, IDialogResult where E : AModel, IModel
    {
        protected ISession _session;
        protected DAO<E> daoEntidade;
        protected ICadastrarVMStrategy viewModelStrategy;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        protected E _entidade;
        protected bool _result;
        protected IMessageBoxService _messageBoxService;
        protected IWindowService _windowService;

        private bool antesInserirBDChecagem;
        private bool issoEUmUpdate;
        private string _btnSalvarToolTip;

        public delegate void AntesDeInserirNoBancoDeDadosEventHandler();
        public delegate void AposInserirNoBancoDeDadosEventHandler(AposCRUDEventArgs e);
        public event AposInserirNoBancoDeDadosEventHandler AposSalvarEvento;
        public event AntesDeInserirNoBancoDeDadosEventHandler AntesDeSalvarEvento;
        public ICommand SalvarComando { get; set; }
        /// <summary>
        /// Construtor abstrato para ViewModel de telas de cadastro de entidade
        /// </summary>
        /// <param name="session">Session do Hibernate que será usada na tela de cadastro</param>
        /// <param name="issoEUmUpdate">Marca se esta ViewModel está sendo usada em uma tela de cadastro ou tela de edição de entidade</param>
        public ACadastrarViewModel(ISession session, bool isUpdate = false)
        {
            _session = session;
            IssoEUmUpdate = isUpdate;
            _windowService = new WindowService();
            _messageBoxService = new MessageBoxService();
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);

            AposSalvarEvento += RedefinirTela;
            AposSalvarEvento += RefreshEntidade;
            AntesDeSalvarEvento += ResetaChecagem;

            AntesInserirBDChecagem = true;
        }

        private void ResetaChecagem()
        {
            AntesInserirBDChecagem = true;
        }

        /// <summary>
        /// Lê novamente o estado da Entidade do banco de dados para atualizar os valores criados através de triggers
        /// </summary>
        /// <param name="e">Argumento Do Evento Após Inserir Em Banco De Dados</param>
        protected virtual async void RefreshEntidade(AposCRUDEventArgs e)
        {
            if (e.Sucesso && e.UuidEntidade != null)
            {
                try
                {
                    await daoEntidade.RefreshEntidade(await daoEntidade.ListarPorUuid((Guid)e.UuidEntidade));
                }
                catch (Exception ex)
                {
                    _messageBoxService.Show($"Erro ao dar refresh em entidade. Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}\n\n" +
                        $"{ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Método atrelado ao comando do botão Salvar
        /// </summary>
        /// <param name="parameter">Parâmetro da View</param>
        public virtual async void Salvar(object parameter)
        {
            try
            {
                AntesDeSalvarEvento?.Invoke();
                if (AntesInserirBDChecagem)
                {
                    var e = await ExecutarSalvar(parameter);
                    AposSalvarEvento?.Invoke(e);
                }
            }
            catch (Exception e)
            {
                _messageBoxService.Show(e.Message, viewModelStrategy.MessageBoxCaption());
            }
        }
        /// <summary>
        /// Executa a operação de salvar entidade
        /// </summary>
        /// <returns>Parâmetros do evento após inserção, em caso de sucesso ou falha</returns>
        protected async virtual Task<AposCRUDEventArgs> ExecutarSalvar(object parametro)
        {
            _result = false;
            try
            {
                await daoEntidade.InserirOuAtualizar(Entidade);
                _result = true;
                _messageBoxService.Show(viewModelStrategy.MensagemEntidadeSalvaComSucesso(), viewModelStrategy.MessageBoxCaption(),
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _messageBoxService.Show($"{viewModelStrategy.MensagemEntidadeErroAoSalvar()}\n\n{ex.Message}\n\n{ex.InnerException?.Message}", viewModelStrategy.MessageBoxCaption(),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AposCRUDEventArgs e = new AposCRUDEventArgs()
            {
                IssoEhUpdate = IssoEUmUpdate,
                UuidEntidade = Entidade.Uuid,
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }

        /// <summary>
        /// Executa Quando Uma Propriedade da Entidade É Alterada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Retorna as propriedades da entidade a seus valores iniciais
        /// </summary>
        public abstract void ResetaPropriedades(AposCRUDEventArgs e);

        /// <summary>
        /// Realiza os testes para determinar se todos os requisitos necessários para permitir o cadastro foram atingidos
        /// </summary>
        /// <param name="parameter">Parâmetro do comando</param>
        /// <returns></returns>
        public abstract bool ValidacaoSalvar(object parameter);

        /// <summary>
        /// Redefine os campos do formulário para os valores padrões
        /// </summary>
        /// <param name="e"></param>
        private void RedefinirTela(AposCRUDEventArgs e)
        {
            if (e.Sucesso)
            {
                // Se a operação for um Update não há alteração no formulário
                if (!e.IssoEhUpdate)
                    ResetaPropriedades(e);
            }
        }

        /// <summary>
        /// Método utilizado nas telas de ediçao para saber se houve edição.
        /// </summary>
        /// <returns>True se entidade tiver sido salva</returns>
        public bool? ResultadoDialog()
        {
            return _result;
        }

        public Visibility VisibilidadeAvisoItemJaExiste
        {
            get { return visibilidadeAvisoItemJaExiste; }
            set
            {
                visibilidadeAvisoItemJaExiste = value;
                OnPropertyChanged("VisibilidadeAvisoItemJaExiste");
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        public E Entidade
        {
            get
            {
                return _entidade;
            }

            set
            {
                _entidade = value;
                _entidade.PropertyChanged += Entidade_PropertyChanged;
                OnPropertyChanged("Entidade");
            }
        }

        public string BtnSalvarToolTip
        {
            get => _btnSalvarToolTip;
            set
            {
                _btnSalvarToolTip = value;
                OnPropertyChanged("BtnSalvarToolTip");
            }
        }

        public bool IssoEUmUpdate
        {
            get => issoEUmUpdate;
            set
            {
                issoEUmUpdate = value;
                OnPropertyChanged("IssoEUmUpdate");
            }
        }

        protected bool AntesInserirBDChecagem { get => antesInserirBDChecagem; set => antesInserirBDChecagem = value; }
    }
}
