using System.ComponentModel;
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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TKText.Text) || string.IsNullOrEmpty(CookieText.Text))
            {
                MessageBox.Show("请输入tk和cookie", "提示");
            }
            else
            {
                config.TK = TKText.Text;
                CalCookie(CookieText.Text);
                TKText.Text = null;
                CookieText.Text = null;
                Hide();
            }
        }

        private void CalCookie(string cookie)
        {
            config.Cookie.Clear();
            string[] s = cookie.Replace(" ", "").Split(';');
            foreach (var item in s)
            {
                config.Cookie.Add(item.Split('=')[0], item);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
