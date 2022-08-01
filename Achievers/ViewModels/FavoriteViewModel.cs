using Achievers.Models;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels
{
    public class FavoriteViewModel : BaseViewModel
    {
        private readonly string imagePath = "/Achievers;component/Assets/Images/";

        public FavoriteViewModel()
        {
            ListFavorite = new ObservableCollection<WordModel>();
            CurrentFavorite = new WordModel();
        }

        private WordModel currentFavorite;

        public WordModel CurrentFavorite
        {
            get { return currentFavorite; }
            set
            {
                currentFavorite = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WordModel> listFavorite;

        public ObservableCollection<WordModel> ListFavorite
        {
            get { return listFavorite; }
            set
            {
                listFavorite = value;
                OnPropertyChanged();
            }
        }

        private bool manage;

        public bool Manage
        {
            get { return manage; }
            set
            {
                manage = value;
                OnPropertyChanged();
            }
        }
    }
}