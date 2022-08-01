using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Achievers.Converters
{
    public class HideLockedButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value.ToString();
            if (!string.IsNullOrEmpty(number))
                return Math.Round(float.Parse(number)) + "%";
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PercentageToTimerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value.ToString();
            if (!string.IsNullOrEmpty(number))
            {
                float second = 60 - (60 * float.Parse(number)) / 100;
                return Math.Round(second) + "s";
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorEndTimerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value.ToString();
            if (!string.IsNullOrEmpty(number))
            {
                if (float.Parse(number) < 70)
                {
                    return "#ED9691";
                }
            }
            return "#A5E4F2";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorStartTimerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value.ToString();
            if (!string.IsNullOrEmpty(number))
            {
                if (float.Parse(number) < 70)
                {
                    return "#D44942";
                }
            }
            return "#4FBEDB";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorTextTimerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string number = value.ToString();
            if (!string.IsNullOrEmpty(number))
            {
                float second = 60 - (60 * float.Parse(number)) / 100;
                if (second < 15)
                {
                    return "#4E4CD4";
                }
            }
            return "#A5E4F2";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HideEmtyStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            if (!string.IsNullOrEmpty(value.ToString()))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorToStringCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeartLeftImageCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int)value;
            return string.Format("/Achievers;component/Assets/Images/h{0}.png", count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidthByPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            return (276 * ((int)value)) / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidthByPercentageConverter_2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return 0;
            return (200 * int.Parse(value.ToString())) / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidthPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? progress = (int?)value;
            if (progress == null) return "Progress --";
            return $"Progress {progress}%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VocabIconFavoriteConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 1:
                    return "/Achievers;component/Assets/Images/ic-fv-hv.png";
                default:
                    return "/Achievers;component/Assets/Images/ic-fv.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorButtonImageCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            switch (value.ToString())
            {
                case "Review 1":
                    return "/Achievers;component/Assets/Images/ic-button.png";
                case "Review 2":
                    return "/Achievers;component/Assets/Images/ic-button-or.png";
                case "Review 4":
                    return "/Achievers;component/Assets/Images/bt-word-green.png";
                default:
                    break;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}