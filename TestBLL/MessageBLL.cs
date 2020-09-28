using System.Linq;
using System.Xml.Linq;
using TestComm.Helper;

namespace TestBLL
{
    public class MessageBLL
    {
        /// <summary>
        /// 统一全局返回消息处理方法
        /// </summary>
        /// <param name="postStr"></param>
        /// <returns></returns>
        public string DealMessage(string postStr)
        { 
            string content = string.Empty;
            var responseContent = string.Empty;
            XElement xml = XElement.Parse(postStr);
            //获取消息类型
            string msgType = xml.Descendants("MsgType").FirstOrDefault().Value.ToString();
            //开发者微信号
            string ToUserName = xml.Descendants("ToUserName").FirstOrDefault().Value.ToString();
            //发送方帐号（一个OpenID）
            string FromUserName = xml.Descendants("FromUserName").FirstOrDefault().Value.ToString();
            if (msgType != null)
            {
                switch (msgType)
                {
                    case "image":
                        content = xml.Descendants("MediaId").FirstOrDefault().Value.ToString();
                        responseContent = ResMessgeHelper.ReceivedImg(FromUserName, ToUserName, content);
                        break;
                    case "text":
                        content = xml.Descendants("Content").FirstOrDefault().Value.ToString();
                        responseContent = ResMessgeHelper.ReceivedText(FromUserName, ToUserName, content);
                        break;
                    case "voice":
                        content = xml.Descendants("MediaId").FirstOrDefault().Value.ToString();
                        responseContent = ResMessgeHelper.ReceivedVoice(FromUserName, ToUserName, content);
                        break;
                    case "video":
                        content = xml.Descendants("MediaId").FirstOrDefault().Value.ToString();
                        responseContent = ResMessgeHelper.ReceivedVideo(FromUserName, ToUserName, content);
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }
    }
}
