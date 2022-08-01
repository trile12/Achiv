using Achievers.ViewModels;
using Achievers.ViewModels.Game;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Achievers.Views.GameResult
{
    /// <summary>
    /// Interaction logic for DragCorrectWordResult.xaml
    /// </summary>
    public partial class DragCorrectWordResult : UserControl
    {
        private DragCorrectWordResultViewModel viewModel;

        public DragCorrectWordResult(DragCorrectWordViewModel wordViewModel)
        {
            InitializeComponent();
            DataContext = viewModel = new DragCorrectWordResultViewModel()
            {
                List = wordViewModel.Questions
            };
        }
    }

    public class DragCorrectWordResultViewModel : BaseViewModel
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

        public List<QuestionAnwser> List { get; set; }
    }
}