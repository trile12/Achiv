using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for MemosCardItem.xaml
    /// </summary>
    public partial class MemosCardItem : UserControl
    {
        public MemosCardItem()
        {
            InitializeComponent();
        }

        static MemosCardItem()
        {
            OnItemSelectEvent = EventManager.RegisterRoutedEvent("OnItemSelect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MemosCardItem));
        }

        #region Event

        public static readonly RoutedEvent OnItemSelectEvent;

        public event RoutedEventHandler OnItemSelect
        {
            add
            {
                base.AddHandler(OnItemSelectEvent, value);
            }
            remove
            {
                base.RemoveHandler(OnItemSelectEvent, value);
            }
        }

        #endregion Event

        public int word_Id
        {
            get { return (int)GetValue(word_IdProperty); }
            set { SetValue(word_IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for word_Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty word_IdProperty =
            DependencyProperty.Register("word_Id", typeof(int), typeof(MemosCardItem), new PropertyMetadata(0));

        public string Word
        {
            get { return (string)GetValue(WordProperty); }
            set { SetValue(WordProperty, value); }
        }

        public static readonly DependencyProperty WordProperty =
            DependencyProperty.Register("Word", typeof(string), typeof(MemosCardItem), new PropertyMetadata(string.Empty));

        public string Word_Form
        {
            get { return (string)GetValue(Word_FormProperty); }
            set { SetValue(Word_FormProperty, value); }
        }

        public static readonly DependencyProperty Word_FormProperty =
            DependencyProperty.Register("Word_Form", typeof(string), typeof(MemosCardItem), new PropertyMetadata(string.Empty));

        public string Memos
        {
            get { return (string)GetValue(MemosProperty); }
            set { SetValue(MemosProperty, value); }
        }

        public static readonly DependencyProperty MemosProperty =
            DependencyProperty.Register("Memos", typeof(string), typeof(MemosCardItem), new PropertyMetadata(string.Empty));

        public Color Back_ground
        {
            get { return (Color)GetValue(Back_groundProperty); }
            set { SetValue(Back_groundProperty, value); }
        }

        public static readonly DependencyProperty Back_groundProperty =
            DependencyProperty.Register("Back_ground", typeof(Color), typeof(MemosCardItem), new PropertyMetadata(Colors.White));

        public Color Border_brush
        {
            get { return (Color)GetValue(Border_brushProperty); }
            set { SetValue(Border_brushProperty, value); }
        }

        public static readonly DependencyProperty Border_brushProperty =
            DependencyProperty.Register("Border_brush", typeof(Color), typeof(MemosCardItem), new PropertyMetadata(Colors.White));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(MemosCardItem), new PropertyMetadata(false));

        private void Edit_Click(object sender, MouseButtonEventArgs e)
        {
            ItemModel item = new ItemModel();
            item.type = "Edit";
            item.Id = this.word_Id;

            RaiseEvent(new RoutedEventArgs(OnItemSelectEvent, item));
        }

        private void Delete_Click(object sender, MouseButtonEventArgs e)
        {
            ItemModel item = new ItemModel();
            item.type = "Delete";
            item.Id = this.word_Id;
            RaiseEvent(new RoutedEventArgs(OnItemSelectEvent, item));
        }
    }

    public class ItemModel
    {
        public int Id { get; set; }
        public string type { get; set; }
    }
}