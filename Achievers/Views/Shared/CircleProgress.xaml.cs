using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CircleProgressBar.xaml
    /// </summary>
    public partial class CircleProgress : UserControl
    {
        public CircleProgress()
        {
            InitializeComponent();
            Angle = (Percentage * 360) / 100;
            RenderArc();
        }

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Brush SegmentColor
        {
            get { return (Brush)GetValue(SegmentColorProperty); }
            set { SetValue(SegmentColorProperty, value); }
        }

        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(CircleProgress), new PropertyMetadata(65d, new PropertyChangedCallback(OnPercentageChanged)));

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(CircleProgress), new PropertyMetadata(5, new PropertyChangedCallback(OnThicknessChanged)));

        // Using a DependencyProperty as the backing store for SegmentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SegmentColorProperty =
            DependencyProperty.Register("SegmentColor", typeof(Brush), typeof(CircleProgress), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(OnColorChanged)));

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(CircleProgress), new PropertyMetadata(25, new PropertyChangedCallback(OnPropertyChanged)));

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(CircleProgress), new PropertyMetadata(120d, new PropertyChangedCallback(OnPropertyChanged)));

        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircleProgress circle = sender as CircleProgress;
            circle.set_Color((SolidColorBrush)args.NewValue);
        }

        private static void OnThicknessChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircleProgress circle = sender as CircleProgress;
            circle.set_tick((int)args.NewValue);
        }

        private static void OnPercentageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircleProgress circle = sender as CircleProgress;
            if (circle.Percentage > 100) circle.Percentage = 100;
            circle.Angle = (circle.Percentage * 270) / 100;
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircleProgress circle = sender as CircleProgress;
            circle.RenderArc();
        }

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(CircleProgress), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(CircleProgress), new PropertyMetadata(Colors.White));

        public void set_tick(int n)
        {
            pathRoot.StrokeThickness = n;
        }

        public void set_Color(SolidColorBrush n)
        {
            pathRoot.Stroke = n;
        }

        public void RenderArc()
        {
            Point startPoint = new Point(Radius, 0);
            Point endPoint = ComputeCartesianCoordinate(Angle, Radius);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            pathRoot.Width = Radius * 2 + StrokeThickness;
            pathRoot.Height = Radius * 2 + StrokeThickness;
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            bool largeArc = Angle > 180;

            Size outerArcSize = new Size(Radius, Radius);

            pathFigure.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
                endPoint.X -= 0.01;

            arcSegment.Point = endPoint;
            arcSegment.Size = outerArcSize;
            arcSegment.IsLargeArc = largeArc;
        }

        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            double angleRad = (Math.PI / 180) * (angle - 90);
            // convert to radians
            //double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
    }
}