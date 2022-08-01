using Achievers.Models;
using Achievers.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for ProfileCard.xaml
    /// </summary>
    public partial class ProfileCard : UserControl
    {
        public ProfileCard()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ProfileCard), new PropertyMetadata(string.Empty));

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Unit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(ProfileCard), new PropertyMetadata(string.Empty));

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(ProfileCard), new PropertyMetadata(Colors.White));

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(ProfileCard), new PropertyMetadata(Colors.White));

        public Color UnitBorderColor
        {
            get { return (Color)GetValue(UnitBorderColorProperty); }
            set { SetValue(UnitBorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnitBorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitBorderColorProperty =
            DependencyProperty.Register("UnitBorderColor", typeof(Color), typeof(ProfileCard), new PropertyMetadata(Colors.White));

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(ProfileCard), new PropertyMetadata(false));

        private async void CircleProgressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Reviews = new ObservableCollection<UnitTabReviewModel>((DataContext as UnitTabModel).lstReview);
            int id = (sender as CircleProgressBar).Id;
            //GameModel gameModel = await HttpService.GetGameModelById(id);

            GameModel gameModel;
            if (Common.Ping())
            {
                gameModel = await HttpService.GetGameModelById(id);
            }
            else
            {
                gameModel = await DbService.GetGameModel(id);
            }

            App.navigationService.Navigate(new ReviewPage(gameModel));
        }
    }
}