using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for ResultPopup.xaml
    /// </summary>
    public partial class ResultPopup : Border
    {
        public ResultPopup()
        {
            InitializeComponent();
        }

        private void CloseGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ClosePopupResult();
        }
    }
}