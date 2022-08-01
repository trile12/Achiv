using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using Achievers.Views.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Achievers.Views.Game
{
    /// <summary>
    /// Interaction logic for HangMan.xaml
    /// </summary>
    public partial class HangMan : UserControl, INotifyPropertyChanged
    {
        private HangManViewModel viewModel;
        private GameModel gameModel;
        private UIElement dragObject = null;
        private Point offset;
        private Point pos;
        private List<BorderWord> lstPositionHold;
        private List<BorderWord> lstPositionWord;
        private int Ypos = 20;

        private string templateWord;

        private bool isGameOver = false;
        private int checkWordComplete = 0;

        private int _leftTime = 10;

        private bool isHint = false;

        public int leftTime
        {
            get { return _leftTime; }
            set
            {
                _leftTime = value;
                OnPropertyChanged();
            }
        }

        public HangMan(GameModel i_gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new HangManViewModel(i_gameModel);
            leftTime = 10;
            gameModel = i_gameModel;
            viewModel.Instruction = gameModel.instruction;
            viewModel.TotalQuestion = gameModel.total_question;

            viewModel.Questions = new List<QuestionAnwser>();
            foreach (var item in gameModel.questions)
            {
                QuestionAnwser anwser = new QuestionAnwser();
                anwser.Text = item.answers[0].text;
                anwser.Image = Environment.CurrentDirectory + "/Assets/Contents/" + item.answers[0].image;
                anwser.Hint = item.answers[0].hint;
                viewModel.Questions.Add(anwser);
            }
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            leftTime = 10;
            viewModel.CurIndex = 0;
            viewModel.Score = 100;
            viewModel.Title = "Excellent!";
            viewModel.Descrition = "You’ve completed the unit";
            NextQuestion(viewModel.Questions[viewModel.CurIndex]);
        }

        private void NextQuestion(QuestionAnwser question)
        {
            border_hint.Width = 0;
            isHint = false;
            leftTime = 10;
            checkWordComplete = 0;
            CanvasMain.Children.Clear();
            viewModel.Image = question.Image;
            viewModel.Hint = question.Hint;
            templateWord = question.Text.Replace(" ", "");
            AutoGenerateEmptyBorder(templateWord);
            AutoGenBorder("ABCDEFGHIJKLMNOPQRSTUVWXYZ-");
            viewModel.CurIndex++;
        }

        private void AutoGenerateEmptyBorder(string sampleString)
        {
            double space = 255 + (CanvasMain.ActualWidth - sampleString.Length * 48) / 2;
            Ypos = 600;
            double Xpos = 720;
            lstPositionHold = new List<BorderWord>();
            for (int i = 0; i < sampleString.Length; i++)
            {
                Border border2 = new Border();
                ButtonWord bt = new ButtonWord();
                bt.Width = 48;
                bt.Height = 48;
                bt.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
                bt.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#E7EFF8");

                Xpos = space;
                space += 55;

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
            //samepleString = string.Join("", HelpService.ShuffleList<char>(samepleString.ToList()));
            int space = 75;
            int Ypos = 255;
            int Xpos = 75;

            for (int i = 0; i < samepleString.Length; i++)
            {
                Border border2 = new Border();
                ButtonWord bt = new ButtonWord();
                bt.Review = "Review 4";
                bt.Title = samepleString[i].ToString();
                bt.Width = 48;
                bt.Height = 48;
                bt.Foregrounddp = (Color)ColorConverter.ConvertFromString("#44B88F");
                bt.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#F2FAF7");
                bt.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#A1E5CD");
                bt.PreviewMouseDown += Border_PreviewMouseDown;

                if (i == 0)
                    Xpos = 75;
                else
                    Xpos = space += 55;

                if (samepleString[i] == 'I' || samepleString[i] == 'Q' || samepleString[i] == 'Y')
                {
                    space = Xpos = 75;
                    Ypos = Ypos + 55;
                }

                bt.BaseX = Xpos;
                bt.BaseY = Ypos;

                Panel.SetZIndex(bt, -1);
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

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragObject = sender as UIElement;
            Panel.SetZIndex(dragObject, 1);
            this.offset = e.GetPosition(this.CanvasMain);
            this.offset.Y -= Canvas.GetTop(this.dragObject);
            this.offset.X -= Canvas.GetLeft(this.dragObject);
            this.CanvasMain.CaptureMouse();

            if (e.ClickCount > 0 && !isGameOver)
            {
                if (!(dragObject as ButtonWord).IsPlaced && !(dragObject as ButtonWord).IsWrong)
                {
                    foreach (var item in lstPositionWord)
                    {
                        if (item.X == (dragObject as ButtonWord).BaseX && item.Y == (dragObject as ButtonWord).BaseY)
                        {
                            item.IsExist = false;
                            break;
                        }
                    }
                    SolveCorrectWord(dragObject as ButtonWord);
                }
            }
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (this.dragObject == null)
            //    return;

            //ButtonWord buttonWord = dragObject as ButtonWord;
            //if (!buttonWord.IsPlaced)
            //{
            //    var position = e.GetPosition(sender as IInputElement);
            //    pos.X = position.X - offset.X;
            //    pos.Y = position.Y - offset.Y;
            //    Canvas.SetTop(this.dragObject, pos.Y);
            //    Canvas.SetLeft(this.dragObject, pos.X);
            //}
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (dragObject == null)
                return;

            ButtonWord buttonWord = dragObject as ButtonWord;
            if (!buttonWord.IsPlaced)
            {
                AnimateToPosCanvas(dragObject, new Point(buttonWord.BaseX, buttonWord.BaseY));
                Panel.SetZIndex(dragObject, 1);
            }
            this.dragObject = null;
            this.CanvasMain.ReleaseMouseCapture();
        }

        private void PlaceEnter(Point point)
        {
            if (!(this.dragObject as ButtonWord).IsPlaced)
            {
                AnimateToPosCanvas(this.dragObject, new Point(point.X, point.Y));
                (this.dragObject as ButtonWord).Foregrounddp = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                (this.dragObject as ButtonWord).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#44B88F");
                (this.dragObject as ButtonWord).BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#9CABBD");
                (this.dragObject as ButtonWord).IsPlaced = true;

                (this.dragObject as ButtonWord).BaseX = point.X;
                (this.dragObject as ButtonWord).BaseY = point.Y;

                (this.dragObject as ButtonWord).Width = 48;
                (this.dragObject as ButtonWord).Height = 48;
            }
        }

        private void PlaceObjEnter(ButtonWord buttonWord, Point point)
        {
            AnimateToPosCanvas(buttonWord, new Point(point.X - 5, point.Y - 5));
            buttonWord.Foregrounddp = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            buttonWord.IsActive = true;
            //buttonWord.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#44B88F");
            //buttonWord.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#9CABBD");
            buttonWord.IsPlaced = true;
            buttonWord.BaseX = point.X;
            buttonWord.BaseY = point.Y;

            buttonWord.Width = 60;
            buttonWord.Height = 60;
        }

        private async void PlaceAndCloneObjectEnter(List<Point> points)
        {
            if ((this.dragObject as ButtonWord) == null)
                return;

            var thisButton = this.dragObject as ButtonWord;

            for (int i = 0; i < points.Count; i++)
            {
                ButtonWord cloneOb = new ButtonWord();
                cloneOb.Title = thisButton.Title;
                cloneOb.Foregrounddp = thisButton.Foregrounddp;
                cloneOb.BaseX = thisButton.BaseX;
                cloneOb.BaseY = thisButton.BaseY;
                cloneOb.Review = thisButton.Review;

                Canvas.SetTop(cloneOb, cloneOb.BaseY);
                Canvas.SetLeft(cloneOb, cloneOb.BaseX);

                PlaceObjEnter(cloneOb, points[i]);
                CanvasMain.Children.Add(cloneOb);
                checkWordComplete++;
            }
            CanvasMain.Children.Remove(thisButton);

            if (checkWordComplete == templateWord.Length)
            {
                AnimImageFadeIn();
                await Task.Delay(2000);
                if (viewModel.CurIndex < viewModel.TotalQuestion)
                    NextQuestion(viewModel.Questions[viewModel.CurIndex]);
                else
                {
                    App.navigationService.Navigate(new ReviewResultPage(viewModel));
                    //NavigationService?.Navigate(new ResultBar(GameName.Hangman, gameModel));
                }
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
            AnimateToPosCanvas(this.dragObject, new Point(point.X, point.Y));

            (this.dragObject as ButtonWord).Foregrounddp = (Color)ColorConverter.ConvertFromString("#E07031");
            (this.dragObject as ButtonWord).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FCF4EE");
            (this.dragObject as ButtonWord).BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FAE0CD");

            (this.dragObject as ButtonWord).IsPlaced = false;
            (this.dragObject as ButtonWord).BaseX = point.X;
            (this.dragObject as ButtonWord).BaseY = point.Y;

            //(this.dragObject as ButtonWord).Width = 60;
            //(this.dragObject as ButtonWord).Height = 60;
        }

        private void AnimateToPosCanvas(UIElement uIElement, Point point)
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

        private void AnimImageFadeIn()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 3;
            animation.Duration = new TimeSpan(0, 0, 0, 1, 800);
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s, e) =>
            {
                imgResult.Opacity = 0;
            };
            imgResult.BeginAnimation(Image.OpacityProperty, animation);
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

        private void SolveCorrectWord(ButtonWord buttonWord)
        {
            List<int> index = new List<int>();
            for (int i = 0; i < templateWord.Length; i++)
            {
                if (buttonWord.Title == templateWord[i].ToString().ToUpper())
                {
                    index.Add(i);
                }
            }
            if (index.Count > 0)
            {
                List<Point> lstPoint = new List<Point>();
                foreach (int i in index)
                {
                    lstPoint.Add(new Point(lstPositionHold[i].X, lstPositionHold[i].Y));
                }
                PlaceAndCloneObjectEnter(lstPoint);
                AudioGame.Correct();
            }
            else
            {
                CheckGameOver();
                buttonWord.IsWrong = true;
                AudioGame.Incorrect();
            }
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

        private void CheckGameOver()
        {
            if (leftTime >= 0)
                leftTime--;
            if (leftTime == -1)
            {
                viewModel.Score = 0;
                viewModel.Title = "Oh, nooo!";
                viewModel.ImageIcon = "/Achievers;component/Assets/Images/nolife.png";
                viewModel.Descrition = "You have run out of lives.\nTry again or the game will end here";
                App.navigationService.Navigate(new ReviewResultPage(viewModel));
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
        }

        private bool Check_CorrectBorder(string[] sampleString)
        {
            string result = "";
            IEnumerable<ButtonWord> words = CanvasMain.Children.OfType<ButtonWord>().Where(x => !string.IsNullOrEmpty(x.Title) && x.BaseY == 20).OrderBy(x => x.BaseX);
            foreach (var item in words)
            {
                result += item.Title;
            }
            if (result == "BARBECUE")
                return true;
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void QuitGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
        }

        private void Hint_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (!isHint)
            {
                border_hint.RenderTransform = new ScaleTransform();

                var timeLine = new DoubleAnimation(!isHint ? 425 : 0, new TimeSpan(0, 0, 0, 0, 300));
                timeLine.EasingFunction = new BezierEase() { EasingMode = EasingMode.EaseInOut };

                timeLine.FillBehavior = FillBehavior.Stop;
                timeLine.Completed += (s, d) =>
                {
                    border_hint.Width = 425;
                };

                border_hint.BeginAnimation(WidthProperty, timeLine);

                CheckGameOver();
                isHint = true;
            }
        }
    }
}