﻿using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using FolhaPagamentoModel = VandaModaIntimaWpf.Model.FolhaPagamento;
using ParcelaModel = VandaModaIntimaWpf.Model.Parcela;
using AdiantamentoModel = VandaModaIntimaWpf.Model.Adiantamento;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

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

            if (now.Day > 5)
            {
                InicioPagamento = now.AddMonths(1);
            }
            else
            {
                InicioPagamento = now;
            }

            FolhaPagamento = folhaPagamento;
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
                                Id = int.Parse(string.Format("{0}{1}", inicio.Month, inicio.Year))
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
            throw new NotImplementedException();
        }

        public override async void Salvar(object parameter)
        {
            _result = await daoAdiantamento.Inserir(Adiantamento);
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (NumParcelas >= _minParcelas && Valor > 0.0 && InicioPagamento.Month >= FolhaPagamento.Mes)
                return true;

            return false;
        }
    }
}