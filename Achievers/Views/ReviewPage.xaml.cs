using Achievers.Models;
using Achievers.Views.Game;
using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views
{
    /// <summary>
    /// Interaction logic for ReviewPage.xaml
    /// </summary>
    public partial class ReviewPage : Page
    {
        private GameModel gameModel;

        public ReviewPage(GameModel i_gameModel)
        {
            InitializeComponent();
            gameModel = i_gameModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.navigationService = NavigationService;
            UnitPageManager.ShowBlueBG();
            switch (gameModel.activity_type)
            {
                case GameName.LookAndMatch:
                    gameContent.Content = new DragEachWord(gameModel);
                    break;

                case GameName.ListenAndSay:
                    gameContent.Content = new ListenAndSay(gameModel);
                    break;

                case GameName.Unscamble:
                    gameContent.Content = new DragDropSpellWord(gameModel);
                    break;

                case GameName.Spelltheword:
                    gameContent.Content = new DragCorrectWord(gameModel);
                    break;

                case GameName.Hangman:
                    gameContent.Content = new HangMan(gameModel);
                    break;

                case GameName.Wordsearch:
                    gameContent.Content = new WordSearch(gameModel);
                    break;

                case GameName.Memory:
                    gameContent.Content = new Memory(gameModel);
                    break;

                case GameName.MatchWord:
                    gameContent.Content = new MatchWord(gameModel);
                    break;

                default:
                    break;
            }
        }
    }
}