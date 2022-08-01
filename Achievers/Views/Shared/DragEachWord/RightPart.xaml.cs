using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views.Shared.DragEachWord
{
    /// <summary>
    /// Interaction logic for RightButton.xaml
    /// </summary>
    public partial class RightPart : UserControl
    {
        public RightPart()
        {
            InitializeComponent();
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(RightPart), new PropertyMetadata(0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RightPart), new PropertyMetadata(string.Empty));

        public Point CurPoint
        {
            get { return (Point)GetValue(CurPointProperty); }
            set { SetValue(CurPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurPointProperty =
            DependencyProperty.Register("CurPoint", typeof(Point), typeof(RightPart), new PropertyMetadata(new Point()));
    }
}