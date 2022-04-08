using SecKill.Model;
using SecKill.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using config = SecKill.Config.Config;

namespace SecKill.Windows
{
    /// <summary>
    /// SwitchMemberWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchMemberWindow : Window
    {
        public SwitchMemberWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            InitialData();
        }

        private void InitialData()
        {
            List<Member> members = HttpService.GetMembers();
            //List<Member> members = new List<Member>() { new Member() { Id = 1, Name = "1", IdCardNo = "1" }, new Member() { Id = 2, Name = "2", IdCardNo = "2" } };
            if (members.Count == 0)
            {
                MessageBox.Show("你还没有添加任何成员");
            }
            DataContext = members;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (memberGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择接种成员");
            }
            else
            {
                Member member = memberGrid.SelectedItem as Member;
                config.MemberId = member.Id;
                config.IdCard = member.IdCardNo;
                config.MemberName = member.Name;
                Hide();
            }
        }
    }
}
