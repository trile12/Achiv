using Achievers.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for PopupPurchase.xaml
    /// </summary>
    public partial class PopupMemos : Border
    {
        public PopupMemos()
        {
            InitializeComponent();
        }

        public VocabModel Word
        {
            get { return (VocabModel)GetValue(WordProperty); }
            set { SetValue(WordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WordProperty =
            DependencyProperty.Register("Word", typeof(VocabModel), typeof(PopupMemos), new PropertyMetadata(new VocabModel()));

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PopupManager.ClosePopupMemos();
        }
    }
}