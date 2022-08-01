using System.Windows.Controls;
using System.Windows.Media;

namespace Achievers.ControlCustom
{
    public class ClipBorder : Border
    {
        public ClipBorder()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VisualBrush visual = new VisualBrush();
            visual.Visual = new Border() { Width = ActualWidth, Height = ActualHeight, CornerRadius = CornerRadius, Background = Brushes.Black, BorderThickness = new System.Windows.Thickness(1) };
            OpacityMask = visual;
        }
    }
}