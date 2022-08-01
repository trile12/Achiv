using Achievers.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Achievers
{
    public static class ScrollAnimationBehavior
    {
        #region VerticalOffset Property

        public static DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset",
                                                typeof(double),
                                                typeof(ScrollAnimationBehavior),
                                                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static void SetVerticalOffset(FrameworkElement target, double value)
        {
            target.SetValue(VerticalOffsetProperty, value);
        }

        public static double GetVerticalOffset(FrameworkElement target)
        {
            return (double)target.GetValue(VerticalOffsetProperty);
        }

        #endregion VerticalOffset Property

        #region IntendedLocation Property

        public static DependencyProperty IntendedLocationProperty =
            DependencyProperty.RegisterAttached("IntendedLocation",
                                                typeof(double),
                                                typeof(ScrollAnimationBehavior),
                                                new UIPropertyMetadata(0.0));

        public static void SetIntendedLocation(FrameworkElement target, double value)
        {
            if (target == null)
                return;
            target.SetValue(IntendedLocationProperty, value);
        }

        public static double GetIntendedLocation(FrameworkElement target)
        {
            return (double)target.GetValue(IntendedLocationProperty);
        }

        #endregion IntendedLocation Property

        #region TimeDuration Property

        public static DependencyProperty TimeDurationProperty =
            DependencyProperty.RegisterAttached("TimeDuration",
                                                typeof(TimeSpan),
                                                typeof(ScrollAnimationBehavior),
                                                new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 200)));

        public static void SetTimeDuration(FrameworkElement target, TimeSpan value)
        {
            target.SetValue(TimeDurationProperty, value);
        }

        public static TimeSpan GetTimeDuration(FrameworkElement target)
        {
            return (TimeSpan)target.GetValue(TimeDurationProperty);
        }

        #endregion TimeDuration Property

        #region PointsToScroll Property

        public static DependencyProperty PointsToScrollProperty =
            DependencyProperty.RegisterAttached("PointsToScroll",
                                                typeof(double),
                                                typeof(ScrollAnimationBehavior),
                                                new PropertyMetadata(32.0));

        public static void SetPointsToScroll(FrameworkElement target, double value)
        {
            target.SetValue(PointsToScrollProperty, value);
        }

        public static double GetPointsToScroll(FrameworkElement target)
        {
            return (double)target.GetValue(PointsToScrollProperty);
        }

        #endregion PointsToScroll Property

        #region OnVerticalOffset Changed

        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scrollViewer = target as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        #endregion OnVerticalOffset Changed

        #region IsEnabled Property

        public static DependencyProperty IsEnabledProperty =
                                                DependencyProperty.RegisterAttached("IsEnabled",
                                                typeof(bool),
                                                typeof(ScrollAnimationBehavior),
                                                new UIPropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(FrameworkElement target, bool value)
        {
            target.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(FrameworkElement target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        #endregion IsEnabled Property

        #region OnIsEnabledChanged Changed

        private static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = sender;

            if (target != null && target is ScrollViewer)
            {
                ScrollViewer scroller = target as ScrollViewer;
                scroller.Loaded += new RoutedEventHandler(scrollerLoaded);
            }

            if (target != null && target is ListBox)
            {
                ListBox listbox = target as ListBox;
                listbox.Loaded += new RoutedEventHandler(listboxLoaded);
            }
        }

        #endregion OnIsEnabledChanged Changed

        #region AnimateScroll Helper

        public static void AnimateScroll(ScrollViewer scrollViewer, double ToValue)
        {
            if (scrollViewer == null)
                return;
            SetIntendedLocation(scrollViewer, ToValue);
            scrollViewer.BeginAnimation(VerticalOffsetProperty, null);
            DoubleAnimation verticalAnimation = new DoubleAnimation();
            verticalAnimation.From = scrollViewer.VerticalOffset;
            verticalAnimation.To = ToValue;
            verticalAnimation.Duration = new Duration(GetTimeDuration(scrollViewer));
            scrollViewer.BeginAnimation(VerticalOffsetProperty, verticalAnimation);
        }

        public static void AnimateScroll_v2(ScrollViewer scrollViewer, double ToValue)
        {
            if (scrollViewer == null)
                return;
            DoubleAnimation verticalAnimation = new DoubleAnimation();

            verticalAnimation.From = scrollViewer.VerticalOffset;
            verticalAnimation.To = ToValue;
            verticalAnimation.Duration = new Duration(GetTimeDuration(scrollViewer));

            Storyboard storyboard = new Storyboard();

            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, scrollViewer);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(ScrollAnimationBehavior.VerticalOffsetProperty));
            storyboard.Begin();
        }

        #endregion AnimateScroll Helper

        #region NormalizeScrollPos Helper

        private static double NormalizeScrollPos(ScrollViewer scroll, double scrollChange, Orientation o)
        {
            double returnValue = scrollChange;

            if (scrollChange < 0)
            {
                returnValue = 0;
            }

            if (o == Orientation.Vertical && scrollChange > scroll.ScrollableHeight)
            {
                returnValue = scroll.ScrollableHeight;
            }
            else if (o == Orientation.Horizontal && scrollChange > scroll.ScrollableWidth)
            {
                returnValue = scroll.ScrollableWidth;
            }

            return returnValue;
        }

        #endregion NormalizeScrollPos Helper

        #region UpdateScrollPosition Helper

        private static async void UpdateScrollPosition(object sender)
        {
            ListBox listbox = sender as ListBox;

            if (listbox != null && listbox.SelectedIndex > -1)
            {
                var _listBoxScroller = HelpService.FindChildByType<ScrollViewer>(listbox);

                double intendedLocation = (double)GetIntendedLocation(_listBoxScroller);

                double scrollTo = 0;

                for (int i = 0; i <= listbox.SelectedIndex; i++)
                {
                    ListBoxItem tempItem = listbox.ItemContainerGenerator.ContainerFromItem(listbox.Items[i]) as ListBoxItem;

                    while (tempItem == null)
                    {
                        await Task.Delay(100);
                        try
                        {
                            tempItem = listbox.ItemContainerGenerator.ContainerFromItem(listbox.Items[i]) as ListBoxItem;
                        }
                        catch
                        {
                        }
                    }

                    scrollTo += tempItem.ActualHeight;
                }

                var currentItem = listbox.ItemContainerGenerator.ContainerFromItem(listbox.Items[listbox.SelectedIndex]) as ListBoxItem;

                if (scrollTo - currentItem.ActualHeight < _listBoxScroller.VerticalOffset)
                {
                    intendedLocation = scrollTo - currentItem.ActualHeight;
                    AnimateScroll(_listBoxScroller, intendedLocation);
                }
                else if (scrollTo - _listBoxScroller.VerticalOffset > _listBoxScroller.ActualHeight)
                {
                    intendedLocation += scrollTo - _listBoxScroller.VerticalOffset - _listBoxScroller.ActualHeight;
                    AnimateScroll(_listBoxScroller, intendedLocation);
                }
                else
                {
                    SetIntendedLocation(_listBoxScroller, _listBoxScroller.VerticalOffset);
                }
            }
        }

        #endregion UpdateScrollPosition Helper

        #region SetEventHandlersForScrollViewer Helper

        private static void SetEventHandlersForScrollViewer(ScrollViewer scroller)
        {
            scroller.PreviewMouseWheel += new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);
            scroller.PreviewKeyDown += new KeyEventHandler(ScrollViewerPreviewKeyDown);
            scroller.PreviewMouseLeftButtonUp += Scroller_PreviewMouseLeftButtonUp;
        }

        private static void Scroller_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Thumb || e.OriginalSource is RepeatButton)
            {
                var scrollView = (ScrollViewer)sender;
                double intendedLocation = scrollView.VerticalOffset;
                scrollView.SetValue(IntendedLocationProperty, intendedLocation);
            }
        }

        #endregion SetEventHandlersForScrollViewer Helper

        #region scrollerLoaded Event Handler

        private static void scrollerLoaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scroller = sender as ScrollViewer;

            SetEventHandlersForScrollViewer(scroller);
        }

        #endregion scrollerLoaded Event Handler

        #region listboxLoaded Event Handler

        private static void listboxLoaded(object sender, RoutedEventArgs e)
        {
            ListBox listbox = sender as ListBox;

            var _listBoxScroller = HelpService.FindChildByType<ScrollViewer>(listbox);
            SetEventHandlersForScrollViewer(_listBoxScroller);

            SetTimeDuration(_listBoxScroller, GetTimeDuration(listbox));
            SetPointsToScroll(_listBoxScroller, GetPointsToScroll(listbox));
            SetIntendedLocation(_listBoxScroller, GetIntendedLocation(listbox));

            listbox.SelectionChanged += new SelectionChangedEventHandler(ListBoxSelectionChanged);
            listbox.Loaded += new RoutedEventHandler(ListBoxLoaded);
            listbox.LayoutUpdated += new EventHandler(ListBoxLayoutUpdated);
        }

        #endregion listboxLoaded Event Handler

        #region ScrollViewerPreviewMouseWheel Event Handler

        private static void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double mouseWheelChange = (double)e.Delta;
            ScrollViewer scroller = (ScrollViewer)sender;
            var originScroller = e.OriginalSource as ScrollViewer;
            if (originScroller == null && e.OriginalSource is FrameworkElement) originScroller = HelpService.FindAncestorByType<ScrollViewer>(e.OriginalSource as FrameworkElement);
            if (originScroller == null || scroller == originScroller)
            {
                double intendedLocation = GetIntendedLocation(scroller);
                double newVOffset = intendedLocation - (mouseWheelChange * 1.5);
                //We got hit by the mouse again. jump to the offset.
                scroller.ScrollToVerticalOffset(intendedLocation);
                if (newVOffset < 0)
                {
                    newVOffset = 0;
                }
                if (newVOffset > scroller.ScrollableHeight)
                {
                    newVOffset = scroller.ScrollableHeight;
                }

                AnimateScroll(scroller, newVOffset);
                e.Handled = true;
            }
        }

        #endregion ScrollViewerPreviewMouseWheel Event Handler

        #region ScrollViewerPreviewKeyDown Handler

        private static void ScrollViewerPreviewKeyDown(object sender, KeyEventArgs e)
        {
            ScrollViewer scroller = (ScrollViewer)sender;

            Key keyPressed = e.Key;
            double newVerticalPos = GetVerticalOffset(scroller);
            bool isKeyHandled = false;

            if (keyPressed == Key.Down)
            {
                //newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos + GetPointsToScroll(scroller)), Orientation.Vertical);
                //intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }
            else if (keyPressed == Key.PageDown)
            {
                //newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos + scroller.ViewportHeight), Orientation.Vertical);
                //intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }
            else if (keyPressed == Key.Up)
            {
                //newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos - GetPointsToScroll(scroller)), Orientation.Vertical);
                //intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }
            else if (keyPressed == Key.PageUp)
            {
                //newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos - scroller.ViewportHeight), Orientation.Vertical);
                //intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }

            if (newVerticalPos != GetVerticalOffset(scroller))
            {
                //intendedLocation = newVerticalPos;
                //AnimateScroll(scroller, newVerticalPos);
            }

            e.Handled = isKeyHandled;
        }

        #endregion ScrollViewerPreviewKeyDown Handler

        #region ListBox Event Handlers

        private static void ListBoxLayoutUpdated(object sender, EventArgs e)
        {
            UpdateScrollPosition(sender);
        }

        private static void ListBoxLoaded(object sender, RoutedEventArgs e)
        {
            UpdateScrollPosition(sender);
        }

        private static void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateScrollPosition(sender);
        }

        #endregion ListBox Event Handlers
    }
}