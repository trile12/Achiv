using Achievers.Models;
using System;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public abstract class IGameActivity : BaseViewModel
    {
        public string MainStartColor;
        public string MainEndColor;
        public string SubStartColor;
        public string SubEndColor;
        public string ImageStar;
        public string ImageIcon;
        public bool IsFinish;
        public int MinProgress = 70;
        public GameModel GameModel;

        public abstract double GetScore();

        public bool IsGoodJob()
        {
            return GetScore() >= MinProgress;
        }

        public virtual string GetResultTitle()
        {
            return IsGoodJob() ? "Good job!" : "Oh, no!";
        }

        public virtual string GetResultDescription()
        {
            return IsGoodJob() ? string.Format("You've got #b{0}%b# of the correct answer", Math.Floor(GetScore())) : string.Format("You've only got #b{0}%b# of the correct answer", Math.Floor(GetScore()));
        }

        public virtual UIElement GetResultUI()
        {
            return null;
        }
    }
}