using Newtonsoft.Json;
using NHibernate;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel<E> : ObservableObject, ICadastrarVM where E : ObservableObject, IModel
    {
        protected ISession _session;
        protected DAO<E> daoEntidade;
        protected ICadastrarVMStrategy viewModelStrategy;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        protected E _entidade;
        protected bool _result;

        private bool issoEUmUpdate;
        private string _btnSalvarToolTip;
        protected IMessageBoxService MessageBoxService;

        public delegate void AntesDeInserirNoBancoDeDadosEventHandler();
        public delegate void AposInserirNoBancoDeDadosEventHandler(AposInserirBDEventArgs e);

        public event AposInserirNoBancoDeDadosEventHandler AposInserirNoBancoDeDados;
        public event AntesDeInserirNoBancoDeDadosEventHandler AntesDeInserirNoBancoDeDados;
        public ICommand SalvarComando { get; set; }
        /// <summary>
        /// Construtor abstrato para ViewModel de telas de cadastro de entidade
        /// </summary>
        /// <param name="session">Session do Hibernate que será usada na tela de cadastro</param>
        /// <param name="messageBoxService">Serviço de MessageBox que será usado na tela de cadastro</param>
        /// <param name="issoEUmUpdate">Marca se esta ViewModel está sendo usada em uma tela de cadastro ou tela de edição de entidade</param>
        public ACadastrarViewModel(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate)
        {
            _session = session;
            this.issoEUmUpdate = issoEUmUpdate;
            MessageBoxService = messageBoxService;
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);

            AposInserirNoBancoDeDados += ResultadoInsercao;
            AposInserirNoBancoDeDados += RedefinirTela;

            issoEUmUpdate = false;
        }
        /// <summary>
        /// Método atrelado ao comando do botão Salvar
        /// </summary>
        /// <param name="parameter">Parâmetro da View</param>
        public virtual async void Salvar(object parameter)
        {
            try
            {
                AntesDeInserirNoBancoDeDados?.Invoke();
                var e = await ExecutarSalvar(parameter);
                AposInserirNoBancoDeDados?.Invoke(e);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, viewModelStrategy.MessageBoxCaption());
            }
        }
        /// <summary>
        /// Executa a operação de salvar entidade
        /// </summary>
        /// <returns>Parâmetros do evento após inserção, em caso de sucesso ou falha</returns>
        protected async virtual Task<AposInserirBDEventArgs> ExecutarSalvar(object parametro)
        {
            _result = await daoEntidade.InserirOuAtualizar(Entidade);

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IssoEUmUpdate = issoEUmUpdate,
                IdentificadorEntidade = Entidade.GetIdentifier(),
                MensagemSucesso = viewModelStrategy.MensagemEntidadeSalvaComSucesso(),
                MensagemErro = viewModelStrategy.MensagemEntidadeErroAoSalvar(),
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }

        public abstract void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Retorna as propriedades da entidade a seus valores iniciais
        /// </summary>
        public abstract void ResetaPropriedades();

        /// <summary>
        /// Realiza os testes para determinar se todos os requisitos necessários para permitir o cadastro foram atingidos
        /// </summary>
        /// <param name="parameter">Parâmetro do comando</param>
        /// <returns></returns>
        public abstract bool ValidacaoSalvar(object parameter);
        /// <summary>
        /// Checa o resultado da inserção da entidade e retorna ao usuário
        /// </summary>
        /// <param name="e">Parâmetros do evento</param>
        private async void ResultadoInsercao(AposInserirBDEventArgs e)
        {
            //Se foi inserido com sucesso
            if (e.Sucesso)
            {
                if (e.IdentificadorEntidade != null)
                    Entidade = (E)await daoEntidade.ListarPorId(e.IdentificadorEntidade);

                MessageBoxService.Show(e.MensagemSucesso, viewModelStrategy.MessageBoxCaption());
            }
            else
            {
                MessageBoxService.Show(e.MensagemErro, viewModelStrategy.MessageBoxCaption());
            }
        }
        /// <summary>
        /// Redefine os campos do formulário para os valores padrões
        /// </summary>
        /// <param name="e"></param>
        private void RedefinirTela(AposInserirBDEventArgs e)
        {
            if (e.Sucesso)
            {
                // Se a operação for um Update não há alteração no formulário
                if (!e.IssoEUmUpdate)
                    ResetaPropriedades();
            }
            else
            {
                MessageBoxService.Show(e.MensagemErro, viewModelStrategy.MessageBoxCaption());
            }
        }
        /// <summary>
        /// Método utilizado nas telas de ediçao para saber se houve edição.
        /// </summary>
        /// <returns>True se entidade tiver sido salva</returns>
        public bool ResultadoSalvar()
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
    }
}
