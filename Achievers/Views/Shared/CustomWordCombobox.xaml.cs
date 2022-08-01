using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.ViewModels;
using Achievers.ViewModels.Custom;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CustomWordCombobox.xaml
    /// </summary>
    public partial class CustomWordCombobox : UserControl
    {
        private CustomWordViewModel viewModel;
        public static readonly RoutedEvent OnItemSelectEvent;

        public event RoutedEventHandler OnItemSelect
        {
            add
            {
                base.AddHandler(OnItemSelectEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnItemSelectEvent, value);
            }
        }

        public CustomWordCombobox()
        {
            InitializeComponent();
            DataContext = viewModel = new CustomWordViewModel();
            ComboboxUnitManager.Init(viewModel);
        }

        static CustomWordCombobox()
        {
            OnItemSelectEvent = EventManager.RegisterRoutedEvent("OnItemSelect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomWordCombobox));
        }

        private async void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedUnit = (sender as ListBox).SelectedItem as UnitTabModel;
            if (selectedUnit.IsUnlock)
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        RaiseEvent(new RoutedEventArgs(OnItemSelectEvent, sender));
                        viewModel.Unit = (sender as ListBox).SelectedItem as UnitTabModel;
                        popupBox.Visibility = Visibility.Collapsed;
                    });
                });
            }
        }

        private void bd_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (popupBox.Visibility == Visibility.Collapsed)
                popupBox.Visibility = Visibility.Visible;
            else
                popupBox.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void FilterListChange()
        {
        }
    }

    public class ComboboxUnitManager
    {
        private static CustomWordViewModel viewModel;
        private static WordCardDetailViewModel detailViewModel;
        private static ListView Lb;

        public static bool IsSearch = false;

        public static void Init(CustomWordViewModel i_customViewModel)
        {
            viewModel = i_customViewModel;
        }

        public static void InitWordDetail(WordCardDetailViewModel i_detailViewModel)
        {
            detailViewModel = i_detailViewModel;
        }

        public static void InitListView(ListView i_listView)
        {
            Lb = i_listView;
        }

        public static async void SelectUnit(UnitTabModel unitTab)
        {
            viewModel.Unit = unitTab;
            int unit = viewModel.LstUnit.IndexOf(unitTab) + 1;
            GetVocabByUnit(unit);

            //if (scrollViewer != null)
            //{
            //    ScrollAnimationBehavior.AnimateScroll(scrollViewer, 0);
            //}
        }

        public static async void SelectUnitAndWord(WordModel word)
        {
            WordListTabQuery query = new WordListTabQuery();
            await Task.Run(() =>
            {
                var lstWord = App.lstAllWord.Where(x => x.grade_unit_id == word.grade_unit_id).ToList();

                IsSearch = true;
                detailViewModel.LstWord = new ObservableCollection<WordModel>(lstWord);
                viewModel.Unit = viewModel.LstUnit.FirstOrDefault(x => x.Id == word.grade_unit_id);
            });
            var thisWord = detailViewModel.LstWord.FirstOrDefault(x => x.id == word.id);
            detailViewModel.Word = thisWord;
            int index = detailViewModel.LstWord.IndexOf(thisWord);

            var scroll = HelpService.FindChildByType<ScrollViewer>(Lb);
            if (scroll != null)
                ScrollAnimationBehavior.AnimateScroll(scroll, index);
        }

        public static async void GetVocabByUnit(int unit)
        {
            WordListTabQuery query = new WordListTabQuery();
            await Task.Run(() =>
            {
                var lstWord = App.lstAllWord.Where(x => x.grade_unit_id == unit).ToList();
                detailViewModel.LstWord = new ObservableCollection<WordModel>(lstWord);
            });
            detailViewModel.Word = detailViewModel.LstWord.FirstOrDefault();
            var scroll = HelpService.FindChildByType<ScrollViewer>(Lb);
            if (scroll != null)
                ScrollAnimationBehavior.AnimateScroll(scroll, 0);
        }
    }
}