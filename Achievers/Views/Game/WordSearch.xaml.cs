using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Achievers.Views.Game
{
    /// <summary>
    /// Interaction logic for WordSearch.xaml
    /// </summary>
    public partial class WordSearch : UserControl
    {
        private WordSearchViewModel viewModel;
        private double cellSize = 0;
        private double margin = 2;
        private int numOfCell = 10;
        private Brush correctColor = (Brush)new BrushConverter().ConvertFromString("#FFFFFF");

        public WordSearch(GameModel gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new WordSearchViewModel(gameModel);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            cellSize = mainGame.ActualWidth / 10;

            for (int i = 0; i < numOfCell * numOfCell; i++)
            {
                var tbl = new Label();
                tbl.IsHitTestVisible = false;
                tbl.FontSize = 24;
                tbl.VerticalContentAlignment = VerticalAlignment.Center;
                tbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                tbl.Width = cellSize;
                tbl.Height = cellSize;
                tbl.Content = viewModel.Strs[i];
                var top = i / numOfCell * cellSize;
                var left = i % numOfCell * cellSize;
                Canvas.SetTop(tbl, top);
                Canvas.SetLeft(tbl, left);
                mainGame.Children.Add(tbl);
            }

            icList.ItemsSource = viewModel.List;
        }

        private Point startIndex = new Point(-1, -1);
        private Point endIndex = new Point(-1, -1);
        private Rectangle rectTemp = null;
        private double tempAngle = 0;

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mainGame.CaptureMouse();
            var pos = e.GetPosition(mainGame);
            startIndex.X = Math.Floor(pos.X / cellSize);
            startIndex.Y = Math.Floor(pos.Y / cellSize);
            startIndex.X = Math.Min(Math.Max(startIndex.X, 0), numOfCell - 1);
            startIndex.Y = Math.Min(Math.Max(startIndex.Y, 0), numOfCell - 1);
            if (rectTemp == null)
            {
                rectTemp = new Rectangle();
                mainGame.Children.Add(rectTemp);
            }
            //rectTemp.RenderTransform = new RotateTransform(tempAngle, cellSize / 2 - margin, cellSize / 2 - margin);
            rectTemp.Fill = (Brush)new BrushConverter().ConvertFromString("#F2FAF7");
            rectTemp.RadiusX = 16;
            rectTemp.RadiusY = 16;
            rectTemp.Width = cellSize - margin * 2;
            rectTemp.Height = cellSize - margin * 2;
            var top = startIndex.Y * cellSize + margin;
            var left = startIndex.X * cellSize + margin;
            Panel.SetZIndex(rectTemp, -2);
            Canvas.SetTop(rectTemp, top);
            Canvas.SetLeft(rectTemp, left);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!mainGame.IsMouseCaptured || rectTemp == null) return;
            var pos = e.GetPosition(mainGame);
            var currentIndex = new Point(Math.Floor(pos.X / cellSize), Math.Floor(pos.Y / cellSize));
            currentIndex.X = Math.Min(Math.Max(currentIndex.X, 0), numOfCell - 1);
            currentIndex.Y = Math.Min(Math.Max(currentIndex.Y, 0), numOfCell - 1);
            if (AnimationSelectedBoxes(currentIndex))
            {
                endIndex = currentIndex;
            }
        }

        private bool AnimationSelectedBoxes(Point currentIndex)
        {
            double tempWidth = 0;
            double tempX = currentIndex.X - startIndex.X;
            double tempY = currentIndex.Y - startIndex.Y;
            if (currentIndex.X == startIndex.X && currentIndex.Y != startIndex.Y)
            {
                var distanceY = Math.Abs(startIndex.Y - currentIndex.Y) + 1;
                tempWidth = distanceY * cellSize;
                if (startIndex.Y > currentIndex.Y)
                {
                    tempAngle = 270;
                }
                else if (startIndex.Y < currentIndex.Y)
                {
                    tempAngle = 90;
                }
            }
            else if (currentIndex.Y == startIndex.Y)
            {
                var distanceX = Math.Abs(startIndex.X - currentIndex.X) + 1;
                tempWidth = distanceX * cellSize;
                if (startIndex.X > currentIndex.X)
                {
                    tempAngle = 180;
                }
                else if (startIndex.X < currentIndex.X)
                {
                    tempAngle = 0;
                }
            }
            else if (Math.Abs(tempX) == Math.Abs(tempY))
            {
                var crossWidth = Math.Sqrt(tempX * tempX + tempY * tempY) + 1;
                tempWidth = crossWidth * cellSize;
                if (tempX > 0 && tempY > 0) tempAngle = 45;
                else if (tempX < 0 && tempY > 0) tempAngle = 135;
                else if (tempX < 0 && tempY < 0) tempAngle = 225;
                else if (tempX > 0 && tempY < 0) tempAngle = 315;
            }
            if (tempWidth > 0)
            {
                var widthAnim = new DoubleAnimation(tempWidth - margin * 2, TimeSpan.FromMilliseconds(100));
                var heightAnim = new DoubleAnimation(cellSize - margin * 2, TimeSpan.FromMilliseconds(100));
                rectTemp.BeginAnimation(WidthProperty, widthAnim);
                rectTemp.BeginAnimation(HeightProperty, heightAnim);
                //rectTemp.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation(tempAngle, TimeSpan.FromMilliseconds(50)));
                rectTemp.RenderTransform = new RotateTransform(tempAngle, cellSize / 2 - margin, cellSize / 2 - margin);
                return true;
            }
            return false;
        }

        private async void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mainGame.IsMouseCaptured || rectTemp == null) return;
            mainGame.ReleaseMouseCapture();
            var pos = e.GetPosition(mainGame);
            var currentIndex = new Point(Math.Floor(pos.X / cellSize), Math.Floor(pos.Y / cellSize));
            currentIndex.X = Math.Min(Math.Max(currentIndex.X, 0), numOfCell - 1);
            currentIndex.Y = Math.Min(Math.Max(currentIndex.Y, 0), numOfCell - 1);
            if (AnimationSelectedBoxes(currentIndex))
            {
                endIndex = currentIndex;
            }
            var word = GetWords(endIndex);
            var wordReverse = string.Join("", word.Reverse());
            var item = viewModel.List.FirstOrDefault(m => m.Text == word || m.Text == wordReverse);
            if (item != null)
            {
                if (item.IsCorrect != true)
                {
                    AudioGame.Correct();
                    item.IsCorrect = true;
                    Canvas.SetZIndex(rectTemp, -1);
                    rectTemp.Fill = (Brush)new BrushConverter().ConvertFromString("#44B88F");
                    SetStyleCorrectCharacter(endIndex);

                    if (IsUserCorrect())
                    {
                        await Task.Delay(1000);
                        App.navigationService.Navigate(new ReviewResultPage(viewModel));
                    }
                }
            }
            else
            {
                AudioGame.Incorrect();
                mainGame.Children.Remove(rectTemp);
            }
            rectTemp = null;
        }

        private void SetStyleCorrectCharacter(Point currentIndex)
        {
            if (tempAngle == 90 || tempAngle == 270)
            {
                int start = (int)Math.Min(startIndex.Y, currentIndex.Y);
                int end = (int)Math.Max(startIndex.Y, currentIndex.Y);
                for (int i = start; i <= end; i++)
                {
                    var el = mainGame.Children[(int)(startIndex.X + i * numOfCell)] as Label;
                    el.Foreground = correctColor;
                }
            }
            else if (tempAngle == 180 || tempAngle == 0)
            {
                int start = (int)Math.Min(startIndex.X, currentIndex.X);
                int end = (int)Math.Max(startIndex.X, currentIndex.X);
                for (int i = start; i <= end; i++)
                {
                    var el = mainGame.Children[(int)(i + startIndex.Y * numOfCell)] as Label;
                    el.Foreground = correctColor;
                }
            }
            else if (tempAngle == 45 || tempAngle == 225)
            {
                int end = (int)Math.Abs(startIndex.X - currentIndex.X);
                var offsetStart = tempAngle == 45 ? startIndex : currentIndex;
                for (int i = 0; i <= end; i++)
                {
                    var el = mainGame.Children[(int)((offsetStart.X + i) + (offsetStart.Y + i) * numOfCell)] as Label;
                    el.Foreground = correctColor;
                }
            }
            else if (tempAngle == 135 || tempAngle == 315)
            {
                int end = (int)Math.Abs(startIndex.X - currentIndex.X);
                var offsetStart = tempAngle == 135 ? startIndex : currentIndex;
                for (int i = 0; i <= end; i++)
                {
                    var el = mainGame.Children[(int)(offsetStart.X - i + (offsetStart.Y + i) * numOfCell)] as Label;
                    el.Foreground = correctColor;
                }
            }
        }

        private string GetWords(Point currentIndex)
        {
            string word = "";
            if (tempAngle == 90 || tempAngle == 270)
            {
                int start = (int)Math.Min(startIndex.Y, currentIndex.Y);
                int end = (int)Math.Max(startIndex.Y, currentIndex.Y);
                for (int i = start; i <= end; i++)
                {
                    word += viewModel.Strs[(int)(startIndex.X + i * numOfCell)];
                }
            }
            else if (tempAngle == 180 || tempAngle == 0)
            {
                int start = (int)Math.Min(startIndex.X, currentIndex.X);
                int end = (int)Math.Max(startIndex.X, currentIndex.X);
                for (int i = start; i <= end; i++)
                {
                    word += viewModel.Strs[(int)(i + startIndex.Y * numOfCell)];
                }
            }
            else if (tempAngle == 45 || tempAngle == 225)
            {
                int end = (int)Math.Abs(startIndex.X - currentIndex.X);
                var offsetStart = tempAngle == 45 ? startIndex : currentIndex;
                for (int i = 0; i <= end; i++)
                {
                    word += viewModel.Strs[(int)((offsetStart.X + i) + (offsetStart.Y + i) * numOfCell)];
                }
            }
            else if (tempAngle == 135 || tempAngle == 315)
            {
                int end = (int)Math.Abs(startIndex.X - currentIndex.X);
                var offsetStart = tempAngle == 135 ? startIndex : currentIndex;
                for (int i = 0; i <= end; i++)
                {
                    word += viewModel.Strs[(int)(offsetStart.X - i + (offsetStart.Y + i) * numOfCell)];
                }
            }
            return word;
        }

        private bool IsUserCorrect()
        {
            return viewModel.GetScore() == 100;
        }

        private void QuitGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
        }
    }
}