using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CircleProgressBar.xaml
    /// </summary>
    public partial class CircleProgressBar : UserControl
    {
        public CircleProgressBar()
        {
            InitializeComponent();
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            CircleProgressBarManager.InitProgress(progressBar);
            //Animate();
        }

        public void Animate()
        {
            var timeLine = new DoubleAnimation();
            timeLine.From = 0;
            timeLine.To = Percentage;
            timeLine.Duration = new TimeSpan(0, 0, 0, 1);
            //timeLine.RepeatBehavior = RepeatBehavior.Forever;
            //timeLine.AutoReverse = true;
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
            DependencyProperty.Register("Percentage", typeof(int), typeof(CircleProgressBar), new PropertyMetadata(0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CircleProgressBar), new PropertyMetadata(string.Empty));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(CircleProgressBar), new PropertyMetadata(-1));

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(CircleProgressBar), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(CircleProgressBar), new PropertyMetadata(Colors.White));

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(CircleProgressBar), new PropertyMetadata(false));
    }

    public class CircleProgressBarManager
    {
        private static List<CircleProgress> lstProgress = new List<CircleProgress>();

        public static void InitProgress(CircleProgress circleProgress)
        {
            lstProgress.Add(circleProgress);
        }

        public static void DoProgress()
        {
            int i = 1;

            foreach (var item in lstProgress)
            {
                var timeLine = new DoubleAnimation();
                timeLine.From = 0;
                timeLine.To = item.Percentage;
                timeLine.Duration = new TimeSpan(0, 0, 0, 0, 800);
                //timeLine.RepeatBehavior = RepeatBehavior.Forever;
                //timeLine.AutoReverse = true;
                Storyboard.SetTargetProperty(timeLine, new PropertyPath("Percentage"));

                Storyboard.SetTarget(timeLine, item);
                var storyboard = new Storyboard();
                storyboard.Children.Add(timeLine);
                storyboard.Begin();
            }
        }
    }
}