using Achievers.ViewModels.Game;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Achievers.Views.GameResult
{
    /// <summary>
    /// Interaction logic for MatchWordResult.xaml
    /// </summary>
    public partial class MatchWordResult : UserControl
    {
        private MatchWordResultViewModel viewModel;

        public MatchWordResult(MatchWordViewModel game)
        {
            InitializeComponent();
            viewModel = new MatchWordResultViewModel();
            viewModel.List = game.Questions.Select((m, mIndex) => new MatchWordResultQuestion()
            {
                QuestionName = string.Format("QUESTION {0}", mIndex + 1),
                Questions = m.questionAnwsers.Select(g => new MatchWordResultItem()
                {
                    Text = g.Text,
                    Definition = g.Def,
                    IsCorrect = g.isCorrect
                }).ToList()
            }).ToList();
            DataContext = viewModel;
        }
    }

    public class MatchWordResultViewModel
    {
        public List<MatchWordResultQuestion> List { get; set; }
    }

    public class MatchWordResultQuestion
    {
        public string QuestionName { get; set; }
        public List<MatchWordResultItem> Questions { get; set; }
    }

    public class MatchWordResultItem
    {
        public string Text { get; set; }
        public string Definition { get; set; }
        public bool IsCorrect { get; set; }
    }
}