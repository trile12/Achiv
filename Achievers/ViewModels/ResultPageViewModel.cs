using Achievers.ViewModels.Game;
using System.Windows;

namespace Achievers.ViewModels
{
    public class ResultPageViewModel : BaseViewModel
    {
        public ResultPageViewModel(IGameActivity game)
        {
            Title = game.GetResultTitle();
            Description = game.GetResultDescription();
            ImageIcon = game.ImageIcon;
            ImageStar = game.ImageStar;
            MainStartColor = game.MainStartColor;
            MainEndColor = game.MainEndColor;
            ResultUI = game.GetResultUI();
            IsSeeResult = ResultUI != null;
            IsGoodJob = game.IsGoodJob();
            Percent = game.GetScore();
            IsFinish = game.IsFinish;
            if (game.IsFinish && !IsGoodJob)
            {
                IsFinish = false;
            }
            if (IsGoodJob)
            {
                SubStartColor = game.SubStartColor;
                SubEndColor = game.SubEndColor;
            }
            NextReviewName = game.GameModel.next_unit_review?.name;
            Game = game;
            App.UpdateProgress(Game.GameModel.id, Percent);
            if (!IsFinish && Game.GameModel.next_unit_review != null)
            {
                IsShowNextReview = App.CheckIfReviewIsLock(Game.GameModel.next_unit_review.id);
            }
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageIcon { get; set; }
        public string ImageStar { get; set; }
        public double Percent { get; set; }
        public string MainStartColor { get; set; }
        public string MainEndColor { get; set; }
        public string SubStartColor { get; set; } = "#FCF2F2";
        public string SubEndColor { get; set; } = "#F7D1D0";
        public bool IsFinish { get; set; }
        public bool IsGoodJob { get; set; }
        public bool IsShowNextReview { get; set; }
        public string NextReviewName { get; set; }
        public bool IsSeeResult { get; set; }
        public UIElement ResultUI { get; set; }
        public IGameActivity Game { get; set; }
    }
}