using Achievers.ViewModels.Game;
using System.Linq;
using System.Windows.Controls;

namespace Achievers.Views.GameResult
{
    /// <summary>
    /// Interaction logic for ListenAndSayResult.xaml
    /// </summary>
    public partial class ListenAndSayResult : UserControl
    {
        public ListenAndSayResult(ListenAndSayViewModel game)
        {
            InitializeComponent();
            DataContext = game.Questions.Select(m => new ListenAndSayResultQuestion()
            {
                ImageUrl = m.Image,
                IsCorrect = m.IsCorrect
            });
        }
    }

    public class ListenAndSayResultQuestion
    {
        public string ImageUrl { get; set; }
        public bool IsCorrect { get; set; }
    }
}