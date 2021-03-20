using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VandaModaIntimaWpf.View"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VandaModaIntimaWpf.View;assembly=VandaModaIntimaWpf.View"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TelaPesquisaTemplate3/>
    ///
    /// </summary>
    public class TelaPesquisaTemplate : Control
    {
        static TelaPesquisaTemplate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TelaPesquisaTemplate), new FrameworkPropertyMetadata(typeof(TelaPesquisaTemplate)));
        }

        public static readonly DependencyProperty BtnApagarMarcadoTextoProperty =
            DependencyProperty.Register("BtnApagarMarcadoTexto", typeof(string), typeof(TelaPesquisaTemplate));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(TelaPesquisaTemplate), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(TelaPesquisaTemplate), new PropertyMetadata(null));

        public static readonly DependencyProperty LeftButtonPresenterProperty =
            DependencyProperty.Register("LeftButtonPresenter", typeof(object), typeof(TelaPesquisaTemplate), new PropertyMetadata(null));

        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(object), typeof(TelaPesquisaTemplate), new PropertyMetadata(null));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public object Menu
        {
            get { return GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }

        public object LeftButtonPresenter
        {
            get => GetValue(LeftButtonPresenterProperty);
            set { SetValue(LeftButtonPresenterProperty, value); }
        }

        public string BtnApagarMarcadoTexto
        {
            get => (string)GetValue(BtnApagarMarcadoTextoProperty);
            set => SetValue(BtnApagarMarcadoTextoProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ContentPresenter mainContentPresenter = GetTemplateChild("Presenter") as ContentPresenter;
            mainContentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content") { Source = this });

            ContentPresenter menuContentPresenter = GetTemplateChild("Menu") as ContentPresenter;
            menuContentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Menu") { Source = this });

            ContentPresenter leftButtonPresenter = GetTemplateChild("LeftButtonPresenter") as ContentPresenter;
            leftButtonPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("LeftButtonPresenter") { Source = this });
        }
    }
}
