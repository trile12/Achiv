using Achievers.Models;
using Achievers.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Tab
{
    /// <summary>
    /// Interaction logic for MemosTab.xaml
    /// </summary>
    public partial class MemosTabList : Page
    {
        public MemosTabList()
        {
            InitializeComponent();
            DataContext = new MemosViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VocabModel model = (sender as ListView).SelectedItem as VocabModel;

            var viewModel = (MemosViewModel)DataContext;
        }

        private void BounceButton_OnClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (MemosViewModel)DataContext;

            PopupManager.ShowPopupMemos();
        }
    }
}