using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecKill.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using config = SecKill.Config.Config;

namespace SecKill.Service
{
    public class HttpService
    {
        private string baseUrl = "https://miaomiao.scmttec.com";

        /// <summary>
        /// 获取秒杀资格
        /// </summary>
        /// <param name="seckillId"></param>
        /// <param name="vaccineIndex"></param>
        /// <param name="linkmanId"></param>
        /// <param name="idCard"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public string SecKill(string seckillId, string vaccineIndex, string linkmanId, string idCard, string st)
        {
            string path = baseUrl + "/seckill/seckill/subscribe.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            urlParams.Add("seckillId", seckillId);
            urlParams.Add("vaccineIndex", vaccineIndex);
            urlParams.Add("linkmanId", linkmanId);
            urlParams.Add("idCardNo", idCard);
            Dictionary<string, string> extHeaders = new Dictionary<string, string>();
            extHeaders.Add("ecc-hs", EccHs(seckillId, st));
            return Get(path, urlParams, extHeaders);
        }
        /// <summary>
        /// 获取疫苗列表
        /// </summary>
        /// <returns></returns>
        public List<VaccineList> GetVaccineLists()
        {
            HasAvailableConfig();
            string path = baseUrl + "/seckill/seckill/list.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            urlParams.Add("offset", "0");
            urlParams.Add("limit", "100");
            urlParams.Add("regionCode", config.RegionCode);
            string json = Get(path, urlParams, null);
            return JsonConvert.DeserializeObject<List<VaccineList>>(json);
        }
        /// <summary>
        /// 获取接种人信息
        /// </summary>
        /// <returns></returns>
        public List<Member> GetMembers()
        {
            string path = baseUrl + "/seckill/linkman/findByUserId.do";
            string json = Get(path, null, null);
            return JsonConvert.DeserializeObject<List<Member>>(json);
        }
        /// <summary>
        /// 获取加密参数st
        /// </summary>
        /// <param name="vaccineId"></param>
        /// <returns></returns>
        public string GetSt(string vaccineId)
        {
            string path = baseUrl + "/seckill/seckill/checkstock2.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            urlParams.Add("id", vaccineId);
            return JsonConvert.DeserializeObject<JObject>(Get(path, urlParams, null)).GetValue("st").ToString();
        }

        public void Log(string vaccineId)
        {
            string path = baseUrl + "/seckill/seckill/log.do";
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            urlParams.Add("id", vaccineId);
            Get(path, urlParams, null);
        }

        private void HasAvailableConfig()
        {
            if (config.Cookie.Count == 0)
            {
                throw new BusinessException("请先配置cookie");
            }
        }

        private string Get(string path, Dictionary<string, string> urlParams, Dictionary<string, string> headers)
        {
            if (urlParams != null && urlParams.Count != 0)
            {
                StringBuilder paramStr = new StringBuilder("?");
                urlParams.ToList().ForEach(param => paramStr.Append(param.Key).Append("=").Append(param.Value).Append("&"));
                path = path + paramStr.ToString();
                if (path.EndsWith("&"))
                {
                    path = path.Substring(0, path.Length - 1);
                }
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.Method = "GET";
            headers = GetCommonHeader();

            foreach (KeyValuePair<string, string> kvp in headers)
            {
                request.Headers.Add(kvp.Key, kvp.Value);
            }
            request.UserAgent = "Mozilla/5.0 (Linux; Android 5.1.1; SM-N960F Build/JLS36C; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/74.0.3729.136 Mobile Safari/537.36 MMWEBID/1042 MicroMessenger/7.0.15.1680(0x27000F34) Process/appbrand0 WeChat/arm32 NetType/WIFI Language/zh_CN ABI/arm32";
            request.Referer = "https://servicewechat.com/wxff8cad2e9bf18719/2/page-frame.html";
            request.Accept = "application/json, text/plain, */*";
            request.Host = "miaomiao.scmttec.com";
            request.ContinueTimeout = 2500;
            request.Timeout = 2500;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            DealHeader(response);
            string json;
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                json = sr.ReadToEnd();
            }
            var jsonObject = JObject.Parse(json);
            if ("0000".Equals(jsonObject.SelectToken("code").ToString()))
            {
                return jsonObject.SelectToken("data").ToString();
            }
            throw new BusinessException(jsonObject.SelectToken("code").ToString(), jsonObject.SelectToken("ok").ToString());
        }

        private Dictionary<string, string> GetCommonHeader()
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

        private void DealHeader(HttpWebResponse response)
        {
            string[] cookies = response.Headers.GetValues("Set-Cookie");
            if (cookies != null && cookies.Length > 0)
            {
                foreach (var item in cookies)
                {
                    string cookie = item.Split(';')[0].Split('=')[1].Trim();
                    config.Cookie.Add(cookie.Split('=')[0], cookie);
                }
            }
        }

        private string EccHs(string secKillId, string st)
        {
            string salt = "ux$ad70*b";
            int memberId = config.MemberId;
            string md5Str = Md5Hex(secKillId + memberId + st);
            return Md5Hex(md5Str + salt);
        }

        private string Md5Hex(string str)
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
    }
}