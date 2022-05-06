using SecKill.Constants;
using SecKill.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SecKill.Windows;
using config = SecKill.Config.Config;
using SecKill.Service;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace SecKill
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Area> areas = Areas.GetAreas();
        SettingCookieWindow settingWindow;
        SwitchMemberWindow MemberWindow;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Province.ItemsSource = areas;
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string provinceName = Province.SelectedValue.ToString();
            List<Area> cities = areas.First(p => p.Value == provinceName)?.Children;
            City.ItemsSource = cities;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string provinceName = Province.SelectedValue.ToString();
            string cityName = City.SelectedValue.ToString();
            if (provinceName.Length == 0 || cityName.Length == 0)
            {
                MessageBox.Show("省份或城市不能为空");
            }
            else
            {
                config.RegionCode = cityName;
                Area city = (Area)City.SelectedItem;
                string message = $"已选择区域:{city.Name}";
                MessageBox.Show(message);
                LogModel.UpdateLogStr(message);
            }
        }

        private void SettingCookie_Click(object sender, RoutedEventArgs e)
        {
            if (settingWindow == null)
            {
                SettingCookieWindow settingCookieWindow = new SettingCookieWindow
                {
                    Title = "设置抢购参数"
                };
                settingWindow = settingCookieWindow;
            }
            settingWindow.Show();
            settingWindow.Focus();
        }

        private void SwitchMember_Click(object sender, RoutedEventArgs e)
        {

            if (MemberWindow == null)
            {
                SwitchMemberWindow switchMemberWindow = new SwitchMemberWindow
                {
                    Title = "选择成员"
                };
                MemberWindow = switchMemberWindow;
            }

            MemberWindow.Show();
        }

        private void RefreshVaccineList_Click(object sender, RoutedEventArgs e)
        {
            List<VaccineList> vaccineLists = HttpService.GetVaccineLists();
            DataGrid.DataContext = vaccineLists;
            MessageBox.Show("疫苗列表刷新成功！");
        }

        private void StartKill_Click(object sender, RoutedEventArgs e)
        {
            if (config.Cookie.Count == 0)
            {
                throw new BusinessException("请配置cookie!!!");
            }
            if (DataGrid.SelectedItems.Count == 0)
            {
                throw new BusinessException("请选择要抢购的疫苗");
            }

            VaccineList selectedItem = DataGrid.SelectedItem as VaccineList;
            int id = selectedItem.Id;
            string startIime = selectedItem.StartTime;
            if (!int.TryParse(interval.Text, out int intervalInt))
            {
                intervalInt = 200;
            }

            Task.Run(() =>
            {
                SecKillService.StartSecKill(id, startIime, intervalInt);
            });
            MessageBox.Show("设置抢购成功");
        }

        private void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {

            Regex re = new Regex("[^0-9]+");

            e.Handled = re.IsMatch(e.Text);

        }
    }
}
