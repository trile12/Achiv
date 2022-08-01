using Achievers.Models;
using Achievers.ViewModels.Custom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CustomSearchComboBox.xaml
    /// </summary>
    public partial class CustomSearchComboBox : UserControl
    {
        private CustomSearchViewModel viewModel;

        public CustomSearchComboBox()
        {
            InitializeComponent();
            DataContext = viewModel = new CustomSearchViewModel();
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WordModel word = (sender as ListBox).SelectedItem as WordModel;
            if (word != null)
            {
                WordDetaiPopupManager.ShowWordDetail();

                Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        ComboboxUnitManager.SelectUnitAndWord(word);
                        textSearch.Text = string.Empty;
                    });
                });
            }

            var tb = new TextBlock();
        }

        private void textSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var noSpaceText = textSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(noSpaceText))
            {
                listbox.Visibility = Visibility.Collapsed;
                listBoxClip.Visibility = Visibility.Collapsed;
            }
            else
            {
                listbox.Visibility = Visibility.Visible;
                listBoxClip.Visibility = Visibility.Visible;
                var searchedVocabList = App.lstAllWord.Where(x => x.word.ToLower().StartsWith(noSpaceText)).ToList();

                //var searchedVocabList = App.lstAllWord.Where(x => x.word.ToLower().Contains(noSpaceText)).ToList();

                var unlockUnits = App.Units.Where(x => x.IsUnlock);
                var visibleVocab = from vocab in searchedVocabList
                                   join u in unlockUnits on vocab.grade_unit_id equals u.Id into temp
                                   select vocab;
                foreach (var word in visibleVocab) { word.Search = noSpaceText; }
                viewModel.LstWord = new ObservableCollection<WordModel>(visibleVocab);
            }
        }

        private void ClearSearchClicked(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            textSearch.Text = "";
        }
    }
}