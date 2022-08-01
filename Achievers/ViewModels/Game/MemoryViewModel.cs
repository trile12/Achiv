using Achievers.Models;
using System.Collections.Generic;

namespace Achievers.ViewModels.Game
{
    public class MemoryViewModel : IGameActivity
    {
        public MemoryViewModel(GameModel gameModel)
        {
            MainStartColor = "#A1E5CD";
            MainEndColor = "#44B88F";
            SubStartColor = "#A1E5CD";
            SubEndColor = "#44B88F";
            ImageStar = "/Achievers;component/Assets/Images/game-result-star-4.png";
            ImageIcon = "/Achievers;component/Assets/Images/ft-r4-g.png";
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
            return 100;
        }

        public override string GetResultTitle()
        {
            return "Excellent!";
        }

        public override string GetResultDescription()
        {
            return "You’ve completed the unit";
        }
    }
}