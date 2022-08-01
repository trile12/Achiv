using Achievers.ViewModels;
using System.Collections.Generic;

namespace Achievers.Models
{
    public class UnitTabModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public string Title { get; set; }
        public string TitleVI { get; set; }

        private int? progress;

        public int? Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Icon { get; set; }
        public bool IsUnlock { get; set; }

        public bool IsLocked
        {
            get
            {
                return !IsUnlock;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string StartColor { get; set; }
        public string EndColor { get; set; }
        public string UnitBackground { get; set; }
        public string UnitForeground { get; set; }
        public string ColorBoder { get; set; }
        public string ForeGround { get; set; }
        public string ImageWordList { get; set; }
        public List<UnitTabReviewModel> lstReview { get; set; }
        public string InactiveColor { get; set; }
        public string InactiveColor2 { get; set; }
    }

    public class UnitTabReviewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int IdUnit { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        private int? progress;

        public int? Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isLocked;

        public bool IsLocked
        {
            get { return isLocked; }
            set
            {
                if (isLocked != value)
                {
                    isLocked = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Index { get; set; }
        public int MinProgress { get; set; }
    }
}