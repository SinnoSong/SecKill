using SecKill.Constants;
using SecKill.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            ItemCollection collection = City.Items;
            collection.Clear();

            string provinceName = Province.SelectedValue.ToString();
            List<Area> cities = areas.First(p => p.Value == provinceName)?.Children;
            City.ItemsSource = cities;
        }
    }
}
