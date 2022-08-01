using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for ButtonWord.xaml
    /// </summary>
    public partial class ButtonWord : Border
    {
        public ButtonWord()
        {
            InitializeComponent();
        }

        public string Review
        {
            get { return (string)GetValue(ReviewProperty); }
            set { SetValue(ReviewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Review.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReviewProperty =
            DependencyProperty.Register("Review", typeof(string), typeof(ButtonWord), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ButtonWord), new PropertyMetadata(string.Empty));

        public Color Foregrounddp
        {
            get { return (Color)GetValue(ForegrounddpProperty); }
            set { SetValue(ForegrounddpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foregrounddp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegrounddpProperty =
            DependencyProperty.Register("Foregrounddp", typeof(Color), typeof(ButtonWord), new PropertyMetadata(Colors.White));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ButtonWord), new PropertyMetadata(false));

        public bool IsPlaced
        {
            get { return (bool)GetValue(IsPlacedProperty); }
            set { SetValue(IsPlacedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaced.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlacedProperty =
            DependencyProperty.Register("IsPlaced", typeof(bool), typeof(ButtonWord), new PropertyMetadata(false));

        public bool IsWrong
        {
            get { return (bool)GetValue(IsWrongProperty); }
            set { SetValue(IsWrongProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsWrong.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsWrongProperty =
            DependencyProperty.Register("IsWrong", typeof(bool), typeof(ButtonWord), new PropertyMetadata(false));

        public int fontSize
        {
            get { return (int)GetValue(fontSizeProperty); }
            set { SetValue(fontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fontSizeProperty =
            DependencyProperty.Register("fontSize", typeof(int), typeof(ButtonWord), new PropertyMetadata(20));

        public double BaseX { get; set; }
        public double BaseY { get; set; }
    }
}