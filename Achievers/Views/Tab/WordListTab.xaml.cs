using Achievers.Models;
using Achievers.ViewModels;
using Achievers.Views.Shared;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Tab
{
    /// <summary>
    /// Interaction logic for WordListTab.xaml
    /// </summary>
    public partial class WordListTab : Page
    {
        private ScrollViewer MainScroll;
        private WordListViewModel viewModel;

        public WordListTab(ScrollViewer scrollViewer)
        {
            InitializeComponent();
            MainScroll = scrollViewer;
            DataContext = new WordListViewModel() { LstUnit = App.Units };

            viewModel = DataContext as WordListViewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollAnimationBehavior.AnimateScroll(MainScroll, 0);
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

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((sender as ListBox).SelectedItem as UnitTabModel);
            if (selectedItem != null)
            {
                if (!selectedItem.IsUnlock)
                {
                    ShowPopupPurchase();
                }
                else
                {
                    ComboboxUnitManager.SelectUnit(selectedItem);
                    WordDetaiPopupManager.ShowWordDetail();
                }
                Lb.SelectedIndex = -1;
            }
        }

        private void ShowPopupPurchase()
        {
            PopupManager.ShowPopup();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}