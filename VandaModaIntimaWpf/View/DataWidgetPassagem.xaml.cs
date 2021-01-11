using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for DataWidgetPassagem.xaml
    /// </summary>
    public partial class DataWidgetPassagem : UserControl, INotifyPropertyChanged
    {
        private bool _isDiaUtil;
        private string _tipoDia;
        private int _numDia;

        public event PropertyChangedEventHandler PropertyChanged;

        public DataWidgetPassagem()
        {
            InitializeComponent();
        }

        public bool IsDiaUtil
        {
            get => _isDiaUtil;
            set
            {
                _isDiaUtil = value;

                if (value)
                {
                    GradientStopFundo.Color = Colors.Green;
                }
                else
                {
                    if (TipoDia.ToLower().Equals("facultativo"))
                    {
                        GradientStopFundo.Color = Colors.Yellow;
                    }
                    else
                    {
                        GradientStopFundo.Color = Colors.Red;
                    }
                }

                OnPropertyChanged("IsDiaUtil");
            }
        }
        public string TipoDia
        {
            get => _tipoDia;
            set
            {
                _tipoDia = value;

                if (value.ToLower().Equals("facultativo"))
                {
                    GradientStopFundo.Color = Colors.Yellow;
                    IsDiaUtil = false;
                }
                else if (value.ToLower().Equals("feriado nacional")
                    || value.ToLower().Equals("feriado estadual")
                    || value.ToLower().Equals("feriado municipal")
                    || value.ToLower().Equals("dia não útil"))
                {
                    GradientStopFundo.Color = Colors.Red;
                    IsDiaUtil = false;
                }
                else
                {
                    GradientStopFundo.Color = Colors.Green;
                    IsDiaUtil = true;
                }

                if (value.ToLower().Equals("feriado nacional")
                    || value.ToLower().Equals("feriado estadual")
                    || value.ToLower().Equals("feriado municipal")
                    || value.ToLower().Equals("facultativo"))
                {
                    BorderThickness = new Thickness(0, 0, 0, 2);
                    BorderBrush = Brushes.Black;
                }

                OnPropertyChanged("TipoDia");
            }
        }

        public int NumDia
        {
            get => _numDia;
            set
            {
                _numDia = value;
                BtnAlternaDiaUtil.Content = value.ToString();
                OnPropertyChanged("NumDia");
            }
        }

        private void BtnAlternaDiaUtil_Click(object sender, RoutedEventArgs e)
        {
            if (IsDiaUtil)
            {
                IsDiaUtil = false;
            }
            else
            {
                IsDiaUtil = true;
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
    }
}
