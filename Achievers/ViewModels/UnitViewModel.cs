namespace Achievers.ViewModels
{
    public class UnitViewModel : BaseViewModel
    {
        private TopicTabView currentTab;

        public TopicTabView CurrentTab
        {
            get => currentTab;
            set
            {
                currentTab = value;
                OnPropertyChanged();
            }
        }

        public UnitViewModel()
        {
            CurrentTab = TopicTabView.UNIT;
        }

        private double _TotalProgress;

        public double TotalProgress
        {
            get { return _TotalProgress; }
            set
            {
                _TotalProgress = value;
                OnPropertyChanged();
            }
        }
    }

    public enum TopicTabView
    {
        UNIT,
        WORDLIST,
        FAVORITE,
        MEMOS,
        Empty
    }
}