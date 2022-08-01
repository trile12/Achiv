using Achievers.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;

namespace Achievers.Models
{
    public class WordModel : BaseViewModel
    {
        public int id { get; set; }
        public string word { get; set; }
        public string word_form { get; set; }
        public string word_vi { get; set; }
        public string phonetic { get; set; }
        public string meaning { get; set; }
        public string example { get; set; }

        private string _image_url;

        public string image_url
        {
            get { return _image_url; }
            set
            {
                _image_url = value;
                OnPropertyChanged();
            }
        }

        private string _audio_url;

        public string audio_url
        {
            get { return _audio_url; }
            set
            {
                _audio_url = value;
                OnPropertyChanged();
            }
        }

        public int? grade_unit_id { get; set; }
        public int deleted { get; set; }
        public string word_parts { get; set; }

        //Search props
        private string search;

        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                //word_end = String.Concat(word.Split(new[] { value }, StringSplitOptions.None));
                word_start = word.Substring(0, value.Length);
                word_end = word.Substring(value.Length, word.Length - value.Length);
            }
        }

        public string word_start { get; set; }
        public string word_end { get; set; }

        private int? is_favorite_private;

        public int? is_favorite
        {
            get { return is_favorite_private; }
            set { is_favorite_private = value; OnPropertyChanged(); }
        }

        public int? memo_id { get; set; }

        private string memo_private;

        public string memo
        {
            get { return memo_private; }
            set { memo_private = value; OnPropertyChanged(); }
        }

        public Color back_ground { get; set; }
        public Color border_brush { get; set; }

        private string isChecked;
        private string search1;

        public string Checked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }
    }

    public class UnitModel : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string title_vi { get; set; }
        public string image_url { get; set; }
        public int deleted { get; set; }
        public int is_free { get; set; }
        public List<ReviewModel> reviews { get; set; }
    }

    public class ReviewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image_url { get; set; }
        public int? progress { get; set; }

        public int progress_2
        {
            get
            {
                return progress ?? 0;
            }
        }

        public int min_progress { get; set; }
    }
}