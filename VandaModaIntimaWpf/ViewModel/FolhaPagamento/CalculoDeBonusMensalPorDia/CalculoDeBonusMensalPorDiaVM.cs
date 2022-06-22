using Newtonsoft.Json;
using NHibernate;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        private BindingList<DataWidget> _widgetsMes1;
        private BindingList<DataWidget> _widgetsMes2;
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

            WidgetsMes1 = new BindingList<DataWidget>();
            WidgetsMes1.ListChanged += CalcultaTotal;

            WidgetsMes2 = new BindingList<DataWidget>();
            WidgetsMes2.ListChanged += CalcultaTotal;

            DataEscolhida = dataEscolhida;

            AbrirAdicionarBonusComando = new RelayCommand(AbrirAdicionarBonus);
        }

        private void ValorDiarioAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ValorDiario"))
            {
                var result = messageBoxService.Show("O Valor Diário Da Passagem Foi Alterado. Deseja Confirmar A Alteração E Salvar O Novo Valor?", "Cálculo De Passagem de Ônibus", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    Config.Instancia.ValorDiarioPassagemOnibus = ValorDiario;
                    var json = JsonConvert.SerializeObject(Config.Instancia);
                    File.WriteAllText("Config.json", json);
                    CalcultaTotal(null, null);
                }
                else
                {
                    ValorDiario = Config.Instancia.ValorDiarioPassagemOnibus;
                }
            }
        }

        private void AbrirAdicionarBonus(object obj)
        {
            DateTime primeiroDia = new DateTime(), ultimoDia = new DateTime();

            if (WidgetsMes1.Where(w => w.IsDiaUtil).Count() == 0 && WidgetsMes2.Where(w => w.IsDiaUtil).Count() == 0)
            {
                messageBoxService.Show("Não Há Dias Marcados Como Dias Úteis Nos Calendários!");
                return;
            }
            else if (WidgetsMes2.Where(w => w.IsDiaUtil).Count() == 0)
            {
                primeiroDia = WidgetsMes1.Where(w => w.IsDiaUtil).First().Date;
                ultimoDia = WidgetsMes1.Where(w => w.IsDiaUtil).Last().Date;
            }
            else if (WidgetsMes1.Where(w => w.IsDiaUtil).Count() == 0)
            {
                primeiroDia = WidgetsMes2.Where(w => w.IsDiaUtil).First().Date;
                ultimoDia = WidgetsMes2.Where(w => w.IsDiaUtil).Last().Date;
            }
            else
            {
                primeiroDia = WidgetsMes1.Where(w => w.IsDiaUtil).First().Date;
                ultimoDia = WidgetsMes2.Where(w => w.IsDiaUtil).Last().Date;
            }

            int numDias = WidgetsMes1.Where(w => w.IsDiaUtil).Count() + WidgetsMes2.Where(w => w.IsDiaUtil).Count();
            calculoDeBonus.AbrirAdicionarBonus(_session, false, DataEscolhida, ValorTotal, ValorDiario, numDias, primeiroDia, ultimoDia);
        }

        private void CalcultaTotal(object sender, ListChangedEventArgs e)
        {
            int quantDiasUteis = WidgetsMes1.Where(w => w.IsDiaUtil).Count() + WidgetsMes2.Where(w => w.IsDiaUtil).Count();
            ValorTotal = quantDiasUteis * ValorDiario;
        }

        private void CalculaDatas(object sender, PropertyChangedEventArgs e)
        {
            //TODO: Mostrar mensagem ao usuário caso download de arquivos de feriado falhe.
            if (e.PropertyName.Equals("DataEscolhida"))
            {
                if (DataEscolhida.Year < 2000)
                    return;

                WidgetsMes1.Clear();
                WidgetsMes2.Clear();

                //if (!File.Exists($"Resources/Feriados/{DataEscolhida.Year}.json"))
                //{
                //    try
                //    {
                //        string url = string.Format("https://api.calendario.com.br/?json=true&ano={0}&estado=MA&cidade=SAO_LUIS&token=amZwc2JfZmVsaXBlMkBob3RtYWlsLmNvbSZoYXNoPTE1NDcxMDY0NA", DataEscolhida.Year);
                //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //        WebResponse response = request.GetResponse();
                //        using (Stream responseStream = response.GetResponseStream())
                //        {
                //            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                //            File.WriteAllText($"Resources/Feriados/{DataEscolhida.Year}.json", reader.ReadToEnd());
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                //var datasFeriadosJson = File.ReadAllText($"Resources/Feriados/{DataEscolhida.Year}.json");
                datasFeriados = FeriadoJsonUtil.RetornaListagemDeFeriados(DataEscolhida.Year);

                Metodo(DataEscolhida, WidgetsMes1, 1);
                Metodo(DataEscolhida.AddMonths(1), WidgetsMes2, 2);
            }
        }

        private void Metodo(DateTime Data, BindingList<DataWidget> Widgets, int ordemMes)
        {
            int row = 0;
            DateTime quintoDiaUtil = DateTimeUtil.RetornaDataUtil(5, Data.Month, Data.Year);

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

                if (ordemMes == 1)
                {
                    if (dateTime.Day <= quintoDiaUtil.Day)
                    {
                        dataWidgetPassagem.TipoDia = "DIA NÃO ÚTIL";
                    }
                }
                else
                {
                    if (dateTime.Day > quintoDiaUtil.Day)
                    {
                        dataWidgetPassagem.TipoDia = "DIA NÃO ÚTIL";
                    }
                }

                Widgets.Add(dataWidgetPassagem);
            }
        }

        public BindingList<DataWidget> WidgetsMes1
        {
            get => _widgetsMes1;
            set
            {
                _widgetsMes1 = value;
                OnPropertyChanged("WidgetsMes1");
            }
        }

        public BindingList<DataWidget> WidgetsMes2
        {
            get => _widgetsMes2;
            set
            {
                _widgetsMes2 = value;
                OnPropertyChanged("WidgetsMes2");
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
