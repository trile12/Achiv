using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CardItem.xaml
    /// </summary>
    public partial class CardItem : UserControl
    {
        #region Event OnClick

        public static readonly RoutedEvent OnClickEvent = EventManager.RegisterRoutedEvent("OnClick", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(CardItem));

        public event MouseButtonEventHandler OnClick
        {
            add
            {
                base.AddHandler(OnClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnClickEvent, value);
            }
        }

        #endregion Event OnClick

        #region Notification Property

        public bool IsUnlock
        {
            get { return (bool)GetValue(IsUnlockProperty); }
            set { SetValue(IsUnlockProperty, value); }
        }

        public static readonly DependencyProperty IsUnlockProperty =
            DependencyProperty.Register("IsUnlock", typeof(bool), typeof(CardItem), new PropertyMetadata(false));

        public int? Progress
        {
            get { return (int?)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(int?), typeof(CardItem), new PropertyMetadata(null));

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(CardItem), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(CardItem), new PropertyMetadata(Colors.White));

        public Color InactiveColor
        {
            get { return (Color)GetValue(InactiveColorProperty); }
            set { SetValue(InactiveColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveColorProperty =
            DependencyProperty.Register("InactiveColor", typeof(Color), typeof(CardItem), new PropertyMetadata(Colors.White));

        public Color InactiveColor2
        {
            get { return (Color)GetValue(InactiveColor2Property); }
            set { SetValue(InactiveColor2Property, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveColor2Property =
            DependencyProperty.Register("InactiveColor2", typeof(Color), typeof(CardItem), new PropertyMetadata(Colors.White));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(CardItem), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CardItem), new PropertyMetadata(string.Empty));

        public string TitleVI
        {
            get { return (string)GetValue(TitleVIProperty); }
            set { SetValue(TitleVIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleVI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleVIProperty =
            DependencyProperty.Register("TitleVI", typeof(string), typeof(CardItem), new PropertyMetadata(string.Empty));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(CardItem), new PropertyMetadata(null));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(CardItem), new PropertyMetadata(false));

        //private static void OnIsActivePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue != null)
        //    {
        //        var el = sender as CardItem;
        //        if ((bool)e.NewValue == true)
        //        {
        //            var gradient = new LinearGradientBrush(el.StartColor, el.EndColor, new Point(0, 0), new Point(1, 1));
        //            el.bg.Background = gradient;
        //        }
        //        else
        //        {
        //            el.bg.ClearValue(BackgroundProperty);
        //        }
        //    }
        //}

        public float FontScale
        {
            get { return (float)GetValue(FontScaleProperty); }
            set { SetValue(FontScaleProperty, value); }
        }

        public static readonly DependencyProperty FontScaleProperty =
            DependencyProperty.Register("FontScale", typeof(float), typeof(CardItem), new PropertyMetadata(1.0f));

        #endregion Notification Property

        public CardItem()
        {
            InitializeComponent();
        }

        private void BounceButton_OnClick(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton) { RoutedEvent = OnClickEvent });
        }
    }

    public class ProgressToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null) return 0.0;
            int? progress = (int?)values[0];
            double width = (double)values[1];
            return (double)progress / 100 * width;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BgCardItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 5 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue
                || values[2] == DependencyProperty.UnsetValue || values[3] == DependencyProperty.UnsetValue || values[4] == DependencyProperty.UnsetValue) return null;
            Color startColor = (Color)ColorConverter.ConvertFromString("#C5D2E0");
            Color endColor = (Color)ColorConverter.ConvertFromString("#9CABBD");
            bool isUnlock = (bool)values[3];
            bool isActive = (bool)values[4];
            if (isUnlock)
            {
                if (isActive)
                {
                    startColor = (Color)values[0];
                    endColor = (Color)values[1];
                }
                else
                {
                    return new SolidColorBrush((Color)values[2]);
                }
            }
            return new LinearGradientBrush(startColor, endColor, new Point(0, 0), new Point(1, 1));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BgCardItem2Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3) return null;
            Color color = (Color)ColorConverter.ConvertFromString("#F1F0FC");
            color.A = 38;
            bool isUnlock = (bool)values[1];
            bool isActive = (bool)values[2];
            if (isUnlock)
            {
                if (!isActive)
                {
                    color = (Color)values[0];
                    color.A = 89;
                }
            }
            return new SolidColorBrush(color);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CardItemUnitNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return $"{value.ToString()}: ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CardItemMainBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3) return null;
            Color color = Colors.White;
            bool isUnlock = (bool)values[1];
            bool isActive = (bool)values[2];
            if (isUnlock)
            {
                if (!isActive)
                {
                    color = (Color)values[0];
                }
            }
            return new SolidColorBrush(color);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}