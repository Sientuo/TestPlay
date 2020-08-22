
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using WetChat.Models;

namespace WetChat.Helper
{
    public class TokenHelper
    {
        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static  string IsExistAccess_Token()
        {
            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            //string filepath = HttpContext.Current.Server.MapPath(".") + "\\Config" + "\\AccessToken.xml";
            string filepath = @"D:\Kiaser\VSPro\Test\TestPlay\TestPlay\Config\AccessToken.xml";
                                    
        StreamReader str = new StreamReader(filepath, Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

            //TimeSpan st1 = new TimeSpan(YouXRQ.Ticks); //最后刷新的时间
            //TimeSpan st2 = new TimeSpan(DateTime.Now.Ticks); //当前时间
            //TimeSpan st = st2 - st1; //两者相差时间
            if (DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                xml.Save(filepath);
                Token = mode.access_token;
            }
            return Token;
        }


        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        private static Access_token GetAccess_token()
        {
            //订阅号信息
            string appid = "wxa6081d07acd03286"; //微信公众号appid
            string secret = "150c29b922e5d0fabc368320ccb8597f";  //微信公众号appsecret

            //企业号信息
            //string appid = "wx95792357cac264a6"; //微信公众号appid
            //string secret = "7f2ddb7c441418570134fd403670930c";  //微信公众号appsecret
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);  //用GET形式请求指定的地址 
            req.Method = "GET";

            using (WebResponse wr = req.GetResponse())
            {
                //HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();  
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