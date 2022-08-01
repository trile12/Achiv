using Achievers.EasingCustom;
using Achievers.ViewModels;
using Achievers.Views.Tab;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Achievers.Views
{
    /// <summary>
    /// Interaction logic for UnitPage.xaml
    /// </summary>
    ///
    internal delegate void OnLoadTopicData();

    public partial class UnitPage : Page
    {
        private UnitViewModel viewModel = new UnitViewModel();
        private OnLoadTopicData onLoadTopic;
        private Grid popup;

        public UnitPage()
        {
            InitializeComponent();
            DataContext = viewModel;
            frame.Navigate(new UnitTab(MainScroll));
            UnitPageManager.Init(img_blue_bg, grid_footer, viewModel);
        }

        private void Tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var el = sender as FrameworkElement;
            if (viewModel.CurrentTab == (TopicTabView)(sender as FrameworkElement).Tag)
            {
                return;
            }

            viewModel.CurrentTab = (TopicTabView)(sender as FrameworkElement).Tag;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    SwitchTab(viewModel.CurrentTab);
                    var translate = animatedBorder.RenderTransform as TranslateTransform;
                    Point pos = el.TransformToVisual(animatedBorder).Transform(new Point(0, 0));
                    DoubleAnimation anim = new DoubleAnimation();
                    anim.To = pos.X;
                    anim.Duration = new TimeSpan(0, 0, 0, 0, 350);
                    anim.EasingFunction = new BezierEase();
                    anim.Completed += BarAnim_Completed;
                    translate.BeginAnimation(TranslateTransform.XProperty, anim);
                });
            });
        }

        private void BarAnim_Completed(object sender, EventArgs e)
        {
            var translate = animatedBorder.RenderTransform as TranslateTransform;
            translate.BeginAnimation(TranslateTransform.XProperty, null);
            Grid.SetColumn(animatedBorder, (int)viewModel.CurrentTab + 1);
            //onLoadTopic();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.TotalProgress = App.totalProgress;
            //onLoadTopic = async () =>
            //{
            //    if (viewModel.CurrentTab == TopicTabView.UNIT)
            //    {
            //        await Task.Delay(100);
            //    }
            //    else if (viewModel.CurrentTab == TopicTabView.WORDLIST)
            //    {
            //        await Task.Delay(100);
            //    }
            //    else if (viewModel.CurrentTab == TopicTabView.MEMOS)
            //    {
            //        await Task.Delay(100);
            //    }
            //    else if (viewModel.CurrentTab == TopicTabView.FAVORITE)
            //    {
            //        await Task.Delay(100);
            //    }
            //};
            //onLoadTopic();
        }

        private async void SwitchTab(TopicTabView topicTabView)
        {
            animatedBorder.Visibility = Visibility.Visible;
            UnitPageManager.HideBlueBG();
            switch (topicTabView)
            {
                case TopicTabView.UNIT:
                    animatedBorder.Width = 150;
                    animatedBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EFFAFC"));
                    frame.Navigate(new UnitTab(MainScroll));
                    break;

                case TopicTabView.WORDLIST:
                    animatedBorder.Width = 180;
                    animatedBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EFEEFA"));
                    frame.Navigate(new WordListTab(MainScroll));
                    break;

                case TopicTabView.MEMOS:
                    animatedBorder.Width = 150;
                    animatedBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FDF8EE"));
                    frame.Navigate(new MemosTab(animatedBorder, MainScroll, viewModel));
                    break;

                case TopicTabView.FAVORITE:
                    animatedBorder.Width = 150;
                    animatedBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F9EEF6"));
                    frame.Navigate(new FavoriteTab(viewModel, MainScroll, animatedBorder));
                    break;
            }
        }

        private void Profile_Click(object sender, MouseButtonEventArgs e)
        {
            UnitPageManager.HideBlueBG();
            viewModel.CurrentTab = TopicTabView.Empty;
            animatedBorder.Visibility = Visibility.Collapsed;
            frame.Navigate(new ProfileTab(MainScroll));
        }

        private void OnCloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    public class UnitPageManager
    {
        private static UnitViewModel viewModel;
        private static ImageBrush imageBrush = new ImageBrush();
        private static Grid img_blue_bg;
        private static Grid grid_footer;
        private static BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Achievers;Component/Assets/Images/bg-game-page.png"));

        public static void Init(Grid i_img, Grid i_grid, UnitViewModel i_viewModel)
        {
            img_blue_bg = i_img;
            grid_footer = i_grid;
            viewModel = i_viewModel;
        }

        public static void ShowBlueBG()
        {
            imageBrush.ImageSource = bitmap;
            img_blue_bg.Background = imageBrush;
            grid_footer.Visibility = Visibility.Collapsed;
        }

        public static void HideBlueBG()
        {
            img_blue_bg.Background = (Brush)(new BrushConverter().ConvertFrom("#F6FAFE"));
            grid_footer.Visibility = Visibility.Visible;
        }

        public static void UpdateTotalProgress()
        {
            viewModel.TotalProgress = App.totalProgress;
        }
    }
}