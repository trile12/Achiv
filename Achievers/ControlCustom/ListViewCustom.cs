using System;
using System.Windows;
using System.Windows.Controls;

namespace Achievers.ControlCustom
{
    public class ListViewCustom : ListView
    {
        public ListViewCustom()
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Achievers;component/Themes/ScrollViewCustom.xaml") });
            var dictionary = new ResourceDictionary() { Source = new Uri("pack://application:,,,/Achievers;component/Themes/ListViewCustom_v2.xaml") };
            Resources.MergedDictionaries.Add(dictionary);
            var style = dictionary[typeof(ListViewItem)] as Style;
            if (style != null)
            {
                style.Setters.Add(new EventSetter(ListViewItem.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(ListViewItem_RequestBringIntoView)));
            }
            ScrollAnimationBehavior.SetIsEnabled(this, true);
            BorderThickness = new Thickness(0);
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
        }

        private void ListViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}