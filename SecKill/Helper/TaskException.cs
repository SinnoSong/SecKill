using SecKill.Model;
using System.Threading.Tasks;

namespace SecKill.Helper
{
    internal static class TaskException
    {
        public static void LogExcetion(this Task task)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var ex in aggException.InnerExceptions)
                {
                    LogModel.UpdateLogStr(ex.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
