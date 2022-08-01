using Achievers.Models;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels.Custom
{
    public class CustomSearchViewModel : BaseViewModel
    {
        public CustomSearchViewModel()
        {
            lstWord = new ObservableCollection<WordModel>(App.lstAllWord);
        }

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
    }
}