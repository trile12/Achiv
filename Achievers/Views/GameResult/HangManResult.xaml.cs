using Achievers.ViewModels;
using Achievers.ViewModels.Game;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Achievers.Views.GameResult
{
    /// <summary>
    /// Interaction logic for HangManResult.xaml
    /// </summary>
    public partial class HangManResult : UserControl
    {
        private HangManResultViewModel viewModel;

        public HangManResult(HangManViewModel game)
        {
            InitializeComponent();
            DataContext = viewModel = new HangManResultViewModel()
            {
                List = game.Questions
            };
        }
    }

    public class HangManResultViewModel : BaseViewModel
    {
        public List<QuestionAnwser> List { get; set; }
    }
}