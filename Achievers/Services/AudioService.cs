using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Achievers.Services
{
    public class AudioService
    {
        private List<WaveOutEvent> Audios { get; set; }

        public AudioService()
        {
            Audios = new List<WaveOutEvent>();
        }

        public Task Init(string[] audioUrls)
        {
            return Task.Run(() =>
            {
                Dispose();
                foreach (string audio in audioUrls)
                {
                    using (var mf = new MediaFoundationReader(audio))
                    {
                        WaveOutEvent waveOut = new WaveOutEvent();
                        waveOut.Init(mf);
                        Audios.Add(waveOut);
                    }
                }
            });
        }

        public void Dispose()
        {
            foreach (var audio in Audios)
            {
                audio.Dispose();
            }
            Audios.Clear();
        }

        public void Play(int index = 0)
        {
            if (index < Audios.Count)
            {
                var audio = Audios[index];
                if (audio.PlaybackState != PlaybackState.Playing)
                {
                    Audios[index].Play();
                }
            }
        }

        public WaveOutEvent GetAudio(int index = 0)
        {
            if (index < Audios.Count)
            {
                return Audios[index];
            }
            return null;
        }
    }

    public static class AudioGame
    {
        public static void Click()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/click.mp3");
        }

        public static void Correct()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/correct.mp3");
        }

        public static void Incorrect()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/incorrect.mp3");
        }

        public static void GoodJob()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/goodjob.mp3");
        }

        public static void OhNo()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/ohNo.mp3");
        }

        public static void PageFlip()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/page-flip-01a.mp3");
        }

        public static void Flip()
        {
            Play("pack://application:,,,/Achievers;component/Assets/Audios/flip.mp3");
        }

        public static void Play(string resource)
        {
            Task.Run(() =>
            {
                using (Stream stream = Application.GetResourceStream(new Uri(resource)).Stream)
                {
                    stream.Position = 0;
                    using (WaveStream blockAlignedStream =
                        new BlockAlignReductionStream(
                            WaveFormatConversionStream.CreatePcmStream(
                                new Mp3FileReader(stream))))
                    {
                        WaveOutEvent waveOut = new WaveOutEvent();
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            });
        }
    }
}