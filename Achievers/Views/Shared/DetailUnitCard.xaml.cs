using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for DetailUnitCard.xaml
    /// </summary>
    public partial class DetailUnitCard : Border
    {
        public DetailUnitCard()
        {
            InitializeComponent();
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(DetailUnitCard), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DetailUnitCard), new PropertyMetadata(string.Empty));
    }
}