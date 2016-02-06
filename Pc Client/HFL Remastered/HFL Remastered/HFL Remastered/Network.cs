using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using SuperSocket.ClientEngine;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using System.Media;
using System.Windows.Documents;

namespace HFL_Remastered
{
    public class Network : INotifyPropertyChanged
    {
        //public static Socket socket;
        private static WebSocket websocket = new WebSocket("ws://handsfreeleveler.com:4447/");
        private CommandManager cmd;
        private bool underRecon = false;
        public static bool socketLive {get; set;}
        private bool underSettingsNotify = false;

        public void init(double remainingTrial,int type)
        {
            socketLive = false;
            if (remainingTrial > 0 || type != (int)0)
            {
                cmd = new CommandManager();
                injectCallbacks();
                websocket.Open();
            }
            else
            {
                Text = "Trial Expired";
                Foreground = "Red";
            }
        }

        private void injectCallbacks()
        {
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.AutoSendPingInterval = 1000;
            websocket.EnableAutoSendPing = true;
        }

        private async void restartSocket()
        {
            if (!underRecon)
            {
                underRecon = true;
                string _text = Text;
                int i = 0;
                while (i < 3)
                {
                    Text = _text + ", Reconnecting in " + (3 - i) + " ...";
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    i++;
                }
                Text = "Reconnecting...";
                websocket = new WebSocket("ws://handsfreeleveler.com:4447/");
                injectCallbacks();
                websocket.Open();
                underRecon = false;
            }
        }

        private async void messageHandler(JToken msg)
        {
            switch (msg.Value<string>("type"))
            {
                case "status":
                    string remoteCount = msg.Value<string>("remoteLength");
                    Remotes = "Remote Controllers: " + remoteCount;
                    SystemSounds.Exclamation.Play();
                    SmurfManager.updateStatus();
                break;

                case "cmdWrite":
                    string input = msg.Value<string>("text");
                    cmd.writeLine(input);
                break;

                case "updateSettings":
                    Logger.Log log = new Logger.Log("info");
                    log.Text = "Settings Updated";
                    Logger.Push(log);
                    dynamic settings = msg.Value<dynamic>("settings");
                    App.Client.UserData.Settings.PacketSearch = (bool)settings.packetSearch;
                    App.Client.UserData.Settings.BuyBoost = (bool)settings.buyBoost;
                    App.Client.UserData.Settings.Reconnect = (bool)settings.reconnect;
                    App.Client.UserData.Settings.DisableGpu = (bool)settings.disableGpu;
                    App.Client.UserData.Settings.ManualInjection = (bool)settings.manualInjection;
                    SystemSounds.Asterisk.Play();
                    if (!underSettingsNotify) {
                        underSettingsNotify = true;
                        string _text = Text;
                        Text = _text + ", Settings updated.";
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        if (Text == _text + ", Settings updated.")
                        {
                            Text = _text;
                        }
                        underSettingsNotify = false;
                    }
                break;

                case "smurf":
                    if (msg.Value<string>("action") == "start")
                    {
                        Smurf newSmurf = msg.Value<dynamic>("smurf").ToObject<Smurf>();
                        SmurfManager.addSmurf(newSmurf);
                        SmurfManager.updateStatus();
                    }
                    else
                    {
                        Smurf oldSmurf = msg.Value<dynamic>("smurf").ToObject<Smurf>();
                        SmurfManager.stopSmurf(oldSmurf);
                        SmurfManager.updateStatus();
                    }
                break;
                case "group":
                    if (msg.Value<string>("action") == "start")
                    {
                        Group newGroup = msg.Value<dynamic>("group").ToObject<Group>();
                        var list = msg.Value<dynamic>("group");

                        SmurfManager.addGroup(newGroup);
                        SmurfManager.updateStatus();
                    }
                    else
                    {
                        Group newGroup = msg.Value<dynamic>("group").ToObject<Group>();
                        SmurfManager.stopGroup(newGroup);
                        SmurfManager.updateStatus();
                    }
                break;
            }
        }

        public static void upateSmurfs(List<Smurf> smurfList,List<Group> groupList){
            dynamic smurfPacket = new JObject();
            smurfPacket.type = "smurfupdate";
            smurfPacket.token = App.Client.Token;
            smurfPacket.smurfs = (JArray)JToken.FromObject(smurfList);
            smurfPacket.groups = (JArray)JToken.FromObject(groupList);
            string buffer = smurfPacket.ToString(Formatting.None);
            websocket.Send(buffer);
        }


        public void cmdNewLine(string line)
        {
            if (socketLive)
            {
                dynamic cmdPacket = new JObject();
                cmdPacket.type = "cmdLog";
                cmdPacket.text = line;
                string buffer = cmdPacket.ToString(Formatting.None);
                websocket.Send(buffer);
            }
        }

        public void sendLog(string buffer){
            if (socketLive)
            {
                websocket.Send(buffer);
            }
        }


        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            Text = "Socket crashed";
            socketLive = false;
            Foreground = "Red";
            restartSocket();
        }
        private void websocket_Closed(object sender, EventArgs e)
        {
            Text = "Disconnected";
            Foreground = "Orange";
            socketLive = false;
            restartSocket();
        }
        private void websocket_Opened(object sender, EventArgs e)
        {
            socketLive = true;
            underRecon = false;
            Text = "Connected";
            Foreground = "Green";

            dynamic loginPacket = new JObject();
            loginPacket.hwid = HWID.Generate();
            loginPacket.type = "login";
            loginPacket.token = App.Client.Token;
            string buffer = loginPacket.ToString(Formatting.None);

            websocket.Send(buffer);

            Logger.Log log = new Logger.Log("info");
            log.Text = "Client started and ready to listen commands.";
            Logger.Push(log);
        }

        protected void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            JToken res;
            try
            {
                res = JToken.Parse(e.Message);
                if (IsPropertyExists(res,"type"))
                {
                    messageHandler(res);
                }
            }
            catch (Exception)
            {

            }
        }

        /*Binding Part */
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private string _color;
        public string Foreground
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged("Foreground");
            }
        }

        private string _remotes;
        public string Remotes
        {
            get
            {
                return _remotes;
            }
            set
            {
                _remotes = value;
                OnPropertyChanged("Remotes");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public static bool IsPropertyExists(dynamic dynamicObj, string property)
        {
            try
            {
                var value = dynamicObj[property].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }

        }
    }
}


