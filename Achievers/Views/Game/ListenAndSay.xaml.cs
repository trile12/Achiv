using Achievers.ControlCustom;
using Achievers.Models;
using Achievers.Services;
using Achievers.ViewModels.Game;
using NAudio.Wave;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Achievers.Views.Game
{
    /// <summary>
    /// Interaction logic for ListenAndSay.xaml
    /// </summary>
    public partial class ListenAndSay : UserControl
    {
        private ListenAndSayViewModel viewModel;
        private AudioService audioService = new AudioService();
        private WaveInEvent waveIn = new WaveInEvent();
        private WaveFileWriter writer = null;
        private string[] recordUrls;
        private string outputFolder = "";
        private int numOfWrong = 0;

        public ListenAndSay(GameModel gameModel)
        {
            InitializeComponent();
            DataContext = viewModel = new ListenAndSayViewModel(gameModel);

            recordUrls = new string[viewModel.Questions.Count];
            outputFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(outputFolder);

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.RecordingStopped += WaveIn_RecordingStopped;
        }

        ~ListenAndSay()
        {
            if (System.IO.Directory.Exists(outputFolder))
            {
                System.IO.Directory.Delete(outputFolder, true);
            }
        }

        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            writer?.Dispose();
            writer = null;
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            writer.Write(e.Buffer, 0, e.BytesRecorded);
            if (writer.Position > waveIn.WaveFormat.AverageBytesPerSecond * 10)
            {
                waveIn.StopRecording();
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.QuestionIndex = -1;
            await audioService.Init(viewModel.Questions.Select(m => m.Audio).ToArray());
            viewModel.QuestionIndex++;
            await Task.Delay(1000);
            audioService.Play(viewModel.QuestionIndex);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            audioService?.Dispose();
            waveIn?.StopRecording();
            waveIn?.Dispose();
        }

        private void OnClickRecord(object sender, DataRoutedEventArgs e)
        {
            if ((bool)e.Data == true)
            {
                viewModel.IsRecording = true;
                timer.Restart();

                recordUrls[viewModel.QuestionIndex] = System.IO.Path.Combine(outputFolder, string.Format("record_{0}.wav", viewModel.QuestionIndex));
                writer = new WaveFileWriter(recordUrls[viewModel.QuestionIndex], waveIn.WaveFormat);
                waveIn.StartRecording();
            }
        }

        private async void OnTimeOver(object sender, RoutedEventArgs e)
        {
            var btnRecord = HelpService.FindChildByName(mainGame, "btnRecord") as RecordButton;
            btnRecord.Stop();
            waveIn.StopRecording();
            viewModel.IsRecording = false;
            if (++viewModel.QuestionIndex < viewModel.Questions.Count)
            {
                await Task.Delay(500);
                audioService.Play(viewModel.QuestionIndex);
            }
            else
            {
                viewModel.QuestionStage = true;
                viewModel.QuestionIndex = -1;
                await audioService.Init(recordUrls);
                LoadQuestion();
            }
        }

        private async void LoadQuestion()
        {
            if (++viewModel.QuestionIndex < viewModel.Questions.Count)
            {
                numOfWrong = 0;
                viewModel.ShuffleQuestions.Clear();
                foreach (var item in HelpService.ShuffleList<ListenAndSayQuestion>(viewModel.Questions))
                {
                    viewModel.ShuffleQuestions.Add(item);
                }
                await Task.Delay(500);
                audioService.Play(viewModel.QuestionIndex);
                mainGame.IsHitTestVisible = true;
            }
            else
            {
                NavigationService.GetNavigationService(this)?.Navigate(new ReviewResultPage(viewModel));
            }
        }

        private async void OnClickAnswer(object sender, MouseButtonEventArgs e)
        {
            var el = sender as FrameworkElement;
            var data = el.DataContext as ListenAndSayQuestion;
            bool isNext = false;
            if (data.Text == viewModel.CurrentQuestion.Text)
            {
                viewModel.Questions[viewModel.QuestionIndex].IsCorrect = true;
                isNext = true;
                AudioGame.Correct();
            }
            else
            {
                numOfWrong++;
                el.Visibility = Visibility.Hidden;
                el.IsHitTestVisible = false;
                if (numOfWrong > 1)
                {
                    isNext = true;
                }
                AudioGame.Incorrect();
            }

            if (isNext)
            {
                viewModel.ShowAnswer = true;
                mainGame.IsHitTestVisible = false;
                await Task.Delay(1000);
                LoadQuestion();
            }
        }

        private void QuitGame_Click(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ShowPopupQuitGame();
            timer.Pause();
            App.barTimer = timer;
        }
    }
}