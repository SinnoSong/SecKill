using System.Windows;
using config = SecKill.Config.Config;

namespace SecKill.Windows
{
    /// <summary>
    /// SettingCookieWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingCookieWindow : Window
    {
        public SettingCookieWindow()
        {
            InitializeComponent();
            reqHeader.Text = config.RequestHeader;
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
