using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achievers.ControlCustom
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class CircleProgressBar : UserControl
    {
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(CircleProgressBar), new PropertyMetadata(Double.NaN, OnPercentageChanged));

        private static void OnPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as CircleProgressBar;
            if (el.progress != null)
            {
                el.SetPercentage((double)e.NewValue);
            }
        }

        public CircleProgressBar()
        {
            InitializeComponent();
        }

        public void SetPercentage(double value)
        {
            double total = (progress.Width - progress.StrokeThickness) * Math.PI / progress.StrokeThickness;
            progress.StrokeDashCap = value == 0 ? PenLineCap.Flat : PenLineCap.Round;
            progress.StrokeDashArray = new DoubleCollection { value * total / 100, total };
        }

        public void SetColor(Color start, Color end)
        {
            var gradient = progress.Stroke as LinearGradientBrush;
            if (gradient == null) return;
            gradient.GradientStops[0].Color = start;
            gradient.GradientStops[1].Color = end;
        }
    }
}