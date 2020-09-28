using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TestComm.Helper
{
    public class ConfigHelper
    {
        //取值
        public static object ReadConfig(string key)
        {
            Dictionary<string, string> des = new Dictionary<string, string>();
            try
            {
                var obj = new object();
                lock (obj)
                {
                    var exePath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                    string Path = exePath + "Config" + "\\ArcConfig.xml";
                    XElement root = XElement.Load(Path);
                    var quests = from c in root.Elements() select c;
                    foreach (var item in quests)
                    {
                        //des.Add(item.Name.LocalName, item.Value);
                        CacheHelper.SetCache(item.Name.LocalName, item.Value);
                    }
                    return CacheHelper.GetCache(key);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("读取配置文件失败:" + ex.Message);
                return des;
            }
        }

        //赋值-内存
        public static void SetConfigValue(Dictionary<string, string> dic)
        {
            try
            {
                //var tis = new ArcConfigHelper();
                //PropertyInfo[] propertyInfos =tis.GetType().GetProperties();
                foreach (var item in dic)
                {
                    //CacheHelper.SetCache(item.Name, dic[item.Name]);
                    CacheHelper.SetCache(item.Key, item.Value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("写入缓存失败:" + ex.Message);
            }
        }


        //判断
        public static object GetCacheValue(string key)
        {
            return CacheHelper.GetCache(key) ?? ReadConfig(key);
        }
    }
}