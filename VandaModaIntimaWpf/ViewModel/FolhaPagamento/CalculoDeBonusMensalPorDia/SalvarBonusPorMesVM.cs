﻿using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class SalvarBonusPorMesVM : ObservableObject
    {
        private ISession _session;
        private DAOFuncionario daoFuncionario;
        private DAOBonus daoBonus;
        private IMessageBoxService MessageBoxService;
        private double _valorTotal;
        private double _valorDiario;
        private int _numDias;
        private DateTime _primeiroDia;
        private DateTime _ultimoDia;
        private ISalvarBonus _salvarBonus;
        private DateTime _dataEscolhida;
        private string _recebeRegularmenteHeader;
        private ObservableCollection<EntidadeComCampo<Model.Funcionario>> _entidades;

        public ICommand AdicionarBonusComando { get; set; }

        public SalvarBonusPorMesVM(DateTime dataEscolhida, double valorTotal, double valorDiario, int numDias, DateTime primeiroDia, DateTime ultimoDia, IMessageBoxService messageBoxService, ISalvarBonus salvarBonus)
        {
            _session = SessionProvider.GetSession();
            _numDias = numDias;
            _primeiroDia = primeiroDia;
            _ultimoDia = ultimoDia;
            _salvarBonus = salvarBonus;
            _valorDiario = valorDiario;
            RecebeRegularmenteHeader = _salvarBonus.RecebeRegularmenteHeader();
            ValorTotal = valorTotal;
            DataEscolhida = dataEscolhida;
            MessageBoxService = messageBoxService;
            daoFuncionario = new DAOFuncionario(_session);
            daoBonus = new DAOBonus(_session);

            AdicionarBonusComando = new RelayCommand(AdicionarBonus);

            GetFuncionarios();
        }

        private async void AdicionarBonus(object obj)
        {
            var marcados = Entidades.Where(w => w.IsChecked).Select(s => s.Entidade).ToList();

            if (marcados.Count == 0)
            {
                MessageBoxService.Show("Nenhum Funcionário Foi Selecionado!");
            }
            else
            {
                // O mês sendo calculado é adicionado na folha referente ao mês anterior
                // para ser pago no vencimento desta folha
                DateTime dataFolha = DataEscolhida.AddMonths(-1);
                IList<Model.Bonus> bonuses = new List<Bonus>();
                foreach (var funcionario in marcados)
                {
                    Model.Bonus bonus = new Model.Bonus();
                    var now = DateTime.Now;

                    bonus.Funcionario = funcionario;
                    bonus.Data = now;
                    bonus.Descricao = _salvarBonus.DescricaoBonus(_numDias, _valorDiario, _primeiroDia, _ultimoDia);
                    bonus.Valor = ValorTotal;
                    bonus.MesReferencia = dataFolha.Month;
                    bonus.AnoReferencia = dataFolha.Year;

                    bonuses.Add(bonus);
                }

                var result = await daoBonus.Inserir(bonuses);

                if (result)
                {
                    MessageBoxService.Show(_salvarBonus.MensagemInseridoSucesso(), _salvarBonus.MensagemCaption());
                }
                else
                {
                    MessageBoxService.Show(_salvarBonus.MensagemInseridoErro(), _salvarBonus.MensagemCaption());
                }
            }
        }

        public ObservableCollection<EntidadeComCampo<Model.Funcionario>> Entidades
        {
            get { return _entidades; }
            set
            {
                _entidades = value;
                OnPropertyChanged("Entidades");
            }
        }

        public double ValorTotal
        {
            get => _valorTotal;
            set
            {
                _valorTotal = value;
                OnPropertyChanged("ValorTotal");
            }
        }
        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }

        public string RecebeRegularmenteHeader
        {
            get => _recebeRegularmenteHeader;
            set
            {
                _recebeRegularmenteHeader = value;
                OnPropertyChanged("RecebeRegularmenteHeader");
            }
        }

        private async void GetFuncionarios()
        {
            Entidades = new ObservableCollection<EntidadeComCampo<Model.Funcionario>>(EntidadeComCampo<Model.Funcionario>.CriarListaEntidadeComCampo(await daoFuncionario.Listar()));
        }

        public void DisposeSession()
        {
            SessionProvider.FechaSession(_session);
        }
    }
}
