using System;

namespace TestComm.Helper
{
    public class ResMessgeHelper
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="ToUserName"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string ReceivedText(string FromUserName, string ToUserName, string Content)
        {
            string textpl = string.Empty;
            Content = "您发送的消息为：" + Content + "\n" + "您的openId：" + FromUserName;
            textpl = "<xml>" +
                     "<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
                     "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
                     "<CreateTime>" + DateTime.Now + "</CreateTime>" +
                     "<MsgType><![CDATA[text]]></MsgType>" +
                     "<Content><![CDATA[" + Content + "]]></Content>" +
                     "</xml>";
            return textpl;
        }

        /// <summary>
        /// 图片消息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="ToUserName"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string ReceivedImg(string FromUserName, string ToUserName, string content)
        {
            string textpl = string.Empty;
            textpl = "<xml>" +
                     "<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
                     "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
                     "<CreateTime>" + DateTime.Now + "</CreateTime>" +
                     "<MsgType><![CDATA[image]]></MsgType>" +
                     "<Image>"+
                     "<MediaId><![CDATA[" + content + "]]></MediaId>" +
                     "</Image>" +
                     "</xml>";
            return textpl;
        }

        /// <summary>
        /// 语音消息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="ToUserName"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string ReceivedVoice(string FromUserName, string ToUserName, string content)
        {
            string textpl = string.Empty;
            textpl = "<xml>" +
                     "<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
                     "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
                     "<CreateTime>" + DateTime.Now + "</CreateTime>" +
                     "<MsgType><![CDATA[voice]]></MsgType>" +
                     "<Voice>" +
                     "<MediaId><![CDATA[" + content + "]]></MediaId>" +
                     "</Voice>" +
                     "</xml>";
            return textpl;
        }

    }
}