using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CircleProgressBarTimer.xaml
    /// </summary>
    public partial class CircleProgressBarTimer : UserControl
    {
        private Timer timer = null;
        private int timeCount = 0;
        private Brush color = (Brush)new BrushConverter().ConvertFromString("#4E4CD4");
        private Brush warnColor = (Brush)new BrushConverter().ConvertFromString("#D44942");
        private Storyboard storyboard;

        #region Dependency Property

        public int Time
        {
            get { return (int)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(int), typeof(CircleProgressBarTimer), new PropertyMetadata(0, OnTimeChanged));

        private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as CircleProgressBarTimer;
            if (el != null && el.AutoStart)
            {
                el.Start((int)e.NewValue);
            }
        }

        #endregion Dependency Property

        #region Event OnTimeOver

        public static readonly RoutedEvent OnTimeOverEvent = EventManager.RegisterRoutedEvent("OnTimeOver", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(CircleProgressBarTimer));

        public event RoutedEventHandler OnTimeOver
        {
            add
            {
                base.AddHandler(OnTimeOverEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnTimeOverEvent, value);
            }
        }

        #endregion Event OnTimeOver

        public int WarnTime { get; set; } = -1;
        public bool AutoStart { get; set; } = true;

        public CircleProgressBarTimer()
        {
            InitializeComponent();
        }

        private void UnLoaded(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
            timer?.Dispose();
        }

        public void Start(int time)
        {
            if (timer == null)
            {
                timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += Timer_Elapsed;
            }
            timeCount = time;
            txtProgress.Text = string.Format("{0}s", timeCount);
            txtProgress.Foreground = color;
            progressBar.SetColor(Color.FromRgb(167, 166, 232), Color.FromRgb(78, 76, 212));
            timer.Stop();
            timer.Start();
            storyboard?.Stop();
            var timeLine = new DoubleAnimation();
            timeLine.From = 100;
            timeLine.To = 0;
            timeLine.Duration = TimeSpan.FromSeconds(time);

            Storyboard.SetTarget(timeLine, progressBar);
            Storyboard.SetTargetProperty(timeLine, new PropertyPath("Percentage"));
            storyboard = new Storyboard();
            storyboard.Children.Add(timeLine);
            storyboard.Begin();
        }

        public void Restart()
        {
            Start(Time);
        }

        public void Stop()
        {
            timer?.Stop();
            timer?.Dispose();
            timer = null;
        }

        public void Pause()
        {
            timer?.Stop();
            if (storyboard != null)
                storyboard.Pause();
        }

        public void Resume()
        {
            timer?.Start();
            if (storyboard != null)
                storyboard.Resume();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeCount--;
            Dispatcher.Invoke(() =>
            {
                txtProgress.Text = string.Format("{0}s", timeCount);
                txtProgress.Foreground = timeCount <= WarnTime ? warnColor : color;
                if (timeCount <= WarnTime)
                {
                    progressBar.SetColor(Color.FromRgb(237, 150, 145), Color.FromRgb(212, 73, 66));
                }
                if (timeCount <= 0)
                {
                    timer.Stop();
                    RaiseEvent(new RoutedEventArgs(OnTimeOverEvent, this));
                }
            });
        }
    }
}