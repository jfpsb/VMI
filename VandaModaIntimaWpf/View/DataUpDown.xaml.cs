using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VandaModaIntimaWpf.View.Interfaces.DataUpDown;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for DataUpDown.xaml
    /// </summary>
    public partial class DataUpDown : UserControl, INotifyPropertyChanged
    {
        private IDataUpDown dataUpDown;

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(DateTime), typeof(DataUpDown));

        public static readonly DependencyProperty DataStringFormatProperty =
            DependencyProperty.Register("DataStringFormat", typeof(string), typeof(DataUpDown), new PropertyMetadata(DataStringFormatCallback));

        private static void DataStringFormatCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DataUpDown()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding("Data")
            {
                Source = this,
                StringFormat = DataStringFormat,
                Mode = BindingMode.TwoWay,
                ConverterCulture = new System.Globalization.CultureInfo("pt-BR")
            };

            TxtNumero.SetBinding(TextBox.TextProperty, binding);

            switch (DataStringFormat)
            {
                case "MM/yyyy":
                    dataUpDown = new SomaMes();
                    break;
                case "MMM/yyyy":
                    dataUpDown = new SomaMes();
                    break;
                case "dd/MM/yyyy":
                    dataUpDown = new SomaAno();
                    break;
                case "yyyy":
                    dataUpDown = new SomaAno();
                    break;
                default:
                    throw new Exception("Não existe implementação para este formato de data em DataUpDown.");
            }
        }

        private void BtnSomar_Click(object sender, RoutedEventArgs e)
        {
            Data = dataUpDown.Somar(Data);
        }

        private void BtnSubtrair_Click(object sender, RoutedEventArgs e)
        {
            Data = dataUpDown.Subtrair(Data);
        }
        public DateTime Data
        {
            get { return (DateTime)GetValue(DataProperty); }
            set
            {
                SetValue(DataProperty, value);
                OnPropertyChanged("Data");
            }
        }

        public string DataStringFormat
        {
            get
            {
                return (string)GetValue(DataStringFormatProperty);
            }
            set
            {
                SetValue(DataStringFormatProperty, value);
                OnPropertyChanged("StringFormat");
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
    }
}
