using NHibernate;
using System;
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
        public ICommand CheckBoxPagaComando { get; set; }

        public GerenciarParcelasVM(ISession session, Adiantamento adiantamento, IMessageBoxService messageBoxService)
        {
            daoParcela = new DAOParcela(session);
            Adiantamento = adiantamento;
            this.messageBoxService = messageBoxService;

            Parcelas = new BindingList<Parcela>(Adiantamento.Parcelas);
            Parcelas.ListChanged += Parcelas_ListChanged;

            AtrasarParcelaComando = new RelayCommand(AtrasarParcela);
            AdiantarParcelaComando = new RelayCommand(AdiantarParcela);
            SalvarParcelasComando = new RelayCommand(SalvarParcelas);
            AoFecharTelaComando = new RelayCommand(AoFecharTela);
            CheckBoxPagaComando = new RelayCommand(CheckBoxPaga);
        }

        private void Parcelas_ListChanged(object sender, ListChangedEventArgs e)
        {
            isParcelasDirty = true;
        }

        private void CheckBoxPaga(object obj)
        {
            isParcelasDirty = true;
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
            var result = messageBoxService.Show("Tem Certeza Que Deseja Salvar As Alterações? Parcelas Marcadas Como Pagas Não Podem Ser Desmarcadas Posteriormente!", "Gerenciar Parcelas", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var parcelas = Parcelas.Where(w => !w.Paga).ToList();
                parcelas.ForEach(f =>
                {
                    if (!f.Paga && f.StatusPagaAtual)
                    {
                        f.Paga = true;
                    }
                });


                try
                {
                    await daoParcela.InserirOuAtualizar(parcelas);
                    resultadoSalvar = true;
                    messageBoxService.Show("Parcelas Foram Salvas Com Sucesso!", "Gerenciar Parcelas", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    messageBoxService.Show($"Erro Ao Salvar Parcelas!\n\n{ex.Message}\n\n{ex.InnerException.Message}", "Gerenciar Parcelas", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
