﻿using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private DateTime _dataEscolhida;
        private double _valorMaximoParcela;

        public AdicionarAdiantamentoVM(ISession session, DateTime dataEscolhida, Model.Funcionario funcionario, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarAdiantamentoVMStrategy();
            daoEntidade = new DAO<Model.Adiantamento>(session);
            PropertyChanged += AdicionarAdiantamento_PropertyChanged;
            Parcelas = new ObservableCollection<ParcelaModel>();

            _dataEscolhida = dataEscolhida;

            Entidade = new AdiantamentoModel()
            {
                Funcionario = funcionario,
                Valor = 0
            };

            AntesDeInserirNoBancoDeDados += AtribuiData;

            InicioPagamento = new DateTime(_dataEscolhida.Year, _dataEscolhida.Month, 1);
            ValorMaximoParcela = 625.85;
        }

        private void AtribuiData()
        {
            Entidade.Data = DateTime.Now;
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

        public double ValorMaximoParcela
        {
            get => _valorMaximoParcela;
            set
            {
                _valorMaximoParcela = value;
                OnPropertyChanged("ValorMaximoParcela");
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
                            Mes = inicio.Month,
                            Ano = inicio.Year
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
                    CalculaNumParcelas();
                    break;
                case "ValorMaximoParcela":
                    CalculaNumParcelas();
                    break;
            }
        }

        private void CalculaNumParcelas()
        {
            Entidade.Valor = Valor;

            if (Valor <= ValorMaximoParcela)
            {
                _minParcelas = 1;
            }
            else
            {
                _minParcelas = 2;
                while ((Valor / _minParcelas) > ValorMaximoParcela)
                {
                    _minParcelas++;
                }
            }

            NumParcelas = _minParcelas;
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades()
        {
            Entidade = new AdiantamentoModel()
            {
                Funcionario = Entidade.Funcionario,
                Valor = 0
            };

            Valor = NumParcelas = _minParcelas = 0;

            Parcelas.Clear();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            //TODO: botar strings em resources
            if (NumParcelas < _minParcelas)
            {
                BtnSalvarToolTip += "O NÚMERO DE PARCELAS É MENOR QUE O NÚMERO MÍNIMO\n";
                valido = false;
            }

            if (Valor <= 0.0)
            {
                BtnSalvarToolTip += "O VALOR DO ADIANTAMENTO NÃO PODE SER MENOR OU IGUAL A ZERO\n";
                valido = false;
            }

            return valido;
        }
    }
}
