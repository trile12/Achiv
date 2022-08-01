using Achievers.Services;
using Achievers.ViewModels;
using Achievers.ViewModels.Game;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.GameResult
{
    /// <summary>
    /// Interaction logic for DragEachWordResult.xaml
    /// </summary>
    public partial class DragEachWordResult : UserControl
    {
        private DragEachWordResultViewModel viewModel;

        public DragEachWordResult(DragEachWordViewModel game)
        {
            InitializeComponent();
            viewModel = new DragEachWordResultViewModel();
            viewModel.List = game.UserAnswers.Select((m, mIndex) => new DragEachWordResultQuestion()
            {
                QuestionName = string.Format("QUESTION {0}", mIndex + 1),
                Questions = m.Select((g, gIndex) => new DragEachWordResultItem()
                {
                    ImageUrl = string.Format("{0}/contents{1}", HttpService.baseUrl, game.GameModel.questions[mIndex].answers[gIndex].image),
                    Text = game.GameModel.questions[mIndex].answers[g.Index].text,
                    UserText = game.GameModel.questions[mIndex].answers[g.Index].text,
                    CorrectText = game.GameModel.questions[mIndex].answers[gIndex].text,
                    IsCorrect = g.IsCorrect,
                    UserIsCorrect = g.IsCorrect
                }).ToList()
            }).ToList();
            DataContext = viewModel;
        }

        private void OnClickViewAnswer(object sender, MouseButtonEventArgs e)
        {
            viewModel.IsViewAnswer = !viewModel.IsViewAnswer;
            foreach (var item in viewModel.List)
            {
                foreach (var question in item.Questions)
                {
                    question.Text = viewModel.IsViewAnswer ? question.CorrectText : question.UserText;
                    question.IsCorrect = viewModel.IsViewAnswer || question.UserIsCorrect;
                }
            }
        }
    }

    public class DragEachWordResultViewModel : BaseViewModel
    {
        private bool isViewAnswer;

        public bool IsViewAnswer
        {
            get { return isViewAnswer; }
            set
            {
                if (isViewAnswer != value)
                {
                    isViewAnswer = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<DragEachWordResultQuestion> List { get; set; }
    }

    public class DragEachWordResultQuestion
    {
        public string QuestionName { get; set; }
        public List<DragEachWordResultItem> Questions { get; set; }
    }

    public class DragEachWordResultItem : BaseViewModel
    {
        public string ImageUrl { get; set; }

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

        public string UserText { get; set; }
        public string CorrectText { get; set; }

        public bool UserIsCorrect { get; set; }

        private bool isCorrect;

        public bool IsCorrect
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