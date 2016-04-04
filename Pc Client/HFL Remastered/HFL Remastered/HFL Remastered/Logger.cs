using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HFL_Remastered
{
    public static class Logger
    {
        public static void Push(string text, string code, string smurf)
        {
            if (Properties.Settings.Default.logging)
            {
                try
                {
                    dynamic logPacket = new JObject();
                    logPacket.date = String.Format("{0:G}", DateTime.Now);
                    logPacket.smurf = smurf;
                    logPacket.text = text;
                    logPacket.code = code;
                    logPacket.token = App.Client.Token;
                    logPacket.type = "log";
                    string buffer = logPacket.ToString(Formatting.None);
                    App.mainwindow.net.sendLog(buffer);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void Push(string text, string code)
        {
            if (Properties.Settings.Default.logging)
            {
                try
                {
                    dynamic logPacket = new JObject();
                    logPacket.date = String.Format("{0:G}", DateTime.Now);
                    logPacket.text = text;
                    logPacket.code = code;
                    logPacket.token = App.Client.Token;
                    logPacket.type = "log";
                    string buffer = logPacket.ToString(Formatting.None);
                    App.mainwindow.net.sendLog(buffer);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
