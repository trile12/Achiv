using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Achievers.ControlCustom
{
    /// <summary>
    /// Interaction logic for RecordButton.xaml
    /// </summary>
    public partial class RecordButton : UserControl
    {
        #region Event OnClick

        public static readonly RoutedEvent OnRecordEvent = EventManager.RegisterRoutedEvent("OnRecord", RoutingStrategy.Bubble, typeof(DataRoutedEventHandler), typeof(RecordButton));

        public event DataRoutedEventHandler OnRecord
        {
            add
            {
                base.AddHandler(OnRecordEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnRecordEvent, value);
            }
        }

        #endregion Event OnClick

        private bool isAnimating = false;
        private Storyboard storyBoard;

        public RecordButton()
        {
            InitializeComponent();
            storyBoard = FindResource("storyBoard") as Storyboard;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Tag.ToString() != "active")
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        public void Start()
        {
            if (!isAnimating) storyBoard.Begin();
            Tag = "active";
            isAnimating = true;
            RaiseEvent(new DataRoutedEventArgs() { RoutedEvent = OnRecordEvent, Data = true });
        }

        public void Stop()
        {
            Tag = "inactive";
            RaiseEvent(new DataRoutedEventArgs() { RoutedEvent = OnRecordEvent, Data = false });
        }

        private void OnAnimatedCompleted(object sender, EventArgs e)
        {
            if (Tag.ToString() == "active")
            {
                storyBoard.Seek(new TimeSpan(0));
            }
            else
            {
                isAnimating = false;
            }
        }
    }
}