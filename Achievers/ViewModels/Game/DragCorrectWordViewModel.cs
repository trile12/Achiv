using Achievers.Models;
using Achievers.Views.GameResult;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public class DragCorrectWordViewModel : IGameActivity
    {
        public DragCorrectWordViewModel(GameModel gameModel)
        {
            MainStartColor = "#F2B28A";
            MainEndColor = "#E07031";
            SubStartColor = "#FCF4EE";
            SubEndColor = "#FAE0CD";
            ImageStar = "/Achievers;component/Assets/Images/game-result-star-2.png";
            ImageIcon = "/Achievers;component/Assets/Images/ft-r2-g.png";
            IsFinish = gameModel.next_unit_review == null;
            GameModel = gameModel;
            Instruction = gameModel.instruction;
            TotalQuestion = gameModel.total_question;
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

        public override double GetScore()
        {
            return (double)Questions.Count(m => m.isCorrect) / TotalQuestion * 100;
        }

        public override UIElement GetResultUI()
        {
            return new DragCorrectWordResult(this);
        }
    }

    public class QuestionAnwser
    {
        public string Text { get; set; }
        public string Def { get; set; }
        public string CorrectText { get; set; }
        public string Image { get; set; }
        public int numPart { get; set; }
        public string Hint { get; set; }
        public List<string> parts { get; set; }

        public bool isCorrect { get; set; }
    }
}