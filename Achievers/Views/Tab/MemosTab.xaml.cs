using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.ViewModels;
using Achievers.Views.Shared;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Tab
{
    /// <summary>
    /// Interaction logic for MemosTab.xaml
    /// </summary>
    public partial class MemosTab : Page
    {
        private ScrollViewer MainScroll;
        private MemosViewModel viewModel;
        private UnitViewModel unitViewModel;
        private Border animBorder;

        private AudioService audioService;

        public MemosTab(Border i_animBorder, ScrollViewer i_scroll, UnitViewModel i_unitViewModel)
        {
            InitializeComponent();
            DataContext = viewModel = new MemosViewModel();
            animBorder = i_animBorder;
            MainScroll = i_scroll;
            unitViewModel = i_unitViewModel;
            audioService = new AudioService();
        }

        private void AnimMenu()
        {
            animBorder.Width = 200;
            animBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EFEEFA"));
            NavigationService?.Navigate(new WordListTab(MainScroll));

            unitViewModel.CurrentTab = TopicTabView.WORDLIST;

            var translate = animBorder.RenderTransform as TranslateTransform;
            DoubleAnimation anim = new DoubleAnimation();
            anim.To = -350;
            anim.Duration = new TimeSpan(0, 0, 0, 0, 350);
            anim.EasingFunction = new BezierEase();
            anim.Completed += BarAnim_Completed;
            translate.BeginAnimation(TranslateTransform.XProperty, anim);
        }

        private void BarAnim_Completed(object sender, EventArgs e)
        {
            var translate = animBorder.RenderTransform as TranslateTransform;
            translate.BeginAnimation(TranslateTransform.XProperty, null);
            Grid.SetColumn(animBorder, 2);
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Memosdetail.Visibility = Visibility.Collapsed;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var listMemos = (await (new WordListTabQuery()).GetAllWord()).Where(x => !String.IsNullOrWhiteSpace(x.memo)).ToList();
            //var listMemos = await HttpService.GetAllMemos();
            int i = 0;
            listMemos.ForEach(x =>
            {
                x.image_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + x.image_url.Replace("/", @"\"));
                x.audio_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + x.audio_url.Replace("/", @"\"));
                x.back_ground = (Color)ColorConverter.ConvertFromString(App.lstBackground[i]);
                x.border_brush = (Color)ColorConverter.ConvertFromString(App.lstBorderBrush[i]);
                i++;
                if (i == 4)
                    i = 0;
            });
            viewModel.LstWord = new ObservableCollection<WordModel>(listMemos);
            viewModel.Word = viewModel.LstWord.FirstOrDefault();

            ItemListView.Height = Application.Current.MainWindow.ActualHeight - 334;
            VocabDetailPanel.Height = Application.Current.MainWindow.ActualHeight - 104;
        }

        private void Edit_Remove_Item(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as ItemModel;
            if (item.type == "Delete")
            {
                PopupManager.Id = item.Id;
                PopupManager.memosViewModel = viewModel;
                PopupManager.ShowPopupDelete();
            }
            else
            {
                Memosdetail.Visibility = Visibility.Visible;
                tb.Text = viewModel.Word.memo;
            }
        }

        private void Goto_WordList(object sender, RoutedEventArgs e)
        {
            AnimMenu();
        }

        private async void SaveMemo_Click(object sender, MouseButtonEventArgs e)
        {
            if (tb.Text != null)
            {
                if (tb.Text.Length == 0)
                {
                    //await HttpService.DeleteMemos(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id });
                    await new WordListTabQuery().UpdateMemos(viewModel.Word.id, "");
                    viewModel.LstWord.Remove(viewModel.Word);
                    viewModel.Word = viewModel.LstWord.FirstOrDefault();
                }
                else
                {
                    viewModel.Word.memo = tb.Text;
                    //await HttpService.CreateMemos(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id, content = tb.Text });
                    await new WordListTabQuery().UpdateMemos(viewModel.Word.id, tb.Text);
                }
                Memosdetail.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelMemo_Click(object sender, MouseButtonEventArgs e)
        {
            Memosdetail.Visibility = Visibility.Collapsed;
        }

        private async void RefreshMemoList()
        {
            int i = 0;
            var listMemos = (await (new WordListTabQuery()).GetAllWord()).Where(x => !String.IsNullOrWhiteSpace(x.memo)).ToList();
            //var listMemos = await HttpService.GetAllMemos();
            listMemos.ForEach(x =>
            {
                x.image_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + x.image_url.Replace("/", @"\"));
                x.back_ground = (Color)ColorConverter.ConvertFromString(App.lstBackground[i]);
                i++;
                if (i == 4)
                    i = 0;
            });
            viewModel.LstWord = new ObservableCollection<WordModel>(listMemos);
        }

        private void Edit_Click(object sender, MouseButtonEventArgs e)
        {
            Memosdetail.Visibility = Visibility.Visible;
            tb.Text = viewModel.Word.memo;
        }

        private void Prev_OnClick(object sender, MouseButtonEventArgs e)
        {
            var currentIndex = viewModel.LstWord.IndexOf(viewModel.Word);

            if (currentIndex > 0)
                viewModel.Word = viewModel.LstWord[currentIndex - 1];
        }

        private void Next_OnClick(object sender, MouseButtonEventArgs e)
        {
            var currentIndex = viewModel.LstWord.IndexOf(viewModel.Word);

            if (currentIndex < viewModel.LstWord.Count - 1)
                viewModel.Word = viewModel.LstWord[currentIndex + 1];
        }

        private async void Favorite_Click(object sender, MouseButtonEventArgs e)
        {
            switch (viewModel.Word.is_favorite)
            {
                case 1:
                    //await HttpService.DeleteFavorite(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id });
                    await (new WordListTabQuery()).UpdateFavorite(viewModel.Word.id, 0);

                    viewModel.Word.is_favorite = 0;
                    break;

                default:
                    //await HttpService.CreateFavorite(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id });
                    await (new WordListTabQuery()).UpdateFavorite(viewModel.Word.id, 1);

                    viewModel.Word.is_favorite = 1;
                    break;
            }
        }

        private async void Audio_Click(object sender, MouseButtonEventArgs e)
        {
            if (viewModel.Word == null)
                return;

            var data = viewModel.Word;
            var element = (FrameworkElement)sender;
            element.IsHitTestVisible = false;

            await audioService.Init(new string[] { data.audio_url });
            audioService.Play(0);
            element.IsHitTestVisible = true;
        }

        private void VocabDetailPanel_MouseWheel(object sender, MouseWheelEventArgs e)
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
    }
}