using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CircleProgressProfile.xaml
    /// </summary>
    public partial class CircleProgressProfile : UserControl
    {
        public CircleProgressProfile()
        {
            InitializeComponent();
        }

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(CircleProgressProfile), new PropertyMetadata(70));

        public string Images
        {
            get { return (string)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Images.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(string), typeof(CircleProgressProfile), new PropertyMetadata(string.Empty));

        public bool IsGood
        {
            get { return (bool)GetValue(IsGoodProperty); }
            set { SetValue(IsGoodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGood.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGoodProperty =
            DependencyProperty.Register("IsGood", typeof(bool), typeof(CircleProgressProfile), new PropertyMetadata(false));

        public int Percent
        {
            get { return (int)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register("Percent", typeof(int), typeof(CircleProgressProfile), new PropertyMetadata(OnPercentChangedCallBack));

        private static void OnPercentChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as CircleProgressProfile;
            Animate(double.Parse(e.OldValue.ToString()), double.Parse(e.NewValue.ToString()), el.progressBar);
        }

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(CircleProgressProfile), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(CircleProgressProfile), new PropertyMetadata(Colors.White));

        public static void Animate(double old_percent, double percent, CircleProgressTimer progressBar)
        {
            double t1 = (percent * 0.6);

            var storyboard = new Storyboard();

            var timeLine = new DoubleAnimation();
            timeLine.From = old_percent;
            timeLine.To = t1;
            timeLine.Duration = new TimeSpan(0, 0, 0, 0, 700);

            timeLine.Completed += (s, e) =>
            {
                timeLine.FillBehavior = FillBehavior.Stop;
            };

            Storyboard.SetTarget(timeLine, progressBar);
            Storyboard.SetTargetProperty(timeLine, new PropertyPath("Percentage"));

            storyboard.Children.Add(timeLine);

            var timeLine2 = new DoubleAnimation();
            timeLine2.BeginTime = TimeSpan.FromMilliseconds(700);
            timeLine2.From = t1;
            timeLine2.To = percent;
            timeLine2.Duration = new TimeSpan(0, 0, 0, 1, 500);

            timeLine2.Completed += (s, e) =>
            {
                timeLine2.FillBehavior = FillBehavior.Stop;
            };

            Storyboard.SetTarget(timeLine2, progressBar);
            Storyboard.SetTargetProperty(timeLine2, new PropertyPath("Percentage"));

            storyboard.Children.Add(timeLine2);

            storyboard.Begin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}