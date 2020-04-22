using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Menu utilizado nas telas de pesquisa.
    /// </summary>
    public class TelaPesquisaMenu : Menu
    {
        public TelaPesquisaMenu()
        {
            DefaultStyleKey = typeof(TelaPesquisaMenu);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TelaPesquisaMenu), new FrameworkPropertyMetadata());

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
