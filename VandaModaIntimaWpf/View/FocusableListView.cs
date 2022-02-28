using System.Windows;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.View
{
    public class FocusableListView
    {
        public static DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusableListView), new UIPropertyMetadata(false, OnIsFocusedChanged));

        public static bool GetIsFocused(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsFocusedProperty, value);
        }

        public static void OnIsFocusedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ListView listView = dependencyObject as ListView;
            bool newValue = (bool)dependencyPropertyChangedEventArgs.NewValue;
            bool oldValue = (bool)dependencyPropertyChangedEventArgs.OldValue;
            if (newValue && !oldValue && !listView.IsFocused) listView.Focus();

            listView.SelectedIndex = 0;

            if (listView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                int index = listView.SelectedIndex;

                if (index >= 0)
                {
                    ListViewItem item = listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;

                    if (item != null)
                        item.Focus();
                }
            }
        }
    }
}
