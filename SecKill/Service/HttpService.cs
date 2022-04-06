using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecKill.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using config = SecKill.Config.Config;

namespace SecKill.Service
{
    public class HttpService
    {
        private const string BaseUrl = "https://miaomiao.scmttec.com";
        private static readonly HttpClient HttpClient = GetHttpClient();

        /// <summary>
        /// 获取秒杀资格
        /// </summary>
        /// <param name="seckillId"></param>
        /// <param name="vaccineIndex"></param>
        /// <param name="linkmanId"></param>
        /// <param name="idCard"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public static async Task<string> SecKill(string seckillId, string vaccineIndex, string linkmanId, string idCard, string st)
        {
            string path = BaseUrl + "/seckill/seckill/subscribe.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>
            {
                { "seckillId", seckillId },
                { "vaccineIndex", vaccineIndex },
                { "linkmanId", linkmanId },
                { "idCardNo", idCard }
            };
            Dictionary<string, string> extHeaders = new Dictionary<string, string>
            {
                { "ecc-hs", EccHs(seckillId, st) }
            };
            return await SendRequest(HttpMethod.Get, path, queryPairs: urlParams, headers: extHeaders);
        }
        /// <summary>
        /// 获取疫苗列表
        /// </summary>
        /// <returns></returns>
        public static async Task<List<VaccineList>> GetVaccineLists()
        {
            if (config.Cookie.Count == 0)
            {
                throw new BusinessException("请先配置cookie");
            }
            string path = BaseUrl + "/seckill/seckill/list.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>
            {
                { "offset", "0" },
                { "limit", "100" },
                { "regionCode", config.RegionCode }
            };
            string json = await SendRequest(HttpMethod.Get, path, queryPairs: urlParams);
            return JsonConvert.DeserializeObject<List<VaccineList>>(json);
        }
        /// <summary>
        /// 获取接种人信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Member>> GetMembers()
        {
            string path = BaseUrl + "/seckill/linkman/findByUserId.do";
            string json = await SendRequest(HttpMethod.Get, path);
            return JsonConvert.DeserializeObject<List<Member>>(json);
        }
        /// <summary>
        /// 获取加密参数st
        /// </summary>
        /// <param name="vaccineId"></param>
        /// <returns></returns>
        public static async Task<string> GetSt(string vaccineId)
        {
            string path = BaseUrl + "/seckill/seckill/checkstock2.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>
            {
                { "id", vaccineId }
            };
            return JsonConvert.DeserializeObject<JObject>(await SendRequest(HttpMethod.Get, path, queryPairs: urlParams)).GetValue("st").Value<string>();
        }

        private static async Task<string> SendRequest(HttpMethod method, string queryUrl, HttpContent httpContent = null, Dictionary<string, string> queryPairs = null, Dictionary<string, string> headers = null)
        {
            if (queryPairs != null)
            {
                StringBuilder queryParams = new StringBuilder("?");
                foreach (KeyValuePair<string, string> pair in queryPairs)
                {
                    queryParams.Append(pair.Key + "=" + pair.Value + "&");
                }
                queryParams.Remove(queryParams.Length - 1, 1);
                queryUrl += queryParams.ToString();
            }
            var request = new HttpRequestMessage(method, queryUrl);
            request.Headers.Add("ContinueTimeout", "2500");
            request.Headers.Add("Timeout", "2500");
            foreach (KeyValuePair<string, string> kvp in GetCommonHeader())
            {
                request.Headers.Add(kvp.Key, kvp.Value);
            }
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> kvp in headers)
                {
                    request.Headers.Add(kvp.Key, kvp.Value);
                }
            }
            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            var responseMessage = await HttpClient.SendAsync(request);
            DealHeader(responseMessage);

            var jsonObject = JObject.Parse(await responseMessage.Content.ReadAsStringAsync());
            if ("0000" == jsonObject.SelectToken("code").Value<string>())
            {
                return jsonObject.SelectToken("data").Value<string>();
            }
            else
            {
                throw new BusinessException(jsonObject.SelectToken("code").Value<string>(), jsonObject.SelectToken("msg").Value<string>());
            }
        }

        private static Dictionary<string, string> GetCommonHeader()
        {
            Dictionary<string, string> commHeader = new Dictionary<string, string>();
            if (config.TK == null || config.Cookie.Count == 0)
            {
                throw new BusinessException("请先设置Cookie!");
            }
            commHeader.Add("tk", config.TK);
            if (config.Cookie.Count > 0)
            {
                string cookie = string.Join(";", new List<string>(config.Cookie.Values));
                commHeader.Add("Cookie", cookie);
            }
            return commHeader;
        }

        private static void DealHeader(HttpResponseMessage response)
        {
            var cookies = response.Headers.GetValues("Set-Cookie");
            if (cookies != null && cookies.Count() > 0)
            {
                foreach (var item in cookies)
                {
                    string cookie = item.Split(';')[0].Split('=')[1].Trim();
                    var cookieKey = cookie.Split('=')[0];
                    if (!config.Cookie.ContainsKey(cookieKey))
                    {
                        config.Cookie.Add(cookieKey, cookie);
                    }
                }
            }
        }

        private static string EccHs(string secKillId, string st)
        {
            string salt = "ux$ad70*b";
            int memberId = config.MemberId;
            string md5Str = Md5Hex(secKillId + memberId + st);
            return Md5Hex(md5Str + salt);
        }

        private static string Md5Hex(string str)
        {
            string result = "";
            MD5 md5 = MD5.Create();
            byte[] buffers = md5.ComputeHash(Encoding.Default.GetBytes(str));
            for (int i = 0; i < buffers.Length; i++)
            {
                result += buffers[i].ToString("x2");
            }
            return result;
        }

        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Referrer = new System.Uri("https://servicewechat.com/wxff8cad2e9bf18719/2/page-frame.html");
            client.DefaultRequestHeaders.Host = "miaomiao.scmttec.com";
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 5.1.1; SM-N960F Build/JLS36C; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/74.0.3729.136 Mobile Safari/537.36 MMWEBID/1042 MicroMessenger/7.0.15.1680(0x27000F34) Process/appbrand0 WeChat/arm32 NetType/WIFI Language/zh_CN ABI/arm32");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json, text/plain, */*");
            return client;
        }
    }
}