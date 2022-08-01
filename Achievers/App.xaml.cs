using Achievers.ControlCustom;
using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using Achievers.Views.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Achievers
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string UserId = "sangtest";
        public static ObservableCollection<UnitTabModel> Units;
        public static ObservableCollection<UnitTabReviewModel> Reviews;

        public static List<WordModel> lstAllWord;

        public static List<string> lstBackground;
        public static List<string> lstBorderBrush;

        public static NavigationService navigationService;
        public static CircleProgressBarTimer barTimer;

        public static double totalProgress = 0;

        public static DateTime DateExpired { get; set; }

        public static bool IsTrial { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogService.Debug("App", "=============================");
            if (!File.Exists(Common.LicenseFile))
            {
                LogService.Debug("App", "License file not exist");
                Application.Current.Shutdown();
                return;
            }
            string data = Common.Decrypt(File.ReadAllText(Common.LicenseFile));
            if (string.IsNullOrEmpty(data))
            {
                LogService.Debug("App", "Can't read License file");
                Application.Current.Shutdown();
                return;
            }
            var userInfo = JsonConvert.DeserializeObject<UserInfoModel>(data);

            if (userInfo.AppId != Common.AppId || (userInfo.Device_Id != Common.DeviceIdTest && userInfo.Device_Id != Common.GetDeviceId()))
            {
                LogService.Debug("App", "Device_id not exist");
                Application.Current.Shutdown();
                return;
            }
            App.IsTrial = userInfo.Mode == "Trial";

            LogService.Debug("App", "IsTrial:" + App.IsTrial);
            LogService.Debug("App", userInfo.Mode);

#if DEBUG
            App.UserId = "sangtest2";
#else
            App.UserId = userInfo.User_Id != string.Empty ? userInfo.User_Id : userInfo.Device_Id;
#endif
            if (Common.Ping())
            {
                //DbService.CreateGameModel();
                DbService.SynceData();
            }

            InitData();
            GetAllWord();

            lstBackground = new List<string>() { "#FCF0CF", "#D4F4FA", "#F5D7ED", "#DCF5EC" };
            lstBorderBrush = new List<string> { "#F5D98C", "#A5E4F2", "#EB96D2", "#A1E5CD" };
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogService.Error("", e.Exception);
            e.Handled = true;
        }

        private async void InitData()
        {
            //var lstUnitModel = await HttpService.GetAllUnitByUser();
            UnitTabQuery query = new UnitTabQuery();
            var lstUnitModel = await query.GetUnit();

            Units = new ObservableCollection<UnitTabModel>();

            string[] startColors = new string[] { "#A5E4F2", "#A3A2EB", "#EB96D2", "#99C5F7", "#F2B28A", "#A1E5CD", "#EBD085", "#ED9691", "#BFCCDB", "#A3A2EB" };
            string[] endColors = new string[] { "#4FBEDB", "#4E4CD4", "#C93497", "#3C87E8", "#E07031", "#44B88F", "#E0AC31", "#D44942", "#738498", "#4E4CD4" };

            string[] unitBackground = new string[] { "", "#F1F0FC", "#F9EEF6", "#EEF5FD", "#FCF4EE", "#F2FAF7", "#FDF8EE", "#FCF2F2", "#F6FAFE", "#F1F0FC" };
            string[] unitForeground = new string[] { "", "#3939B2", "#AD2880", "#2964B8", "#C25C25", "#389C77", "#CC9A2D", "#B23E36", "#526375", "#3939B2" };

            string[] borderColors = new string[] { "", "#F1F0FC", "#F9EEF6", "#EEF5FD", "#FCF4EE", "#F2FAF7", "#FDF8EE", "#FCF2F2", "#F6FAFE", "#F1F0FC" };
            string[] foregroundColors = new string[] { "", "#3939B2", "#AD2880", "#2964B8", "#C25C25", "#389C77", "#CC9A2D", "#B23E36", "#526375", "#3939B2" };

            string[] inactiveColors = new string[] { "#EFFAFC", "#F1F0FC", "#F9EEF6", "#EEF5FD", "#FCF4EE", "#F2FAF7", "#FDF8EE", "#FCF2F2", "#F6FAFE", "#F1F0FC" };
            string[] inactiveColors2 = new string[] { "#D4F4FA", "#D7D7FC", "#F5D7ED", "#D2E6FC", "#FAE0CD", "#DCF5EC", "#FCF0CF", "#F7D1D0", "#E7EFF8", "#D7D7FC" };

            for (int i = 0; i < lstUnitModel.Count; i++)
            {
                var unitModel = lstUnitModel[i];
                UnitTabModel unit = new UnitTabModel();
                unit.Id = lstUnitModel[i].id;
                unit.Icon = "/Achievers;component/Assets/Images/b" + (i + 1) + ".png";
                unit.Title = lstUnitModel[i].title;
                unit.TitleVI = lstUnitModel[i].title_vi;
                unit.Unit = lstUnitModel[i].name;
                unit.IsUnlock = lstUnitModel[i].is_free == 1 || !App.IsTrial;
                unit.StartColor = startColors[i];
                unit.EndColor = endColors[i];
                unit.ImageWordList = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + unitModel.image_url.Replace("/", @"\"));
                unit.ColorBoder = borderColors[i];
                unit.ForeGround = foregroundColors[i];
                unit.UnitBackground = unitBackground[i];
                unit.UnitForeground = unitForeground[i];
                unit.InactiveColor = inactiveColors[i];
                unit.InactiveColor2 = inactiveColors2[i];
                unit.IsActive = true;

                unit.lstReview = lstUnitModel[i].reviews.Select(x => new UnitTabReviewModel
                {
                    Id = x.id,
                    IdUnit = unit.Id,
                    Progress = x.progress,
                    MinProgress = x.min_progress
                }).ToList();

                if (lstUnitModel[i].reviews.Any(m => m.progress != null))
                {
                    unit.Progress = lstUnitModel[i].reviews.Sum(m => m.progress ?? 0) / lstUnitModel[i].reviews.Count;
                }

                Units.Add(unit);
            }
            totalProgress = Units.Sum(x => x.Progress ?? 0) / Units.Count;
        }

        private async void GetAllWord()
        {
            lstAllWord = new List<WordModel>();
            //lstAllWord = await HttpService.GetAllVocab();
            WordListTabQuery query = new WordListTabQuery();
            lstAllWord = await query.GetAllWord();

            await Task.Run(() =>
             {
                 foreach (var item in lstAllWord)
                 {
                     item.image_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + item.image_url.Replace("/", @"\"));
                     item.audio_url = Path.Combine(Environment.CurrentDirectory, @"Assets\Contents" + item.audio_url.Replace("/", @"\"));
                 }
             });
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (System.IO.Directory.Exists(ImageLoading.pathFolder))
            {
                System.IO.Directory.Delete(ImageLoading.pathFolder, true);
            }
        }

        public static void UpdateProgress(int idReview, double progress)
        {
            var review = Reviews.FirstOrDefault(m => m.Id == idReview);
            var unit = Units.FirstOrDefault(m => m.Id == review.IdUnit);
            if (review.Progress == null || review.Progress < progress)
            {
                review.Progress = (int)Math.Floor(progress);
                int unitProgress = Reviews.Sum(m => m.Progress ?? 0) / Reviews.Count;
                unit.Progress = unitProgress;

                var lockReview = Reviews.FirstOrDefault(m => m.IsLocked);
                if (lockReview != null)
                {
                    lockReview.IsLocked = Reviews.Any(m => !m.IsLocked && (m.Progress ?? 0) < lockReview.MinProgress);
                }
            }
            unit.lstReview = new List<UnitTabReviewModel>(Reviews);
            totalProgress = Units.Sum(x => x.Progress ?? 0) / Units.Count;
            LogService.Debug("UpdateProgress", new { review = review.Id, progress = review.Progress, totalProgress = totalProgress });
        }

        public static bool CheckIfReviewIsLock(int idReview)
        {
            var review = Reviews.FirstOrDefault(m => m.Id == idReview);
            if (review == null) return false;
            return !review.IsLocked;
        }
    }
}