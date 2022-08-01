using Achievers.Models;
using Achievers.Views.GameResult;
using System;
using System.Linq;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public class DragDropSpellWordViewModel : IGameActivity
    {
        public DragDropSpellWordViewModel(GameModel gameModel)
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
            UserAnswers = gameModel.questions.Select(m => new WordResultModel()
            {
                Text = m.answers[0].text,
                Image = Environment.CurrentDirectory + "/Assets/Contents/" + m.answers[0].image
            }).ToArray();
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

        private string _image;

        public string image
        {
            get { return _image; }
            set
            {
                _image = value;
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

        public WordResultModel[] UserAnswers { get; set; }

        public override double GetScore()
        {
            return (double)UserAnswers.Count(m => m.IsCorrect) / TotalQuestion * 100;
        }

        public override UIElement GetResultUI()
        {
            return new DragDropSpellWordResult(this);
        }
    }

    public class WordResultModel
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public string TextResult { get; set; }
        public bool IsCorrect { get; set; }
    }
}