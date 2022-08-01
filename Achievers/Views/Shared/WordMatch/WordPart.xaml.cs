using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views.Shared.WordMatch
{
    /// <summary>
    /// Interaction logic for WordPart.xaml
    /// </summary>
    public partial class WordPart : UserControl
    {
        public WordPart()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WordPart), new PropertyMetadata(string.Empty));

        public string Def
        {
            get { return (string)GetValue(DefProperty); }
            set { SetValue(DefProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Def.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefProperty =
            DependencyProperty.Register("Def", typeof(string), typeof(WordPart), new PropertyMetadata(string.Empty));

        public string Images
        {
            get { return (string)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Images.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(string), typeof(WordPart), new PropertyMetadata(string.Empty));

        public string BookImage
        {
            get { return (string)GetValue(BookImageProperty); }
            set { SetValue(BookImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BookImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BookImageProperty =
            DependencyProperty.Register("BookImage", typeof(string), typeof(WordPart), new PropertyMetadata(string.Empty));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(WordPart), new PropertyMetadata(-1));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(WordPart), new PropertyMetadata(false));
    }
}