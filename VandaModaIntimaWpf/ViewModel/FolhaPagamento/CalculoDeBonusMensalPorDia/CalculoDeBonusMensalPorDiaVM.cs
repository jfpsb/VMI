using Newtonsoft.Json;
using NHibernate;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDeBonusMensalPorDiaVM : ObservableObject
    {
        private BindingList<DataWidget> _widgetsMes;
        private DataFeriado[] datasFeriados;
        private DateTime _dataEscolhida;
        private double _valorTotal;
        private double _valorDiario;
        private IMessageBoxService messageBoxService;
        private string _menuItemHeader1;
        private string _windowCaption;
        private ICalculoDeBonus calculoDeBonus;
        private ISession _session;

        // Comando para adicionar valor do bônus para os funcionários
        public ICommand AbrirAdicionarBonusComando { get; set; }

        public CalculoDeBonusMensalPorDiaVM(ISession session, DateTime dataEscolhida, ICalculoDeBonus calculoDeBonus)
        {
            _session = session;
            this.calculoDeBonus = calculoDeBonus;
            MenuItemHeader1 = calculoDeBonus.MenuItemHeader1();
            WindowCaption = calculoDeBonus.WindowCaption();
            ValorDiario = calculoDeBonus.ValorDiario();
            messageBoxService = new MessageBoxService();

            PropertyChanged += CalculaDatas;
            PropertyChanged += ValorDiarioAlterado;

            WidgetsMes = new BindingList<DataWidget>();
            WidgetsMes.ListChanged += CalcultaTotal;

            DataEscolhida = dataEscolhida;

            AbrirAdicionarBonusComando = new RelayCommand(AbrirAdicionarBonus);
        }

        private void ValorDiarioAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ValorDiario"))
            {
                var result = messageBoxService.Show("O valor diário foi alterado. Deseja confirmar a alteração e salvar o novo valor?", calculoDeBonus.WindowCaption(), System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    calculoDeBonus.AtribuirNovoValorDiario(ValorDiario);
                    var json = JsonConvert.SerializeObject(Config.Instancia);
                    File.WriteAllText("Config.json", json);
                    CalcultaTotal(null, null);
                }
                else
                {
                    ValorDiario = calculoDeBonus.ValorDiario();
                }
            }
        }

        private void AbrirAdicionarBonus(object obj)
        {
            DateTime primeiroDia = new DateTime(), ultimoDia = new DateTime();

            if (WidgetsMes.Where(w => w.IsDiaUtil).Count() == 0)
            {
                messageBoxService.Show("Não há dias marcados como dias úteis no calendário!");
                return;
            }
            else
            {
                primeiroDia = WidgetsMes.Where(w => w.IsDiaUtil).First().Date;
                ultimoDia = WidgetsMes.Where(w => w.IsDiaUtil).Last().Date;
            }

            int numDias = WidgetsMes.Where(w => w.IsDiaUtil).Count();
            calculoDeBonus.AbrirAdicionarBonus(_session, false, DataEscolhida, ValorTotal, ValorDiario, numDias, primeiroDia, ultimoDia);
        }

        private void CalcultaTotal(object sender, ListChangedEventArgs e)
        {
            int quantDiasUteis = WidgetsMes.Where(w => w.IsDiaUtil).Count();
            ValorTotal = quantDiasUteis * ValorDiario;
        }

        private void CalculaDatas(object sender, PropertyChangedEventArgs e)
        {
            //TODO: Mostrar mensagem ao usuário caso download de arquivos de feriado falhe.
            if (e.PropertyName.Equals("DataEscolhida"))
            {
                if (DataEscolhida.Year < 2000)
                    return;

                WidgetsMes.Clear();

                datasFeriados = FeriadoJsonUtil.RetornaListagemDeFeriados(DataEscolhida.Year);

                Metodo(MesSeguinte, WidgetsMes);
            }
        }

        private void Metodo(DateTime Data, BindingList<DataWidget> Widgets)
        {
            int row = 0;

            foreach (DateTime dateTime in DateTimeUtil.RetornaDiasEmMes(Data.Year, Data.Month))
            {
                DataWidget dataWidgetPassagem = new DataWidget
                {
                    TipoDia = "DIA ÚTIL",
                    NumDia = dateTime.Day,
                    Date = new DateTime(Data.Year, Data.Month, dateTime.Day)
                };

                Grid.SetColumn(dataWidgetPassagem, (int)dateTime.DayOfWeek);
                Grid.SetRow(dataWidgetPassagem, row);

                if (((int)dateTime.DayOfWeek + 1) % 7 == 0)
                    row++;

                if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    dataWidgetPassagem.TipoDia = "DIA NÃO ÚTIL";
                }

                var feriado = datasFeriados.FirstOrDefault(s => s.Date.Day == dateTime.Day && s.Date.Month == dateTime.Month);

                if (feriado != null && dateTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    dataWidgetPassagem.TipoDia = feriado.Type;
                    dataWidgetPassagem.BtnAlternaDiaUtil.ToolTip = $"Nome: {feriado.Name}\nTipo: {feriado.Type}\nDescrição: {feriado.Description}";
                }

                Widgets.Add(dataWidgetPassagem);
            }
        }

        public BindingList<DataWidget> WidgetsMes
        {
            get => _widgetsMes;
            set
            {
                _widgetsMes = value;
                OnPropertyChanged("WidgetsMes");
            }
        }

        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("MesSeguinte");
            }
        }

        public DateTime MesSeguinte
        {
            get => DataEscolhida.AddMonths(1);
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

        public double ValorDiario
        {
            get => _valorDiario;
            set
            {
                _valorDiario = value;

                if (value != calculoDeBonus.ValorDiario())
                    OnPropertyChanged("ValorDiario");
            }
        }

        public string MenuItemHeader1
        {
            get => _menuItemHeader1;
            set
            {
                _menuItemHeader1 = value;
                OnPropertyChanged("MenuItemHeader1");
            }
        }

        public string WindowCaption
        {
            get => _windowCaption;
            set
            {
                _windowCaption = value;
                OnPropertyChanged("WindowCaption");
            }
        }
    }
}
