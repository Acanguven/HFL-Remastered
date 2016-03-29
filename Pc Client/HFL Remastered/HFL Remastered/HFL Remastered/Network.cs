using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using System.ComponentModel;
using SuperSocket.ClientEngine;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using System.Media;

namespace HFL_Remastered
{
    public class Network : INotifyPropertyChanged
    {
        //public static Socket socket;
        private static WebSocket websocket = new WebSocket("ws://handsfreeleveler.com:4447/");
        private static List<string> sendqueue = new List<string>();
        private CommandManager cmd;
        private bool underRecon = false;
        public static bool socketLive { get; set; }
        private bool underSettingsNotify = false;
        private DateTime lastSend = DateTime.Now;

        private void sendManager()
        {
            while (true)
            {
                try
                {
                    DateTime tLast = lastSend;
                    tLast = tLast.AddMilliseconds(200);
                    if (tLast < DateTime.Now)
                    {
                        string msg = sendqueue.FirstOrDefault();
                        if (msg != null)
                        {
                            lastSend = DateTime.Now;
                            websocket.Send(msg);
                            sendqueue.RemoveAt(0);
                        }
                    }
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void init(double remainingTrial, int type)
        {
            socketLive = false;
            if (remainingTrial > 0 || type != (int)0)
            {
                cmd = new CommandManager();
                injectCallbacks();
                if (websocket.State != WebSocketState.Open)
                {
                    websocket.Open();
                }
            }
            else
            {
                Text = "Trial Expired";
                Foreground = "Red";
            }
            Thread managerThread = new Thread(new ThreadStart(sendManager));
            managerThread.Start();
        }

        private void injectCallbacks()
        {
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
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
                    Logger.Push("Settings Updated", "info");
                    dynamic settings = msg.Value<dynamic>("settings");

                    App.Client.UserData.Settings.PacketSearch = (bool)settings.packetSearch;
                    Properties.Settings.Default.packetSearch = App.Client.UserData.Settings.PacketSearch;
                    App.Client.UserData.Settings.BuyBoost = (bool)settings.buyBoost;
                    Properties.Settings.Default.buyBoost = App.Client.UserData.Settings.BuyBoost;
                    App.Client.UserData.Settings.Reconnect = (bool)settings.reconnect;
                    Properties.Settings.Default.reconnect = App.Client.UserData.Settings.Reconnect;
                    App.Client.UserData.Settings.DisableGpu = (bool)settings.disableGpu;
                    Properties.Settings.Default.disableGpu = App.Client.UserData.Settings.DisableGpu;
                    App.Client.UserData.Settings.ManualInjection = (bool)settings.manualInjection;
                    Properties.Settings.Default.manualInjection = App.Client.UserData.Settings.ManualInjection;
                    Properties.Settings.Default.Save();

                    App.gameContainer.Dispatcher.Invoke(new Action(() =>
                    {
                        if (App.Client.UserData.Settings.DisableGpu)
                        {
                            App.gameContainer.Show();
                            FileManager.LockCamera();
                        }
                        else
                        {
                            if (App.gameContainer.runningCount() == 0)
                            {
                                App.gameContainer.Hide();
                            }
                        }
                    }), DispatcherPriority.ContextIdle);



                    SystemSounds.Asterisk.Play();
                    if (!underSettingsNotify)
                    {
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
                    double remainingTrialSm = Math.Round((double)((App.Client.UserData.Trial - App.Client.Date) / 60000));
                    bool valid = SmurfManager.smurfs.Count == 0 || App.Client.UserData.Type == (int)2 || (App.Client.UserData.Type == (int)0 && remainingTrialSm > 0);
                    if (msg.Value<string>("action") == "start")
                    {
                        if (valid)
                        {
                            Smurf newSmurf = msg.Value<dynamic>("smurf").ToObject<Smurf>();
                            SmurfManager.addSmurf(newSmurf);
                            SmurfManager.updateStatus();
                        }
                    }
                    else
                    {
                        Smurf oldSmurf = msg.Value<dynamic>("smurf").ToObject<Smurf>();
                        SmurfManager.stopSmurf(oldSmurf);
                        SmurfManager.updateStatus();
                    }
                    break;
                case "group":
                    double remainingTrialGm = Math.Round((double)((App.Client.UserData.Trial - App.Client.Date) / 60000));
                    if (App.Client.UserData.Type == (int)2 || (App.Client.UserData.Type == (int)0 && remainingTrialGm > 0))
                    {
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
                    }
                    break;
            }
        }

        public static void upateSmurfs(List<Smurf> smurfList, List<Group> groupList)
        {
            dynamic smurfPacket = new JObject();
            smurfPacket.type = "smurfupdate";
            smurfPacket.token = App.Client.Token;
            smurfPacket.smurfs = (JArray)JToken.FromObject(smurfList);
            smurfPacket.groups = (JArray)JToken.FromObject(groupList);
            string buffer = smurfPacket.ToString(Formatting.None);
            sendqueue.Add(buffer);
        }

        public static void updateSmurfData(Smurf smurf)
        {
            dynamic smurfPacket = new JObject();
            smurfPacket.type = "smurfdbupdate";
            smurfPacket.token = App.Client.Token;
            smurfPacket.smurf = JToken.FromObject(smurf);
            smurfPacket.smurf.currentLevel = smurf.currentLevel;
            smurfPacket.smurf.currentip = smurf.currentip;
            smurfPacket.smurf.currentrp = smurf.currentrp;
            string buffer = smurfPacket.ToString(Formatting.None);
            sendqueue.Add(buffer);
        }


        public void cmdNewLine(string line)
        {
            if (socketLive)
            {
                dynamic cmdPacket = new JObject();
                cmdPacket.type = "cmdLog";
                cmdPacket.text = line;
                string buffer = cmdPacket.ToString(Formatting.None);
                sendqueue.Add(buffer);
            }
        }

        public void sendLog(string buffer)
        {
            if (socketLive)
            {
                sendqueue.Add(buffer);
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

            sendqueue.Add(buffer);

            Logger.Push("Client started and ready to listen commands.", "info");
        }

        protected void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            JToken res;
            try
            {
                res = JToken.Parse(e.Message);
                if (IsPropertyExists(res, "type"))
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

        public bool IsPropertyExists(dynamic dynamicObj, string property)
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


