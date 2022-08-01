using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for PopupQuitGame.xaml
    /// </summary>
    public partial class PopupQuitGame : Border
    {
        public PopupQuitGame()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ClosePopupQuitGame();
            if (App.barTimer != null)
                App.barTimer.Resume();
        }

        private void QuitGame_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.ClosePopupQuitGame();
            while (App.navigationService.CanGoBack)
            {
                if (App.navigationService.Content is UnitPage) break;
                App.navigationService.GoBack();
            }
            UnitPageManager.HideBlueBG();
            if (App.barTimer != null)
            {
                App.barTimer = null;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.ClosePopupQuitGame();
            if (App.barTimer != null)
                App.barTimer.Resume();
        }
    }
}