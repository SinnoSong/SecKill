using SecKill.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using config = SecKill.Config.Config;

namespace SecKill.Service
{
    public static class SecKillService
    {

        public static void StartSecKill(int vaccineId, string startDateStr, int interval)
        {
            long startDate = (Convert.ToDateTime(startDateStr).ToUniversalTime().Ticks - 621355968000000000) / 10000;

            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (now < startDate)
            {
                LogModel.UpdateLogStr("还未到获取st时间，等待中。。。");
                Thread.Sleep((int)(startDate - now));
            }
            while (true)
            {
                // 开始抢之后获取服务器时间戳接口，计算加密用
                try
                {
                    LogModel.UpdateLogStr($"线程名称：{Thread.CurrentThread.Name},请求获取加密参数ST");
                    config.ST = HttpService.GetSt(vaccineId.ToString());
                    LogModel.UpdateLogStr($"线程名称：{Thread.CurrentThread.Name}，成功获取加密参数st:{config.ST}");
                    break;
                }
                catch (TimeoutException)
                {
                    LogModel.UpdateLogStr($"线程名称：{Thread.CurrentThread.Name}，获取st失败，超时");
                }
                catch (BusinessException e)
                {
                    LogModel.UpdateLogStr($"线程名称：{Thread.CurrentThread.Name}，获取st失败：{e.Msg}");
                }
                catch (Exception exception)
                {
                    LogModel.UpdateLogStr($"线程名称：{Thread.CurrentThread.Name}，获取st失败：{exception.Message}");
                }
            }

            // 把task添加到队列中
            Queue<Action> tasks = new Queue<Action>();
            tasks.Enqueue(() => SecKillTask(false, vaccineId));
            tasks.Enqueue(() => SecKillTask(true, vaccineId));
            // 5秒后或者秒杀成功后停止
            while (now < startDate + 5000 && !config.Success)
            {
                Action action = tasks.Dequeue();
                Task.Run(action);
                tasks.Enqueue(action);
                Thread.Sleep(interval);
                now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            if (config.Success)
            {
                LogModel.UpdateLogStr("秒杀结束，抢购成功，请登录秒苗小程序查看");
            }
            else
            {
                LogModel.UpdateLogStr("秒杀结束，抢购失败");
            }
        }

        private static void SecKillTask(bool resetSt, int vaccineId)
        {
            long id = Thread.CurrentThread.ManagedThreadId;
            try
            {
                if (resetSt)
                {
                    LogModel.UpdateLogStr($"Thread ID:{id},请求获取加密参数ST");
                    config.ST = HttpService.GetSt(vaccineId.ToString());
                    LogModel.UpdateLogStr($"Thread ID:{id},成功获取加密参数ST");
                }
                LogModel.UpdateLogStr($"Thread ID:{id},秒杀请求");
                HttpService.SecKill(vaccineId.ToString(), "1", config.MemberId.ToString(),
                    config.IdCard, config.ST);
                config.Success = true;
                LogModel.UpdateLogStr($"Thread ID:{id},抢购成功");
            }
            catch (BusinessException e)
            {
                LogModel.UpdateLogStr($"Thread ID:{id},抢购失败：{e.Msg}");
                if (e.Msg.Contains("没抢到"))
                {
                    config.Success = false;
                }
            }
            catch (TimeoutException)
            {
                LogModel.UpdateLogStr($"Thread Id :{Thread.CurrentThread.ManagedThreadId},抢购失败：超时了");
            }
            catch (Exception exception)
            {
                LogModel.UpdateLogStr("未知异常：" + exception.Message);
            }
        }
    }
}