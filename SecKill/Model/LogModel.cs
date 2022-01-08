using System.ComponentModel;
using System.Text;

namespace SecKill.Model
{
    internal class LogModel
    {
        private static string logStr = "";

        public static string LogStr
        {
            get => logStr;
            set
            {
                if (logStr == value)
                    return;

                logStr = value;

                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(LogStr)));
            }
        }

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        public static void UpdateLogStr(string str)
        {
            StringBuilder stringBuilder = new StringBuilder(LogStr);
            LogStr = stringBuilder.AppendLine(str).ToString();
        }
    }
}
