using Achievers.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Achievers.Views.Tab
{
    /// <summary>
    /// Interaction logic for ProfileTab.xaml
    /// </summary>
    public partial class ProfileTab : Page
    {
        private ProfileTabViewModel viewModel;
        private ScrollViewer MainScroll;

        public ProfileTab(ScrollViewer scrollViewer)
        {
            InitializeComponent();
            DataContext = viewModel = new ProfileTabViewModel();
            MainScroll = scrollViewer;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.TotalProgress = App.totalProgress;
            viewModel.LstUnit = App.Units;
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void listBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset + 40);
            }
            else if (e.Delta > 0)
            {
                MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset - 40);
            }
        }

        private void Home_Click(object sender, MouseButtonEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                if (NavigationService.Content is UnitPage) break;
                NavigationService.GoBack();
            }
        }
    }
}