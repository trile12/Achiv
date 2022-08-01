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
    /// Interaction logic for DragCorrectWord.xaml
    /// </summary>
    public partial class DragCorrectWord : UserControl
    {
        private DragCorrectWordViewModel viewModel;
        private GameModel gameModel;
        private UIElement dragObject = null;
        private Point offset;
        private Point pos;
        private List<BorderWord> lstPositionHold;
        private List<BorderWord> lstPositionWord;
        private int Ypos = 20;
        private string[] arrWord;

        private bool isFinish = false;

        public DragCorrectWord(GameModel i_gameModel)
        {
            InitializeComponent();

            DataContext = viewModel = new DragCorrectWordViewModel(i_gameModel);
            gameModel = i_gameModel;
            viewModel.CurIndex = 0;
            viewModel.Questions = new List<QuestionAnwser>();
            foreach (var item in gameModel.questions)
            {
                QuestionAnwser anwser = new QuestionAnwser();
                anwser.Text = item.answers[0].text;
                anwser.CorrectText = item.answers[0].correctText;
                anwser.Image = Environment.CurrentDirectory + "/Assets/Contents/" + item.answers[0].image;
                anwser.numPart = item.answers[0].numOfPart;
                anwser.parts = item.answers[0].parts;
                viewModel.Questions.Add(anwser);
            }
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Restart();
            viewModel.CurIndex = 0;
            NextQuestion(viewModel.Questions[viewModel.CurIndex]);
        }

        private void AutoGenerateEmptyBorder(int n)
        {
            int Xpos = 50;
            int space = 320;
            Ypos = 20;

            lstPositionHold = new List<BorderWord>();
            for (int i = 0; i < n; i++)
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

        private void AutoGenBorder(string[] samepleString)
        {
            lstPositionWord = new List<BorderWord>();
            var samepleString_s = HelpService.ShuffleList<string>(samepleString.ToList());
            int space = 200;
            int Ypos = this.Ypos + 110;
            int Xpos = 0;
            for (int i = 0; i < samepleString_s.Count; i++)
            {
                Border border2 = new Border();
                ButtonWord bt = new ButtonWord();
                bt.Review = "Review 2";
                bt.Title = samepleString_s[i].ToString();
                bt.Foregrounddp = (Color)ColorConverter.ConvertFromString("#E07031");
                bt.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FCF4EE");
                bt.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FAE0CD");
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
                    Text = samepleString_s[i].ToString()
                });
                CanvasMain.Children.Add(bt);
            }
        }

        private async void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isFinish)
                return;
            dragObject = sender as UIElement;
            Panel.SetZIndex(dragObject, 5);
            this.offset = e.GetPosition(this.CanvasMain);
            this.offset.Y -= Canvas.GetTop(this.dragObject);
            this.offset.X -= Canvas.GetLeft(this.dragObject);
            this.CanvasMain.CaptureMouse();
            if (!(dragObject as ButtonWord).IsPlaced)
            {
                AudioGame.Click();
                isFinish = true;
                foreach (var item in lstPositionWord)
                {
                    if (item.X == (dragObject as ButtonWord).BaseX && item.Y == (dragObject as ButtonWord).BaseY)
                    {
                        item.IsExist = false;
                        break;
                    }
                }
                PlaceEnter(AutoPlaceHold(lstPositionHold));

                if (lstPositionHold.Where(x => x.IsExist).ToList().Count == 3)
                {
                    await Task.Delay(500);
                    viewModel.Questions[viewModel.CurIndex - 1].isCorrect = Check_CorrectBorder();
                    if (viewModel.CurIndex < gameModel.total_question)
                    {
                        AudioGame.PageFlip();
                        NextQuestion(viewModel.Questions[viewModel.CurIndex]);
                    }
                    else
                    {
                        await Task.Delay(200);
                        timer.Stop();
                        App.navigationService.Navigate(new ReviewResultPage(viewModel));
                        return;
                    }
                }
                isFinish = false;
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
                PlaceExist(AutoPlaceHold(lstPositionWord));
            }
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragObject == null)
                return;
            var position = e.GetPosition(sender as IInputElement);
            pos.X = position.X - offset.X;
            pos.Y = position.Y - offset.Y;
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (dragObject == null)
                return;
            ButtonWord buttonWord = dragObject as ButtonWord;
            AnimateToPosCanvas(dragObject, new Point(buttonWord.BaseX, buttonWord.BaseY), 300);
            Panel.SetZIndex(dragObject, 1);
            this.dragObject = null;
            this.CanvasMain.ReleaseMouseCapture();
        }

        private void PlaceEnter(Point point)
        {
            if (!(this.dragObject as ButtonWord).IsPlaced)
            {
                AnimateToPosCanvas(this.dragObject, new Point(point.X, point.Y), 300);
                (this.dragObject as ButtonWord).Foregrounddp = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                (this.dragObject as ButtonWord).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#E8894F");
                (this.dragObject as ButtonWord).BorderThickness = new Thickness();
                (this.dragObject as ButtonWord).IsPlaced = true;
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
            (this.dragObject as ButtonWord).Foregrounddp = (Color)ColorConverter.ConvertFromString("#E07031");
            (this.dragObject as ButtonWord).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FCF4EE");
            (this.dragObject as ButtonWord).BorderThickness = new Thickness(2);
            (this.dragObject as ButtonWord).IsPlaced = false;
            (this.dragObject as ButtonWord).BaseX = point.X;
            (this.dragObject as ButtonWord).BaseY = point.Y;
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

        private Point CheckIsPlaceHold(List<BorderWord> lstPosHold, UIElement uIElement)
        {
            ButtonWord buttonWord = uIElement as ButtonWord;
            double x = 0;
            double y = 0;
            x = Canvas.GetLeft(buttonWord);
            y = Canvas.GetTop(buttonWord);

            Point point = new Point(x, y);
            foreach (var item in lstPosHold)
            {
                if (!item.IsExist)
                    if (Math.Abs(item.X - point.X) <= 50 && Math.Abs(item.Y - point.Y) <= 50)
                    {
                        item.IsExist = true;
                        return new Point(item.X, item.Y);
                    }
            }
            return new Point(x, y);
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

        private void Play_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
        }

        private bool Check_CorrectBorder()
        {
            string result = "";
            IEnumerable<ButtonWord> words = CanvasMain.Children.OfType<ButtonWord>().Where(x => !string.IsNullOrEmpty(x.Title) && x.BaseY == 20).OrderBy(x => x.BaseX);
            foreach (var item in words)
            {
                result += item.Title;
            }
            if (result == viewModel.Questions[viewModel.CurIndex - 1].CorrectText)
                return true;
            return false;
        }

        private async void NextQuestion(QuestionAnwser question)
        {
            await Task.Delay(700);
            CanvasMain.Children.Clear();
            timer.Restart();
            viewModel.Image = question.Image;
            arrWord = question.parts.ToArray();
            int numPart = question.numPart;
            AutoGenerateEmptyBorder(numPart);
            AutoGenBorder(arrWord);
            viewModel.CurIndex++;
            isFinish = false;
        }

        private void QuitGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
            timer.Pause();
            App.barTimer = timer;
        }

        private void OnTimeOver(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurIndex < viewModel.TotalQuestion)
            {
                AudioGame.PageFlip();
                NextQuestion(viewModel.Questions[viewModel.CurIndex - 1]);
            }
            else
            {
                App.navigationService.Navigate(new ReviewResultPage(viewModel));
            }
        }
    }
}