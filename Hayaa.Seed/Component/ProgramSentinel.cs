using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Hayaa.Seed.Component
{
   public class ProgramSentinel
    {
        internal string InitSentinelService(bool isWebSystem)
        {
            if (isWebSystem) {
                return CreateWebSentinel();               
            } else {
                return CreateWindowsSentinel();
            }
        }

        private string CreateWebSentinel()
        {
            var root = HttpContext.Current.Server.MapPath("~");
            string fileTxt = "<%@ WebHandler Language=\"C#\"  Class=\"Hayaa.Seed.SentinelService\" %>";
            try {
                File.WriteAllText(root + "/Prorgrma_SentinelService.ashx", fileTxt, Encoding.UTF8);//故意拼写错误
                return "";
            }catch(Exception ex)//避免权限以及写错误
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// exe的方式更为主动
        /// </summary>
        private string CreateWindowsSentinel()
        {
            return "TODO";
           //TODO
        }
    }
}
