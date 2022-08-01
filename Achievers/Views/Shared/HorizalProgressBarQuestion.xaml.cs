using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for HorizalProgressBarQuestion.xaml
    /// </summary>
    public partial class HorizalProgressBarQuestion : UserControl
    {
        public HorizalProgressBarQuestion()
        {
            InitializeComponent();
        }

        public int CurQuestion
        {
            get { return (int)GetValue(CurQuestionProperty); }
            set { SetValue(CurQuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurQuestionProperty =
            DependencyProperty.Register("CurQuestion", typeof(int), typeof(HorizalProgressBarQuestion), new PropertyMetadata(OnCurQuestionChangedCallBack));

        public int TotalQuestion
        {
            get { return (int)GetValue(TotalQuestionProperty); }
            set { SetValue(TotalQuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalQuestionProperty =
            DependencyProperty.Register("TotalQuestion", typeof(int), typeof(HorizalProgressBarQuestion), new PropertyMetadata(0));

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(HorizalProgressBarQuestion), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(HorizalProgressBarQuestion), new PropertyMetadata(Colors.White));

        private void Animate(double cur, double total)
        {
            double width = parent_border.ActualWidth;
            double from = progress_bar.Width;
            double to = (width * cur) / total;

            var timeLine = new DoubleAnimation();
            timeLine.From = from;
            timeLine.To = to;
            timeLine.Duration = new TimeSpan(0, 0, 0, 0, 800);

            progress_bar.BeginAnimation(WidthProperty, timeLine);
        }

        private static void OnCurQuestionChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as HorizalProgressBarQuestion;
            var total = el.TotalQuestion;
            if (total > 0)
            {
                Border parent_border = el.parent_border;
                Border progress_bar = el.progress_bar;
                double width = parent_border.Width;
                double from = progress_bar.Width;
                double to = (width * (double.Parse(e.NewValue.ToString()))) / total;
                var timeLine = new DoubleAnimation();
                timeLine.From = from;
                timeLine.To = to;
                timeLine.Duration = new TimeSpan(0, 0, 0, 0, 800);
                progress_bar.BeginAnimation(WidthProperty, timeLine);
            }
        }
    }
}