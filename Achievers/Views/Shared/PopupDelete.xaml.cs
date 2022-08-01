using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for PopupDelete.xaml
    /// </summary>
    public partial class PopupDelete : Border
    {
        public PopupDelete()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ClosePopupDelete();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.ClosePopupDelete();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            //await HttpService.DeleteMemos(new { userId = App.UserId, unit_vocab_id = PopupManager.Id });
            await new WordListTabQuery().UpdateMemos(PopupManager.Id, "");

            //var listMemos = await HttpService.GetAllMemos();
            var listMemos = (await new WordListTabQuery().GetAllWord()).Where(x => !String.IsNullOrWhiteSpace(x.memo)).ToList();
            int i = 0;
            listMemos.ForEach(x =>
            {
                x.image_url = HttpService.baseUrl + "/contents" + x.image_url;
                x.back_ground = (Color)ColorConverter.ConvertFromString(App.lstBackground[i]);
                i++;
                if (i == 4)
                    i = 0;
            });
            PopupManager.memosViewModel.LstWord = new ObservableCollection<WordModel>(listMemos);
            PopupManager.memosViewModel.Word = listMemos.FirstOrDefault();
            PopupManager.ClosePopupDelete();
        }
    }
}