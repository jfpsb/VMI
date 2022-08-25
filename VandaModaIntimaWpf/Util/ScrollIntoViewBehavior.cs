using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace VandaModaIntimaWpf.Util
{
    public class ScrollIntoViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            ListBox listBox = AssociatedObject;
            ((INotifyCollectionChanged)listBox.Items).CollectionChanged += OnListView_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            ListBox listBox = AssociatedObject;
            ((INotifyCollectionChanged)listBox.Items).CollectionChanged -= OnListView_CollectionChanged;
        }

        private void OnListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ListBox listBox = AssociatedObject;
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (listBox.Items.Count > 0)
                    // scroll the new item into view   
                    listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
            }
        }
    }
}
