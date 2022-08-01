using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views.Shared.DragEachWord
{
    /// <summary>
    /// Interaction logic for LeftButton.xaml
    /// </summary>
    public partial class LeftPart : UserControl
    {
        public LeftPart()
        {
            InitializeComponent();
        }

        public string image_url
        {
            get { return (string)GetValue(image_urlProperty); }
            set { SetValue(image_urlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for image_url.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty image_urlProperty =
            DependencyProperty.Register("image_url", typeof(string), typeof(LeftPart), new PropertyMetadata(string.Empty));
    }
}