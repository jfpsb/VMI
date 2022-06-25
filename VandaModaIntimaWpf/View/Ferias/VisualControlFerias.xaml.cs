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
                rowDefinition.MinHeight = 35;
                (d as VisualControlFerias).GridItensFerias.RowDefinitions.Add(rowDefinition);

                for (int i = 0; i < 13; i++)
                {
                    if (i == 0)
                    {
                        Border textBlockBorderNome = new Border
                        {
                            BorderThickness = new Thickness(0.3),
                            BorderBrush = new SolidColorBrush(Colors.Black)
                        };
                        TextBlock textBlockNome = new TextBlock
                        {
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            Text = funcionario.Nome,
                            FontWeight = FontWeights.Bold
                        };

                        textBlockBorderNome.Child = textBlockNome;
                        (d as VisualControlFerias).GridItensFerias.Children.Add(textBlockBorderNome);
                        Grid.SetColumn(textBlockBorderNome, i);
                        Grid.SetRow(textBlockBorderNome, row);
                        continue;
                    }

                    Model.Ferias ferias = funcionario.Ferias.Where(w => w.Inicio.Year == 2022 && w.Inicio.Month == i && w.Deletado == false).FirstOrDefault();

                    Border textBlockBorder = new Border
                    {
                        BorderThickness = new Thickness(0.3),
                        BorderBrush = new SolidColorBrush(Colors.Black)
                    };
                    TextBlock textBlock = new TextBlock
                    {
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap
                    };

                    if (ferias != null)
                    {
                        textBlock.Background = new SolidColorBrush(Colors.Yellow);
                        textBlock.Text = $"Início em {ferias.Inicio:dd/MM/yyyy}";
                        textBlockBorder.BorderThickness = new Thickness(2.0);
                    }
                    else
                    {
                        textBlock.Background = new SolidColorBrush(Colors.White);
                    }

                    textBlockBorder.Child = textBlock;

                    (d as VisualControlFerias).GridItensFerias.Children.Add(textBlockBorder);
                    Grid.SetColumn(textBlockBorder, i);
                    Grid.SetRow(textBlockBorder, row);
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
