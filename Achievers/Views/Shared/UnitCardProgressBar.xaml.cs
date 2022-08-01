using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for UnitCardProgressBar.xaml
    /// </summary>
    public partial class UnitCardProgressBar : Border
    {
        public UnitCardProgressBar()
        {
            InitializeComponent();
        }

        public void Animate()
        {
            var timeLine = new DoubleAnimation();
            timeLine.From = 0;
            timeLine.To = Percentage;
            timeLine.Duration = new TimeSpan(0, 0, 0, 0, 600);

            timeLine.Completed += (s, e) =>
            {
            };

            Storyboard.SetTarget(timeLine, progressBar);
            Storyboard.SetTargetProperty(timeLine, new PropertyPath("Percentage"));
            Storyboard.SetTarget(timeLine, progressBar);
            var storyboard = new Storyboard();
            storyboard.Children.Add(timeLine);
            storyboard.Begin();
        }

        public int Percentage
        {
            get { return (int)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(int), typeof(UnitCardProgressBar), new PropertyMetadata(0));

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animate();
        }
    }
}