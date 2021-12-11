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
            InitializeComponent();
            reqHeader.Text = config.RequestHeader;
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            string[] data = parseHeader(reqHeader.Text);
            if (data == null)
            {
                MessageBox.Show("数据格式错误", "提示");
            }
            else
            {
                TKText.Text = data[0];
                CookieText.Text = data[1];
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TKText.Text) || string.IsNullOrEmpty(CookieText.Text))
            {
                MessageBox.Show("请输入tk和cookie", "提示");
            }
            else
            {
                config.RequestHeader = reqHeader.Text;
                config.TK = TKText.Text;
                calCookie(CookieText.Text);
                Hide();
            }
        }

        private string[] parseHeader(string reqHeader)
        {
            if (string.IsNullOrEmpty(reqHeader))
            {
                return null;
            }
            string[] data = new string[2];
            reqHeader = reqHeader.Replace("cookie: ", "Cookie: ");
            int start = reqHeader.IndexOf("tk: ");
            int end = reqHeader.IndexOf("\n", start);
            if (start == -1 || end == -1)
            {
                return null;
            }
            System.Console.WriteLine(reqHeader.Length);
            data[0] = reqHeader.Substring(start + "tk: ".Length, end - start - "tk: ".Length);
            start = reqHeader.IndexOf("Cookie: ");
            end = reqHeader.IndexOf("\n", start);
            if (start == -1 || end == -1)
            {
                return null;
            }
            data[1] = reqHeader.Substring(start + "Cookie: ".Length, end - start - "Cookie: ".Length);
            return data;
        }

        private void calCookie(string cookie)
        {
            string[] s = cookie.Replace(" ", "").Split(';');
            foreach (var item in s)
            {
                config.Cookie.Add(item.Split('=')[0], item);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
