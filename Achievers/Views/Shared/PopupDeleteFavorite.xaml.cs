using Achievers.Models;
using Achievers.Queries;
using Achievers.Services;
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
    public partial class PopupDeleteFavorite : Border
    {
        public PopupDeleteFavorite()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ClosePopupFavoriteDelete();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.ClosePopupFavoriteDelete();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PopupManager.favoriteViewModel.ListFavorite.Where(x => x.Checked == "Visible"))
            {
                //await HttpService.DeleteFavorite(new { userId = App.UserId, unit_vocab_id = item.id });
                await (new WordListTabQuery()).UpdateFavorite(item.id, 0);
            }

            //var listFavorite = await HttpService.GetAllFavorite();
            var listFavorite = (await (new WordListTabQuery()).GetAllWord()).Where(x => x.is_favorite > 0 && x.is_favorite != null).ToList();
            int i = 0;
            listFavorite.ForEach(x =>
            {
                x.image_url = HttpService.baseUrl + "/contents" + x.image_url;
                x.back_ground = (Color)ColorConverter.ConvertFromString(App.lstBackground[i]);
                x.border_brush = (Color)ColorConverter.ConvertFromString(App.lstBorderBrush[i]);
                i++;
                if (i == 4)
                    i = 0;
            });
            PopupManager.favoriteViewModel.ListFavorite = new ObservableCollection<WordModel>(listFavorite);
            PopupManager.favoriteViewModel.CurrentFavorite = listFavorite.FirstOrDefault();
            PopupManager.ClosePopupFavoriteDelete();
        }
    }
}