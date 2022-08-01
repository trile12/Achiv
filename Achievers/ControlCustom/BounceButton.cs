using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.ControlCustom
{
    public partial class BounceButton : Border, INotifyPropertyChanged
    {
        #region Event OnClick

        public static readonly RoutedEvent OnClickEvent = EventManager.RegisterRoutedEvent("OnClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(BounceButton));

        public event MouseButtonEventHandler OnClick
        {
            add
            {
                base.AddHandler(OnClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnClickEvent, value);
            }
        }

        #endregion Event OnClick

        #region Property Notification

        private bool isClick;

        public bool IsClick
        {
            get => isClick;
            protected set
            {
                if (isClick != value)
                {
                    isClick = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Property Notification

        public BounceButton()
        {
            this.Cursor = Cursors.Hand;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.RenderTransform = new ScaleTransform(1, 1);
            this.MouseLeftButtonDown += OnMouseLeftButtonDown;
            this.MouseLeftButtonUp += OnMouseLeftButtonUp;
            this.MouseLeave += OnMouseLeave;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            if (e.ClickCount > 1 || IsClick) return;
            IsClick = true;
            var scale = RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation();
            timeLine.To = 0.90;
            timeLine.Duration = new TimeSpan(0, 0, 0, 0, 600);
            timeLine.EasingFunction = new ElasticEase() { EasingMode = EasingMode.EaseOut };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsClick) return;
            //e.Handled = true;
            var scale = RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation();
            timeLine.To = 1;
            timeLine.Duration = new TimeSpan(0, 0, 0, 0, 600);
            timeLine.EasingFunction = new ElasticEase() { EasingMode = EasingMode.EaseOut };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
            IsClick = false;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            if (!IsClick) return;
            var scale = RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation();
            timeLine.To = 1;
            timeLine.Duration = new TimeSpan(0, 0, 0, 0, 600);
            timeLine.EasingFunction = new ElasticEase() { EasingMode = EasingMode.EaseOut };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
            RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton) { RoutedEvent = OnClickEvent });
            IsClick = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}