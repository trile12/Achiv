using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for PopupPurchase.xaml
    /// </summary>
    public partial class PopupPurchase : Border
    {
        public PopupPurchase()
        {
            InitializeComponent();
        }

        private void PopupClose_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.ClosePopup();
        }
    }
}