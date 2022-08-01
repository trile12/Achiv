using Achievers.ViewModels;
using Achievers.ViewModels.Game;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.GameResult
{
    /// <summary>
    /// Interaction logic for DragDropSpellWordResult.xaml
    /// </summary>
    public partial class DragDropSpellWordResult : UserControl
    {
        private DragDropSpellWordResultViewModel viewModel;

        public DragDropSpellWordResult(DragDropSpellWordViewModel game)
        {
            InitializeComponent();
            DataContext = viewModel = new DragDropSpellWordResultViewModel()
            {
                List = game.UserAnswers.Select(m => new DragDropSpellWordResultItem()
                {
                    Text = m.TextResult,
                    UserText = m.TextResult,
                    CorrectText = m.Text.ToUpper(),
                    ImageUrl = m.Image,
                    IsCorrect = m.IsCorrect
                }).ToList()
            };
        }

        private void OnClickViewAnswer(object sender, MouseButtonEventArgs e)
        {
            viewModel.IsViewAnswer = !viewModel.IsViewAnswer;
            foreach (var item in viewModel.List)
            {
                item.Text = viewModel.IsViewAnswer ? item.CorrectText : item.UserText;
            }
        }
    }

    public class DragDropSpellWordResultViewModel : BaseViewModel
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

        public List<DragDropSpellWordResultItem> List { get; set; }
    }

    public class DragDropSpellWordResultItem : BaseViewModel
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