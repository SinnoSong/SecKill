using SecKill.Constants;
using System.Windows;

namespace SecKill
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Province.ItemsSource = Areas.GetAreas();
        }

        private void Province_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Province.SelectedValue.ToString());
        }

        private void Province_Selected_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
