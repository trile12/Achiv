using Achievers.ControlCustom;
using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Game
{
    /// <summary>
    /// Interaction logic for Memory.xaml
    /// </summary>
    public partial class Memory : UserControl
    {
        private MemoryViewModel viewModel;

        private List<FlipCard> lstPart;
        private FlipCard p1;
        private FlipCard p2;

        public Memory(GameModel i_gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new MemoryViewModel(i_gameModel);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.CurIndex = 1;

            viewModel.Questions = new List<QuestionMatchWord>();
            int id = 0;
            foreach (var item in viewModel.GameModel.questions)
            {
                QuestionMatchWord questionMatch = new QuestionMatchWord();
                questionMatch.questionAnwsers = new List<QuestionAnwser>();
                id = 0;
                foreach (var i in item.answers)
                {
                    QuestionAnwser anwser = new QuestionAnwser();
                    anwser.Text = i.text;
                    anwser.Def = i.definition;
                    anwser.Image = Environment.CurrentDirectory + "/Assets/Contents/" + i.image;
                    anwser.numPart = id++;

                    questionMatch.questionAnwsers.Add(anwser);
                }
                viewModel.Questions.Add(questionMatch);
            }

            GenCardGame(viewModel.Questions[viewModel.CurIndex - 1].questionAnwsers);
        }

        private void GenCardGame(List<QuestionAnwser> lstWord)
        {
            lstPart = new List<FlipCard>();
            int countLine = 0;
            int space = -120;
            int posY = 20;
            lstWord = ShuffleList(lstWord);
            for (int i = 0; i < lstWord.Count; i++)
            {
                FlipCard part = new FlipCard();
                part.BookImage = "/Achievers;component/Assets/Images/general.png";
                part.Title = lstWord[i].Text;
                part.FontSize = 50;
                part.Images = lstWord[i].Image;
                //part.Def = lstWord[i].Def;
                part.Id = lstWord[i].numPart;
                part.Width = 250;
                part.Height = 195;

                part.MouseLeftButtonUp += Part_MouseLeftButtonUp;

                if (countLine == 4)
                {
                    space = -120;
                    posY += 230;
                    countLine = 0;
                }

                Panel.SetZIndex(part, -1);

                Canvas.SetLeft(part, space);
                Canvas.SetTop(part, posY);

                space += 280;
                mainGame.Children.Add(part);
                lstPart.Add(part);
                countLine++;
            }
        }

        private List<QuestionAnwser> ShuffleList(List<QuestionAnwser> lstWord)
        {
            List<QuestionAnwser> newList = new List<QuestionAnwser>();
            for (int i = 0; i < lstWord.Count; i++)
            {
                newList.Add(new QuestionAnwser { Text = lstWord[i].Text, numPart = lstWord[i].numPart });
                newList.Add(new QuestionAnwser { Image = lstWord[i].Image, numPart = lstWord[i].numPart });
            }
            newList = HelpService.ShuffleList<QuestionAnwser>(newList);
            return newList;
        }

        private async void Part_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ob = sender as FlipCard;

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
                AudioGame.Flip();

                if (p1.Id == p2.Id)
                {
                    var question = viewModel.Questions[viewModel.CurIndex - 1].questionAnwsers.FirstOrDefault(x => x.numPart == p1.Id);
                    question.isCorrect = true;

                    //AnimatePart(p1, p2);
                    AudioGame.Correct();
                    await Task.Delay(1000);
                    mainGame.Children.Remove(p1);
                    mainGame.Children.Remove(p2);
                    lstPart.Remove(p1);
                    lstPart.Remove(p2);
                    if (lstPart.Count == 0)
                    {
                        NextQuestion();
                    }
                }
                else
                {
                    AudioGame.Incorrect();
                    await Task.Delay(1000);
                    p1.IsActive = false;
                    p2.IsActive = false;
                }
                p1 = null;
                p2 = null;
            }
        }

        private async void NextQuestion()
        {
            await Task.Delay(800);
            mainGame.Children.Clear();
            if (viewModel.CurIndex < viewModel.TotalQuestion)
            {
                viewModel.CurIndex++;
                GenCardGame(viewModel.Questions[viewModel.CurIndex - 1].questionAnwsers);
            }
            else
            {
                App.navigationService?.Navigate(new ReviewResultPage(viewModel));
            }
        }

        private void QuitGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
        }
    }
}