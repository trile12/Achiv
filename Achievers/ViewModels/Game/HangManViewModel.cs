using Achievers.Models;
using Achievers.Views.GameResult;
using System.Collections.Generic;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public class HangManViewModel : IGameActivity
    {
        public HangManViewModel(GameModel gameModel)
        {
            MainStartColor = "#A1E5CD";
            MainEndColor = "#44B88F";
            SubStartColor = "#A1E5CD";
            SubEndColor = "#44B88F";
            ImageStar = "/Achievers;component/Assets/Images/game-result-star-4.png";
            ImageIcon = "/Achievers;component/Assets/Images/ft-r4-g.png";
            IsFinish = gameModel.next_unit_review == null;
            GameModel = gameModel;
            Instruction = gameModel.instruction;
            TotalQuestion = gameModel.total_question;
            Score = 100;
            Title = "Excellent!";
            Descrition = "You've completed the unit.";
        }

        private string instruction;

        public string Instruction
        {
            get { return instruction; }
            set
            {
                if (instruction != value)
                {
                    instruction = value;
                    OnPropertyChanged();
                }
            }
        }

        private string hint;

        public string Hint
        {
            get { return hint; }
            set
            {
                hint = value;
                OnPropertyChanged();
            }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
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

        private List<QuestionAnwser> questions;

        public List<QuestionAnwser> Questions
        {
            get { return questions; }
            set { questions = value; }
        }

        public double Score { get; set; }
        public string Title { get; set; }
        public string Descrition { get; set; }

        public override double GetScore()
        {
            return Score;
        }

        public override string GetResultTitle()
        {
            return Title;
        }

        public override string GetResultDescription()
        {
            return Descrition;
        }

        public override UIElement GetResultUI()
        {
            return new HangManResult(this);
        }
    }
}