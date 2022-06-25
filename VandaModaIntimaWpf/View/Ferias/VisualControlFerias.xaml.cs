using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VandaModaIntimaWpf.View.Ferias
{
    /// <summary>
    /// Interaction logic for VisualControlFerias.xaml
    /// </summary>
    public partial class VisualControlFerias : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty FuncionariosProperty =
            DependencyProperty.Register("Funcionarios", typeof(IEnumerable<Model.Funcionario>), typeof(VisualControlFerias),
                new FrameworkPropertyMetadata(OnPropertyChanged));

        public static readonly DependencyProperty AnoProperty =
            DependencyProperty.Register("Ano", typeof(DateTime), typeof(VisualControlFerias),
                new FrameworkPropertyMetadata(OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridLengthConverter converter = new GridLengthConverter();
            (d as VisualControlFerias).GridItensFerias.RowDefinitions.Clear();
            int row = 0;

            if ((d as VisualControlFerias).Funcionarios == null) return;

            foreach (var funcionario in (d as VisualControlFerias).Funcionarios)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = (GridLength)converter.ConvertFromString("*");
                rowDefinition.MaxHeight = 35;
                (d as VisualControlFerias).GridItensFerias.RowDefinitions.Add(rowDefinition);

                for (int i = 0; i < 13; i++)
                {
                    if (i == 0)
                    {
                        Border textBoxBorderNome = new Border
                        {
                            BorderThickness = new Thickness(0.6),
                            BorderBrush = new SolidColorBrush(Colors.Black)
                        };
                        ScrollViewer scrollViewer = new ScrollViewer()
                        {
                            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                            CanContentScroll = true,
                            MaxHeight = 35
                        };
                        TextBlock textBoxNome = new TextBlock
                        {
                            FontSize = 14,
                            Text = funcionario.Nome,
                            TextWrapping = TextWrapping.Wrap,
                            FontWeight = FontWeights.Bold,
                            Background = new SolidColorBrush(Colors.LightGray),
                            Margin = new Thickness(1, 1, 1, 1),
                            Padding = new Thickness(0)
                        };

                        scrollViewer.Content = textBoxNome;
                        textBoxBorderNome.Child = scrollViewer;
                        (d as VisualControlFerias).GridItensFerias.Children.Add(textBoxBorderNome);
                        Grid.SetColumn(textBoxBorderNome, i);
                        Grid.SetRow(textBoxBorderNome, row);
                        continue;
                    }

                    Model.Ferias ferias = funcionario.Ferias.Where(w => w.Inicio.Year == (d as VisualControlFerias).Ano.Year && w.Inicio.Month == i && w.Deletado == false).FirstOrDefault();

                    Border textBoxBorder = new Border
                    {
                        BorderThickness = new Thickness(0.3),
                        BorderBrush = new SolidColorBrush(Colors.Black)
                    };
                    TextBlock textBox = new TextBlock
                    {
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(1),
                        Padding = new Thickness(0)
                    };

                    if (ferias != null)
                    {
                        textBox.Background = new SolidColorBrush(Colors.Yellow);
                        textBox.Text = $"Início em {ferias.Inicio:dd/MM/yyyy}";
                        textBoxBorder.BorderThickness = new Thickness(1.5);
                    }
                    else
                    {
                        textBox.Background = new SolidColorBrush(Colors.White);
                    }

                    textBoxBorder.Child = textBox;

                    (d as VisualControlFerias).GridItensFerias.Children.Add(textBoxBorder);
                    Grid.SetColumn(textBoxBorder, i);
                    Grid.SetRow(textBoxBorder, row);
                }

                row++;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public VisualControlFerias()
        {
            InitializeComponent();
            PropertyChanged += VisualControlFerias_PropertyChanged;
            SetValue(FuncionariosProperty, new List<Model.Funcionario>());
        }

        private void VisualControlFerias_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Funcionarios":
                    CriaLinhasFerias();
                    break;
            }
        }

        private void CriaLinhasFerias()
        {
            GridLengthConverter converter = new GridLengthConverter();
            GridItensFerias.RowDefinitions.Clear();
            int row = 0;

            foreach (var funcionario in Funcionarios)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = (GridLength)converter.ConvertFromString("*");
                GridItensFerias.RowDefinitions.Add(rowDefinition);

                for (int i = 0; i < 12; i++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = new SolidColorBrush(Colors.AliceBlue);
                    GridItensFerias.Children.Add(rectangle);
                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, row);
                }

                row++;
            }
        }

        public IEnumerable<Model.Funcionario> Funcionarios
        {
            get
            {
                return (IEnumerable<Model.Funcionario>)GetValue(FuncionariosProperty);
            }
            set
            {
                SetValue(FuncionariosProperty, value);
                OnPropertyChanged("Funcionarios");
            }
        }

        public DateTime Ano
        {
            get => (DateTime)GetValue(AnoProperty);
            set
            {
                SetValue(AnoProperty, value);
                OnPropertyChanged("Ano");
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
