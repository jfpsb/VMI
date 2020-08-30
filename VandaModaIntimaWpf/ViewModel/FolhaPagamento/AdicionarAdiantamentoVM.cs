using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using FolhaPagamentoModel = VandaModaIntimaWpf.Model.FolhaPagamento;
using ParcelaModel = VandaModaIntimaWpf.Model.Parcela;
using AdiantamentoModel = VandaModaIntimaWpf.Model.Adiantamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarAdiantamentoVM : ACadastrarViewModel<FolhaPagamentoModel>
    {
        private DateTime _inicioPagamento;
        private int _numParcelas;
        private double _valor;
        private ObservableCollection<ParcelaModel> _parcelas;
        private AdiantamentoModel _adiantamento;
        private int _minParcelas;

        public AdicionarAdiantamentoVM(ISession session, FolhaPagamentoModel folhaPagamento, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            cadastrarViewModelStrategy = new CadastrarAdiantMsgVMStrategy();
            daoEntidade = new DAOFolhaPagamento(session);
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
            Entidade = folhaPagamento;

            AposInserirBD += RefreshFolhasPagamento;
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
            DAOFolhaPagamento daoFolha = (DAOFolhaPagamento)daoEntidade;
            switch (e.PropertyName)
            {
                case "NumParcelas":
                    Adiantamento.Parcelas.Clear();
                    Parcelas.Clear();

                    DateTime inicio = InicioPagamento;

                    for (int i = 0; i < NumParcelas; i++)
                    {
                        FolhaPagamentoModel folha = await daoFolha.ListarPorMesAnoFuncionario(Entidade.Funcionario, inicio.Month, inicio.Year);

                        if (folha == null)
                        {
                            folha = new FolhaPagamentoModel()
                            {
                                Funcionario = Entidade.Funcionario,
                                Mes = inicio.Month,
                                Ano = inicio.Year,
                                Id = string.Format("{0}{1}{2}", inicio.Month, inicio.Year, Entidade.Funcionario.Cpf)
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

                    if (Valor < Entidade.Funcionario.Salario)
                    {
                        _minParcelas = 1;
                    }
                    else
                    {
                        _minParcelas = 2;
                        while ((Valor / _minParcelas) > Entidade.Funcionario.Salario)
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
                Funcionario = Entidade.Funcionario,
                Valor = 0
            };

            Valor = NumParcelas = _minParcelas = 0;

            Parcelas.Clear();
        }
        private void RefreshFolhasPagamento(AposInserirBDEventArgs e)
        {
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

            if (InicioPagamento.Month < Entidade.Mes)
            {
                BotaoSalvarToolTip += "O INÍCIO DO PAGAMENTO É ANTERIOR À DATA DA FOLHA DE PAGAMENTO\n";
            }

            if (NumParcelas >= _minParcelas && Valor > 0.0 && InicioPagamento.Month >= Entidade.Mes)
                return true;

            return false;
        }

        protected override void ExecutarAntesCriarDocumento()
        {
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
