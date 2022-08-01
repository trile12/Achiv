using Achievers.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CardWordListItem.xaml
    /// </summary>
    public partial class CardWordListItem : UserControl
    {
        public CardWordListItem()
        {
            InitializeComponent();
        }

        public ImageSource Images
        {
            get { return (ImageSource)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Images.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ImageSource), typeof(CardWordListItem), new PropertyMetadata(null));

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(CardWordListItem), new PropertyMetadata(false));

        public UnitTabModel WordCardModel
        {
            get { return (UnitTabModel)GetValue(WordCardModelProperty); }
            set { SetValue(WordCardModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WordCardModelProperty =
            DependencyProperty.Register("WordCardModel", typeof(UnitTabModel), typeof(CardWordListItem), new PropertyMetadata(new UnitTabModel()));
    }
}