using Achievers.EasingCustom;
using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using Achievers.Views.Shared.WordMatch;
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
    /// Interaction logic for MatchWord.xaml
    /// </summary>
    public partial class MatchWord : UserControl
    {
        private MatchWordViewModel viewModel;
        private GameModel gameModel;

        private List<WordPart> lstPart;

        private WordPart p1;
        private WordPart p2;

        public MatchWord(GameModel i_gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new MatchWordViewModel(i_gameModel);
            gameModel = i_gameModel;
        }

        private void GenCardGame(List<QuestionAnwser> lstWord)
        {
            lstPart = new List<WordPart>();
            int countLine = 0;
            int space = 0;
            int posY = 0;
            lstWord = ShuffleList(lstWord);
            for (int i = 0; i < lstWord.Count; i++)
            {
                WordPart part = new WordPart();
                part.Title = lstWord[i].Text;
                part.Def = lstWord[i].Def;
                part.Id = lstWord[i].numPart;

                part.MouseLeftButtonUp += Part_MouseLeftButtonUp;

                if (countLine == 4)
                {
                    space = 0;
                    posY += 145;
                    countLine = 0;
                }

                Panel.SetZIndex(part, -1);

                Canvas.SetLeft(part, space);
                Canvas.SetTop(part, posY);

                space += 200;
                mainGame.Children.Add(part);
                lstPart.Add(part);
                countLine++;
            }
        }

        private async void Part_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ob = sender as WordPart;

            if (p1 == null)
            {
                p1 = ob;
                p1.IsActive = true;
                Panel.SetZIndex(p1, 1);
                AudioGame.Flip();
            }
            else if (p2 == null && !ob.IsActive)
            {
                p2 = ob;
                p2.IsActive = true;
                Panel.SetZIndex(p2, 1);
                AudioGame.Flip();
                if (p1.Id == p2.Id)
                {
                    var question = viewModel.Questions[viewModel.CurIndex - 1].questionAnwsers.FirstOrDefault(x => x.numPart == p1.Id);
                    question.isCorrect = true;

                    AnimatePart(p1, p2);
                    AudioGame.Correct();
                    await Task.Delay(1000);
                    mainGame.Children.Remove(p1);
                    mainGame.Children.Remove(p2);
                    lstPart.Remove(p1);
                    lstPart.Remove(p2);
                    img_star.Visibility = Visibility.Collapsed;
                    if (lstPart.Count == 0)
                    {
                        NextQuestion();
                    }
                }
                else
                {
                    AudioGame.Incorrect();
                    await Task.Delay(400);
                    Panel.SetZIndex(p1, -1);
                    Panel.SetZIndex(p2, -1);
                    p1.IsActive = false;
                    p2.IsActive = false;
                }
                p1 = null;
                p2 = null;
            }
        }

        private async void AnimatePart(WordPart p1, WordPart p2)
        {
            Point point_1 = new Point(100, 100);
            Point point_2 = new Point(550, 100);

            Animate(p1, point_1, 250);
            Animate(p2, point_2, 250);

            await Task.Delay(250);
            Animate(p1, new Point(230, 100), 100);
            Animate(p2, new Point(430, 100), 100);
        }

        private void Animate(WordPart p, Point point, int duration)
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
            timeLineX.Completed += (s, o) =>
            {
                Canvas.SetLeft(p, point.X);
                img_star.Visibility = Visibility.Visible;
            };
            timeLineY.FillBehavior = FillBehavior.Stop;
            timeLineY.Completed += (s, o) =>
            {
                Canvas.SetTop(p, point.Y);
            };
            p.BeginAnimation(Canvas.LeftProperty, timeLineX);
            p.BeginAnimation(Canvas.TopProperty, timeLineY);
        }

        private List<QuestionAnwser> ShuffleList(List<QuestionAnwser> lstWord)
        {
            List<QuestionAnwser> newList = new List<QuestionAnwser>();
            for (int i = 0; i < lstWord.Count; i++)
            {
                newList.Add(new QuestionAnwser { Text = lstWord[i].Text, numPart = lstWord[i].numPart });
                newList.Add(new QuestionAnwser { Def = lstWord[i].Def, numPart = lstWord[i].numPart });
            }
            newList = HelpService.ShuffleList<QuestionAnwser>(newList);
            return newList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Restart();
            mainGame.Children.Clear();
            viewModel.Title = gameModel.name;
            viewModel.Instruction = gameModel.instruction;
            viewModel.TotalQuestion = gameModel.total_question;
            viewModel.CurIndex = 1;

            viewModel.Questions = new List<QuestionMatchWord>();
            int id = 0;
            int number = 1;
            foreach (var item in gameModel.questions)
            {
                QuestionMatchWord questionMatch = new QuestionMatchWord();
                questionMatch.number = number++;
                questionMatch.questionAnwsers = new List<QuestionAnwser>();
                id = 0;
                foreach (var i in item.answers)
                {
                    QuestionAnwser anwser = new QuestionAnwser();
                    anwser.Text = i.text.Replace(";", "");
                    anwser.Def = i.definition.Replace(";", "");
                    anwser.numPart = id++;

                    questionMatch.questionAnwsers.Add(anwser);
                }
                viewModel.Questions.Add(questionMatch);
            }

            GenCardGame(viewModel.Questions[viewModel.CurIndex - 1].questionAnwsers);
        }

        private async void NextQuestion()
        {
            await Task.Delay(800);
            timer.Restart();
            mainGame.Children.Clear();
            if (viewModel.CurIndex < viewModel.TotalQuestion)
            {
                AudioGame.PageFlip();
                viewModel.CurIndex++;
                GenCardGame(viewModel.Questions[viewModel.CurIndex - 1].questionAnwsers);
            }
            else
            {
                timer.Stop();
                App.navigationService?.Navigate(new ReviewResultPage(viewModel));
            }
        }

        private void QuitGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
            timer.Pause();
            App.barTimer = timer;
        }

        private void OnTimeOver(object sender, RoutedEventArgs e)
        {
            NextQuestion();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}