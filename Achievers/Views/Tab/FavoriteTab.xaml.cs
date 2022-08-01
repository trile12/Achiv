using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.ViewModels;
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
    /// Interaction logic for FavoriteTab.xaml
    /// </summary>
    public partial class FavoriteTab : Page
    {
        private UnitViewModel unitViewModel;
        private FavoriteViewModel viewModel;
        private ScrollViewer MainScroll;
        private AudioService audioService;
        private Border animBorder;

        public FavoriteTab(UnitViewModel i_unitViewModel, ScrollViewer i_scroll, Border i_animBorder)
        {
            InitializeComponent();
            DataContext = viewModel = new FavoriteViewModel();
            audioService = new AudioService();
            unitViewModel = i_unitViewModel;
            MainScroll = i_scroll;
            animBorder = i_animBorder;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            switch (viewModel.Manage)
            {
                case true:

                    foreach (var item in viewModel.ListFavorite)
                    {
                        item.Checked = "Collapsed";
                    }

                    foreach (WordModel item in (sender as ListView).SelectedItems)
                    {
                        item.Checked = "Visible";
                    }
                    break;

                default:
                    WordModel model = (sender as ListView).SelectedItem as WordModel;
                    if (model != null)
                    {
                        viewModel.CurrentFavorite = model;
                        Memosdetail.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void Manage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            foreach (var item in viewModel.ListFavorite)
            {
                item.Checked = "Collapsed";
            }
            viewModel.Manage = !viewModel.Manage;

            var currFavorite = viewModel.CurrentFavorite;
            if (viewModel.Manage)
            {
                ItemListView.SelectionMode = SelectionMode.Multiple;
                ItemListView.SelectedItems.Clear();
                ItemListView.Height = Application.Current.MainWindow.ActualHeight - 334 - 80;
            }
            else
            {
                ItemListView.SelectionMode = SelectionMode.Single;
                ItemListView.SelectedItem = currFavorite;
                ItemListView.Height = Application.Current.MainWindow.ActualHeight - 334;
            }
        }

        private void BounceButton_OnClick_Next(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            var currentIndex = viewModel.ListFavorite.IndexOf(viewModel.CurrentFavorite);

            if (currentIndex < viewModel.ListFavorite.Count - 1)
                viewModel.CurrentFavorite = viewModel.ListFavorite[currentIndex + 1];
        }

        private void BounceButton_OnClick_Prev(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            var currentIndex = viewModel.ListFavorite.IndexOf(viewModel.CurrentFavorite);

            if (currentIndex > 0)
                viewModel.CurrentFavorite = viewModel.ListFavorite[currentIndex - 1];
        }

        private void mngBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void mngBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        //Popups
        private void BounceButton_OnClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;

            PopupManager.ShowPopupMemos();
        }

        private void BounceButton_OnClick_Delete(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            if (viewModel.ListFavorite.Where(x => x.Checked == "Visible").ToList().Count > 0)
            {
                PopupManager.favoriteViewModel = viewModel;
                PopupManager.ShowPopupFavoriteDelete();
            }
        }

        private async void Favorite_Clicked(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            //await HttpService.DeleteFavorite(new { userId = App.UserId, unit_vocab_id = viewModel.CurrentFavorite.id });
            await (new WordListTabQuery()).UpdateFavorite(viewModel.CurrentFavorite.id, 0);

            viewModel.ListFavorite.Remove(viewModel.CurrentFavorite);
            viewModel.CurrentFavorite = viewModel.ListFavorite.FirstOrDefault();
        }

        private async void OnPageLoad(object sender, RoutedEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;

            var wordModels = (await (new WordListTabQuery()).GetAllWord()).Where(x => x.is_favorite > 0 && x.is_favorite != null).ToList();
            viewModel.ListFavorite = new ObservableCollection<WordModel>((await (new WordListTabQuery()).GetAllWord()).Where(x => x.is_favorite > 0 && x.is_favorite != null));
            int i = 0;
            foreach (var item in viewModel.ListFavorite)
            {
                item.image_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + item.image_url.Replace("/", @"\"));
                item.audio_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + item.audio_url.Replace("/", @"\"));
                item.back_ground = (Color)ColorConverter.ConvertFromString(App.lstBackground[i]);
                item.border_brush = (Color)ColorConverter.ConvertFromString(App.lstBorderBrush[i]);
                i++;
                if (i == 4)
                    i = 0;
            }
            viewModel.CurrentFavorite = viewModel.ListFavorite.FirstOrDefault();
            ItemListView.SelectedItem = viewModel.CurrentFavorite;
            ItemListView.Height = Application.Current.MainWindow.ActualHeight - 334;
            VocabDetailPanel.Height = Application.Current.MainWindow.ActualHeight - 104;
            //ItemListView.MinHeight = 644;
        }

        private async void SaveMemo_Click(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            if ((tb.Text != null) && (tb.Text != viewModel.CurrentFavorite.memo))
            {
                if (tb.Text.Length == 0)
                    //await HttpService.DeleteMemos(new { userId = App.UserId, unit_vocab_id = viewModel.CurrentFavorite.id });
                    await new WordListTabQuery().UpdateMemos(viewModel.CurrentFavorite.id, "");
                else
                    //await HttpService.CreateMemos(new { userId = App.UserId, unit_vocab_id = viewModel.CurrentFavorite.id, content = tb.Text });
                    await new WordListTabQuery().UpdateMemos(viewModel.CurrentFavorite.id, tb.Text);
                viewModel.CurrentFavorite.memo = tb.Text;
                Memosdetail.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelMemo_Click(object sender, MouseButtonEventArgs e)
        {
            Memosdetail.Visibility = Visibility.Collapsed;
        }

        private void Create_Memo_Click(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FavoriteViewModel)DataContext;
            tb.Text = viewModel.CurrentFavorite.memo;
            Memosdetail.Visibility = Visibility.Visible;
        }

        private async void Audio_Click(object sender, MouseButtonEventArgs e)
        {
            if (viewModel.CurrentFavorite == null)
                return;

            var data = viewModel.CurrentFavorite;
            var element = (FrameworkElement)sender;
            element.IsHitTestVisible = false;

            await audioService.Init(new string[] { data.audio_url });
            audioService.Play(0);
            element.IsHitTestVisible = true;
        }

        ~FavoriteTab()
        {
            audioService.Dispose();
        }

        private void BackToWordList(object sender, RoutedEventArgs e)
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
    }
}