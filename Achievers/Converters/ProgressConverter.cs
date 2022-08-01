using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace Achievers.Converters
{
    public class CheckResultProgress : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value >= 70)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ResultDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue) return null;
            var list = new ObservableCollection<Inline>();
            int i = 0;
            foreach (string item in value.ToString().Split(new string[2] { "#b", "b#" }, StringSplitOptions.None))
            {
                if (i % 2 == 1)
                {
                    list.Add(new Run(item) { FontWeight = FontWeights.SemiBold });
                }
                else
                {
                    list.Add(new Run(item));
                }
                i++;
            }
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}