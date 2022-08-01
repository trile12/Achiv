using Achievers.Models;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels
{
    public class WordCardDetailViewModel : BaseViewModel
    {
        private ObservableCollection<WordModel> lstWord;

        public ObservableCollection<WordModel> LstWord
        {
            get { return lstWord; }
            set
            {
                lstWord = value;
                OnPropertyChanged();
            }
        }

        private WordModel word;

        public WordModel Word
        {
            get { return word; }
            set
            {
                word = value;
                OnPropertyChanged();
            }
        }

        public WordCardDetailViewModel()
        {
            word = new WordModel();
            LstWord = new ObservableCollection<WordModel>();
        }
    }
}