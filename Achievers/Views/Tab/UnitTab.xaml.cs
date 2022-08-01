using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.ViewModels;
using Achievers.Views.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Achievers.Views.Tab
{
    /// <summary>
    /// Interaction logic for UnitTab.xaml
    /// </summary>
    public partial class UnitTab : Page
    {
        private GameModel gameModel;
        private ScrollViewer MainScroll;
        private UnitTabViewModel viewModel;

        public UnitTab(ScrollViewer scrollViewer)
        {
            InitializeComponent();
            DataContext = new UnitTabViewModel()
            {
                LstUnit = App.Units
            };

            viewModel = DataContext as UnitTabViewModel;
            MainScroll = scrollViewer;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.IsExpanded = true;
            bdProfile.Height = 80;
            App.navigationService = NavigationService;
        }

        private void listBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset + 40);
            }
            else if (e.Delta > 0)
            {
                MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset - 40);
            }
        }

        private async void OnUnitClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is UnitTabModel data)
            {
                if (!data.IsUnlock)
                {
                    ShowPopupPurchase();
                }
                else
                {
                    UnitTabQuery query = new UnitTabQuery();
                    var tempList = await query.GetAllReviewByUnit(data.Id);
                    viewModel.Unit = data;
                    viewModel.LstReview = new ObservableCollection<UnitTabReviewModel>(tempList.Select((m, mIndex) => new UnitTabReviewModel()
                    {
                        Index = mIndex,
                        Id = m.id,
                        Name = m.name,
                        IdUnit = data.Id,
                        ImageUrl = m.image_url,
                        Progress = m.progress,
                        MinProgress = m.min_progress,
                        IsLocked = m.min_progress >= 0 && tempList.Any(g => g.min_progress == 0 && (g.progress ?? 0) < m.min_progress),
                    }));
                    App.Reviews = viewModel.LstReview;
                    foreach (var item in viewModel.LstUnit)
                    {
                        item.IsActive = false;
                    }
                    data.IsActive = true;
                }
            }
        }

        private void ShowPopupPurchase()
        {
            PopupManager.ShowPopup();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            //begin : 80
            //end:
            //bdProfile.RenderTransform = new ScaleTransform(1, 1);
            //ScaleTransform scale = bdProfile.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(viewModel.IsExpanded ? 1080 : 80, new TimeSpan(0, 0, 0, 0, 300));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            //bdProfile.BeginAnimation(OpacityProperty, timeLine);
            bdProfile.BeginAnimation(Border.HeightProperty, timeLine);

            if (viewModel.IsExpanded)
            {
                bdProfile.Margin = new Thickness(0, 0, 0, 0);
                CircleProgressBarManager.DoProgress();
            }
            else
            {
                bdProfile.Margin = new Thickness(0, 0, 0, 0);
                //CircleProgressBarManager.DoProgress();
                //CardUnitItemManager.DoProgressCardItem();
            }

            viewModel.IsExpanded = !viewModel.IsExpanded;
        }

        private async void Review_Click(object sender, MouseButtonEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as UnitTabReviewModel;
            if (data.IsLocked) return;
            int index = viewModel.UnitIndex;
            //gameModel = await HttpService.GetGameModelById(data.Id);

            if (Common.Ping())
            {
                gameModel = await HttpService.GetGameModelById(data.Id);
            }
            else
            {
                gameModel = await DbService.GetGameModel(data.Id);
            }
            NavigationService?.Navigate(new ReviewPage(gameModel));
        }

        private void OnBoxHintClose(object sender, MouseButtonEventArgs e)
        {
            boxHint.Visibility = Visibility.Collapsed;
        }
    }
}