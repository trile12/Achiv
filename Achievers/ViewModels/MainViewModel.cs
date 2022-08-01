using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Achievers.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        { }
    }

    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
        }

        private bool _ifAnimateProgressCircle;

        public bool IfAnimateProgressCircle
        {
            get => _ifAnimateProgressCircle;
            set
            {
                _ifAnimateProgressCircle = value;
                OnPropertyChanged(nameof(IfAnimateProgressCircle));
            }
        }
    }
}