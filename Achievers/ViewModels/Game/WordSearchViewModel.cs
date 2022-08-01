using Achievers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Achievers.ViewModels.Game
{
    public class WordSearchViewModel : IGameActivity
    {
        public WordSearchViewModel(GameModel gameModel)
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
            Strs = gameModel.questions[0].grid_str.Select(m => m.ToString()).ToArray();
            List = gameModel.questions[0].answers.Select(m => new WordSearchItem() { Text = m.text.ToUpper() }).ToList();
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

        public List<WordSearchItem> List { get; set; }

        public string[] Strs { get; set; }

        public override double GetScore()
        {
            return (double)List.Count(m => m.IsCorrect == true) / List.Count * 100;
        }

        public override string GetResultTitle()
        {
            return "Excellent!";
        }

        public override string GetResultDescription()
        {
            return "You've completed the game";
        }
    }

    public class WordSearchItem : BaseViewModel
    {
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool? isCorrect;

        public bool? IsCorrect
        {
            get { return isCorrect; }
            set
            {
                if (isCorrect != value)
                {
                    isCorrect = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}