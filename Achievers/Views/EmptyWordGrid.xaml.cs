using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views
{
    /// <summary>
    /// Interaction logic for EmptyWordGrid.xaml
    /// </summary>
    public partial class EmptyWordGrid : UserControl
    {
        public EmptyWordGrid()
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
            DependencyProperty.Register("Title", typeof(string), typeof(EmptyWordGrid), new PropertyMetadata(string.Empty));

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Detail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetailProperty =
            DependencyProperty.Register("Detail", typeof(string), typeof(EmptyWordGrid), new PropertyMetadata(string.Empty));
    }
}