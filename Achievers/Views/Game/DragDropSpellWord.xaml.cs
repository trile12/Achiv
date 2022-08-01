using Achievers.ControlCustom;
using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using Achievers.Views.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Game
{
    /// <summary>
    /// Interaction logic for DragDropSpellWord.xaml
    /// </summary>
    public partial class DragDropSpellWord : UserControl
    {
        private SolidColorBrush borderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#A5E4F2");

        private DragDropSpellWordViewModel viewModel;
        private List<BorderWord> lstPositionHold;
        private List<BorderWord> lstPositionWord;
        private int Ypos = 20;
        private UIElement dragObject = null;
        private Point offset;
        private Point pos;

        private string currentWord;

        public DragDropSpellWord(GameModel i_gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new DragDropSpellWordViewModel(i_gameModel);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.CurIndex = 1;
            var questions = viewModel.UserAnswers[viewModel.CurIndex - 1];
            currentWord = questions.Text.Replace(" ", "");
            viewModel.image = questions.Image;
            InitData(currentWord);
        }

        private void InitData(string sampleString)
        {
            AutoGenerateEmptyBorder(sampleString.ToUpper());
            AutoGenBorder(sampleString.ToUpper());
        }

        private void AutoGenerateEmptyBorder(string sampleString)
        {
            double space = (CanvasMain.ActualWidth - (sampleString.Length * 80 - 20)) / 2;
            double Xpos = 0;
            Ypos = 20;
            lstPositionHold = new List<BorderWord>();
            for (int i = 0; i < sampleString.Length; i++)
            {
                Border border2 = new Border();
                ButtonWord bt = new ButtonWord();
                bt.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                bt.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#E7EFF8");

                Xpos = space;
                space += 80;

                Panel.SetZIndex(bt, 0);
                Canvas.SetTop(bt, Ypos);
                Canvas.SetLeft(bt, Xpos);

                lstPositionHold.Add(new BorderWord
                {
                    X = Xpos,
                    Y = Ypos,
                });
                CanvasMain.Children.Add(bt);
            }
        }

        private void AutoGenBorder(string samepleString)
        {
            lstPositionWord = new List<BorderWord>();
            samepleString = string.Join("", HelpService.ShuffleList<char>(samepleString.ToList()));

            int Ypos = this.Ypos + 60 + 24;
            double space = (CanvasMain.ActualWidth - (samepleString.Length * 80 - 20)) / 2;
            double Xpos = 0;

            for (int i = 0; i < samepleString.Length; i++)
            {
                Border border2 = new Border();
                ButtonWord bt = new ButtonWord();
                bt.Tag = i;
                bt.Title = samepleString[i].ToString();
                bt.Foregrounddp = (Color)ColorConverter.ConvertFromString("#3DA4BF");
                bt.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#EFFAFC");
                bt.BorderBrush = borderColor;
                bt.PreviewMouseDown += Border_PreviewMouseDown;

                Xpos = space;
                space += 80;

                bt.BaseX = Xpos;
                bt.BaseY = Ypos;

                Panel.SetZIndex(bt, 1);
                Canvas.SetTop(bt, Ypos);
                Canvas.SetLeft(bt, Xpos);
                lstPositionWord.Add(new BorderWord
                {
                    IsExist = true,
                    X = Xpos,
                    Y = Ypos,
                    Text = samepleString[i].ToString()
                });
                CanvasMain.Children.Add(bt);
            }
        }

        private int timeStamp = 0;

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            timeStamp = e.Timestamp;
            dragObject = sender as UIElement;
            Panel.SetZIndex(dragObject, 5);
            this.offset = e.GetPosition(this.CanvasMain);
            this.offset.Y -= Canvas.GetTop(this.dragObject);
            this.offset.X -= Canvas.GetLeft(this.dragObject);
            this.CanvasMain.CaptureMouse();
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ButtonWord buttonWord = dragObject as ButtonWord;
            if (this.dragObject == null || buttonWord.IsPlaced)
                return;
            var position = e.GetPosition(sender as IInputElement);
            pos.X = position.X - offset.X;
            pos.Y = position.Y - offset.Y;
            Canvas.SetTop(this.dragObject, pos.Y);
            Canvas.SetLeft(this.dragObject, pos.X);
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.CanvasMain.ReleaseMouseCapture();
            if (dragObject == null) return;
            ButtonWord buttonWord = dragObject as ButtonWord;
            int index = (int)buttonWord.Tag;
            if (e.Timestamp - timeStamp > 200)
            {
                var pos = new Point(Canvas.GetLeft(this.dragObject), Canvas.GetTop(this.dragObject));
                var posHold = GetPosIndex(lstPositionHold, pos);
                if (posHold != null && !posHold.IsExist)
                {
                    foreach (var item in lstPositionWord)
                    {
                        if (item.X == (dragObject as ButtonWord).BaseX && item.Y == (dragObject as ButtonWord).BaseY)
                        {
                            item.IsExist = false;
                            break;
                        }
                    }
                    posHold.IsExist = true;
                    PlaceEnter(new Point(posHold.X, posHold.Y));
                }
                else
                {
                    AnimateToPosCanvas(dragObject, new Point(buttonWord.BaseX, buttonWord.BaseY), 300);
                }
            }
            else
            {
                if (!(dragObject as ButtonWord).IsPlaced)
                {
                    foreach (var item in lstPositionWord)
                    {
                        if (item.X == (dragObject as ButtonWord).BaseX && item.Y == (dragObject as ButtonWord).BaseY)
                        {
                            item.IsExist = false;
                            break;
                        }
                    }
                    PlaceEnter(AutoPlaceHold(lstPositionHold));
                }
                else
                {
                    foreach (var item in lstPositionHold)
                    {
                        if (item.X == (dragObject as ButtonWord).BaseX && item.Y == (dragObject as ButtonWord).BaseY)
                        {
                            item.IsExist = false;
                            break;
                        }
                    }
                    lstPositionWord[index].IsExist = true;
                    PlaceExist(new Point(lstPositionWord[index].X, lstPositionWord[index].Y));
                }
            }

            Panel.SetZIndex(dragObject, 1);
            this.dragObject = null;
        }

        private void AnimateToPosCanvas(UIElement uIElement, Point point, int duration)
        {
            var timeLineX = new DoubleAnimation()
            {
                To = point.X,
                Duration = new TimeSpan(0, 0, 0, 0, 300),
                EasingFunction = new BezierEase()
            };
            var timeLineY = new DoubleAnimation()
            {
                To = point.Y,
                Duration = new TimeSpan(0, 0, 0, 0, 300),
                EasingFunction = new BezierEase()
            };
            timeLineX.FillBehavior = FillBehavior.Stop;
            timeLineX.Completed += (s, o) =>
            {
                Canvas.SetLeft(uIElement, point.X);
            };
            timeLineY.FillBehavior = FillBehavior.Stop;
            timeLineY.Completed += (s, o) =>
            {
                Canvas.SetTop(uIElement, point.Y);
            };

            uIElement.BeginAnimation(Canvas.LeftProperty, timeLineX);
            uIElement.BeginAnimation(Canvas.TopProperty, timeLineY);
        }

        private BorderWord GetPosIndex(List<BorderWord> lstPosHold, Point point)
        {
            foreach (var item in lstPosHold)
            {
                if (Math.Abs(item.X - point.X) <= 60 && Math.Abs(item.Y - point.Y) <= 60)
                {
                    return item;
                }
            }
            return null;
        }

        private Point AutoPlaceHold(List<BorderWord> lstPosHold)
        {
            foreach (var item in lstPosHold)
            {
                if (!item.IsExist)
                {
                    item.IsExist = true;
                    return new Point(item.X, item.Y);
                }
            }
            return new Point(0, 0);
        }

        private void PlaceEnter(Point point)
        {
            if (!(this.dragObject as ButtonWord).IsPlaced)
            {
                AudioGame.Click();
                AnimateToPosCanvas(this.dragObject, new Point(point.X, point.Y), 300);
                (this.dragObject as ButtonWord).Foregrounddp = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                (this.dragObject as ButtonWord).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#4FBEDB");
                (this.dragObject as ButtonWord).BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#9CABBD");
                (this.dragObject as ButtonWord).IsPlaced = true;
                (this.dragObject as ButtonWord).BorderThickness = new Thickness();

                (this.dragObject as ButtonWord).BaseX = point.X;
                (this.dragObject as ButtonWord).BaseY = point.Y;
            }
        }

        private void PlaceExist(Point point)
        {
            foreach (var item in lstPositionHold)
            {
                if (item.X == point.X && item.Y == point.Y)
                {
                    item.IsExist = false;
                }
            }
            AnimateToPosCanvas(this.dragObject, new Point(point.X, point.Y), 300);
            (this.dragObject as ButtonWord).Foregrounddp = (Color)ColorConverter.ConvertFromString("#3DA4BF");
            (this.dragObject as ButtonWord).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#EFFAFC");
            (this.dragObject as ButtonWord).BorderBrush = borderColor;
            (this.dragObject as ButtonWord).BorderThickness = new Thickness(2);
            (this.dragObject as ButtonWord).IsPlaced = false;
            (this.dragObject as ButtonWord).BaseX = point.X;
            (this.dragObject as ButtonWord).BaseY = point.Y;
        }

        private bool Check_CorrectBorder(string sampleString)
        {
            bool result = false;
            string text = "";
            IEnumerable<ButtonWord> words = CanvasMain.Children.OfType<ButtonWord>().Where(x => !string.IsNullOrEmpty(x.Title) && x.Tag is int).OrderBy(x => x.BaseX);
            foreach (var item in words)
            {
                if (item.IsPlaced)
                {
                    text += item.Title;
                }
            }
            if (text == sampleString.ToUpper())
            {
                result = true;
            }
            viewModel.UserAnswers[viewModel.CurIndex - 1].TextResult = text;
            viewModel.UserAnswers[viewModel.CurIndex - 1].IsCorrect = result;
            return result;
        }

        private async void Continue_Click(object sender, MouseButtonEventArgs e)
        {
            BounceButton button = (sender as BounceButton);
            button.IsHitTestVisible = false;

            viewModel.IsCorrect = Check_CorrectBorder(currentWord);

            if (viewModel.IsCorrect)
                AudioGame.Correct();
            else
                AudioGame.Incorrect();

            if (viewModel.CurIndex < viewModel.TotalQuestion)
            {
                AudioGame.PageFlip();
                await Task.Delay(300);
                CanvasMain.Children.Clear();
                viewModel.CurIndex++;
                currentWord = viewModel.UserAnswers[viewModel.CurIndex - 1].Text.Replace(" ", "");
                viewModel.image = viewModel.UserAnswers[viewModel.CurIndex - 1].Image;
                InitData(currentWord);
            }
            else
            {
                CanvasMain.Children.Clear();
                App.navigationService.Navigate(new ReviewResultPage(viewModel));
            }
            button.IsHitTestVisible = true;
        }

        private void CloseGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
        }

        private void Shuffle_Click(object sender, MouseButtonEventArgs e)
        {
            AudioGame.Flip();
            int i = 0, j = 0;
            var arr = HelpService.ShuffleList<string>(lstPositionWord.Where(m => m.IsExist).Select(m => m.Text).ToList());
            foreach (var item in CanvasMain.Children.OfType<ButtonWord>().Where(m => m.Tag is int))
            {
                if (!item.IsPlaced)
                {
                    item.Title = arr[j];
                    lstPositionWord[i].Text = arr[j];
                    j++;
                }
                i++;
            }
        }
    }

    public class BorderWord
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsExist { get; set; }
        public string Text { get; set; }
    }

    public class DragDropSpellWordManager
    {
        public static List<WordResultModel> lstWordResult;

        public static void Sync(List<WordResultModel> i_lstWordResult)
        {
            lstWordResult = i_lstWordResult;
        }
    }
}