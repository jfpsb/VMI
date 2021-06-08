using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
