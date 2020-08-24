using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestComm.Helper;

namespace TestPlay
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //写入配置文件到缓存
            ConfigHelper.ExitCache("AppId");

            LogHelper.WiteInfo("开始工作");
        }
    }
}
