
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TestModel;

namespace TestComm.Helper
{
    public class TokenHelper
    {
        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static  string GetStringToken()
        {
            string token = string.Empty;
            //Linq读取XML文件中的数据，并显示出来 ，注意文件路径
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Config" + "\\ArcConfig.xml";
            token = ConfigHelper.GetCacheValue("Access_Token").ToStr();
            DateTime lastTime = Convert.ToDateTime(ConfigHelper.GetCacheValue("Access_YouXRQ").ToStr());
            //判断当前 token 是否过期
            if (DateTime.Now > lastTime)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                XElement xml = XElement.Load(xmlPath);
                xml.Descendants("Access_Token").FirstOrDefault().Value = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.Descendants("Access_YouXRQ").FirstOrDefault().Value= _youxrq.ToString();
                xml.Save(xmlPath);
                token = mode.access_token;
            }
            return token;
        }


        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        private static Access_token GetAccess_token()
        {
            var appid = ConfigHelper.GetCacheValue("AppId").ToStr();
            var secret = ConfigHelper.GetCacheValue("AppSecret").ToStr();

            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
            Access_token mode = new Access_token();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strUrl);  //用GET形式请求指定的地址 
            req.Method = "GET";

            using (WebResponse wr = req.GetResponse())
            {
                StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                //在这里对Access_token 赋值  
                Access_token token = new Access_token();
                token = JsonHelper.ParseFromJson<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }
    }
}