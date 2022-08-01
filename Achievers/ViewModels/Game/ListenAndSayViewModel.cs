using Achievers.Models;
using Achievers.Views.GameResult;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Achievers.ViewModels.Game
{
    public class ListenAndSayViewModel : IGameActivity
    {
        public ListenAndSayViewModel(GameModel gameModel)
        {
            MainStartColor = "#9E9DE3";
            MainEndColor = "#4D4BD1";
            SubStartColor = "#EFEEFA";
            SubEndColor = "#D5D4FC";
            ImageStar = "/Achievers;component/Assets/Images/game-result-star-3.png";
            ImageIcon = "/Achievers;component/Assets/Images/ft-r3-g.png";
            IsFinish = gameModel.next_unit_review == null;
            GameModel = gameModel;

            Instruction = gameModel.instruction;

            Questions = new List<ListenAndSayQuestion>();
            foreach (var item in gameModel.questions)
            {
                ListenAndSayQuestion listenAndSay = new ListenAndSayQuestion();
                listenAndSay.Text = item.answers[0].text;
                listenAndSay.Audio = Environment.CurrentDirectory + "/Assets/Contents/" + item.answers[0].audio;
                listenAndSay.Image = Environment.CurrentDirectory + "/Assets/Contents/" + item.answers[0].image;
                Questions.Add(listenAndSay);
            }

            ShuffleQuestions = new ObservableCollection<ListenAndSayQuestion>();
        }

        private string instruction;

        public string Instruction
        {
            get { return instruction; }
            set
            {
                if (instruction != value)
                {
                    instruction = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isRecording;

        public bool IsRecording
        {
            get { return isRecording; }
            set
            {
                if (isRecording != value)
                {
                    isRecording = value;
                    OnPropertyChanged();
                }
            }
        }

        private int questionIndex = -1;

        public int QuestionIndex
        {
            get { return questionIndex; }
            set
            {
                if (questionIndex != value)
                {
                    questionIndex = value;
                    ShowAnswer = false;
                    if (questionIndex < Questions.Count)
                    {
                        if (questionIndex > -1) CurrentQuestion = Questions[questionIndex];
                        else currentQuestion = null;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public List<ListenAndSayQuestion> Questions { get; set; }

        public ObservableCollection<ListenAndSayQuestion> ShuffleQuestions { get; set; }

        private ListenAndSayQuestion currentQuestion;

        public ListenAndSayQuestion CurrentQuestion
        {
            get { return currentQuestion; }
            set
            {
                if (currentQuestion != value)
                {
                    currentQuestion = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool questionStage;

        public bool QuestionStage
        {
            get { return questionStage; }
            set
            {
                if (questionStage != value)
                {
                    questionStage = value;
                    if (questionStage) Instruction = GameModel.sub_instruction;
                    OnPropertyChanged();
                }
            }
        }

        private bool showAnswer;

        public bool ShowAnswer
        {
            get { return showAnswer; }
            set
            {
                if (showAnswer != value)
                {
                    showAnswer = value;
                    OnPropertyChanged();
                }
            }
        }

        public override double GetScore()
        {
            return (double)Questions.Count(m => m.IsCorrect == true) / Questions.Count * 100;
        }

        public override UIElement GetResultUI()
        {
            return new ListenAndSayResult(this);
        }
    }

    public class ListenAndSayQuestion
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public string Audio { get; set; }
        public bool IsCorrect { get; set; }
    }
}