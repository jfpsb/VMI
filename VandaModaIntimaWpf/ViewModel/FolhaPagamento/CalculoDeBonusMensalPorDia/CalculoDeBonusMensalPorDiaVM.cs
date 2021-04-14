using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoDeBonusMensalPorDia
{
    public class CalculoDeBonusMensalPorDiaVM : ObservableObject
    {
        private BindingList<DataWidget> _widgets;
        private DataFeriado[] datasFeriados;
        private DateTime _dataEscolhida;
        private double _valorTotal;
        private double _valorDiario;
        private IMessageBoxService messageBoxService;
        private string _menuItemHeader1;
        private string _windowCaption;
        private ICalculoDeBonus calculoDeBonus;

        // Comando para adicionar valor do bônus para os funcionários
        public ICommand AbrirAdicionarBonusComando { get; set; }

        public CalculoDeBonusMensalPorDiaVM(DateTime dataEscolhida, IMessageBoxService messageBoxService, ICalculoDeBonus calculoDeBonus)
        {
            this.calculoDeBonus = calculoDeBonus;
            MenuItemHeader1 = calculoDeBonus.MenuItemHeader1();
            WindowCaption = calculoDeBonus.WindowCaption();
            ValorDiario = calculoDeBonus.ValorDiario();
            this.messageBoxService = messageBoxService;

            PropertyChanged += CalculaDatas;
            PropertyChanged += ValorDiarioAlterado;

            Widgets = new BindingList<DataWidget>();
            Widgets.ListChanged += CalcultaTotal;
            DataEscolhida = dataEscolhida;

            AbrirAdicionarBonusComando = new RelayCommand(AbrirAdicionarBonus);
        }

        private void ValorDiarioAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ValorDiarioPassagem"))
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
            calculoDeBonus.AbrirAdicionarBonus(DataEscolhida, ValorTotal, Widgets.Where(w => w.IsDiaUtil).Count(), messageBoxService);
        }

        private void CalcultaTotal(object sender, ListChangedEventArgs e)
        {
            int quantDiasUteis = Widgets.Where(w => w.IsDiaUtil).Count();
            ValorTotal = quantDiasUteis * ValorDiario;
        }

        private void CalculaDatas(object sender, PropertyChangedEventArgs e)
        {
            //TODO: Mostrar mensagem ao usuário caso download de arquivos de feriado falhe.
            if (e.PropertyName.Equals("DataEscolhida") || e.PropertyName.Equals("Widgets"))
            {
                if (DataEscolhida.Year < 2000)
                    return;

                Widgets.Clear();

                if (!File.Exists($"Resources/Feriados/{DataEscolhida.Year}.json") && DataEscolhida.Year > 1999)
                {
                    try
                    {
                        string url = string.Format("https://api.calendario.com.br/?json=true&ano={0}&estado=MA&cidade=SAO_LUIS&token=amZwc2JfZmVsaXBlMkBob3RtYWlsLmNvbSZoYXNoPTE1NDcxMDY0NA", DataEscolhida.Year);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            File.WriteAllText($"Resources/Feriados/{DataEscolhida.Year}.json", reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var datasFeriadosJson = File.ReadAllText($"Resources/Feriados/{DataEscolhida.Year}.json");
                datasFeriados = JsonConvert.DeserializeObject<DataFeriado[]>(datasFeriadosJson);
                int row = 0;

                foreach (DateTime dateTime in AllDatesInMonth(DataEscolhida.Year, DataEscolhida.Month))
                {
                    DataWidget dataWidgetPassagem = new DataWidget
                    {
                        TipoDia = "DIA ÚTIL",
                        NumDia = dateTime.Day
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
        }

        public BindingList<DataWidget> Widgets
        {
            get => _widgets;
            set
            {
                _widgets = value;
                OnPropertyChanged("Widgets");
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

        private IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
    }
}
