using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Achievers.Views.Shared
{
    /// <summary>
    /// Interaction logic for CardUnitItem.xaml
    /// </summary>
    public partial class CardUnitItem : UserControl
    {
        private static List<Border> lstCardItem = new List<Border>();

        public CardUnitItem()
        {
            InitializeComponent();
        }

        public int? Percentage
        {
            get { return (int?)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(int?), typeof(CardUnitItem), new PropertyMetadata(0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CardUnitItem), new PropertyMetadata(string.Empty));

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(CardUnitItem), new PropertyMetadata(true));

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(CardUnitItem), new PropertyMetadata(0));
    }
}