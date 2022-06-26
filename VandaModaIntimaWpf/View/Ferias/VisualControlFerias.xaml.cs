using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using VandaModaIntimaWpf.ViewModel;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;

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

        public static readonly DependencyProperty NHibernateSessionProperty =
            DependencyProperty.Register("NHibernateSession", typeof(ISession), typeof(VisualControlFerias),
                new FrameworkPropertyMetadata(OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridLengthConverter converter = new GridLengthConverter();
            (d as VisualControlFerias).GridItensFerias.RowDefinitions.Clear();
            (d as VisualControlFerias).GridItensFerias.Children.Clear();
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
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem itemNome = new MenuItem(); //Somente mostra nome de funcionário no contextmenu
                    MenuItem itemAbrirTelaEditar = new MenuItem();

                    itemNome.Header = funcionario.Nome;
                    itemNome.FontWeight = FontWeights.Bold;
                    itemAbrirTelaEditar.Header = "Editar férias";
                    itemAbrirTelaEditar.Command = new RelayCommand(new Action<object>((parameter) =>
                    {
                        new WindowService().ShowDialog(new EditarFuncionarioVM((d as VisualControlFerias).NHibernateSession, funcionario, 2), new Action<bool?, object>((result, viewModel) =>
                        {
                            if (result == true)
                                OnPropertyChanged(d, e);
                        }));
                    }));

                    contextMenu.Items.Add(itemNome);
                    contextMenu.Items.Add(new Separator());
                    contextMenu.Items.Add(itemAbrirTelaEditar);

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
                            Background = new SolidColorBrush(Colors.DarkGray),
                            Margin = new Thickness(1, 1, 1, 1),
                            Padding = new Thickness(0)
                        };

                        textBoxBorderNome.ContextMenu = contextMenu;
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
                        textBoxBorder.Tag = ferias;
                    }
                    else
                    {
                        if(row % 2 == 0)
                            textBox.Background = new SolidColorBrush(Colors.White);
                        else
                            textBox.Background = new SolidColorBrush(Colors.LightGray);
                    }

                    textBoxBorder.Child = textBox;

                    textBoxBorder.ContextMenu = contextMenu;

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

        public ISession NHibernateSession
        {
            get => (ISession)GetValue(NHibernateSessionProperty);
            set
            {
                SetValue(NHibernateSessionProperty, value);
                OnPropertyChanged("NHibernateSession");
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
