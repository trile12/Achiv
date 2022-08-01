using Achievers.ViewModels.Game;
using Achievers.Views.Game;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels.Popup
{
    public class ResultPopupViewModel : BaseViewModel
    {
        private ObservableCollection<Question> lstQuestion;

        public ObservableCollection<Question> LstQuestion
        {
            get { return lstQuestion; }
            set
            {
                lstQuestion = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WordResultModel> wordResultModels;

        public ObservableCollection<WordResultModel> WordResultModels
        {
            get { return wordResultModels; }
            set
            {
                wordResultModels = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<QuestionMatchWord> questionMatchWords;

        public ObservableCollection<QuestionMatchWord> QuestionMatchWords
        {
            get { return questionMatchWords; }
            set
            {
                questionMatchWords = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<QuestionAnwser> questionSpellWord;

        public ObservableCollection<QuestionAnwser> QuestionSpellWord
        {
            get { return questionSpellWord; }
            set
            {
                questionSpellWord = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ListenAndSayQuestion> listenAndSayQuestions;

        public ObservableCollection<ListenAndSayQuestion> ListenAndSayQuestions
        {
            get { return listenAndSayQuestions; }
            set
            {
                listenAndSayQuestions = value;
                OnPropertyChanged();
            }
        }

        private bool isMyResult;

        public bool IsMyResult
        {
            get { return isMyResult; }
            set
            {
                isMyResult = value;
                OnPropertyChanged();
            }
        }

        private GameName gameView;

        public GameName GameView
        {
            get { return gameView; }
            set
            {
                gameView = value;
                OnPropertyChanged();
            }
        }
    }
}