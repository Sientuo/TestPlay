using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using TestBLL;
using TestComm.Helper;

namespace TestPlay.Controllers
{
    public class WeChatController : Controller
    {
        public  MessageBLL messageBLL = new MessageBLL();

        // GET: WeChat
        [HttpGet]
        [ActionName("Index")]
        public Task<ActionResult> Get(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew(() =>
            {
                var token = ConfigHelper.GetCacheValue("Token").ToStr();
                if (CheckSignature.Check(signature, timestamp, nonce, token))
                {
                    //获取Token
                    var acestoken = TokenHelper.GetStringToken();
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
        public void Post(PostModel postModel)
        {
            //校验签名
            var token = ConfigHelper.GetCacheValue("Token").ToStr();
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                //System.Web.HttpContext.Current.Response.Write("参数验证失败");
                LogHelper.WriteError("签名验证失败");
                return;
            }
            string postString = string.Empty;
            using (Stream stream = System.Web.HttpContext.Current.Request.InputStream)
            {
                byte[] byts = new byte[stream.Length];
                stream.Read(byts, 0, (int)stream.Length);
                postString = Encoding.UTF8.GetString(byts);
                //处理 接收到数据
                string responseContent = messageBLL.DealMessage(postString);
                System.Web.HttpContext.Current.Response.Write(responseContent);
            }
        }

    }
}