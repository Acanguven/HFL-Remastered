namespace RitoBot
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
    using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
    using LoLLauncher.RiotObjects.Platform.Game;
    using LoLLauncher.RiotObjects.Platform.Matchmaking;
    using LoLLauncher.RiotObjects.Platform.Statistics;
    using LoLLauncher.RiotObjects.Platform.Trade;
    using RitoBot.Utils;
    using RitoBot.Utils.Region;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using VoliBot.LoLLauncher.RiotObjects.Platform.Messaging;
    using VoliBot.LoLLauncher.RiotObjects.Platform.ServiceProxy.Dispatch;
    using VoliBot.Utils;

    internal class VoliBot
    {
        public string Accountname;
        public List<ChampionDTO> availableChamps = new List<ChampionDTO>();
        public ChampionDTO[] availableChampsArray;
        public BaseRegion baseRegion;
        public LoLConnection connection = new LoLConnection();
        public GameDTO currentGame = new GameDTO();
        public string errorMSG1;
        public string errorMSG2;
        public Process exeProcess;
        public bool firstTimeInCustom = true;
        public bool firstTimeInLobby = true;
        public bool firstTimeInPostChampSelect = true;
        public bool firstTimeInQueuePop = true;
        public string ipath;
        public LoginDataPacket loginPacket = new LoginDataPacket();
        public string Password;
        public bool reAttempt;
        public int relogTry;

        public VoliBot(string username, string password, BaseRegion region, string path, QueueTypes QueueType)
        {
            this.ipath = path;
            this.Accountname = username;
            this.Password = password;
            this.queueType = QueueType;
            this.baseRegion = region;
            this.connection.OnConnect += new LoLConnection.OnConnectHandler(this.connection_OnConnect);
            Config.connectedAccs++;
            Basic.ChangeTitel();
            this.connection.OnDisconnect += new LoLConnection.OnDisconnectHandler(this.connection_OnDisconnect);
            this.connection.OnError += new LoLConnection.OnErrorHandler(this.connection_OnError);
            this.connection.OnLogin += new LoLConnection.OnLoginHandler(this.connection_OnLogin);
            this.connection.OnLoginQueueUpdate += new LoLConnection.OnLoginQueueUpdateHandler(this.connection_OnLoginQueueUpdate);
            this.connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(this.connection_OnMessageReceived);
            string str = Regex.Replace(password, @"\s+", "");
            this.connection.Connect(username, str, this.baseRegion.PVPRegion, Config.clientVersion);
        }

        [AsyncStateMachine(typeof(<AttachToQueue>d__58))]
        private void AttachToQueue()
        {
            <AttachToQueue>d__58 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<AttachToQueue>d__58>(ref d__);
        }

        [AsyncStateMachine(typeof(<buyBoost>d__67))]
        private void buyBoost()
        {
            <buyBoost>d__67 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<buyBoost>d__67>(ref d__);
        }

        [AsyncStateMachine(typeof(<CallWithArgs>d__69))]
        public void CallWithArgs(string UUID, string GameMode, string ProcedureCall, string Parameters)
        {
            <CallWithArgs>d__69 d__;
            d__.<>4__this = this;
            d__.UUID = UUID;
            d__.GameMode = GameMode;
            d__.ProcedureCall = ProcedureCall;
            d__.Parameters = Parameters;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CallWithArgs>d__69>(ref d__);
        }

        private void connection_OnConnect(object sender, EventArgs e)
        {
        }

        private void connection_OnDisconnect(object sender, EventArgs e)
        {
            Config.connectedAccs--;
            Basic.ChangeTitel();
            this.updateStatus("Disconnected", this.Accountname);
        }

        private void connection_OnError(object sender, LoLLauncher.Error error)
        {
            if ((error.Type != ErrorType.AuthKey) && (error.Type != ErrorType.General))
            {
                if (!error.Message.Contains("is not owned by summoner"))
                {
                    if (error.Message.Contains("Your summoner level is too low to select the spell"))
                    {
                        Random random = new Random();
                        List<int> list = new List<int> { 13, 6, 7, 10, 1, 11, 0x15, 12, 3, 14, 2, 4 };
                        int num = random.Next(list.Count);
                        int num2 = random.Next(list.Count);
                        int num3 = list[num2];
                        int local1 = list[num];
                        if (local1 == num3)
                        {
                            int num4 = random.Next(list.Count);
                            num3 = list[num4];
                        }
                        Convert.ToInt32(local1);
                        Convert.ToInt32(num3);
                    }
                    else if (error.Message.Contains("Unable to get Auth Key"))
                    {
                        this.updateStatus("Login Failed", this.Accountname);
                    }
                    else
                    {
                        this.updateStatus(string.Concat(new object[] { "[", error.Type, "]error received:\n", error.Message }), this.Accountname);
                    }
                }
            }
            else if (!this.reAttempt)
            {
                this.updateStatus("Unable to connect. Try one reconnect.", this.Accountname);
                this.reAttempt = true;
                this.connection.Connect(this.Accountname, this.Password, this.baseRegion.PVPRegion, Config.clientVersion);
            }
        }

        private void connection_OnLogin(object sender, string username, string ipAddress)
        {
            new Thread(delegate {
                <>c__DisplayClass60_0.<<connection_OnLogin>b__0>d local;
                local.<>4__this = (<>c__DisplayClass60_0) this;
                local.<>t__builder = AsyncVoidMethodBuilder.Create();
                local.<>1__state = -1;
                local.<>t__builder.Start<<>c__DisplayClass60_0.<<connection_OnLogin>b__0>d>(ref local);
            }).Start();
        }

        private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
        {
            if (positionInLine > 0)
            {
                this.updateStatus("Position to login: " + positionInLine, this.Accountname);
            }
        }

        [AsyncStateMachine(typeof(<connection_OnMessageReceived>d__57))]
        public void connection_OnMessageReceived(object sender, object message)
        {
            <connection_OnMessageReceived>d__57 d__;
            d__.<>4__this = this;
            d__.message = message;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<connection_OnMessageReceived>d__57>(ref d__);
        }

        [AsyncStateMachine(typeof(<exeProcess_Exited>d__64))]
        private void exeProcess_Exited(object sender, EventArgs e)
        {
            <exeProcess_Exited>d__64 d__;
            d__.<>4__this = this;
            d__.sender = sender;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<exeProcess_Exited>d__64>(ref d__);
        }

        private string FindLoLExe()
        {
            string ipath = this.ipath;
            if (ipath.Contains("notfound"))
            {
                return ipath;
            }
            return (Directory.EnumerateDirectories(ipath + @"RADS\solutions\lol_game_client_sln\releases\").OrderBy<string, DateTime>((<>c.<>9__68_0 ?? (<>c.<>9__68_0 = new Func<string, DateTime>(<>c.<>9.<FindLoLExe>b__68_0)))).Last<string>() + @"\deploy\");
        }

        private void HandleProxyResponse(LcdsServiceProxyResponse Response)
        {
            this.updateStatus(Response.MethodName, this.Accountname);
        }

        private void levelUp()
        {
            this.updateStatus("Level Up: " + this.sumLevel, this.Accountname);
            this.rpBalance = this.loginPacket.RpBalance;
            if (this.sumLevel >= Config.maxLevel)
            {
                this.connection.Disconnect();
                if (!this.connection.IsConnected())
                {
                    Core.lognNewAccount();
                }
            }
            if (((this.rpBalance == 400.0) && Config.buyBoost) && (this.sumLevel < 5.0))
            {
                this.updateStatus("Buying XP Boost", this.Accountname);
                try
                {
                    new Task(new Action(this.buyBoost)).Start();
                }
                catch (Exception exception)
                {
                    this.updateStatus("Couldn't buy RP Boost.\n" + exception, this.Accountname);
                }
            }
        }

        [DllImport("user32.dll", SetLastError=true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError=true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        private void updateStatus(string status, string accname)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[" + DateTime.Now + "] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[" + accname + "] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(status + "\n");
        }

        public QueueTypes actualQueueType { get; set; }

        public double archiveSumLevel { get; set; }

        public string m_accessToken { get; set; }

        public int m_leaverBustedPenalty { get; set; }

        public QueueTypes queueType { get; set; }

        public double rpBalance { get; set; }

        public double sumId { get; set; }

        public double sumLevel { get; set; }

        public string sumName { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly VoliBot.<>c <>9 = new VoliBot.<>c();
            public static Func<ChampionDTO, bool> <>9__57_0;
            public static Func<ChampionDTO, bool> <>9__57_1;
            public static Func<string, DateTime> <>9__68_0;

            internal bool <connection_OnMessageReceived>b__57_0(ChampionDTO champ)
            {
                if (!champ.Owned)
                {
                    return champ.FreeToPlay;
                }
                return true;
            }

            internal bool <connection_OnMessageReceived>b__57_1(ChampionDTO champ)
            {
                if (!champ.Owned)
                {
                    return champ.FreeToPlay;
                }
                return true;
            }

            internal DateTime <FindLoLExe>b__68_0(string f)
            {
                return new DirectoryInfo(f).CreationTime;
            }
        }

        [CompilerGenerated]
        private struct <AttachToQueue>d__58 : IAsyncStateMachine
        {
            public int <>1__state;
            public VoliBot <>4__this;
            private List<QueueDodger>.Enumerator <>7__wrap1;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<SearchingForMatchNotification> <>u__1;
            private TaskAwaiter<object> <>u__2;
            private MatchMakerParams <matchParams>5__1;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<SearchingForMatchNotification> awaiter;
                    switch (num)
                    {
                        case 0:
                            break;

                        case 1:
                        case 2:
                            goto Label_0226;

                        case 3:
                            goto Label_04E9;

                        default:
                            this.<matchParams>5__1 = new MatchMakerParams();
                            if (this.<>4__this.queueType == QueueTypes.INTRO_BOT)
                            {
                                this.<matchParams>5__1.BotDifficulty = "INTRO";
                            }
                            else if (this.<>4__this.queueType == QueueTypes.BEGINNER_BOT)
                            {
                                this.<matchParams>5__1.BotDifficulty = "EASY";
                            }
                            else if (this.<>4__this.queueType == QueueTypes.MEDIUM_BOT)
                            {
                                this.<matchParams>5__1.BotDifficulty = "MEDIUM";
                            }
                            if ((this.<>4__this.sumLevel == 3.0) && (this.<>4__this.actualQueueType == QueueTypes.NORMAL_5x5))
                            {
                                this.<>4__this.queueType = this.<>4__this.actualQueueType;
                            }
                            else if ((this.<>4__this.sumLevel == 6.0) && (this.<>4__this.actualQueueType == QueueTypes.ARAM))
                            {
                                this.<>4__this.queueType = this.<>4__this.actualQueueType;
                            }
                            else if ((this.<>4__this.sumLevel == 7.0) && (this.<>4__this.actualQueueType == QueueTypes.NORMAL_3x3))
                            {
                                this.<>4__this.queueType = this.<>4__this.actualQueueType;
                            }
                            this.<matchParams>5__1.QueueIds = new int[] { this.<>4__this.queueType };
                            awaiter = this.<>4__this.connection.AttachToQueue(this.<matchParams>5__1).GetAwaiter();
                            if (awaiter.IsCompleted)
                            {
                                goto Label_01BF;
                            }
                            this.<>1__state = num = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<SearchingForMatchNotification>, VoliBot.<AttachToQueue>d__58>(ref awaiter, ref this);
                            return;
                    }
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<SearchingForMatchNotification>();
                    this.<>1__state = num = -1;
                Label_01BF:
                    SearchingForMatchNotification introduced8 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<SearchingForMatchNotification>();
                    SearchingForMatchNotification notification = introduced8;
                    if (notification.PlayerJoinFailures == null)
                    {
                        this.<>4__this.updateStatus("In Queue: " + this.<>4__this.queueType.ToString(), this.<>4__this.Accountname);
                        goto Label_0585;
                    }
                    this.<>7__wrap1 = notification.PlayerJoinFailures.GetEnumerator();
                Label_0226:;
                    try
                    {
                        TaskAwaiter<object> awaiter2;
                        if (num != 1)
                        {
                            if (num != 2)
                            {
                                goto Label_0305;
                            }
                            awaiter2 = this.<>u__2;
                            this.<>u__2 = new TaskAwaiter<object>();
                            this.<>1__state = num = -1;
                        }
                        else
                        {
                            awaiter2 = this.<>u__2;
                            this.<>u__2 = new TaskAwaiter<object>();
                            this.<>1__state = num = -1;
                            goto Label_033A;
                        }
                    Label_0275:
                        awaiter2.GetResult();
                        awaiter2 = new TaskAwaiter<object>();
                        this.<>4__this.connection_OnMessageReceived(null, new EndOfGameStats());
                    Label_0305:
                        while (this.<>7__wrap1.MoveNext())
                        {
                            QueueDodger current = this.<>7__wrap1.Current;
                            if (current.ReasonFailed == "LEAVER_BUSTED")
                            {
                                this.<>4__this.m_accessToken = current.AccessToken;
                                if (current.LeaverPenaltyMillisRemaining > this.<>4__this.m_leaverBustedPenalty)
                                {
                                    this.<>4__this.m_leaverBustedPenalty = current.LeaverPenaltyMillisRemaining;
                                }
                            }
                            else if (current.ReasonFailed == "LEAVER_BUSTER_TAINTED_WARNING")
                            {
                                goto Label_0317;
                            }
                        }
                        goto Label_041D;
                    Label_0317:
                        awaiter2 = this.<>4__this.connection.ackLeaverBusterWarning().GetAwaiter();
                        if (!awaiter2.IsCompleted)
                        {
                            goto Label_03E3;
                        }
                    Label_033A:
                        awaiter2.GetResult();
                        awaiter2 = new TaskAwaiter<object>();
                        SimpleDialogMessageResponse response = new SimpleDialogMessageResponse {
                            AccountID = this.<>4__this.loginPacket.AllSummonerData.Summoner.SumId,
                            MsgID = this.<>4__this.loginPacket.AllSummonerData.Summoner.SumId,
                            Command = "ack"
                        };
                        awaiter2 = this.<>4__this.connection.callPersistenceMessaging(response).GetAwaiter();
                        if (awaiter2.IsCompleted)
                        {
                            goto Label_0275;
                        }
                        this.<>1__state = num = 2;
                        this.<>u__2 = awaiter2;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<AttachToQueue>d__58>(ref awaiter2, ref this);
                        return;
                    Label_03E3:
                        this.<>1__state = num = 1;
                        this.<>u__2 = awaiter2;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<AttachToQueue>d__58>(ref awaiter2, ref this);
                        return;
                    }
                    finally
                    {
                        if (num < 0)
                        {
                            this.<>7__wrap1.Dispose();
                        }
                    }
                Label_041D:
                    this.<>7__wrap1 = new List<QueueDodger>.Enumerator();
                    if (string.IsNullOrEmpty(this.<>4__this.m_accessToken))
                    {
                        goto Label_0585;
                    }
                    this.<>4__this.updateStatus("Waiting out leaver buster: " + (((float) (this.<>4__this.m_leaverBustedPenalty / 0x3e8)) / 60f) + " minutes!", this.<>4__this.Accountname);
                    Thread.Sleep(TimeSpan.FromMilliseconds((double) this.<>4__this.m_leaverBustedPenalty));
                    awaiter = this.<>4__this.connection.AttachToLowPriorityQueue(this.<matchParams>5__1, this.<>4__this.m_accessToken).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0505;
                    }
                    this.<>1__state = num = 3;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<SearchingForMatchNotification>, VoliBot.<AttachToQueue>d__58>(ref awaiter, ref this);
                    return;
                Label_04E9:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<SearchingForMatchNotification>();
                    this.<>1__state = num = -1;
                Label_0505:
                    SearchingForMatchNotification introduced10 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<SearchingForMatchNotification>();
                    notification = introduced10;
                    if (notification.PlayerJoinFailures == null)
                    {
                        this.<>4__this.updateStatus("Succesfully joined lower priority queue!", this.<>4__this.Accountname);
                    }
                    else
                    {
                        this.<>4__this.updateStatus("There was an error in joining lower priority queue.\nDisconnecting.", this.<>4__this.Accountname);
                        this.<>4__this.connection.Disconnect();
                        Core.lognNewAccount();
                    }
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
            Label_0585:
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <buyBoost>d__67 : IAsyncStateMachine
        {
            public int <>1__state;
            public VoliBot <>4__this;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<string> <>u__1;
            private TaskAwaiter<HttpResponseMessage> <>u__2;
            private HttpClient <httpClient>5__1;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    try
                    {
                        TaskAwaiter<string> awaiter;
                        switch (num)
                        {
                            case 0:
                                break;

                            case 1:
                                goto Label_00F0;

                            case 2:
                                goto Label_017D;

                            case 3:
                                goto Label_02AB;

                            default:
                                awaiter = this.<>4__this.connection.GetStoreUrl().GetAwaiter();
                                if (awaiter.IsCompleted)
                                {
                                    goto Label_0095;
                                }
                                this.<>1__state = num = 0;
                                this.<>u__1 = awaiter;
                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VoliBot.<buyBoost>d__67>(ref awaiter, ref this);
                                return;
                        }
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter<string>();
                        this.<>1__state = num = -1;
                    Label_0095:
                        string introduced8 = awaiter.GetResult();
                        awaiter = new TaskAwaiter<string>();
                        string requestUri = introduced8;
                        this.<httpClient>5__1 = new HttpClient();
                        awaiter = this.<httpClient>5__1.GetStringAsync(requestUri).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_010D;
                        }
                        this.<>1__state = num = 1;
                        this.<>u__1 = awaiter;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VoliBot.<buyBoost>d__67>(ref awaiter, ref this);
                        return;
                    Label_00F0:
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter<string>();
                        this.<>1__state = num = -1;
                    Label_010D:
                        awaiter.GetResult();
                        awaiter = new TaskAwaiter<string>();
                        string str2 = "https://store." + this.<>4__this.baseRegion.ChatName + ".lol.riotgames.com/store/tabs/view/boosts/1";
                        awaiter = this.<httpClient>5__1.GetStringAsync(str2).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_019A;
                        }
                        this.<>1__state = num = 2;
                        this.<>u__1 = awaiter;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VoliBot.<buyBoost>d__67>(ref awaiter, ref this);
                        return;
                    Label_017D:
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter<string>();
                        this.<>1__state = num = -1;
                    Label_019A:
                        awaiter.GetResult();
                        awaiter = new TaskAwaiter<string>();
                        string str3 = "https://store." + this.<>4__this.baseRegion.ChatName + ".lol.riotgames.com/store/purchase/item";
                        List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
                            new KeyValuePair<string, string>("item_id", "boosts_2"),
                            new KeyValuePair<string, string>("currency_type", "rp"),
                            new KeyValuePair<string, string>("quantity", "1"),
                            new KeyValuePair<string, string>("rp", "260"),
                            new KeyValuePair<string, string>("ip", "null"),
                            new KeyValuePair<string, string>("duration_type", "PURCHASED"),
                            new KeyValuePair<string, string>("duration", "3")
                        };
                        HttpContent content = new FormUrlEncodedContent(nameValueCollection);
                        TaskAwaiter<HttpResponseMessage> awaiter2 = this.<httpClient>5__1.PostAsync(str3, content).GetAwaiter();
                        if (awaiter2.IsCompleted)
                        {
                            goto Label_02C8;
                        }
                        this.<>1__state = num = 3;
                        this.<>u__2 = awaiter2;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<HttpResponseMessage>, VoliBot.<buyBoost>d__67>(ref awaiter2, ref this);
                        return;
                    Label_02AB:
                        awaiter2 = this.<>u__2;
                        this.<>u__2 = new TaskAwaiter<HttpResponseMessage>();
                        this.<>1__state = num = -1;
                    Label_02C8:
                        awaiter2.GetResult();
                        awaiter2 = new TaskAwaiter<HttpResponseMessage>();
                        this.<>4__this.updateStatus("Bought 'XP Boost: 3 Days'!", this.<>4__this.Accountname);
                        this.<httpClient>5__1.Dispose();
                        this.<httpClient>5__1 = null;
                    }
                    catch (Exception exception1)
                    {
                        Console.WriteLine(exception1);
                    }
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CallWithArgs>d__69 : IAsyncStateMachine
        {
            public int <>1__state;
            public VoliBot <>4__this;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<object> <>u__1;
            public string GameMode;
            public string Parameters;
            public string ProcedureCall;
            public string UUID;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<object> awaiter;
                    if (num != 0)
                    {
                        awaiter = this.<>4__this.connection.Call(this.UUID, this.GameMode, this.ProcedureCall, this.Parameters).GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            this.<>1__state = num = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<CallWithArgs>d__69>(ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter<object>();
                        this.<>1__state = num = -1;
                    }
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <connection_OnMessageReceived>d__57 : IAsyncStateMachine
        {
            public int <>1__state;
            public VoliBot <>4__this;
            private VoliBot <>7__wrap1;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<object> <>u__1;
            private TaskAwaiter<LoginDataPacket> <>u__2;
            private GameDTO <game>5__1;
            public object message;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<object> awaiter;
                    TaskAwaiter<LoginDataPacket> awaiter2;
                    switch (num)
                    {
                        case 0:
                            goto Label_063E;

                        case 1:
                            goto Label_0A6A;

                        case 2:
                            goto Label_0AE5;

                        case 3:
                            goto Label_0B56;

                        case 4:
                            goto Label_0B88;

                        case 5:
                            goto Label_0C36;

                        case 6:
                            goto Label_0CA7;

                        case 7:
                            goto Label_0CD9;

                        case 8:
                            goto Label_0D7E;

                        case 9:
                            goto Label_0DF0;

                        case 10:
                            goto Label_0E1F;

                        case 11:
                            goto Label_0E58;

                        case 12:
                            goto Label_0E8A;

                        default:
                        {
                            if (this.message.GetType() == typeof(LcdsServiceProxyResponse))
                            {
                                LcdsServiceProxyResponse message = this.message as LcdsServiceProxyResponse;
                                this.<>4__this.HandleProxyResponse(message);
                            }
                            if (!(this.message is GameDTO))
                            {
                                goto Label_03A0;
                            }
                            this.<game>5__1 = this.message as GameDTO;
                            string gameState = this.<game>5__1.GameState;
                            uint num2 = <PrivateImplementationDetails><VoliBot.exe>.ComputeStringHash(gameState);
                            if (num2 <= 0x70adda98)
                            {
                                switch (num2)
                                {
                                    case 0x14bb05e0:
                                        if (!(gameState == "TERMINATED"))
                                        {
                                            break;
                                        }
                                        this.<>4__this.updateStatus("Re-entering queue", this.<>4__this.Accountname);
                                        this.<>4__this.firstTimeInPostChampSelect = true;
                                        this.<>4__this.firstTimeInQueuePop = true;
                                        goto Label_0E4C;

                                    case 0x6dcd8384:
                                        if (!(gameState == "POST_CHAMP_SELECT"))
                                        {
                                            break;
                                        }
                                        this.<>4__this.firstTimeInLobby = false;
                                        if (this.<>4__this.firstTimeInPostChampSelect)
                                        {
                                            this.<>4__this.firstTimeInPostChampSelect = false;
                                            this.<>4__this.updateStatus("(Post Champ Select)", this.<>4__this.Accountname);
                                        }
                                        goto Label_0E4C;
                                }
                                if ((num2 == 0x70adda98) && (gameState == "START_REQUESTED"))
                                {
                                    goto Label_0E4C;
                                }
                            }
                            else
                            {
                                if (num2 <= 0xe01dbad2)
                                {
                                    if (num2 == 0x8be25265)
                                    {
                                        if (!(gameState == "CHAMP_SELECT"))
                                        {
                                            break;
                                        }
                                        this.<>4__this.firstTimeInCustom = true;
                                        this.<>4__this.firstTimeInQueuePop = true;
                                        if (this.<>4__this.firstTimeInLobby)
                                        {
                                            this.<>4__this.firstTimeInLobby = false;
                                            this.<>4__this.updateStatus("In Champion Select", this.<>4__this.Accountname);
                                            awaiter = this.<>4__this.connection.SetClientReceivedGameMessage(this.<game>5__1.Id, "CHAMP_SELECT_CLIENT").GetAwaiter();
                                            if (!awaiter.IsCompleted)
                                            {
                                                this.<>1__state = num = 0;
                                                this.<>u__1 = awaiter;
                                                this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                                                return;
                                            }
                                            goto Label_065B;
                                        }
                                    }
                                    else
                                    {
                                        if ((num2 != 0xe01dbad2) || !(gameState == "IN_QUEUE"))
                                        {
                                            break;
                                        }
                                        this.<>4__this.updateStatus("In Queue", this.<>4__this.Accountname);
                                    }
                                    goto Label_0E4C;
                                }
                                if (num2 != 0xee365784)
                                {
                                    if ((num2 != 0xfcd22a67) || !(gameState == "FAILED_TO_START"))
                                    {
                                        break;
                                    }
                                    Console.WriteLine("Failed to Start!");
                                    goto Label_0E4C;
                                }
                                if (gameState == "JOINING_CHAMP_SELECT")
                                {
                                    goto Label_02FA;
                                }
                            }
                            break;
                        }
                    }
                    this.<>4__this.updateStatus("[DEFAULT]" + this.<game>5__1.GameStateString, this.<>4__this.Accountname);
                    goto Label_0E4C;
                Label_02FA:
                    if (!this.<>4__this.firstTimeInQueuePop || !this.<game>5__1.StatusOfParticipants.Contains("1"))
                    {
                        goto Label_0E4C;
                    }
                    this.<>4__this.updateStatus("Accepted Queue", this.<>4__this.Accountname);
                    this.<>4__this.firstTimeInQueuePop = false;
                    this.<>4__this.firstTimeInLobby = true;
                    awaiter = this.<>4__this.connection.AcceptPoppedGame(true).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0E3C;
                    }
                    this.<>1__state = num = 10;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_03A0:
                    if (this.message.GetType() == typeof(TradeContractDTO))
                    {
                        TradeContractDTO tdto = this.message as TradeContractDTO;
                        if ((tdto == null) || (!(tdto.State == "PENDING") || (tdto == null)))
                        {
                            goto Label_0F50;
                        }
                        awaiter = this.<>4__this.connection.AcceptTrade(tdto.RequesterInternalSummonerName, (int) tdto.RequesterChampionId).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_0E75;
                        }
                        this.<>1__state = num = 11;
                        this.<>u__1 = awaiter;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    }
                    else
                    {
                        if (this.message is PlayerCredentialsDto)
                        {
                            VoliBot.<>c__DisplayClass57_0 class_;
                            this.<>4__this.firstTimeInPostChampSelect = true;
                            PlayerCredentialsDto dto = this.message as PlayerCredentialsDto;
                            ProcessStartInfo startInfo = new ProcessStartInfo {
                                CreateNoWindow = false,
                                WorkingDirectory = this.<>4__this.FindLoLExe(),
                                FileName = "League of Legends.exe"
                            };
                            object[] objArray1 = new object[] { "\"8394\" \"LoLLauncher.exe\" \"\" \"", dto.ServerIp, " ", dto.ServerPort, " ", dto.EncryptionKey, " ", dto.SummonerId, "\"" };
                            startInfo.Arguments = string.Concat(objArray1);
                            this.<>4__this.updateStatus("Launching League of Legends\n", this.<>4__this.Accountname);
                            new Thread(new ThreadStart(class_.<connection_OnMessageReceived>b__2)).Start();
                            goto Label_0F50;
                        }
                        if (!(this.message is EndOfGameStats))
                        {
                            goto Label_0F50;
                        }
                        if (this.<>4__this.exeProcess == null)
                        {
                            goto Label_0F2A;
                        }
                        this.<>4__this.exeProcess.Exited -= new EventHandler(this.<>4__this.exeProcess_Exited);
                        this.<>4__this.exeProcess.Kill();
                        Thread.Sleep(500);
                        if (this.<>4__this.exeProcess.Responding)
                        {
                            Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                        }
                        this.<>7__wrap1 = this.<>4__this;
                        awaiter2 = this.<>4__this.connection.GetLoginDataPacketForUser().GetAwaiter();
                        if (awaiter2.IsCompleted)
                        {
                            goto Label_0EA7;
                        }
                        this.<>1__state = num = 12;
                        this.<>u__2 = awaiter2;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<LoginDataPacket>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter2, ref this);
                    }
                    return;
                Label_063E:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_065B:
                    object introduced37 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    if (this.<>4__this.queueType == QueueTypes.ARAM)
                    {
                        goto Label_0E4C;
                    }
                    if ((Config.championToPick != "") && (Config.championToPick != "RANDOM"))
                    {
                        int num3;
                        int num4;
                        if (!Config.rndSpell)
                        {
                            num3 = Enums.spellToId(Config.spell1);
                            num4 = Enums.spellToId(Config.spell2);
                        }
                        else
                        {
                            Random random = new Random();
                            List<int> list = new List<int> { 13, 6, 7, 10, 1, 11, 0x15, 12, 3, 14, 2, 4 };
                            int num5 = random.Next(list.Count);
                            int num6 = random.Next(list.Count);
                            int num7 = list[num6];
                            int local1 = list[num5];
                            if (local1 == num7)
                            {
                                int num8 = random.Next(list.Count);
                                num7 = list[num8];
                            }
                            num3 = Convert.ToInt32(local1);
                            num4 = Convert.ToInt32(num7);
                        }
                        awaiter = this.<>4__this.connection.SelectSpells(num3, num4).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_0A87;
                        }
                        this.<>1__state = num = 1;
                        this.<>u__1 = awaiter;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    }
                    else if (Config.championToPick == "RANDOM")
                    {
                        int num9;
                        int num10;
                        if (!Config.rndSpell)
                        {
                            num9 = Enums.spellToId(Config.spell1);
                            num10 = Enums.spellToId(Config.spell2);
                        }
                        else
                        {
                            Random random2 = new Random();
                            List<int> list2 = new List<int> { 13, 6, 7, 10, 1, 11, 0x15, 12, 3, 14, 2, 4 };
                            int num11 = random2.Next(list2.Count);
                            int num12 = random2.Next(list2.Count);
                            int num13 = list2[num12];
                            int local2 = list2[num11];
                            if (local2 == num13)
                            {
                                int num14 = random2.Next(list2.Count);
                                num13 = list2[num14];
                            }
                            num9 = Convert.ToInt32(local2);
                            num10 = Convert.ToInt32(num13);
                        }
                        awaiter = this.<>4__this.connection.SelectSpells(num9, num10).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_0BA5;
                        }
                        this.<>1__state = num = 4;
                        this.<>u__1 = awaiter;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    }
                    else
                    {
                        int num15;
                        int num16;
                        if (!Config.rndSpell)
                        {
                            num15 = Enums.spellToId(Config.spell1);
                            num16 = Enums.spellToId(Config.spell2);
                        }
                        else
                        {
                            Random random3 = new Random();
                            List<int> list3 = new List<int> { 13, 6, 7, 10, 1, 11, 0x15, 12, 3, 14, 2, 4 };
                            int num17 = random3.Next(list3.Count);
                            int num18 = random3.Next(list3.Count);
                            int num19 = list3[num18];
                            int local3 = list3[num17];
                            if (local3 == num19)
                            {
                                int num20 = random3.Next(list3.Count);
                                num19 = list3[num20];
                            }
                            num15 = Convert.ToInt32(local3);
                            num16 = Convert.ToInt32(num19);
                        }
                        awaiter = this.<>4__this.connection.SelectSpells(num15, num16).GetAwaiter();
                        if (awaiter.IsCompleted)
                        {
                            goto Label_0CF6;
                        }
                        this.<>1__state = num = 7;
                        this.<>u__1 = awaiter;
                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    }
                    return;
                Label_0A6A:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0A87:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    awaiter = this.<>4__this.connection.SelectChampion(Enums.championToId(Config.championToPick)).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0B02;
                    }
                    this.<>1__state = num = 2;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_0AE5:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0B02:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    awaiter = this.<>4__this.connection.ChampionSelectCompleted().GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0B73;
                    }
                    this.<>1__state = num = 3;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_0B56:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0B73:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    goto Label_0E4C;
                Label_0B88:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0BA5:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    IEnumerable<ChampionDTO> source = this.<>4__this.availableChampsArray.Shuffle<ChampionDTO>();
                    awaiter = this.<>4__this.connection.SelectChampion(source.First<ChampionDTO>((VoliBot.<>c.<>9__57_0 ?? (VoliBot.<>c.<>9__57_0 = new Func<ChampionDTO, bool>(VoliBot.<>c.<>9.<connection_OnMessageReceived>b__57_0)))).ChampionId).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0C53;
                    }
                    this.<>1__state = num = 5;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_0C36:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0C53:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    awaiter = this.<>4__this.connection.ChampionSelectCompleted().GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0CC4;
                    }
                    this.<>1__state = num = 6;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_0CA7:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0CC4:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    goto Label_0E4C;
                Label_0CD9:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0CF6:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    awaiter = this.<>4__this.connection.SelectChampion(this.<>4__this.availableChampsArray.First<ChampionDTO>((VoliBot.<>c.<>9__57_1 ?? (VoliBot.<>c.<>9__57_1 = new Func<ChampionDTO, bool>(VoliBot.<>c.<>9.<connection_OnMessageReceived>b__57_1)))).ChampionId).GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0D9B;
                    }
                    this.<>1__state = num = 8;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_0D7E:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0D9B:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    awaiter = this.<>4__this.connection.ChampionSelectCompleted().GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        goto Label_0E0D;
                    }
                    this.<>1__state = num = 9;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<object>, VoliBot.<connection_OnMessageReceived>d__57>(ref awaiter, ref this);
                    return;
                Label_0DF0:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0E0D:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    goto Label_0E4C;
                Label_0E1F:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0E3C:
                    object introduced44 = awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                Label_0E4C:
                    this.<game>5__1 = null;
                    goto Label_0F50;
                Label_0E58:
                    awaiter = this.<>u__1;
                    this.<>u__1 = new TaskAwaiter<object>();
                    this.<>1__state = num = -1;
                Label_0E75:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter<object>();
                    goto Label_0F50;
                Label_0E8A:
                    awaiter2 = this.<>u__2;
                    this.<>u__2 = new TaskAwaiter<LoginDataPacket>();
                    this.<>1__state = num = -1;
                Label_0EA7:
                    LoginDataPacket introduced45 = awaiter2.GetResult();
                    awaiter2 = new TaskAwaiter<LoginDataPacket>();
                    LoginDataPacket packet = introduced45;
                    this.<>7__wrap1.loginPacket = packet;
                    this.<>7__wrap1 = null;
                    this.<>4__this.archiveSumLevel = this.<>4__this.sumLevel;
                    this.<>4__this.sumLevel = this.<>4__this.loginPacket.AllSummonerData.SummonerLevel.Level;
                    if (this.<>4__this.sumLevel != this.<>4__this.archiveSumLevel)
                    {
                        this.<>4__this.levelUp();
                    }
                Label_0F2A:
                    this.<>4__this.AttachToQueue();
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
            Label_0F50:
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <exeProcess_Exited>d__64 : IAsyncStateMachine
        {
            public int <>1__state;
            public VoliBot <>4__this;
            private VoliBot <>7__wrap1;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<LoginDataPacket> <>u__1;
            public object sender;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter<LoginDataPacket> awaiter;
                    if (num != 0)
                    {
                        this.<>4__this.updateStatus("Restart League of Legends.", this.<>4__this.Accountname);
                        this.<>7__wrap1 = this.<>4__this;
                        awaiter = this.<>4__this.connection.GetLoginDataPacketForUser().GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            this.<>1__state = num = 0;
                            this.<>u__1 = awaiter;
                            this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<LoginDataPacket>, VoliBot.<exeProcess_Exited>d__64>(ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter<LoginDataPacket>();
                        this.<>1__state = num = -1;
                    }
                    LoginDataPacket result = awaiter.GetResult();
                    awaiter = new TaskAwaiter<LoginDataPacket>();
                    LoginDataPacket packet = result;
                    this.<>7__wrap1.loginPacket = packet;
                    this.<>7__wrap1 = null;
                    if ((this.<>4__this.loginPacket.ReconnectInfo != null) && (this.<>4__this.loginPacket.ReconnectInfo.Game != null))
                    {
                        this.<>4__this.connection_OnMessageReceived(this.sender, this.<>4__this.loginPacket.ReconnectInfo.PlayerCredentials);
                    }
                    else
                    {
                        this.<>4__this.connection_OnMessageReceived(this.sender, new EndOfGameStats());
                    }
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult();
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }
    }
}

