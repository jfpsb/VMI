using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for DataUpDown.xaml
    /// </summary>
    public partial class DataUpDown : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(DateTime), typeof(DataUpDown));

        public event PropertyChangedEventHandler PropertyChanged;

        public DataUpDown()
        {
            InitializeComponent();
            Binding binding = new Binding("DataString")
            {
                Source = this
            };
            TxtNumero.SetBinding(TextBox.TextProperty, binding);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("Data");
            OnPropertyChanged("DataString");
        }

        private void BtnSomar_Click(object sender, RoutedEventArgs e)
        {
            DateTime data = Data;

            if (data.Month == 12)
            {
                data = data.AddMonths(-11);
                data = data.AddYears(1);
            }
            else
            {
                data = data.AddMonths(1);
            }

            Data = data;
        }

        private void BtnSubtrair_Click(object sender, RoutedEventArgs e)
        {
            DateTime data = Data;

            if (data.Month == 1)
            {
                data = data.AddMonths(11);
                data = data.AddYears(-1);
            }
            else
            {
                data = data.AddMonths(-1);
            }

            Data = data;
        }
        public DateTime Data
        {
            get { return (DateTime)GetValue(DataProperty); }
            set
            {
                SetValue(DataProperty, value);
                OnPropertyChanged("Data");
                OnPropertyChanged("DataString");
            }
        }
        public string DataString
        {
            get { return Data.ToString("MMM/yyyy"); }
            set
            {
                // Se o usuário não colocar a barra separando mes e ano
                if (!value.Contains("/"))
                {
                    OnPropertyChanged("DataString");
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
                        Data = new DateTime(ano, mes, 1);
                    }
                    catch (FormatException fe)
                    {
                        Console.WriteLine(fe.Message);
                        OnPropertyChanged("DataString");
                    }
                }
            }
        }
        private void OnPropertyChanged(string propertyName)
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
