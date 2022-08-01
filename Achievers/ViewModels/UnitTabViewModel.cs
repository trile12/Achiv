using Achievers.Models;
using System.Collections.ObjectModel;

namespace Achievers.ViewModels
{
    public class UnitTabViewModel : BaseViewModel
    {
        public UnitTabViewModel()
        {
            lstUnit = new ObservableCollection<UnitTabModel>();
        }

        private int unitIndex;

        public int UnitIndex
        {
            get { return unitIndex; }
            set { unitIndex = value; }
        }

        private UnitTabModel unit;

        public UnitTabModel Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UnitTabReviewModel> lstReview;

        public ObservableCollection<UnitTabReviewModel> LstReview
        {
            get { return lstReview; }
            set
            {
                lstReview = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UnitTabModel> lstUnit;

        public ObservableCollection<UnitTabModel> LstUnit
        {
            get { return lstUnit; }
            set { lstUnit = value; }
        }

        private bool isExpanded;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged();
            }
        }
    }
}