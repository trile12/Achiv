using Achievers.Models;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels
{
    public class ProfileTabViewModel : BaseViewModel
    {
        public ProfileTabViewModel()
        {
        }

        private double _totalProgress;

        public double TotalProgress
        {
            get { return _totalProgress; }
            set
            {
                _totalProgress = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UnitTabModel> lstUnit;

        public ObservableCollection<UnitTabModel> LstUnit
        {
            get { return lstUnit; }
            set
            {
                lstUnit = value;
                OnPropertyChanged();
            }
        }
    }
}