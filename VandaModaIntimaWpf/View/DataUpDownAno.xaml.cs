using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for DataUpDown.xaml
    /// </summary>
    public partial class DataUpDownAno : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(DateTime), typeof(DataUpDownAno));

        public event PropertyChangedEventHandler PropertyChanged;

        public DataUpDownAno()
        {
            InitializeComponent();
        }

        private void BtnSomar_Click(object sender, RoutedEventArgs e)
        {
            DateTime data = Data;
            data = data.AddYears(1);
            Data = data;
        }

        private void BtnSubtrair_Click(object sender, RoutedEventArgs e)
        {
            DateTime data = Data;
            data = data.AddYears(-1);
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
