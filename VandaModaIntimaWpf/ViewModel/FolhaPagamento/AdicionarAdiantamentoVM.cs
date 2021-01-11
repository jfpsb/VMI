using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using ParcelaModel = VandaModaIntimaWpf.Model.Parcela;
using AdiantamentoModel = VandaModaIntimaWpf.Model.Adiantamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarAdiantamentoVM : ACadastrarViewModel<AdiantamentoModel>
    {
        private DateTime _inicioPagamento;
        private int _numParcelas;
        private double _valor;
        private ObservableCollection<ParcelaModel> _parcelas;
        private int _minParcelas;
        private DateTime _dataAtual;

        public AdicionarAdiantamentoVM(ISession session, Model.Funcionario funcionario, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarAdiantamentoVMStrategy();
            daoEntidade = new DAOAdiantamento(session);
            PropertyChanged += AdicionarAdiantamento_PropertyChanged;
            Parcelas = new ObservableCollection<ParcelaModel>();

            _dataAtual = DateTime.Now;

            Entidade = new AdiantamentoModel()
            {
                Data = _dataAtual,
                Funcionario = funcionario,
                Valor = 0
            };

            InicioPagamento = new DateTime(_dataAtual.Year, _dataAtual.Month, 1);
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

        public void AdicionarAdiantamento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "NumParcelas":
                    Entidade.Parcelas.Clear();
                    Parcelas.Clear();

                    DateTime inicio = InicioPagamento;

                    for (int i = 0; i < NumParcelas; i++)
                    {
                        ParcelaModel p = new ParcelaModel
                        {
                            Numero = i + 1,
                            Paga = false,
                            Valor = Valor / NumParcelas,
                            Adiantamento = Entidade,
                            MesAPagar = inicio.Month,
                            AnoAPagar = inicio.Year
                        };

                        Entidade.Parcelas.Add(p);
                        Parcelas.Add(p);

                        inicio = inicio.AddMonths(1);
                    }
                    break;
                case "InicioPagamento":
                    OnPropertyChanged("NumParcelas");
                    break;
                case "Valor":
                    Entidade.Valor = Valor;

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
            _dataAtual = DateTime.Now;

            Entidade = new AdiantamentoModel()
            {
                Data = _dataAtual,
                Id = _dataAtual.Ticks,
                Funcionario = Entidade.Funcionario,
                Valor = 0
            };

            Valor = NumParcelas = _minParcelas = 0;

            Parcelas.Clear();
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

            if (NumParcelas >= _minParcelas && Valor > 0.0)
                return true;

            return false;
        }
    }
}
