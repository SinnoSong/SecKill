using SecKill.Model;
using SecKill.Service;
using System.Collections.Generic;
using System.Windows;

namespace SecKill.Windows
{
    /// <summary>
    /// SwitchMemberWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchMemberWindow : Window
    {
        HttpService HttpService { get; set; }
        public SwitchMemberWindow()
        {
            InitializeComponent();
            InitialData();
        }

        private void InitialData()
        {
            HttpService = new HttpService();
            List<Member> members = HttpService.GetMembers();
            if (members.Count==0)
            {
                MessageBox.Show("你还没有添加任何成员");
            }
            //DataContext =
        }
    }
}
