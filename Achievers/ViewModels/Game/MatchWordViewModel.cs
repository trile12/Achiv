using Achievers.Models;
using Achievers.Views.GameResult;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public class MatchWordViewModel : IGameActivity
    {
        public MatchWordViewModel(GameModel gameModel)
        {
            MainStartColor = "#F2B28A";
            MainEndColor = "#E07031";
            SubStartColor = "#FCF4EE";
            SubEndColor = "#FAE0CD";
            ImageStar = "/Achievers;component/Assets/Images/game-result-star-2.png";
            ImageIcon = "/Achievers;component/Assets/Images/ft-r2-g.png";
            IsFinish = gameModel.next_unit_review == null;
            GameModel = gameModel;

            Title = gameModel.name;
            Instruction = gameModel.instruction;
            TotalQuestion = gameModel.total_question;
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
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

        private List<QuestionMatchWord> questions;

        public List<QuestionMatchWord> Questions
        {
            get { return questions; }
            set { questions = value; }
        }

        public override double GetScore()
        {
            return (double)Questions.Sum(m => m.questionAnwsers.Count(g => g.isCorrect)) / Questions.Sum(m => m.questionAnwsers.Count) * 100;
        }

        public override UIElement GetResultUI()
        {
            return new MatchWordResult(this);
        }
    }

    public class QuestionMatchWord
    {
        public int number { get; set; }
        public List<QuestionAnwser> questionAnwsers { get; set; }
    }
}