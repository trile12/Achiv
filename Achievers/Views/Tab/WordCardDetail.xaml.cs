using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.ViewModels;
using Achievers.Views.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Tab
{
    /// <summary>
    /// Interaction logic for WordCardDetail.xaml
    /// </summary>
    public partial class WordCardDetail : UserControl
    {
        private WordCardDetailViewModel viewModel;
        private ScrollViewer curScroll;

        private AudioService audioService;

        public WordCardDetail()
        {
            InitializeComponent();
            DataContext = viewModel = new WordCardDetailViewModel();
            ComboboxUnitManager.InitWordDetail(viewModel);
            audioService = new AudioService();
        }

        private void WordCardDetail_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxUnitManager.InitListView(Lb);
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            WordDetaiPopupManager.CloseWordDetail();
        }

        //private void Test_Flip(object sender, RoutedEventArgs e)
        //{
        //    DoubleAnimation anim = new DoubleAnimation();
        //    anim.To = -1;
        //    anim.Duration = new TimeSpan(0, 0, 0, 0, 300);
        //    anim.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };
        //    //grid1.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);

        //    //DoubleAnimation anim_2 = new DoubleAnimation();
        //    //anim_2.To = 1;
        //    //anim_2.Duration = new TimeSpan(0, 0, 0, 0, 500);
        //    //anim_2.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };
        //    //grid1.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim_2);

        //    Storyboard storyBoard = new Storyboard();
        //    Storyboard.SetTarget(anim, grid1);
        //    Storyboard.SetTargetProperty(anim, new PropertyPath(ScaleTransform.ScaleXProperty));

        //    storyBoard.Children.Add(anim);

        //    storyBoard.Completed += (s, ev) =>
        //    {
        //        storyBoard.Seek(new TimeSpan(0));
        //        storyBoard.Stop();
        //    };

        //    storyBoard.Begin();
        //}

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = (sender as ListBox).SelectedItem as WordModel;
            if (model != null)
                viewModel.Word = model;
            Memosdetail.Visibility = Visibility.Collapsed;

            Lb.ScrollIntoView(viewModel.Word);
        }

        private void Lb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var model = (sender as Border).DataContext as WordModel;
            viewModel.Word = model;
            Memosdetail.Visibility = Visibility.Collapsed;
            Lb.ScrollIntoView(viewModel.Word);
        }

        private async void OnItemSelected_Click(object sender, RoutedEventArgs e)
        {
            if (ComboboxUnitManager.IsSearch)
            {
                ComboboxUnitManager.IsSearch = false;
                return;
            }
            List<WordModel> lstWord = new List<WordModel>(); ;
            int unit = (e.OriginalSource as ListBox).SelectedIndex + 1;
            WordListTabQuery query = new WordListTabQuery();

            var T1 = Task.Run(() =>
              {
                  lstWord = App.lstAllWord.Where(x => x.grade_unit_id == unit).ToList();
                  Dispatcher.Invoke(() =>
                  {
                      viewModel.LstWord = new ObservableCollection<WordModel>(lstWord);
                      viewModel.Word = viewModel.LstWord.FirstOrDefault();
                  });
              });

            var t = Task.WhenAll(T1);
            await t.ContinueWith(x =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (curScroll == null)
                    {
                        curScroll = HelpService.FindChildByType<ScrollViewer>(Lb);
                    }
                    ScrollAnimationBehavior.AnimateScroll_v2(curScroll, 0);
                });
            });
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

        private async void SaveMemo_Click(object sender, MouseButtonEventArgs e)
        {
            if (tb.Text != null)
            {
                //if (tb.Text.Length == 0)
                //    await HttpService.DeleteMemos(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id });
                //else
                //    await HttpService.CreateMemos(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id, content = tb.Text });
                WordListTabQuery query = new WordListTabQuery();
                await query.UpdateMemos(viewModel.Word.id, tb.Text);
                viewModel.Word.memo = tb.Text;
                Memosdetail.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelMemo_Click(object sender, MouseButtonEventArgs e)
        {
            Memosdetail.Visibility = Visibility.Collapsed;
        }

        private void Create_Memo_Click(object sender, MouseButtonEventArgs e)
        {
            Memosdetail.Visibility = Visibility.Visible;
            tb.Text = viewModel.Word.memo;
        }

        private async void Favorite_Clicked(object sender, MouseButtonEventArgs e)
        {
            WordListTabQuery query = new WordListTabQuery();
            switch (viewModel.Word.is_favorite)
            {
                case 1:
                    //await HttpService.DeleteFavorite(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id });
                    await query.UpdateFavorite(viewModel.Word.id, 0);
                    viewModel.Word.is_favorite = 0;
                    break;

                default:
                    //await HttpService.CreateFavorite(new { userId = App.UserId, unit_vocab_id = viewModel.Word.id });
                    await query.UpdateFavorite(viewModel.Word.id, 1);
                    viewModel.Word.is_favorite = 1;
                    break;
            }
        }

        private void BounceButton_OnClick_Next(object sender, MouseButtonEventArgs e)
        {
            var currentIndex = viewModel.LstWord.IndexOf(viewModel.Word);
            if (curScroll == null)
            {
                curScroll = HelpService.FindChildByType<ScrollViewer>(Lb);
            }
            if (currentIndex < viewModel.LstWord.Count - 1)
            {
                viewModel.Word = viewModel.LstWord[currentIndex + 1];
                int to = 0;
                if (currentIndex > 20)
                {
                    to = (currentIndex + 1) * 80;
                }
                else
                    to = (currentIndex + 1) * 70;
                ScrollAnimationBehavior.AnimateScroll(curScroll, to);
            }
        }

        private void BounceButton_OnClick_Prev(object sender, MouseButtonEventArgs e)
        {
            var currentIndex = viewModel.LstWord.IndexOf(viewModel.Word);
            if (curScroll == null)
            {
                curScroll = HelpService.FindChildByType<ScrollViewer>(Lb);
            }
            if (currentIndex > 0)
            {
                viewModel.Word = viewModel.LstWord[currentIndex - 1];
                int to = 0;
                if (currentIndex > 20)
                {
                    to = (currentIndex - 1) * 80;
                }
                else
                    to = (currentIndex - 1) * 70;
                ScrollAnimationBehavior.AnimateScroll(curScroll, to);
            }
        }

        private void Prev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentIndex = viewModel.LstWord.IndexOf(viewModel.Word);
            if (curScroll == null)
            {
                curScroll = HelpService.FindChildByType<ScrollViewer>(Lb);
            }
            if (currentIndex > 0)
            {
                viewModel.Word = viewModel.LstWord[currentIndex - 1];
                var to = (currentIndex - 1) * 50;
                ScrollAnimationBehavior.AnimateScroll(curScroll, to);
            }
        }

        ~WordCardDetail()
        {
            audioService?.Dispose();
        }
    }
}