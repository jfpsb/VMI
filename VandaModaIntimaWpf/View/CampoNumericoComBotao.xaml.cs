using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for CampoNumericoComBotao.xaml
    /// </summary>
    public partial class CampoNumericoComBotao : UserControl, INotifyPropertyChanged
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataEscolhidaProperty =
            DependencyProperty.Register("DataEscolhida", typeof(DateTime), typeof(CampoNumericoComBotao), new PropertyMetadata(DateTime.Now));

        public event PropertyChangedEventHandler PropertyChanged;

        public CampoNumericoComBotao()
        {
            InitializeComponent();
            DataEscolhida = DateTime.Now;
            Binding binding = new Binding("DataEscolhidaString");
            binding.Source = this;
            TxtNumero.SetBinding(TextBox.TextProperty, binding);
        }

        private void BtnSomar_Click(object sender, RoutedEventArgs e)
        {
            DateTime data = DataEscolhida;

            if (data.Month == 12)
            {
                data = data.AddMonths(-11);
                data = data.AddYears(1);
            }
            else
            {
                data = data.AddMonths(1);
            }

            DataEscolhida = data;
        }

        private void BtnSubtrair_Click(object sender, RoutedEventArgs e)
        {
            DateTime data = DataEscolhida;

            if (data.Month == 1)
            {
                data = data.AddMonths(11);
                data = data.AddYears(-1);
            }
            else
            {
                data = data.AddMonths(-1);
            }

            DataEscolhida = data;
        }
        public DateTime DataEscolhida
        {
            get { return (DateTime)GetValue(DataEscolhidaProperty); }
            set
            {
                SetValue(DataEscolhidaProperty, value);
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("DataEscolhidaString");
            }
        }
        public string DataEscolhidaString
        {
            get { return DataEscolhida.ToString("MMM/yyyy"); }
            set
            {
                // Se o usuário não colocar a barra separando mes e ano
                if (!value.Contains("/"))
                {
                    OnPropertyChanged("DataEscolhidaString");
                }
                else
                {
                    string[] valores = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    try
                    {
                        if (valores.Length != 2)
                        {
                            throw new FormatException("Usuário Deixou Ano ou Mês Vazio");
                        }

                        int mes = 0, ano = 0;

                        // Se mês for informado em números
                        if (IsDigitsOnly(valores[0]))
                        {
                            if (valores[0].Length > 2)
                            {
                                throw new FormatException("Usuário digitou número de mês inválido, i.e., com mais de 2 números");
                            }

                            mes = int.Parse(valores[0]);
                        }
                        // Se mês for informado em letras
                        else
                        {
                            string mesAbreviado = valores[0].Substring(0, 3);
                            mes = DateTime.ParseExact(mesAbreviado, "MMM", CultureInfo.CurrentCulture).Month;
                        }

                        if (!IsDigitsOnly(valores[1]))
                        {
                            throw new FormatException("Usuário inseriu caracteres inválidos em ano, i.e., letras");
                        }

                        if (valores[1].Length != 4)
                        {
                            throw new FormatException("Usuário inseriu ano no formato errado");
                        }

                        ano = int.Parse(valores[1]);

                        // Seta data
                        DataEscolhida = new DateTime(ano, mes, 1);
                    }
                    catch (FormatException fe)
                    {
                        Console.WriteLine(fe.Message);
                        OnPropertyChanged("DataEscolhidaString");
                    }
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
