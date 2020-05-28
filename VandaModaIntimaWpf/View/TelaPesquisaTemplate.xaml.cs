using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for TelaPesquisaTemplate.xaml
    /// </summary>
    public partial class TelaPesquisaTemplate : UserControl
    {
        public static readonly DependencyProperty BtnApagarMarcadoTextoProperty =
            DependencyProperty.Register("BtnApagarMarcadoTexto", typeof(string), typeof(TelaPesquisaTemplate), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(TelaPesquisaTemplate), new FrameworkPropertyMetadata());

        public TelaPesquisaTemplate()
        {
            InitializeComponent();
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }

        public string BtnApagarMarcadoTexto
        {
            get => (string)GetValue(BtnApagarMarcadoTextoProperty);
            set => SetValue(BtnApagarMarcadoTextoProperty, value);
        }
    }
}
