using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class GerenciarParcelasVM : ObservableObject, IDialogResult
    {
        private ISession _session;
        private Adiantamento _adiantamento;
        private Parcela _parcela;
        private BindingList<Parcela> _parcelas;
        private bool _adiantarAtrasarParcelasConjunto;
        private IMessageBoxService messageBoxService;
        private bool? resultadoSalvar;
        private bool isParcelasDirty;
        private DAOParcela daoParcela;

        public ICommand AtrasarParcelaComando { get; set; }
        public ICommand AdiantarParcelaComando { get; set; }
        public ICommand SalvarParcelasComando { get; set; }
        public ICommand AoFecharTelaComando { get; set; }

        public GerenciarParcelasVM(ISession session, Adiantamento adiantamento, IMessageBoxService messageBoxService)
        {
            _session = session;
            daoParcela = new DAOParcela(session);
            Adiantamento = adiantamento;
            this.messageBoxService = messageBoxService;

            Parcelas = new BindingList<Parcela>(Adiantamento.Parcelas);
            Parcelas.ListChanged += Parcelas_ListChanged;

            AtrasarParcelaComando = new RelayCommand(AtrasarParcela);
            AdiantarParcelaComando = new RelayCommand(AdiantarParcela);
            SalvarParcelasComando = new RelayCommand(SalvarParcelas, ValidacaoSalvar);
            AoFecharTelaComando = new RelayCommand(AoFecharTela);
        }
        private void Parcelas_ListChanged(object sender, ListChangedEventArgs e)
        {
            isParcelasDirty = true;
        }
        private bool ValidacaoSalvar(object arg)
        {
            return isParcelasDirty;
        }
        private void AoFecharTela(object obj)
        {
            if (isParcelasDirty && (resultadoSalvar == null || resultadoSalvar == false))
            {
                var result = messageBoxService.Show("Houve Mudanças Em Parcelas Que Não Foram Salvas. Deseja Salvar?", "Gerenciar Parcelas", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SalvarParcelas(null);
                }
            }
        }
        private async void SalvarParcelas(object obj)
        {
            resultadoSalvar = await daoParcela.InserirOuAtualizar(Parcelas);

            if (resultadoSalvar == true)
            {
                messageBoxService.Show("Parcelas Foram Salvas Com Sucesso!", "Gerenciar Parcelas");
            }
            else
            {
                messageBoxService.Show("Erro Ao Salvar Parcelas!", "Gerenciar Parcelas");
            }
        }

        private void AdiantarParcela(object obj)
        {
            if (Parcela != null)
            {
                if (AdiantarAtrasarParcelasConjunto)
                {
                    var parcelaIndex = Parcelas.IndexOf(Parcela);

                    for (int i = parcelaIndex; i < Parcelas.Count; i++)
                    {
                        DateTime dataParcela = new DateTime(Parcelas[i].Ano, Parcelas[i].Mes, 1);
                        dataParcela = dataParcela.AddMonths(-1);
                        Parcelas[i].Ano = dataParcela.Year;
                        Parcelas[i].Mes = dataParcela.Month;
                    }
                }
                else
                {
                    DateTime dataParcela = new DateTime(Parcela.Ano, Parcela.Mes, 1);
                    dataParcela = dataParcela.AddMonths(-1);
                    Parcela.Ano = dataParcela.Year;
                    Parcela.Mes = dataParcela.Month;
                }
            }
        }

        private void AtrasarParcela(object obj)
        {
            if (Parcela != null)
            {
                if (AdiantarAtrasarParcelasConjunto)
                {
                    var parcelaIndex = Parcelas.IndexOf(Parcela);

                    for (int i = parcelaIndex; i < Parcelas.Count; i++)
                    {
                        DateTime dataParcela = new DateTime(Parcelas[i].Ano, Parcelas[i].Mes, 1);
                        dataParcela = dataParcela.AddMonths(1);
                        Parcelas[i].Ano = dataParcela.Year;
                        Parcelas[i].Mes = dataParcela.Month;
                    }
                }
                else
                {
                    DateTime dataParcela = new DateTime(Parcela.Ano, Parcela.Mes, 1);
                    dataParcela = dataParcela.AddMonths(1);
                    Parcela.Ano = dataParcela.Year;
                    Parcela.Mes = dataParcela.Month;
                }
            }
        }

        public bool? ResultadoDialog()
        {
            return resultadoSalvar;
        }

        public Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
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
        public BindingList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        public bool AdiantarAtrasarParcelasConjunto
        {
            get => _adiantarAtrasarParcelasConjunto;
            set
            {
                _adiantarAtrasarParcelasConjunto = value;
                OnPropertyChanged("AdiantarAtrasarParcelasConjunto");
            }
        }
    }
}
