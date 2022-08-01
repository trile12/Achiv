using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for ButtonCard.xaml
    /// </summary>
    public partial class ButtonCard : UserControl
    {
        public ButtonCard()
        {
            InitializeComponent();
        }

        #region Event OnClick

        public static readonly RoutedEvent OnClickEvent = EventManager.RegisterRoutedEvent("OnClick", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ButtonCard));

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

        private void BounceButton_OnClick(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton) { RoutedEvent = OnClickEvent });
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ButtonCard), new PropertyMetadata(string.Empty));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ButtonCard), new PropertyMetadata(null));

        public Color ColorTitle
        {
            get { return (Color)GetValue(ColorTitleProperty); }
            set { SetValue(ColorTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorTitleProperty =
            DependencyProperty.Register("ColorTitle", typeof(Color), typeof(ButtonCard), new PropertyMetadata(Colors.White));

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(ButtonCard), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(ButtonCard), new PropertyMetadata(Colors.White));
    }
}