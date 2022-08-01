using Achievers.Models;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels
{
    public class WordListViewModel : BaseViewModel
    {
        public WordListViewModel()
        {
            lstUnit = new ObservableCollection<UnitTabModel>();
        }

        private ObservableCollection<UnitTabModel> lstUnit;

        public ObservableCollection<UnitTabModel> LstUnit
        {
            get { return lstUnit; }
            set { lstUnit = value; }
        }
    }
}