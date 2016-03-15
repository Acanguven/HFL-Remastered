using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace HandsFreeLeveler
{
    public class Smurf : INotifyPropertyChanged
    {
        public string username { get; set; }
        public string password { get; set; }
        internal Button button { get; set; }
        private int exp = 0;
        public int expCalc
        {
            get
            {
                return exp;
            }
            set
            {
                exp = value;
                OnPropertyChanged("expCalc");
            }
        }
        public LoLLauncher.Region region { get; set; }
        private int _level = 1;
        public int level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged("level");
            }
        }
        internal string clientMask { get; set; }
        public int maxLevel { get; set; }
        internal ObservableCollection<SmurfLog> Status = new ObservableCollection<SmurfLog>();
        public bool reconnect = false;
        private int _Logs = 0;
        public int Logs
        {
            get { return _Logs; }
            set
            {
                _Logs = value;
                OnPropertyChanged("Logs");
            }
        }
        internal RiotBot thread { get; set; }

        public Smurf()
        {
            log("Idle");
        }

        public void start()
        {
            log("Starting new thread");
            if (reconnect)
            {
                reconnect = false;
            }
            stop();
            thread = new RiotBot(username, password, maxLevel, region.ToString(), Settings.gamePath, Settings.queueType, clientMask, this);
            thread.connection.OnDisconnect += new LoLLauncher.LoLConnection.OnDisconnectHandler(this.connection_OnDisconnect);
            thread.connection.OnConnect += new LoLLauncher.LoLConnection.OnConnectHandler(this.connection_OnConnect);
        }

        public void stop()
        {
            if (thread != null)
            {
                if (thread.connection.IsConnected())
                {
                    thread.connection.Disconnect();
                }
                thread = null;
            }
            log("Stopping existing thread");
        }

        private void connection_OnDisconnect(object sender, EventArgs e)
        {
            log("Smurf Disconnected");
            stop();
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                button.Content = "Start";
            });
            if (reconnect)
            {
                start();
            }
        }

        private void connection_OnConnect(object sender, EventArgs e)
        {
            log("Smurf Connected");
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                button.Content = "Stop";
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void log(string text)
        {
            SmurfLog log = new SmurfLog(text);
            Logs++;
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                try
                {
                    Status.Insert(0, log);
                }
                catch (Exception)
                {

                }
            });
        }

        public void updateExpLevel(double extNeeded, double expOwner)
        {
            this.expCalc = (int)(expOwner * 100 / extNeeded);
        }
    }

    public class SmurfLog
    {
        public string Date { get; set; }
        public string Log { get; set; }

        public SmurfLog(string _log)
        {
            Log = _log;
            Date = DateTime.Now.ToString();
        }
    }
}
