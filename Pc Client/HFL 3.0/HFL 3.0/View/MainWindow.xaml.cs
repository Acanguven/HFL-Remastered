using System;
using System.Windows;
using Awesomium.Core;
using Awesomium.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.IO;
using static HFL_3._0.Connection;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Linq;
using HFL_3._0.Core;

namespace HFL_3._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point dragPoint;
        private bool dragStarted = false;
        private bool dragAllowed = true;

        private JSObject bolCb;
        private bool bolListenerReady = false;
        private bool _bolRunning = false;
        public bool bolRunning
        {
            get { return _bolRunning; }
            set
            {
                if (_bolRunning != value)
                {
                    _bolRunning = value;
                    Dispatcher.Invoke(() =>
                    {
                        bolCb.Clone()?.Invoke("call", bolCb, _bolRunning);
                    });
                }
            }
        }

        private BackgroundWorker bolCheckerWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            Source = WebCore.Configuration.HomeURL;
            webControl.Loaded += WebViewOnDocumentReady;
            //WebControlService.SetShowDesignTimeLogo(webControl, false);
            webControl.ShowContextMenu += Html5_ShowContextMenu;
            AllowDrop = false;
            webControl.DragEnter += OnDragEnter;
            webControl.Crashed += OnCrashed;
            webControl.Drop += OnDrop;
            PreviewMouseMove += WebControl_MouseMove;

#if DEBUG
            webControl.ConsoleMessage += delegate (object sender, ConsoleMessageEventArgs e)
            {
                Console.WriteLine("console.log: " + e.Message);
            };
#endif


            AddHandler(MouseDownEvent, new MouseButtonEventHandler(Window_MouseDown), true);
            AddHandler(MouseUpEvent, new MouseButtonEventHandler(Window_MouseUp), true);

            bolCheckerWorker.DoWork += new DoWorkEventHandler(bolCheck);
            bolCheckerWorker.RunWorkerAsync();


        }

        private static void ClientMessageReceived(int id, String msg)
        {
            Console.WriteLine("Server get Message from client. {0} ", msg);
        }

        private void bolCheck(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (bolListenerReady)
                {
                    bolRunning = Process.GetProcessesByName("BoL Studio").Length > 0;
                }
                Thread.Sleep(200);
            }
        }

        private void OnCrashed(object sender, CrashedEventArgs e)
        {
            webControl.Reload(true);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.None;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                var x = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (x != null && x.Length > 0)
                {
                    string droppedFile = x[0];
                    //handle drop here
                    webControl.ExecuteJavascript("fileDropped('tt');");
                }
            }
        }

        private void WebControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStarted)
            {
                Point pointMoveTo;
                pointMoveTo = PointToScreen(e.GetPosition(this));
                pointMoveTo.Offset(-dragPoint.X, -dragPoint.Y);
                Left = pointMoveTo.X;
                Top = pointMoveTo.Y;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left && !dragStarted && dragAllowed)
            {
                dragPoint = e.GetPosition(this);
                dragStarted = true;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left && dragStarted)
            {
                dragStarted = false;
            }
        }

        private void Html5_ShowContextMenu(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void WebViewOnDocumentReady(object sender, EventArgs eventArgs)
        {
            App.NET = webControl.CreateGlobalJavascriptObject("NET");
            App.NET.Bind("isAuthenticated", isAuthenticated);
            App.NET.Bind("resize", Resizer);
            App.NET.Bind("centerScreen", CenterWindowOnScreen);
            App.NET.BindAsync("login", Login);
            App.NET.Bind("openRegister", openRegister);
            App.NET.Bind("disableMove", disableMove);
            App.NET.Bind("enableMove", enableMove);
            App.NET.Bind("loadSettings", loadSettings);
            App.NET.BindAsync("saveSettings", saveSettings);
            App.NET.BindAsync("loadSmurfs", loadSmurfs);
            App.NET.BindAsync("saveSmurfs", saveSmurfs);
            App.NET.BindAsync("loadGroups", loadGroups);
            App.NET.BindAsync("saveGroups", saveGroups);
            App.NET.BindAsync("requestLogin", requestLogin);
            App.NET.BindAsync("loadFromFile", loadFromFile);
            App.NET.BindAsync("saveRotations", saveRotations);
            App.NET.BindAsync("loadRotations", loadRotations);
            App.NET.BindAsync("registerHomeCb", registerHomeCb);
            App.NET.BindAsync("action", action);
            App.NET.BindAsync("getSessionTime", getSessionTime);
            App.NET.BindAsync("ready", ready);
            App.NET.BindAsync("getBolStatus", getBolStatus);
            App.NET.BindAsync("getSessions", getSessions);

            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetApplicationIdForSpecificWindow(new WindowInteropHelper(this).Handle, "HFLGROUP");
            }
        }

        private void getSessions(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg;

            List<sessionSave> sessions;
            if (File.Exists("data/sessions.xml"))
            {
                sessions = Storage.DeSerializeObject<List<sessionSave>>("data/sessions.xml");
                JSValue[] js_arr = new JSValue[sessions.Count];
                int count = 0;
                JSObject session;
                foreach(var ses in sessions)
                {
                    session = new JSObject();
                    session["startTime"] = ses.startTime;
                    session["endTime"] = ses.endTime;
                    session["rotations"] = ses.rotations.Count;
                    session["state"] = ses.state.ToString();
                    session["errorText"] = ses.errorText;

                    JSValue[] js_Smurfarr = new JSValue[ses.smurfs.Count];
                    int count2 = 0;
                    JSObject smrfI;
                    foreach (var smurfSave in ses.smurfs)
                    {
                        smrfI = new JSObject();
                        smrfI["username"] = smurfSave.username;
                        smrfI["region"] = smurfSave.region.ToString();
                        smrfI["startXp"] = smurfSave.startXp;
                        smrfI["startIp"] = smurfSave.startIp;
                        smrfI["startLevel"] = smurfSave.startLevel;
                        smrfI["startXpToNextLevel"] = smurfSave.startXpToNextLevel;
                        if (smurfSave.endIp != null)
                        {
                            smrfI["endIp"] = smurfSave.endIp;
                        }
                        if (smurfSave.endXp != null)
                        {
                            smrfI["endXp"] = smurfSave.endXp;
                        }
                        if (smurfSave.endLevel != null)
                        {
                            smrfI["endLevel"] = smurfSave.endLevel;
                        }
                        if (smurfSave.endXpToNextLevel != null)
                        {
                            smrfI["endXpToNextLevel"] = smurfSave.endXpToNextLevel;
                        }
                        js_Smurfarr[count2++] = smrfI;
                    }
                    session["smurfs"] = js_Smurfarr;
                    js_arr[count++] = session;
                }
                cb?.Invoke("call", callbackarg, js_arr);
            }
            else
            {
                JSValue[] js_arr = new JSValue[] { };
                cb?.Invoke("call", callbackarg, js_arr);
            }
        }

        private void getBolStatus(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            bolCb = callbackarg;
            bolListenerReady = true;
        }

        private void ready(object sender, JavascriptMethodEventArgs e)
        {
            loaderGif.Visibility = Visibility.Hidden;
            loaderText.Visibility = Visibility.Hidden;
        }

        private void getSessionTime(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();
            cb?.Invoke("call", callbackarg, Rotator.getSessionTime());
        }

        private void action(object sender, JavascriptMethodEventArgs e)
        {
            if (e.Arguments[0] == 0)
            {
                Rotator.pause();
            }
            if (e.Arguments[0] == 1)
            {
                Rotator.stop();
            }
            if (e.Arguments[0] == 2)
            {
                Rotator.setLimit((int)e.Arguments[1]);
                new Thread(() =>
                {
                    checkThingsToStart();
                }).Start();
            }
            if (e.Arguments[0] == 3)
            {
                Rotator.start();
            }
        }

        private void checkThingsToStart()
        {
            if(SystemParameters.PowerLineStatus != PowerLineStatus.Online)
            {
                var confirmResult = MessageBox.Show("Your power cable isn't connected, do you still want to start?","Confirm Start",MessageBoxButton.YesNo);
                if (confirmResult != MessageBoxResult.Yes )
                {
                    return;
                }
            }
            if (!bolRunning)
            {
                var confirmResult = MessageBox.Show("Bot Of Legends is not running, do you still want to start?", "Confirm Start", MessageBoxButton.YesNo);
                if (confirmResult != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            if (App.settings.checkFolderSettings())
            {
                Rotator.start();
            }
        }

        private void registerHomeCb(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();
            Rotator.updateCallback(cb);
        }

        private void loadRotations(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();

            if (File.Exists("rotations.xml"))
            {
                List<rotationJobSave> _TRotList = Storage.DeSerializeObject<List<rotationJobSave>>("rotations.xml");
                JSValue[] js_arr = new JSValue[_TRotList.Count];
                int count = 0;
                JSObject rotation;
                List<SmurfData> _TsmurfList = Storage.DeSerializeObject<List<SmurfData>>("smurfs.xml");
                List<GroupData> _groupList = Storage.DeSerializeObject<List<GroupData>>("groups.xml");
                foreach (var rot in _TRotList)
                {
                    rotation = new JSObject();
                    rotation["type"] = rot.type;
                    rotation["pcsleep"] = rot.pcsleep;
                    rotation["waittime"] = rot.waittime;
                    rotation["left"] = rot.left;
                    rotation["name"] = rot.name;
                    rotation["endType"] = rot.endType;
                    rotation["top"] = rot.top;
                    rotation["id"] = rot.id;
                    if (rot.queuePos != null)
                    {
                        rotation["queuePos"] = rot.queuePos;
                    }
                    else
                    {
                        rotation["queuePos"] = "";
                    }



                    int counter2 = 0;
                    int validSmurfCount = 0;

                    foreach (var smrf in rot.smurfIds)
                    {
                        if (_TsmurfList.Exists(i => i.id == smrf))
                        {
                            validSmurfCount++;
                        }
                    }

                    JSValue[] js_Smurfarr = new JSValue[validSmurfCount];

                    foreach (var smrf in rot.smurfIds)
                    {
                        JSObject smrfI = new JSObject();
                        if (_TsmurfList.Exists(i => i.id == smrf))
                        {
                            SmurfData foundSmurf = _TsmurfList.First(i => i.id == smrf);
                            smrfI["id"] = foundSmurf.id;
                            smrfI["username"] = foundSmurf.username;
                            smrfI["region"] = foundSmurf.region.ToString();
                            js_Smurfarr[counter2] = smrfI;
                            counter2++;
                        }
                    }

                    rotation["smurfs"] = js_Smurfarr;


                    int counter3 = 0;
                    int validGroupCount = 0;

                    foreach (var grp in rot.groupIds)
                    {
                        if (_groupList.Exists(i => i.id == grp))
                        {
                            validGroupCount++;
                        }
                    }

                    JSValue[] js_Grouparr = new JSValue[validGroupCount];

                    foreach (var grp in rot.groupIds)
                    {
                        JSObject grpFI = new JSObject();
                        if (_groupList.Exists(i => i.id == grp))
                        {
                            GroupData group = _groupList.First(i => i.id == grp);
                            grpFI["id"] = group.id;
                            grpFI["name"] = group.name;
                            grpFI["smurfs"] = new JSValue[group.smurfIds.Count];
                            js_Grouparr[counter3] = grpFI;
                            counter3++;
                        }
                    }

                    rotation["groups"] = js_Grouparr;


                    js_arr[count] = rotation;
                    count++;
                }
                cb?.Invoke("call", callbackarg, js_arr);
            }
            else
            {
                JSValue[] js_arr = new JSValue[] { };
                cb?.Invoke("call", callbackarg, js_arr);
            }
        }

        private void saveRotations(object sender, JavascriptMethodEventArgs e)
        {
            if (e.Arguments[0].IsArray)
            {
                List<rotationJobSave> rotationSaves = new List<rotationJobSave>();
                rotationJobSave savingRot;
                JSValue rotationArray = e.Arguments[0];
                foreach (JSObject rot in (JSValue[])rotationArray)
                {
                    savingRot = new rotationJobSave();
                    if (rot["type"] == "wait")
                    {
                        int waitMinutes = int.Parse(rot["waittime"]);
                        bool pcsleep;
                        if (rot["pcsleep"])
                        {
                            pcsleep = bool.Parse(rot["pcsleep"]);
                        }
                        else
                        {
                            pcsleep = false;
                        }

                        savingRot.waittime = waitMinutes;
                        savingRot.pcsleep = pcsleep;
                    }
                    if (rot["type"] == "task")
                    {
                        int waitMinutes = int.Parse(rot["waittime"]);
                        savingRot.waittime = waitMinutes;

                        foreach (JSObject smrf in (JSValue[])rot["smurfs"])
                        {
                            savingRot.smurfIds.Add(smrf["id"]);
                        }

                        foreach (JSObject gr in (JSValue[])rot["groups"])
                        {
                            savingRot.groupIds.Add(gr["id"]);
                        }
                        savingRot.endType = rot["endType"];
                    }

                    savingRot.left = (double)rot["left"];
                    savingRot.top = (double)rot["top"];
                    savingRot.type = rot["type"];
                    savingRot.name = rot["name"];
                    savingRot.id = rot["id"];
                    if (rot["queuePos"] != "")
                    {
                        savingRot.queuePos = rot["queuePos"];
                    }
                    else
                    {
                        savingRot.queuePos = null;
                    }
                    rotationSaves.Add(savingRot);
                }
                Storage.SerializeObject(rotationSaves, "rotations.xml");
            }
        }

        private void loadFromFile(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                string readText = File.ReadAllText(filename);
                cb?.Invoke("call", callbackarg, readText);
            }
        }

        private void loadGroups(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();

            if (File.Exists("groups.xml"))
            {
                List<GroupData> _groupList = Storage.DeSerializeObject<List<GroupData>>("groups.xml");
                List<SmurfData> _TsmurfList = Storage.DeSerializeObject<List<SmurfData>>("smurfs.xml");
                JSValue[] js_arr = new JSValue[_groupList.Count];
                JSObject grp;
                int count = 0;
                foreach (var group in _groupList)
                {

                    grp = new JSObject();
                    grp["name"] = group.name;
                    grp["id"] = group.id;
                    grp["queue"] = group.queue;

                    int counter2 = 0;
                    int validSmurfCount = 0;

                    foreach (var smrf in group.smurfIds)
                    {
                        if (_TsmurfList.Exists(i => i.id == smrf))
                        {
                            validSmurfCount++;
                        }
                    }

                    JSValue[] js_Smurfarr = new JSValue[validSmurfCount];

                    foreach (var smrf in group.smurfIds)
                    {
                        JSObject smrfI = new JSObject();
                        if (_TsmurfList.Exists(i => i.id == smrf))
                        {
                            SmurfData foundSmurf = _TsmurfList.First(i => i.id == smrf);
                            smrfI["id"] = foundSmurf.id;
                            smrfI["username"] = foundSmurf.username;
                            smrfI["region"] = foundSmurf.region.ToString();
                            js_Smurfarr[counter2] = smrfI;
                            counter2++;
                        }
                    }

                    grp["smurfs"] = js_Smurfarr;

                    js_arr[count] = grp;
                    count++;
                }
                cb?.Invoke("call", callbackarg, js_arr);
            }
            else
            {
                JSValue[] arr = new JSValue[] { };
                cb?.Invoke("call", callbackarg, arr);
            }
        }

        private void saveGroups(object sender, JavascriptMethodEventArgs e)
        {
            if (e.Arguments[0].IsArray)
            {
                JSValue GroupArray = e.Arguments[0];
                List<GroupData> groupListOut = new List<GroupData>();
                foreach (JSObject grp in (JSValue[])GroupArray)
                {
                    GroupData group = new GroupData();
                    group.name = grp["name"];
                    group.queue = grp["queue"];
                    group.id = grp["id"];
                    foreach (JSObject smrf in (JSValue[])grp["smurfs"])
                    {
                        group.smurfIds.Add(smrf["id"]);
                    }
                    groupListOut.Add(group);
                }
                Storage.SerializeObject(groupListOut, "groups.xml");
            }
        }

        private void loadSmurfs(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();

            if (File.Exists("smurfs.xml"))
            {
                List<SmurfData> _TsmurfList = Storage.DeSerializeObject<List<SmurfData>>("smurfs.xml");
                JSValue[] js_arr = new JSValue[_TsmurfList.Count];
                JSObject smrf;
                int count = 0;
                foreach (var smurf in _TsmurfList)
                {

                    smrf = new JSObject();
                    smrf["id"] = smurf.id;
                    smrf["username"] = smurf.username;
                    smrf["password"] = smurf.password;
                    smrf["region"] = smurf.region.ToString();
                    smrf["queue"] = smurf.queue;
                    smrf["currentLevel"] = smurf.currentLevel;
                    smrf["currentRp"] = smurf.currentRp;
                    smrf["desiredLevel"] = smurf.desiredLevel;
                    js_arr[count] = smrf;
                    count++;
                }
                cb?.Invoke("call", callbackarg, js_arr);
            }
            else
            {
                JSValue[] arr = new JSValue[] { };
                cb?.Invoke("call", callbackarg, arr);
            }
        }

        private void saveSmurfs(object sender, JavascriptMethodEventArgs e)
        {
            if (e.Arguments[0].IsArray)
            {
                JSValue smurfArray = e.Arguments[0];
                List<SmurfData> smurfListOut = new List<SmurfData>();
                foreach (JSObject smrf in (JSValue[])smurfArray)
                {
                    SmurfData smurf = new SmurfData();
                    smurf.id = smrf["id"];
                    smurf.username = smrf["username"];
                    smurf.password = smrf["password"];
                    smurf.region = (LoLLauncher.Region)Enum.Parse(typeof(LoLLauncher.Region), smrf["region"]);
                    smurf.queue = smrf["queue"];
                    smurf.desiredLevel = double.Parse(smrf["desiredLevel"].ToString());
                    smurfListOut.Add(smurf);
                }
                Storage.SerializeObject(smurfListOut, "smurfs.xml");
            }
        }

        private JSValue loadSettings(object sender, JavascriptMethodEventArgs e)
        {
            JSObject settingsObj = new JSObject();
            settingsObj["theme"] = App.settings.theme;
            settingsObj["language"] = App.settings.language;
            settingsObj["cPacketSearch"] = App.settings.cPacketSearch;
            settingsObj["buyBoost"] = App.settings.buyBoost;
            settingsObj["reconnect"] = App.settings.reconnect;
            settingsObj["disableGpu"] = App.settings.disableGpu;
            settingsObj["injection"] = App.settings.injection;
            return settingsObj;
        }

        private void saveSettings(object sender, JavascriptMethodEventArgs e)
        {
            App.settings.theme = e.Arguments[0];
            App.settings.language = e.Arguments[1];
            App.settings.cPacketSearch = e.Arguments[2];
            App.settings.buyBoost = e.Arguments[3];
            App.settings.reconnect = e.Arguments[4];
            App.settings.disableGpu = e.Arguments[5];
            App.settings.injection = e.Arguments[6];
            App.settings.saveSettings();
        }

        private JSValue Resizer(object sender, JavascriptMethodEventArgs e)
        {
            Height = int.Parse(e.Arguments[1].ToString());
            Width = int.Parse(e.Arguments[0].ToString());

            return JSValue.Undefined;
        }

        private JSValue disableMove(object sender, JavascriptMethodEventArgs e)
        {
            dragAllowed = false;
            return JSValue.Undefined;
        }

        private JSValue enableMove(object sender, JavascriptMethodEventArgs e)
        {
            dragAllowed = true;
            return JSValue.Undefined;
        }

        private JSValue CenterWindowOnScreen(object sender, JavascriptMethodEventArgs e)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (int)Math.Round((screenWidth / 2) - (windowWidth / 2));
            Top = (int)Math.Round((screenHeight / 2) - (windowHeight / 2));
            return JSValue.Undefined;
        }

        private void requestLogin(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();
            if (File.Exists("loginDetails.xml"))
            {
                LoginContract details = Storage.DeSerializeObject<LoginContract>("loginDetails.xml");
                cb?.Invoke("call", callbackarg, details.username, details.password);
            }
        }

        private void Login(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[2];
            JSObject cb = callbackarg.Clone();
            login(e.Arguments[0].ToString(), e.Arguments[1].ToString(), HWID.Generate(), cb);
        }

        private JSValue isAuthenticated(object sender, JavascriptMethodEventArgs e)
        {
            return false;
        }

        private JSValue openRegister(object sender, JavascriptMethodEventArgs e)
        {
            MessageBox.Show("Register");
            return JSValue.Undefined;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Destroy the WebControl and its underlying view.
            webControl.Dispose();
        }

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
                DependencyProperty.Register("Source",
                typeof(Uri), typeof(MainWindow),
                new FrameworkPropertyMetadata(null));

        private void webWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

    }
}
