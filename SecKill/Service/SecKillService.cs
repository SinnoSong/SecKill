using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using config = SecKill.Config.Config;
using SecKill.Model;

namespace SecKill.Service
{
    public class SecKillService
    {
        private HttpService httpService;

        public SecKillService(HttpService httpService)
        {
            this.httpService = httpService;
        }

        public void StartSecKill(int vaccineId, string startDateStr)
        {
            long startDate = (Convert.ToDateTime(startDateStr).ToUniversalTime().Ticks - 621355968000000000) / 10000;

            long now = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            if (now + 5000 < startDate)
            {
                Console.WriteLine("还未到获取st时间，等待中。。。");
                Thread.Sleep((int)(startDate - now - 5000));
            }
            while (true)
            {
                // 提前5秒钟获取服务器时间戳接口，计算加密用
                try
                {
                    Console.WriteLine("Thread ID：main,请求获取加密参数ST");
                    config.ST = httpService.GetSt(vaccineId.ToString());
                    Console.WriteLine($"Thread ID：main，成功获取加密参数st:{config.ST}");
                    break;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Thread ID：main，获取st失败，超时");
                }
                catch (BusinessException e)
                {
                    Console.WriteLine($"Thread ID:main，获取st失败：{e.Msg}");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Thread ID:main，获取st失败：{exception.Message}");
                }
            }
            now = DateTime.Now.ToFileTime();
            if (now + 500 < startDate)
            {
                Console.WriteLine($"获取st参数成功，还未到秒杀开始时间，等待中。。。。。。");
                Thread.Sleep((int)(startDate - now - 500));
            }

            // 添加到Task中
            Task.Factory.StartNew(() => SecKillTask(false, vaccineId, startDate));
            Thread.Sleep(200);
            Task.Factory.StartNew(() => SecKillTask(true, vaccineId, startDate));
            Thread.Sleep(200);
            Task.Factory.StartNew(() => SecKillTask(true, vaccineId, startDate));
            Thread.Sleep(200);
            Task.Factory.StartNew(() => SecKillTask(false, vaccineId, startDate));

            try
            {
                if (config.Success.Value)
                {
                    Console.WriteLine("抢购成功，请登录秒苗小程序查看");
                }
                else
                {
                    Console.WriteLine("抢购失败");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal List<VaccineList> GetVaccineLists()
        {
            return httpService.GetVaccineLists();
        }

        private void SecKillTask(bool resetSt, int vaccineId, long startTime)
        {
            do
            {
                long id = Thread.CurrentThread.ManagedThreadId;
                try
                {
                    if (resetSt)
                    {
                        Console.WriteLine($"Thread ID:{id},请求获取加密参数ST");
                        config.ST = httpService.GetSt(vaccineId.ToString());
                        Console.WriteLine($"Thread ID:{id},成功获取加密参数ST");
                    }
                    Console.WriteLine($"Thread ID:{id},秒杀请求");
                    httpService.SecKill(vaccineId.ToString(), "1", config.MemberId.ToString(),
                        config.IdCard, config.ST);
                    config.Success = true;
                    Console.WriteLine($"Thread ID:{id},抢购成功");
                    break;
                }
                catch (BusinessException e)
                {
                    Console.WriteLine($"Thread ID:{id},抢购失败：{e.Msg}");
                    if (e.Msg.Contains("没抢到"))
                    {
                        config.Success = false;
                        break;
                    }
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"Thread Id :{Thread.CurrentThread.ManagedThreadId},抢购失败：超时了");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("未知异常：" + exception.Message);
                }
                long now = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
                if (now > startTime + 10 * 1000 || config.Success.HasValue)
                {
                    break;
                }
            } while (true);
        }
    }
}