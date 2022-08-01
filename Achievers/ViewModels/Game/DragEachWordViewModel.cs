using Achievers.Models;
using Achievers.Views.GameResult;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public class DragEachWordViewModel : IGameActivity
    {
        public DragEachWordViewModel(GameModel gameModel)
        {
            MainStartColor = "#A5E4F2";
            MainEndColor = "#4FBEDB";
            SubStartColor = "#EFFAFC";
            SubEndColor = "#D4F4FA";
            ImageStar = "/Achievers;component/Assets/Images/game-result-star-1.png";
            ImageIcon = "/Achievers;component/Assets/Images/ft-r1-g.png";
            IsFinish = gameModel.next_unit_review == null;
            GameModel = gameModel;

            Instruction = gameModel.instruction;
            TotalQuestion = gameModel.total_question;
            UserAnswers = new List<DragEachWordAnswer>[gameModel.total_question];
        }

        private string instruction;

        public string Instruction
        {
            get { return instruction; }
            set
            {
                instruction = value;
                OnPropertyChanged();
            }
        }

        private int curIndex;

        public int CurIndex
        {
            get { return curIndex; }
            set
            {
                curIndex = value;
                OnPropertyChanged();
            }
        }

        private int totalQuestion;

        public int TotalQuestion
        {
            get { return totalQuestion; }
            set
            {
                totalQuestion = value;
                OnPropertyChanged();
            }
        }

        private bool isCorrect;

        public bool IsCorrect
        {
            get { return isCorrect; }
            set
            {
                isCorrect = value;
                OnPropertyChanged();
            }
        }

        public List<DragEachWordAnswer>[] UserAnswers { get; set; }

        public override double GetScore()
        {
            return (double)UserAnswers.Sum(m => m.Count(g => g.IsCorrect)) / UserAnswers.Sum(m => m.Count) * 100;
        }

        public override UIElement GetResultUI()
        {
            return new DragEachWordResult(this);
        }
    }

    public class DragEachWordAnswer
    {
        public int Index { get; set; }
        public bool IsConnected { get; set; }
        public bool IsCorrect { get; set; }
    }
}