using Achievers.EasingCustom;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.ControlCustom
{
    public class Draggable : Border
    {
        #region Event OnDropEnd

        public static readonly RoutedEvent OnDropEndEvent = EventManager.RegisterRoutedEvent("OnDropEnd", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(Draggable));

        public event MouseButtonEventHandler OnDropEnd
        {
            add
            {
                base.AddHandler(OnDropEndEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnDropEndEvent, value);
            }
        }

        #endregion Event OnDropEnd

        #region Event OnDragging

        public static readonly RoutedEvent OnDraggingEvent = EventManager.RegisterRoutedEvent("OnDragging", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(Draggable));

        public event MouseEventHandler OnDragging
        {
            add
            {
                base.AddHandler(OnDraggingEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnDraggingEvent, value);
            }
        }

        #endregion Event OnDragging

        #region Event OnBeginDrag

        public static readonly RoutedEvent OnBeginDragEvent = EventManager.RegisterRoutedEvent("OnBeginDrag", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(Draggable));

        public event MouseButtonEventHandler OnBeginDrag
        {
            add
            {
                base.AddHandler(OnBeginDragEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnBeginDragEvent, value);
            }
        }

        #endregion Event OnBeginDrag

        #region Dependency Property

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Draggable), new PropertyMetadata(false));

        public bool IsHorizontalEnable
        {
            get { return (bool)GetValue(IsHorizontalEnableProperty); }
            set { SetValue(IsHorizontalEnableProperty, value); }
        }

        public static readonly DependencyProperty IsHorizontalEnableProperty =
            DependencyProperty.Register("IsHorizontalEnable", typeof(bool), typeof(Draggable), new PropertyMetadata(true));

        public bool IsVerticalEnable
        {
            get { return (bool)GetValue(IsVerticalEnableProperty); }
            set { SetValue(IsVerticalEnableProperty, value); }
        }

        public static readonly DependencyProperty IsVerticalEnableProperty =
            DependencyProperty.Register("IsVerticalEnable", typeof(bool), typeof(Draggable), new PropertyMetadata(true));

        #endregion Dependency Property

        #region Property Notification

        private Point locationPosition = new Point();

        public Point LocationPosition
        {
            get => locationPosition;
        }

        private DraggableStatus status = DraggableStatus.END_DRAG;

        public DraggableStatus Status
        {
            get => status;
        }

        private Point offsetFromParent = new Point();

        public Point OffsetFromParent
        {
            get => offsetFromParent;
        }

        #endregion Property Notification

        public Draggable()
        {
            this.Cursor = Cursors.Hand;
            this.MouseLeftButtonDown += Draggable_MouseLeftButtonDown;
            this.MouseMove += Draggable_MouseMove;
            this.MouseLeftButtonUp += Draggable_MouseLeftButtonUp;
            this.Loaded += Draggable_Loaded;
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform());
            transformGroup.Children.Add(new RotateTransform());
            transformGroup.Children.Add(new TranslateTransform());
            RenderTransform = transformGroup;
        }

        private void Draggable_Loaded(object sender, RoutedEventArgs e)
        {
            offsetFromParent = TransformToVisual(GetParent() as FrameworkElement).Transform(new Point(0, 0));
        }

        public DependencyObject GetParent()
        {
            if (Parent != null)
            {
                return Parent;
            }
            else if (VisualParent != null)
            {
                return VisualParent;
            }
            return null;
        }

        public Point GetPosition()
        {
            var pos = RenderTransform.Transform(new Point(0, 0));
            return pos;
        }

        public void AnimatedToPosition(Point pos, int duration = 300, Action<Draggable> complete = null)
        {
            var a = (RenderTransform as TransformGroup);
            var translate = (RenderTransform as TransformGroup).Children[2] as TranslateTransform;
            var timeLineX = new DoubleAnimation()
            {
                To = pos.X,
                Duration = new TimeSpan(0, 0, 0, 0, duration),
                EasingFunction = new BezierEase()
            };
            var timeLineY = new DoubleAnimation()
            {
                To = pos.Y,
                Duration = new TimeSpan(0, 0, 0, 0, duration),
                EasingFunction = new BezierEase()
            };
            if (complete != null)
            {
                timeLineY.Completed += (s, e) =>
                {
                    complete.Invoke(this);
                };
            }
            translate.BeginAnimation(TranslateTransform.XProperty, timeLineX);
            translate.BeginAnimation(TranslateTransform.YProperty, timeLineY);
        }

        private void Draggable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1) return;
            status = DraggableStatus.BEGIN_DRAG;
            locationPosition = e.GetPosition(this);
            CaptureMouse();
            RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton) { RoutedEvent = OnBeginDragEvent });
        }

        private void Draggable_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseCaptured) return;
            status = DraggableStatus.DRAGGING;
            Point pos = e.GetPosition(GetParent() as FrameworkElement);
            if (IsHorizontalEnable) pos.X -= LocationPosition.X + offsetFromParent.X;
            else pos.X = GetPosition().X;

            if (IsVerticalEnable) pos.Y -= LocationPosition.Y + offsetFromParent.Y;
            else pos.Y = GetPosition().Y;

            AnimatedToPosition(pos, 0);
            RaiseEvent(new MouseEventArgs(e.MouseDevice, e.Timestamp) { RoutedEvent = OnDraggingEvent });
        }

        private void Draggable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured) return;
            status = DraggableStatus.END_DRAG;
            ReleaseMouseCapture();
            RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton) { RoutedEvent = OnDropEndEvent });
            locationPosition.X = 0;
            locationPosition.Y = 0;
        }
    }

    public enum DraggableStatus
    {
        BEGIN_DRAG,
        DRAGGING,
        END_DRAG
    }
}