using Achievers.EasingCustom;
using Achievers.ViewModels;
using Achievers.Views;
using Achievers.Views.Shared;
using Achievers.Views.Tab;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers
{
    /// <summary>
    /// Interaction logic for MainApp.xaml
    /// </summary>
    public partial class MainApp : Grid
    {
        public MainApp()
        {
            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            PopupManager.Init(Popup_Purchase, popup_border, OpacityProperty);
            PopupManager.InitPopupDelte(Popup_Delete, border_delete);
            PopupManager.InitPopupMemos(Popup_Memos, border_memos);
            PopupManager.InitPopupFavoriteDelete(Popup_Delete_Favorite, border_delete_favorite);
            PopupManager.InitPopupQuitGame(Popup_QuitGame, border_quitgame, frame);
            PopupManager.InitPopupResult(Popup_Result, border_Result);

            WordDetaiPopupManager.Init(wordDetail, gridWordDetail);

            await Task.Delay(1000);
            welcomepage.Opacity = 0.9;

            frame.Navigate(new UnitPage());
        }

        private void PopupClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Popup_Purchase.Visibility = Visibility.Collapsed;
            Popup_Memos.Visibility = Visibility.Collapsed;
        }

        private void PopupResult_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ClosePopupResult();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            var parent = (Panel)welcomepage.Parent;
            parent.Children.Remove(welcomepage);
            welcomepage = null;
        }
    }

    public class WordDetaiPopupManager
    {
        private static WordCardDetail wordCardDetail;
        private static Grid gridWordDetail;

        public static void Init(WordCardDetail i_wordDetail, Grid i_gridWord)
        {
            wordCardDetail = i_wordDetail;
            gridWordDetail = i_gridWord;
        }

        public static void ShowWordDetail()
        {
            gridWordDetail.Visibility = Visibility.Visible;
            wordCardDetail.Visibility = Visibility.Visible;

            //var translate = wordCardDetail.RenderTransform as TranslateTransform;
            //DoubleAnimation anim = new DoubleAnimation();
            //anim.To = 0;
            //anim.Duration = new TimeSpan(0, 0, 0, 0, 200);
            //anim.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            //DoubleAnimation animOpacity = new DoubleAnimation();
            //animOpacity.From = 0;
            //animOpacity.To = 0.3;
            //animOpacity.Duration = new TimeSpan(0, 0, 0, 0, 500);
            //animOpacity.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            //gridWordDetail.BeginAnimation(UIElement.OpacityProperty, animOpacity);
            //translate.BeginAnimation(TranslateTransform.YProperty, anim);
        }

        public static void CloseWordDetail()
        {
            gridWordDetail.Visibility = Visibility.Collapsed;
            wordCardDetail.Visibility = Visibility.Collapsed;
            //var translate = wordCardDetail.RenderTransform as TranslateTransform;
            //DoubleAnimation anim = new DoubleAnimation();
            //anim.To = 1100;
            //anim.Duration = new TimeSpan(0, 0, 0, 0, 200);
            //anim.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };
            //translate.BeginAnimation(TranslateTransform.YProperty, anim);
        }
    }

    public class PopupManager
    {
        private static Grid grid_purchase;
        private static Border popup_border;
        private static DependencyProperty dependency;

        private static Grid grid_delete;
        private static Border popup_delete;

        private static Grid grid_delete_favorite;
        private static Border popup_delete_favorite;

        private static Grid grid_memos;
        public static PopupMemos popup_memos;

        public static int Id;
        public static MemosViewModel memosViewModel;
        public static FavoriteViewModel favoriteViewModel;

        private static Grid grid_quitgame;
        private static Border popup_quitgame;
        public static Frame frame;

        private static Grid Popup_Result;
        private static Border border_Result;

        public static void Init(Grid _grid, Border _border, DependencyProperty dependencyProperty)
        {
            grid_purchase = _grid;
            dependency = dependencyProperty;
            popup_border = _border;
        }

        public static void InitPopupDelte(Grid _grid, Border _border)
        {
            grid_delete = _grid;
            popup_delete = _border;
        }

        public static void InitPopupFavoriteDelete(Grid _grid, Border _border)
        {
            grid_delete_favorite = _grid;
            popup_delete_favorite = _border;
        }

        public static void InitPopupMemos(Grid _grid, PopupMemos _border)
        {
            grid_memos = _grid;
            popup_memos = _border;
        }

        public static void InitPopupQuitGame(Grid _grid, Border _border, Frame i_frame)
        {
            grid_quitgame = _grid;
            popup_quitgame = _border;
            frame = i_frame;
        }

        public static void InitPopupResult(Grid _grid, Border _border)
        {
            Popup_Result = _grid;
            border_Result = _border;
        }

        public static void PopupResultInitContent(UIElement content)
        {
            (border_Result as ResultPopup).resultContent.Content = content;
        }

        public static async void ShowPopupResult()
        {
            Popup_Result.Visibility = Visibility.Visible;
            border_Result.RenderTransformOrigin = new Point(0.5, 0.5);
            border_Result.RenderTransform = new ScaleTransform(0, 0);

            ScaleTransform scale = border_Result.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(1, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseOut };

            border_Result.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
            await Task.Delay(200);
            Popup_Result.IsHitTestVisible = true;
        }

        public static async void ClosePopupResult()
        {
            ScaleTransform scale = border_Result.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(0, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseOut };

            border_Result.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
            await Task.Delay(200);
            Popup_Result.Visibility = Visibility.Collapsed;
            Popup_Result.IsHitTestVisible = false;
        }

        public static void ShowPopup()
        {
            grid_purchase.Visibility = Visibility.Visible;
            popup_border.RenderTransformOrigin = new Point(0.5, 0.5);
            popup_border.RenderTransform = new ScaleTransform(0, 0);

            ScaleTransform scale = popup_border.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(1, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            popup_border.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
        }

        public static void ClosePopup()
        {
            grid_purchase.Visibility = Visibility.Collapsed;
        }

        public static void ShowPopupDelete()
        {
            grid_delete.Visibility = Visibility.Visible;
            popup_delete.RenderTransformOrigin = new Point(0.5, 0.5);
            popup_delete.RenderTransform = new ScaleTransform(0, 0);

            ScaleTransform scale = popup_delete.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(1, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            popup_delete.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
        }

        public static void ShowPopupMemos()
        {
            grid_memos.Visibility = Visibility.Visible;
            popup_memos.RenderTransformOrigin = new Point(0.5, 0.5);
            popup_memos.RenderTransform = new ScaleTransform(0, 0);

            ScaleTransform scale = popup_memos.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(1, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            popup_memos.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
        }

        public static void ShowPopupQuitGame()
        {
            grid_quitgame.Visibility = Visibility.Visible;
            popup_quitgame.RenderTransformOrigin = new Point(0.5, 0.5);
            popup_quitgame.RenderTransform = new ScaleTransform(0, 0);

            ScaleTransform scale = popup_quitgame.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(1, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            popup_quitgame.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
        }

        public static void ClosePopupDelete()
        {
            grid_delete.Visibility = Visibility.Collapsed;
        }

        public static void ClosePopupMemos()
        {
            grid_memos.Visibility = Visibility.Collapsed;
        }

        public static void ClosePopupFavoriteDelete()
        {
            grid_delete_favorite.Visibility = Visibility.Collapsed;
        }

        public static void ShowPopupFavoriteDelete()
        {
            grid_delete_favorite.Visibility = Visibility.Visible;
            popup_delete_favorite.RenderTransform = new ScaleTransform(0.7, 0.6);

            ScaleTransform scale = popup_delete_favorite.RenderTransform as ScaleTransform;
            var timeLine = new DoubleAnimation(1, new TimeSpan(0, 0, 0, 0, 200));
            timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

            popup_delete_favorite.BeginAnimation(dependency, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, timeLine);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, timeLine);
        }

        public static void ClosePopupQuitGame()
        {
            grid_quitgame.Visibility = Visibility.Collapsed;
        }
    }
}