using Achievers.ControlCustom;
using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using Achievers.Views.Shared.DragEachWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Achievers.Views.Game
{
    /// <summary>
    /// Interaction logic for DragEachWord.xaml
    /// </summary>
    public partial class DragEachWord : UserControl
    {
        private DragEachWordViewModel viewModel;
        private UIElement dragObject = null;
        private Point offset;
        private Point pos;
        private List<LeftPartModel> lstLeftPart;
        private List<RightPart> lstRightPart;
        private double space_2part = 250;
        private double left_dif = 160;

        private List<Question> questions;

        public DragEachWord(GameModel i_gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new DragEachWordViewModel(i_gameModel);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitData();
            viewModel.CurIndex = 1;
        }

        private Point locationPosition = new Point();

        public Point LocationPosition
        {
            get => locationPosition;
        }

        private void InitData()
        {
            questions = new List<Question>();

            int count = 1;
            foreach (var item in viewModel.GameModel.questions)
            {
                Question question = new Question();
                question.number = count++;
                question.lstCard = new List<CardGameModel>();

                for (int i = 0; i < item.answers.Count; i++)
                {
                    CardGameModel cardGame = new CardGameModel();
                    cardGame.id = i + 1;
                    cardGame.word = item.answers[i].text;
                    cardGame.image = Environment.CurrentDirectory + "/Assets/Contents/" + item.answers[i].image;
                    question.lstCard.Add(cardGame);
                }
                questions.Add(question);
            }
            GenCardGame(questions[0].lstCard);
        }

        private void GenCardGame(List<CardGameModel> lstCard)
        {
            lstLeftPart = new List<LeftPartModel>();
            lstRightPart = new List<RightPart>();
            int stepTop = 150;

            List<CardGameModel> lstRight = new List<CardGameModel>(lstCard);

            var lstOrder = HelpService.ShuffleList<int>(lstCard.Select((m, index) => index).ToList());

            for (int i = 0; i < lstCard.Count; i++)
            {
                LeftPart left = new LeftPart();
                left.image_url = lstCard[i].image;

                RightPart right = new RightPart();

                right.MouseDown += Border_PreviewMouseDown;
                right.Title = lstRight[i].word;
                right.Id = lstRight[i].id;

                int x_left = 0;
                int y_left = stepTop * i;

                Canvas.SetTop(left, y_left);
                Canvas.SetLeft(left, x_left);

                Canvas.SetTop(right, stepTop * lstOrder[i] + 26);
                Canvas.SetLeft(right, space_2part);

                right.Tag = i;

                CanvasMain.Children.Add(left);
                CanvasMain.Children.Add(right);
                lstLeftPart.Add(new LeftPartModel()
                {
                    Layout = new Rect(x_left, stepTop * i + 26, CanvasMain.ActualWidth, stepTop),
                    Index = lstOrder[i]
                });
                lstRightPart.Add(right);
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
            this.CanvasMain.CaptureMouse();
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragObject == null)
                return;
            var position = e.GetPosition(sender as FrameworkElement);
            pos.Y = position.Y - offset.Y;
            Canvas.SetTop(this.dragObject, pos.Y);
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.CanvasMain.ReleaseMouseCapture();
            if (dragObject == null)
                return;
            int itemIndex = (int)dragObject.GetValue(TagProperty);
            int index = lstLeftPart[itemIndex].Index;
            if (e.Timestamp - timeStamp > 200)
            {
                var position = e.GetPosition(sender as FrameworkElement);
                pos.Y = position.Y;
                int collapsedIndex = lstLeftPart.FindIndex(m => m.Layout.Contains(pos));
                int collapsedItemIndex = lstLeftPart.FindIndex(m => m.Index == collapsedIndex);
                if (collapsedIndex != -1 && index != collapsedIndex)
                {
                    var collapsedItem = lstRightPart[collapsedItemIndex];
                    lstLeftPart[itemIndex].Index = collapsedIndex;
                    lstLeftPart[itemIndex].IsMatch = true;
                    lstLeftPart[collapsedItemIndex].Index = index;
                    lstLeftPart[collapsedItemIndex].IsMatch = true;
                    AnimateToPosCanvas(dragObject, new Point(left_dif, lstLeftPart[collapsedIndex].Layout.Y), 300);
                    AnimateToPosCanvas(collapsedItem, new Point(left_dif, lstLeftPart[index].Layout.Y), 300);
                    AudioGame.Flip();
                }
                else
                {
                    AnimateToPosCanvas(dragObject, new Point(Canvas.GetLeft(dragObject), lstLeftPart[index].Layout.Y), 300);
                }
            }
            else
            {
                lstLeftPart[itemIndex].IsMatch = !lstLeftPart[itemIndex].IsMatch;
                if (lstLeftPart[itemIndex].IsMatch)
                    AudioGame.Flip();
                AnimateToPosCanvas(dragObject, new Point(lstLeftPart[itemIndex].IsMatch ? left_dif : space_2part, lstLeftPart[index].Layout.Y), 300);
            }

            Panel.SetZIndex(dragObject, 1);
            this.dragObject = null;
        }

        private void AnimateToPosCanvas(UIElement uIElement, Point point, int duration)
        {
            var timeLineX = new DoubleAnimation()
            {
                To = point.X,
                Duration = new TimeSpan(0, 0, 0, 0, duration),
                EasingFunction = new BezierEase()
            };
            var timeLineY = new DoubleAnimation()
            {
                To = point.Y,
                Duration = new TimeSpan(0, 0, 0, 0, duration),
                EasingFunction = new BezierEase()
            };
            timeLineX.FillBehavior = FillBehavior.Stop;
            timeLineX.Completed += (s, e) =>
            {
                Canvas.SetLeft(uIElement, point.X);
            };
            timeLineY.FillBehavior = FillBehavior.Stop;
            timeLineY.Completed += (s, e) =>
            {
                Canvas.SetTop(uIElement, point.Y);
            };
            uIElement.BeginAnimation(Canvas.LeftProperty, timeLineX);
            uIElement.BeginAnimation(Canvas.TopProperty, timeLineY);
        }

        private bool CheckCorrect(int index)
        {
            if (questions.Count == index)
                return false;

            for (int i = 0; i < lstLeftPart.Count; i++)
            {
                var card = questions[index].lstCard[lstLeftPart[i].Index];

                CardGameModel resultCard = new CardGameModel();
                resultCard.id = card.id;
                resultCard.image = card.image;
                resultCard.word = card.word;
                resultCard.des = card.des;
                resultCard.isCorrect = lstLeftPart[i].Index == i && lstLeftPart[i].IsMatch;

                questions[index].lstCardResult.Add(resultCard);
            }
            return lstLeftPart.Where((m, mIndex) => mIndex != m.Index || !m.IsMatch).Count() == 0;
        }

        public DependencyObject GetParent()
        {
            if (Parent != null)
            {
                return Parent;
            }
            else if (VisualParent != null)
            {
                return VisualParent;
            }
            return null;
        }

        private void CloseGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
        }

        private async void Continue_Click(object sender, MouseButtonEventArgs e)
        {
            BounceButton button = (sender as BounceButton);
            button.IsHitTestVisible = false;

            imgResult.Visibility = Visibility.Visible;
            viewModel.IsCorrect = CheckCorrect(viewModel.CurIndex - 1);

            if (viewModel.IsCorrect)
                AudioGame.Correct();
            else
                AudioGame.Incorrect();

            questions[viewModel.CurIndex - 1].isCorrect = viewModel.IsCorrect;
            viewModel.UserAnswers[viewModel.CurIndex - 1] = lstLeftPart.Select((m, mIndex) => new DragEachWordAnswer()
            {
                Index = m.Index,
                IsConnected = m.IsMatch,
                IsCorrect = mIndex == m.Index && m.IsMatch
            }).ToList();

            if (viewModel.CurIndex < viewModel.GameModel.total_question)
            {
                await Task.Delay(1000);
                imgResult.Visibility = Visibility.Collapsed;
                CanvasMain.Children.Clear();

                GenCardGame(questions[viewModel.CurIndex].lstCard);
                viewModel.CurIndex++;
            }
            else
            {
                await Task.Delay(1000);
                imgResult.Visibility = Visibility.Collapsed;
                CanvasMain.Children.Clear();
                App.navigationService.Navigate(new ReviewResultPage(viewModel));
            }

            button.IsHitTestVisible = true;
        }
    }

    public class CardGameModel
    {
        public int id { get; set; }
        public string word { get; set; }
        public string des { get; set; }
        public string image { get; set; }

        public bool isCorrect { get; set; }
    }

    public class Question
    {
        public Question()
        {
            lstCardResult = new List<CardGameModel>();
        }

        public int number { get; set; }
        public bool isCorrect { get; set; }
        public List<CardGameModel> lstCard { get; set; }
        public List<CardGameModel> lstCardResult { get; set; }
    }

    public class LeftPartModel
    {
        public Rect Layout { get; set; }
        public bool IsMatch { get; set; }
        public int Index { get; set; }
    }

    public class DragEachWordGameManager
    {
        public static List<Question> questions;

        public static void Sync(List<Question> i_questions)
        {
            questions = i_questions;
        }
    }
}