﻿using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class MaisDetalhesVM : ObservableObject, IDialogResult
    {
        private ISession _session;
        private ObservableCollection<Parcela> _parcelas;
        private ObservableCollection<Bonus> _bonus;
        private DAOFolhaPagamento daoFolha;
        private DAOBonus daoBonus;
        private DAOParcela daoParcela;
        private DAO<Model.Adiantamento> daoAdiantamento;
        private Parcela _parcela;
        private Bonus _bonusEscolhido;
        private FolhaModel _folhaPagamento;
        private bool? _dialogResult = false;
        private IMessageBoxService MessageBoxService;

        public ICommand DeletarAdiantamentoComando { get; set; }
        public ICommand DeletarBonusComando { get; set; }
        public ICommand GerenciarParcelasComando { get; set; }
        public MaisDetalhesVM(ISession session, FolhaModel folhaPagamento, IMessageBoxService messageBoxService)
        {
            _session = session;
            MessageBoxService = messageBoxService;

            FolhaPagamento = folhaPagamento;
            daoBonus = new DAOBonus(session);
            daoFolha = new DAOFolhaPagamento(session);
            daoParcela = new DAOParcela(session);
            daoAdiantamento = new DAO<Adiantamento>(session);
            DeletarAdiantamentoComando = new RelayCommand(DeletarAdiantamento);
            DeletarBonusComando = new RelayCommand(DeletarBonus);
            GerenciarParcelasComando = new RelayCommand(GerenciarParcelas);

            Parcelas = new ObservableCollection<Parcela>(FolhaPagamento.Parcelas);
            Bonus = new ObservableCollection<Bonus>(FolhaPagamento.Bonus);
        }

        private async void GerenciarParcelas(object obj)
        {
            GerenciarParcelasVM vm = new GerenciarParcelasVM(_session, Parcela.Adiantamento, MessageBoxService);
            GerenciarParcelas view = new GerenciarParcelas
            {
                DataContext = vm
            };

            var result = view.ShowDialog();

            if (!_dialogResult.HasValue || _dialogResult == false)
                _dialogResult = result;

            if (result == true)
            {
                var parc = await daoParcela.ListarPorFuncionarioMesAno(FolhaPagamento.Funcionario, FolhaPagamento.Mes, FolhaPagamento.Ano);
                Parcelas = new ObservableCollection<Parcela>(parc);
            }

            OnPropertyChanged("TotalParcelas");
        }

        private async void DeletarBonus(object obj)
        {
            //TODO: Colocar strings em resources
            MessageBoxResult telaApagar = MessageBoxService.Show(string.Format("Tem Certeza Que Deseja Apagar o Bônus Criado Em {0}?", BonusEscolhido.Data.ToString("dd/MM/yyyy")),
                "Deletar Bônus",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
                bool result;

                if (BonusEscolhido.BonusMensal)
                {
                    BonusEscolhido.BonusCancelado = true;
                    result = await daoBonus.InserirOuAtualizar(BonusEscolhido);
                }
                else
                {
                    BonusEscolhido.Deletado = true;
                    result = await daoBonus.Atualizar(BonusEscolhido);
                }

                if (result)
                {
                    MessageBoxService.Show("Bônus Deletado Com Sucesso");
                    Bonus.Remove(BonusEscolhido);
                    OnPropertyChanged("TotalBonus");
                }
            }
        }

        private async void DeletarAdiantamento(object obj)
        {
            //TODO: Colocar strings em resources
            MessageBoxResult telaApagar = MessageBoxService.Show($"Tem Certeza Que Deseja Apagar o Adiantamento Criado Em {Parcela.Adiantamento.DataString}?\nTodas as Parcelas Serão Deletadas!",
                "Deletar Adiantamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (telaApagar.Equals(MessageBoxResult.Yes))
            {
                //FolhaPagamento.Funcionario.Adiantamentos.Remove(Parcela.Adiantamento);
                Parcela.Adiantamento.Deletado = true;
                bool resultadoDelete = await daoAdiantamento.Atualizar(Parcela.Adiantamento);

                if (resultadoDelete)
                {
                    MessageBoxService.Show("Adiantamento Deletado Com Sucesso!");
                    Parcelas.Remove(Parcela);
                    OnPropertyChanged("TotalParcelas");
                }
                else
                {
                    await _session.RefreshAsync(FolhaPagamento);
                }
            }
        }

        public bool? ResultadoDialog()
        {
            return _dialogResult;
        }

        public ObservableCollection<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public Parcela Parcela
        {
            get => _parcela;
            set
            {
                _parcela = value;
                OnPropertyChanged("Parcela");
            }
        }

        public ObservableCollection<Bonus> Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }

        public double TotalBonus
        {
            get => Bonus.Sum(s => s.Valor);
        }

        public double TotalParcelas
        {
            get => Parcelas.Sum(s => s.Valor);
        }

        public Bonus BonusEscolhido
        {
            get => _bonusEscolhido;
            set
            {
                _bonusEscolhido = value;
                OnPropertyChanged("BonusEscolhido");
            }
        }

        public FolhaModel FolhaPagamento
        {
            get => _folhaPagamento;
            set
            {
                _folhaPagamento = value;
                OnPropertyChanged("FolhaPagamento");
            }
        }
    }
}
