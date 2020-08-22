using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TestPlay.Help
{
    public class ConfigHelper
    {
        //取值
        public static Dictionary<string, string> GetValue()
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
                        des.Add(item.Name.LocalName, item.Value);
                    }
                    return des;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.ErrLogQueue.Enqueue(ex.Message + "--" + ex.StackTrace);
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
                //LogHelper.ErrLogQueue.Enqueue(ex.Message + "---" + ex.StackTrace);
            }
        }


        //判断
        public static string ExitCache(string key)
        {
            var resValue = CacheHelper.GetCache(key); 
            try
            {
                if (resValue == null)
                {
                    SetConfigValue(GetValue());
                    return CacheHelper.GetCache(key).ToString();
                }
                return resValue.ToString();
            }
            catch (Exception ex)
            {
                return null;
                //LogHelper.ErrLogQueue.Enqueue(ex.Message + "--" + ex.StackTrace);
            }
        }
    }
}