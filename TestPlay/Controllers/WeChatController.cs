using Senparc.Weixin.MP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using TestPlay.Help;
using WetChat.Helper;

namespace TestPlay.Controllers
{
    public class WeChatController : Controller
    {
        // GET: WeChat
        //回复消息

        [HttpGet]
        [ActionName("Index")]
        public Task<ActionResult> Get(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew(() =>
            {
                var token = CacheHelper.GetCache("Token").ToString();
                if (CheckSignature.Check(signature, timestamp, nonce, token))
                {
                    //获取Token
                    var acestoken = TokenHelper.IsExistAccess_Token();
                    return echostr; //返回随机字符串则表示验证通过
                }
                else
                {
                    return "failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, token) + "。" + "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。";
                }
            }).ContinueWith<ActionResult>(task => Content(task.Result));
        }

        [HttpPost]
        [ActionName("Index")]
        public void Post(Senparc.Weixin.MP.Entities.Request.PostModel postModel)
        {
            //校验签名
            var token = CacheHelper.GetCache("Token").ToString();
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                //return new WeixinResult("参数错误！");
            }
            string postString = string.Empty;
            using (Stream stream = System.Web.HttpContext.Current.Request.InputStream)
            {
                byte[] postBytes = new byte[stream.Length];
                stream.Read(postBytes, 0, (int)stream.Length);
                //接收的消息为GBK格式
                postString = Encoding.UTF8.GetString(postBytes);
                string responseContent = ReturnMessage(postString);
                //返回的消息为UTF-8格式
                System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                System.Web.HttpContext.Current.Response.Write(responseContent);
            }
        }


        /// <summary>
        /// 统一全局返回消息处理方法
        /// </summary>
        /// <param name="postStr"></param>
        /// <returns></returns>
        public string ReturnMessage(string postStr)
        {
            var responseContent = string.Empty;
            XElement xml = XElement.Parse(postStr);
            //获取消息类型
            string msgType = xml.Descendants("MsgType").FirstOrDefault().Value.ToString();
            if (msgType != null)
            {
                switch (msgType)
                {
                    case "image":
                        responseContent = ImageHandle(xml);
                        break;
                    case "text":
                        responseContent = TextHandle(xml);
                        break;
                    case "voice":
                        responseContent = VoiceHandle(xml);
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }

        /// <summary>
        /// 接受文本消息并回复自定义消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string TextHandle(XElement xmldoc)
        {
            string responseContent = string.Empty;
            string ToUserName = xmldoc.Descendants("ToUserName").FirstOrDefault().Value.ToString();
            string FromUserName = xmldoc.Descendants("FromUserName").FirstOrDefault().Value.ToString();
            string Content = xmldoc.Descendants("Content").FirstOrDefault().Value.ToString();
            Content = "您发送的消息为：" + Content + "\n" + "您的openId：" + FromUserName;
            responseContent = ResMessgeHelper.ReceivedText(FromUserName, ToUserName, Content);
            return responseContent;
        }


        public string ImageHandle(XElement xmlimg)
        {
            string responseContent = string.Empty;
            string ToUserName = xmlimg.Descendants("ToUserName").FirstOrDefault().Value.ToString();
            string FromUserName = xmlimg.Descendants("FromUserName").FirstOrDefault().Value.ToString();
            string Content = xmlimg.Descendants("MediaId").FirstOrDefault().Value.ToString();
            responseContent = ResMessgeHelper.ReceivedImg(FromUserName, ToUserName, Content);
            return responseContent;
        }

        public string VoiceHandle(XElement xmlimg)
        {
            string responseContent = string.Empty;
            string ToUserName = xmlimg.Descendants("ToUserName").FirstOrDefault().Value.ToString();
            string FromUserName = xmlimg.Descendants("FromUserName").FirstOrDefault().Value.ToString();
            string Content = xmlimg.Descendants("MediaId").FirstOrDefault().Value.ToString();
            responseContent = ResMessgeHelper.ReceivedVoice(FromUserName, ToUserName, Content);
            return responseContent;
        }
    }
}