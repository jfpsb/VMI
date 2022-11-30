using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.VendaEmCartao
{
    public class CadastrarVendaEmCartaoVM : ACadastrarViewModel<Model.VendaEmCartao>
    {
        private ObservableCollection<Model.Loja> lojas;
        private ObservableCollection<Model.OperadoraCartao> operadoras;
        private ObservableCollection<Model.VendaEmCartao> _vendasEmCartao;
        private Model.Loja _loja;
        private Model.OperadoraCartao _operadora;
        private DAOLoja daoLoja;
        private DAO<Model.OperadoraCartao> daoOperadora;
        private ILerCSVVendaEmCartao lerCSVVendaEmCartao;

        public ICommand AbrirCSVComando { get; set; }

        public CadastrarVendaEmCartaoVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            daoEntidade = new DAO<Model.VendaEmCartao>(session);
            daoLoja = new DAOLoja(session);
            daoOperadora = new DAO<OperadoraCartao>(session);

            AbrirCSVComando = new RelayCommand(AbrirCSV);
            PropertyChanged += CadastrarVendaEmCartaoVM_PropertyChanged; //Manter antes das consultas de loja e operadoras

            var task1 = GetLojas();
            task1.Wait();

            var task2 = GetOperadoras();
            task2.Wait();

            VendasEmCartao = new ObservableCollection<Model.VendaEmCartao>();
        }

        private void CadastrarVendaEmCartaoVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Operadora":
                    switch (Operadora.Nome)
                    {
                        case "REDE":
                            lerCSVVendaEmCartao = new LerCSVRede();
                            break;
                        case "CREDISHOP":
                            lerCSVVendaEmCartao = new LerCSVCredishop();
                            break;
                        case "CAIXA PAGAMENTOS":
                            lerCSVVendaEmCartao = new LerCSVCaixaPagamentos();
                            break;
                        default:
                            _messageBoxService.Show("Leitura de arquivo .csv desta operadora está indisponível no momento.");
                            break;
                    }
                    break;
            }
        }

        private void AbrirCSV(object obj)
        {
            VendasEmCartao.Clear();
            IOpenFileDialog openFile = obj as IOpenFileDialog;
            if (openFile != null)
            {
                string caminho = openFile.OpenFileDialog("Arquivo CSV (*.csv)|*.csv");

                if (caminho != null)
                {
                    try
                    {
                        VendasEmCartao = new ObservableCollection<Model.VendaEmCartao>(lerCSVVendaEmCartao.GeraListaVendaEmCartao(caminho, Loja, Operadora));
                    }
                    catch (Exception)
                    {
                        _messageBoxService.Show("Erro ao ler arquivo csv.");
                    }
                }
            }
        }

        protected async override Task<AposInserirBDEventArgs> ExecutarSalvar(object parametro)
        {
            try
            {
                await daoEntidade.InserirOuAtualizar(VendasEmCartao);
                _messageBoxService.Show("Vendas em cartão cadastradas com sucesso.", "Cadastro de Vendas Em Cartão", MessageBoxButton.OK, MessageBoxImage.Information);
                _result = true;
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Erro ao cadastrar vendas em cartão. " +
                    $"Para mais detalhes acesse {Log.LogBanco}.\n\n{ex.Message}\n\n{ex.InnerException.Message}", "Cadastro de Vendas Em Cartão", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IssoEUmUpdate = false,
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }

        private async Task GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarSomenteLojas());
            Loja = Lojas[0];
        }

        private async Task GetOperadoras()
        {
            Operadoras = new ObservableCollection<Model.OperadoraCartao>(await daoOperadora.Listar());
            Operadora = Operadoras[0];
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {

        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (VendasEmCartao.Count == 0) return false;

            return true;
        }

        public ObservableCollection<Model.Loja> Lojas
        {
            get
            {
                return lojas;
            }

            set
            {
                lojas = value;
                OnPropertyChanged("Loja");
            }
        }

        public ObservableCollection<OperadoraCartao> Operadoras
        {
            get
            {
                return operadoras;
            }

            set
            {
                operadoras = value;
                OnPropertyChanged("Operadoras");
            }
        }

        public Model.Loja Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public OperadoraCartao Operadora
        {
            get
            {
                return _operadora;
            }

            set
            {
                _operadora = value;
                OnPropertyChanged("Operadora");
            }
        }

        public ObservableCollection<Model.VendaEmCartao> VendasEmCartao
        {
            get
            {
                return _vendasEmCartao;
            }

            set
            {
                _vendasEmCartao = value;
                OnPropertyChanged("VendasEmCartao");
            }
        }
    }
}
