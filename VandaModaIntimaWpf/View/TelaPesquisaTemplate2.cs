using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View
{
    class TelaPesquisaTemplate2 : ContentControl
    {
        public static readonly DependencyProperty BtnApagarMarcadoTextoProperty =
            DependencyProperty.Register("BtnApagarMarcadoTexto", typeof(string), typeof(TelaPesquisaTemplate2), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(TelaPesquisaTemplate2), new FrameworkPropertyMetadata());

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
