using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WetChat.Helper;

namespace TestPlay
{
    public class SendTempletHelper
    {
        public static string SendTempletMessge(List<string> openId)
        {
            string strReturn = string.Empty;
            try
            {
                #region 获取access_token
                var acestoken = TokenHelper.IsExistAccess_Token();
                #endregion

                foreach (var item in openId)
                {
                    string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + acestoken;
                    //string temp = "{\"touser\": \"o_Ehl1vGpVLqoqH3izFbU9icGqV0,o_Ehl1s77RWjX3fitmUTsdxaM0sE\"," +
                    string temp = "{\"touser\": \""+item+"\"," +
                      "\"template_id\": \"cEVouJbAKKW-iL3o0cMfkjJWnsX3rVAAY0mmxj9aOQA\", " +
                       "\"topcolor\": \"#FF0000\", " +
                       "\"data\": " +
                       "{\"first\": {\"value\": \"同学您好，住宿费暂未缴纳\"}," +
                       "\"keyword1\": { \"value\": \"Kiaser\"}," +
                       "\"keyword2\": { \"value\": \"300元\"}," +
                       "\"remark\": {\"value\": \"缴费提醒\" }}}";
                    //核心代码
                    var resphtml = GetResponseData(temp, url);
                    var result = JObject.Parse(resphtml);
                    var code = result["errcode"].ToString();
                    strReturn = code == "0" ? "推送成功" : "推送失败:原因: " + result["errmsg"].ToString();
                }

                #region 组装信息推送，并返回结果（其它模版消息于此类似）

                //string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + acestoken;
                ////string temp = "{\"touser\": \"o_Ehl1vGpVLqoqH3izFbU9icGqV0,o_Ehl1s77RWjX3fitmUTsdxaM0sE\"," +
                //string temp = "{\"touser\": "+," +
                //      "\"template_id\": \"cEVouJbAKKW-iL3o0cMfkjJWnsX3rVAAY0mmxj9aOQA\", " +
                //       "\"topcolor\": \"#FF0000\", " +
                //       "\"data\": " +
                //       "{\"first\": {\"value\": \"您好，您的住宿费暂未缴纳\"}," +
                //       "\"keyword1\": { \"value\": \"Kiaser\"}," +
                //       "\"keyword2\": { \"value\": \"300\"}," +
                //       "\"remark\": {\"value\": \"\" }}}";
                #endregion
            
                //核心代码
                //var resphtml = GetResponseData(temp, url);

                //var result = JObject.Parse(resphtml);
                //var code = result["errcode"].ToString();
                //strReturn = code == "0" ? "推送成功" : "推送失败:原因: " + result["errmsg"].ToString();

            }
            catch (Exception ex)
            {
                strReturn = ex.Message;
            }
            return strReturn;
        }
        /// <summary>
        /// 返回JSon数据
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <param name="Url">要提交的URL</param>
        /// <returns>返回的JSON处理字符串</returns>
        public static string GetResponseData(string JSONData, string Url)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 90000;
            //设置连接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();
            return strResult;
        }
    }
}