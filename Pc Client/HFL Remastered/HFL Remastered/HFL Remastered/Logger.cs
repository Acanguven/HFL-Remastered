using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HFL_Remastered
{
    public static class Logger
    {
        public static void Push(Log log){

            dynamic logPacket = new JObject();
            logPacket.date = String.Format("{0:G}", log.Date);
            logPacket.smurf = log.Smurf;
            logPacket.text = log.Text;
            logPacket.code = log.Code;
            logPacket.token = App.Client.Token;
            logPacket.type = "log";
            string buffer = logPacket.ToString(Formatting.None);

            App.mainwindow.net.sendLog(buffer);
            log = null;
        }

        public class Log
        {
            public DateTime Date { get; set; }
            public string Smurf { get; set; }
            public string Text { get; set; }
            public string Code { get; set; }

            public Log(string code)
            {
                Date = DateTime.Now;
                Code = code;
            }
        }
    }
}
