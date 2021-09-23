using SecKill.Constants;
using SecKill.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SecKill.Windows;
using config = SecKill.Config.Config;

namespace SecKill
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Area> areas = Areas.GetAreas();
        public MainWindow()
        {
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
                MessageBox.Show($"已选择区域:{city.Name}");
            }
        }

        private void SettingCookie_Click(object sender, RoutedEventArgs e)
        {
            SettingCookieWindow cookiePage = new SettingCookieWindow();
            cookiePage.Title = "设置抢购参数";
            cookiePage.Show();
        }

        private void SwitchMember_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshVaccineList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartKill_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
