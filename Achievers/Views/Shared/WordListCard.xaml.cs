using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for WordListCard.xaml
    /// </summary>
    public partial class WordListCard : UserControl
    {
        public WordListCard()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(WordListCard), new PropertyMetadata(true));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Unit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(WordListCard), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WordListCard), new PropertyMetadata(string.Empty));

        public string TitleVI
        {
            get { return (string)GetValue(TitleVIProperty); }
            set { SetValue(TitleVIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TilteVI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleVIProperty =
            DependencyProperty.Register("TitleVI", typeof(string), typeof(WordListCard), new PropertyMetadata(string.Empty));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(WordListCard), new PropertyMetadata(null));

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(WordListCard), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(WordListCard), new PropertyMetadata(Colors.White));
    }
}