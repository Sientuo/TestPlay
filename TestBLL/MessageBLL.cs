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
            var responseContent = string.Empty;
            XElement xml = XElement.Parse(postStr);
            //获取消息类型
            string msgType = xml.Descendants("MsgType").FirstOrDefault().Value.ToString();
            //开发者微信号
            string ToUserName = xml.Descendants("ToUserName").FirstOrDefault().Value.ToString();
            //发送方帐号（一个OpenID）
            string FromUserName = xml.Descendants("FromUserName").FirstOrDefault().Value.ToString();
            //消息内容
            string Content = xml.Descendants("MediaId").FirstOrDefault().Value.ToString();
            if (msgType != null)
            {
                switch (msgType)
                {
                    case "image":
                        responseContent = ResMessgeHelper.ReceivedText(FromUserName, ToUserName, Content);
                        break;
                    case "text":
                        responseContent = ResMessgeHelper.ReceivedImg(FromUserName, ToUserName, Content);
                        break;
                    case "voice":
                        responseContent = ResMessgeHelper.ReceivedVoice(FromUserName, ToUserName, Content);
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }
    }
}
