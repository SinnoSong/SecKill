using SecKill.Model;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SecKill
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Applicatoin_Startup(object sender, StartupEventArgs s)
        {
            Current.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception exception = e.Exception;
            MessageBox.Show(exception.Message, exception.StackTrace);
            e.SetObserved();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            if (ex is BusinessException)
            {
                var exception = ex as BusinessException;
                string msg;
                if (exception.Code == null)
                {
                    msg = $"Msg:{exception.Msg}";
                }
                else
                {
                    msg = $"Code:{exception.Code},Msg:{exception.Msg}";
                }
                MessageBox.Show(msg);
            }
            else
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void OnExceptionHandler(Exception e)
        {
            if (e != null)
            {
                string errorMsg = "";
                if (e.InnerException != null)
                {
                    errorMsg += $"InnerException:{e.InnerException.Message}\n{e.InnerException.StackTrace}";
                }
                errorMsg += $"{e.Message}\n {e.StackTrace}";
                MessageBox.Show(errorMsg);
            }
        }
    }
}
