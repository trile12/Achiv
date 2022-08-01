using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.ViewModels;
using Achievers.ViewModels.Game;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Achievers.Views
{
    /// <summary>
    /// Interaction logic for ReviewResultPage.xaml
    /// </summary>
    public partial class ReviewResultPage : Page
    {
        private ResultPageViewModel viewModel;

        public ReviewResultPage(IGameActivity game)
        {
            DataContext = viewModel = new ResultPageViewModel(game);
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!viewModel.IsGoodJob)
            {
                AudioGame.OhNo();
            }
            else
            {
                AudioGame.GoodJob();
            }
            PopupManager.PopupResultInitContent(viewModel.ResultUI);
            //await HttpService.SendProgress(new
            //{
            //    unit_review_id = viewModel.Game.GameModel.id,
            //    userId = App.UserId,
            //    progress = viewModel.Percent
            //});
            UnitTabQuery query = new UnitTabQuery();
            await query.UpdateReview(viewModel.Game.GameModel.id, (int)viewModel.Percent);
            UnitPageManager.UpdateTotalProgress();
        }

        private void GoBack_Click(object sender, MouseButtonEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                if (NavigationService.Content is UnitPage) return;
                NavigationService.GoBack();
            }
            UnitPageManager.HideBlueBG();
        }

        private void Result_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupResult();
        }

        private async void NextUnit_Click(object sender, MouseButtonEventArgs e)
        {
            GameModel gameModel;
            if (Common.Ping())
            {
                gameModel = await HttpService.GetGameModelById(viewModel.Game.GameModel.next_unit_review.id);
            }
            else
            {
                gameModel = await DbService.GetGameModel(viewModel.Game.GameModel.next_unit_review.id);
            }

            while (App.navigationService.CanGoBack)
            {
                if (App.navigationService.Content is UnitPage) break;
                App.navigationService.GoBack();
            }
            App.navigationService.Navigate(new ReviewPage(gameModel));
        }

        private void TryAgain_Click(object sender, MouseButtonEventArgs e)
        {
            while (App.navigationService.CanGoBack)
            {
                if (App.navigationService.Content is UnitPage) break;
                App.navigationService.GoBack();
            }
            App.navigationService.Navigate(new ReviewPage(viewModel.Game.GameModel));
        }
    }
}