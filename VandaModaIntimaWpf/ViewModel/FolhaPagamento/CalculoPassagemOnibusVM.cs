using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Controls;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class CalculoPassagemOnibusVM : ObservableObject
    {
        private BindingList<DataWidgetPassagem> _widgets;
        private DataFeriado[] datasFeriados;
        private DateTime _dataEscolhida;
        private double _totalPassagem;

        public CalculoPassagemOnibusVM()
        {
            PropertyChanged += CalculaDatas;

            Widgets = new BindingList<DataWidgetPassagem>();
            Widgets.ListChanged += CalcultaTotalPassagem;
            DataEscolhida = DateTime.Now;
        }

        private void CalcultaTotalPassagem(object sender, ListChangedEventArgs e)
        {
            int quantDiasUteis = Widgets.Where(w => w.IsDiaUtil).Count();
            TotalPassagem = Math.Round(quantDiasUteis * 7.4, 2, MidpointRounding.AwayFromZero);
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
                    DataWidgetPassagem dataWidgetPassagem = new DataWidgetPassagem
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

                    if (feriado != null)
                    {
                        dataWidgetPassagem.TipoDia = feriado.Type;
                    }

                    Widgets.Add(dataWidgetPassagem);
                }
            }
        }

        public BindingList<DataWidgetPassagem> Widgets
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

        public double TotalPassagem
        {
            get => _totalPassagem;
            set
            {
                _totalPassagem = value;
                OnPropertyChanged("TotalPassagem");
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
