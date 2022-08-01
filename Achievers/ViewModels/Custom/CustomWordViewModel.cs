using Achievers.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Achievers.ViewModels.Custom
{
    public class CustomWordViewModel : BaseViewModel
    {
        public CustomWordViewModel()
        {
            while (true)
            {
                if (App.Units != null)
                {
                    lstUnit = App.Units;
                    break;
                }
            }
        }

        private ObservableCollection<UnitTabModel> lstUnit;

        public ObservableCollection<UnitTabModel> LstUnit
        {
            get { return lstUnit; }
            set { lstUnit = value; }
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
    }
}