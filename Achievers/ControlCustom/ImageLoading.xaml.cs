using Achievers.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Achievers.ControlCustom
{
    /// <summary>
    /// Interaction logic for ImageLoading.xaml
    /// </summary>
    public partial class ImageLoading : Grid
    {
        public static string pathFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AchiverVocab/ImageCache/";

        static ImageLoading()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AchiverVocab/ImageCache/"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AchiverVocab/ImageCache/");
            }
        }

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageLoading), new PropertyMetadata(Stretch.Fill));

        //public PixelFormat PixelFormat
        //{
        //    get { return (PixelFormat)GetValue(PixelFormatProperty); }
        //    set { SetValue(PixelFormatProperty, value); }
        //}

        //public static readonly DependencyProperty PixelFormatProperty =
        //    DependencyProperty.Register("PixelFormat", typeof(PixelFormat), typeof(ImageLoading), new PropertyMetadata(PixelFormats.Default));

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(ImageLoading), new PropertyMetadata("", OnPropertyChanged));

        private WebClient Http = new WebClient();

        private static async void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue as string) && e.NewValue != e.OldValue)
            {
                ImageLoading imageLoading = (ImageLoading)sender;
                LoadingSpinner loading = imageLoading.Children[0] as LoadingSpinner;
                loading.IsLoading = true;
                Image image = imageLoading.Children[1] as Image;
                var fileName = WebUtility.UrlEncode(HelpService.RemoveSpecialCharacter(e.NewValue.ToString()));
                var pathFile = pathFolder + fileName;
                if (!File.Exists(pathFile))
                {
                    try
                    {
                        await imageLoading.Http.DownloadFileTaskAsync(new Uri(e.NewValue.ToString()), pathFile);
                    }
                    catch
                    {
                        if (File.Exists(pathFile))
                        {
                            try
                            {
                                File.Delete(pathFile);
                            }
                            catch { }
                        }
                    }
                }

                if (File.Exists(pathFile))
                {
                    BitmapImage bitmap = null;
                    await Task.Run(() =>
                    {
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(pathFile);
                        bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                        //if (imageLoading.ActualWidth > 0)
                        //{
                        //    bitmap.DecodePixelWidth = (int)Math.Ceiling(imageLoading.ActualWidth) + 20;
                        //}
                        //else if (imageLoading.ActualHeight > 0)
                        //{
                        //    bitmap.DecodePixelHeight = (int)Math.Ceiling(imageLoading.ActualHeight) + 20;
                        //}
                        bitmap.EndInit();
                        bitmap.Freeze();
                    });
                    image.ImageFailed -= Image_ImageFailed;
                    image.ImageFailed += Image_ImageFailed;

                    //if(imageLoading.PixelFormat == PixelFormats.Default)
                    //{
                    //    image.Source = bitmap;
                    //}
                    //else
                    //{
                    //    FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                    //    newFormatedBitmapSource.BeginInit();
                    //    newFormatedBitmapSource.Source = bitmap;
                    //    newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
                    //    newFormatedBitmapSource.EndInit();

                    //    image.Source = newFormatedBitmapSource;
                    //}

                    image.Source = bitmap;
                    loading.IsLoading = false;
                }
            }
        }

        private static async void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            await Task.Delay(1000);
            var img = sender as Image;
            var imgLoading = img.Parent as ImageLoading;
            var fileName = WebUtility.UrlEncode(HelpService.RemoveSpecialCharacter(imgLoading.ImageUrl));
            var pathFile = pathFolder + fileName;
            if (File.Exists(pathFile))
            {
                try
                {
                    File.Delete(pathFile);
                }
                catch { }
            }
            var url = imgLoading.ImageUrl;
            imgLoading.ImageUrl = null;
            imgLoading.ImageUrl = url;
        }

        public ImageLoading()
        {
            InitializeComponent();
        }

        private void ImageLoading_Unloaded(object sender, RoutedEventArgs e)
        {
            var imgLoading = sender as ImageLoading;
            imgLoading.Http?.CancelAsync();
            imgLoading.Http?.Dispose();
        }
    }
}