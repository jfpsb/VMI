using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using FolhaPagamentoModel = VandaModaIntimaWpf.Model.FolhaPagamento;
using ParcelaModel = VandaModaIntimaWpf.Model.Parcela;
using AdiantamentoModel = VandaModaIntimaWpf.Model.Adiantamento;
using Newtonsoft.Json;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarAdiantamentoViewModel : ACadastrarViewModel
    {
        DAOAdiantamento daoAdiantamento;
        DAOFolhaPagamento daoFolha;
        private FolhaPagamentoModel _folhaPagamento;
        private DateTime _inicioPagamento;
        private int _numParcelas;
        private double _valor;
        private ObservableCollection<ParcelaModel> _parcelas;
        private AdiantamentoModel _adiantamento;
        private int _minParcelas;

        public AdicionarAdiantamentoViewModel(ISession session, FolhaPagamentoModel folhaPagamento)
        {
            _session = session;
            daoAdiantamento = new DAOAdiantamento(_session);
            daoFolha = new DAOFolhaPagamento(_session);
            PropertyChanged += CadastrarViewModel_PropertyChanged;
            Parcelas = new ObservableCollection<ParcelaModel>();

            var now = DateTime.Now;

            Adiantamento = new AdiantamentoModel()
            {
                Data = now,
                Id = now.Ticks,
                Funcionario = folhaPagamento.Funcionario,
                Valor = 0
            };

            InicioPagamento = new DateTime(folhaPagamento.Ano, folhaPagamento.Mes, 1);
            FolhaPagamento = folhaPagamento;

            AposCriarDocumento += RefreshFolhasPagamento;
        }

        public FolhaPagamentoModel FolhaPagamento
        {
            get => _folhaPagamento;
            set
            {
                _folhaPagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }

        public DateTime InicioPagamento
        {
            get => _inicioPagamento;
            set
            {
                _inicioPagamento = value;
                OnPropertyChanged("InicioPagamento");
            }
        }
        public int NumParcelas
        {
            get => _numParcelas; set
            {
                _numParcelas = value;
                OnPropertyChanged("NumParcelas");
            }
        }
        public double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public ObservableCollection<ParcelaModel> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public AdiantamentoModel Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
            }
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "NumParcelas":
                    Adiantamento.Parcelas.Clear();
                    Parcelas.Clear();

                    DateTime inicio = InicioPagamento;

                    for (int i = 0; i < NumParcelas; i++)
                    {
                        FolhaPagamentoModel folha = await daoFolha.ListarPorMesAnoFuncionario(FolhaPagamento.Funcionario, inicio.Month, inicio.Year);

                        if (folha == null)
                        {
                            folha = new FolhaPagamentoModel()
                            {
                                Funcionario = FolhaPagamento.Funcionario,
                                Mes = inicio.Month,
                                Ano = inicio.Year,
                                Id = string.Format("{0}{1}{2}", inicio.Month, inicio.Year, FolhaPagamento.Funcionario.Cpf)
                            };
                        }

                        ParcelaModel p = new ParcelaModel
                        {
                            Id = DateTime.Now.Ticks,
                            Numero = i + 1,
                            Paga = false,
                            Valor = Valor / NumParcelas,
                            FolhaPagamento = folha,
                            Adiantamento = Adiantamento
                        };

                        Adiantamento.Parcelas.Add(p);
                        Parcelas.Add(p);

                        inicio = inicio.AddMonths(1);
                    }
                    break;
                case "InicioPagamento":
                    OnPropertyChanged("NumParcelas");
                    break;
                case "Valor":
                    Adiantamento.Valor = Valor;

                    if (Valor < FolhaPagamento.Funcionario.Salario)
                    {
                        _minParcelas = 1;
                    }
                    else
                    {
                        _minParcelas = 2;
                        while ((Valor / _minParcelas) > FolhaPagamento.Funcionario.Salario)
                        {
                            _minParcelas++;
                        }
                    }

                    NumParcelas = _minParcelas;

                    break;
            }
        }

        public override void ResetaPropriedades()
        {
            DateTime now = DateTime.Now;

            Adiantamento = new AdiantamentoModel()
            {
                Data = now,
                Id = now.Ticks,
                Funcionario = FolhaPagamento.Funcionario,
                Valor = 0
            };

            Valor = NumParcelas = _minParcelas = 0;

            Parcelas.Clear();
        }

        public override async void Salvar(object parameter)
        {
            string adiantamentoJson = JsonConvert.SerializeObject(Adiantamento);
            var couchDbResponse = await couchDbClient.CreateDocument(Adiantamento.Id.ToString(), adiantamentoJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "Adiantamento Adicionado Com Sucesso",
                MensagemErro = "Erro ao Adicionar Adiantamento",
                ObjetoSalvo = Adiantamento
            };

            ChamaAposCriarDocumento(e);
        }
        private void RefreshFolhasPagamento(AposCriarDocumentoEventArgs e)
        {
            //TODO: botar strings em resources
            foreach (var p in Parcelas)
            {
                _session.Refresh(p.FolhaPagamento);
            }
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BotaoSalvarToolTip = "";

            //TODO: botar strings em resources
            if (NumParcelas < _minParcelas)
            {
                BotaoSalvarToolTip += "O NÚMERO DE PARCELAS É MENOR QUE O NÚMERO MÍNIMO\n";
            }

            if (Valor <= 0.0)
            {
                BotaoSalvarToolTip += "O VALOR DO ADIANTAMENTO NÃO PODE SER MENOR OU IGUAL A ZERO\n";
            }

            if (InicioPagamento.Month < FolhaPagamento.Mes)
            {
                BotaoSalvarToolTip += "O INÍCIO DO PAGAMENTO É ANTERIOR À DATA DA FOLHA DE PAGAMENTO\n";
            }

            if (NumParcelas >= _minParcelas && Valor > 0.0 && InicioPagamento.Month >= FolhaPagamento.Mes)
                return true;

            return false;
        }

        public override async void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoAdiantamento.Inserir(Adiantamento);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    MensagemSucesso = "Adiantamento Inserido com Sucesso",
                    MensagemErro = "Erro ao Inserir Adiantamento",
                    ObjetoSalvo = Adiantamento,
                    CouchDbResponse = e.CouchDbResponse
                };

                ChamaAposInserirNoBD(e2);
            }
        }
    }
}
