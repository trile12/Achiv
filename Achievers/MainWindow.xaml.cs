using Achievers.ViewModels;
using System.Windows;

namespace Achievers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            WindowStyle = WindowStyle.SingleBorderWindow;
#else
            WindowStyle = WindowStyle.None;
#endif

            DataContext = new MainViewModel();
        }
    }
}