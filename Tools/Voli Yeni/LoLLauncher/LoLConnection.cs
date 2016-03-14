namespace LoLLauncher
{
    using LoLLauncher.RiotObjects;
    using LoLLauncher.RiotObjects.Leagues.Pojo;
    using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
    using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
    using LoLLauncher.RiotObjects.Platform.Game;
    using LoLLauncher.RiotObjects.Platform.Game.Message;
    using LoLLauncher.RiotObjects.Platform.Game.Practice;
    using LoLLauncher.RiotObjects.Platform.Harassment;
    using LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto;
    using LoLLauncher.RiotObjects.Platform.Login;
    using LoLLauncher.RiotObjects.Platform.Matchmaking;
    using LoLLauncher.RiotObjects.Platform.Messaging;
    using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
    using LoLLauncher.RiotObjects.Platform.Statistics;
    using LoLLauncher.RiotObjects.Platform.Statistics.Team;
    using LoLLauncher.RiotObjects.Platform.Summoner;
    using LoLLauncher.RiotObjects.Platform.Summoner.Boost;
    using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
    using LoLLauncher.RiotObjects.Platform.Summoner.Runes;
    using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
    using LoLLauncher.RiotObjects.Team;
    using LoLLauncher.RiotObjects.Team.Dto;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using VoliBot.LoLLauncher.RiotObjects.Platform.Messaging;

    public class LoLConnection
    {
        private int accountID;
        private string authToken;
        private Dictionary<int, RiotGamesObject> callbacks = new Dictionary<int, RiotGamesObject>();
        private TcpClient client;
        private string clientVersion;
        public Thread decodeThread;
        private string DSId;
        private string garenaToken;
        private int heartbeatCount = 1;
        public Thread heartbeatThread;
        private int invokeID = 2;
        private string ipAddress;
        private bool isConnected;
        private object isInvokingLock = new object();
        private bool isLoggedIn;
        private string locale;
        private string loginQueue;
        private string password;
        private List<int> pendingInvokes = new List<int>();
        private Random rand = new Random();
        private Dictionary<int, TypedObject> results = new Dictionary<int, TypedObject>();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private string server;
        private string sessionToken;
        private SslStream sslStream;
        private bool useGarena;
        private string user;
        private string userID;

        [field: CompilerGenerated]
        public event OnConnectHandler OnConnect;

        [field: CompilerGenerated]
        public event OnDisconnectHandler OnDisconnect;

        [field: CompilerGenerated]
        public event OnErrorHandler OnError;

        [field: CompilerGenerated]
        public event OnLoginHandler OnLogin;

        [field: CompilerGenerated]
        public event OnLoginQueueUpdateHandler OnLoginQueueUpdate;

        [field: CompilerGenerated]
        public event OnMessageReceivedHandler OnMessageReceived;

        private bool AcceptAllCertificates(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        [AsyncStateMachine(typeof(<AcceptInviteForMatchmakingGame>d__184))]
        public Task<object> AcceptInviteForMatchmakingGame(double gameId)
        {
            <AcceptInviteForMatchmakingGame>d__184 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<AcceptInviteForMatchmakingGame>d__184>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<AcceptPoppedGame>d__185))]
        public Task<object> AcceptPoppedGame(bool accept)
        {
            <AcceptPoppedGame>d__185 d__;
            d__.<>4__this = this;
            d__.accept = accept;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<AcceptPoppedGame>d__185>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<AcceptTrade>d__186))]
        public Task<object> AcceptTrade(string SummonerInternalName, int ChampionId)
        {
            <AcceptTrade>d__186 d__;
            d__.<>4__this = this;
            d__.SummonerInternalName = SummonerInternalName;
            d__.ChampionId = ChampionId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<AcceptTrade>d__186>(ref d__);
            return d__.<>t__builder.Task;
        }

        public double AccountID()
        {
            return (double) this.accountID;
        }

        [AsyncStateMachine(typeof(<ackLeaverBusterWarning>d__85))]
        public Task<object> ackLeaverBusterWarning()
        {
            <ackLeaverBusterWarning>d__85 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<ackLeaverBusterWarning>d__85>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<AttachToLowPriorityQueue>d__70))]
        public Task<SearchingForMatchNotification> AttachToLowPriorityQueue(MatchMakerParams matchMakerParams, string accessToken)
        {
            <AttachToLowPriorityQueue>d__70 d__;
            d__.<>4__this = this;
            d__.matchMakerParams = matchMakerParams;
            d__.accessToken = accessToken;
            d__.<>t__builder = AsyncTaskMethodBuilder<SearchingForMatchNotification>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<AttachToLowPriorityQueue>d__70>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<AttachToQueue>d__151))]
        public Task<SearchingForMatchNotification> AttachToQueue(MatchMakerParams matchMakerParams)
        {
            <AttachToQueue>d__151 d__;
            d__.<>4__this = this;
            d__.matchMakerParams = matchMakerParams;
            d__.<>t__builder = AsyncTaskMethodBuilder<SearchingForMatchNotification>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<AttachToQueue>d__151>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void AttachToQueue(MatchMakerParams matchMakerParams, SearchingForMatchNotification.Callback callback)
        {
            SearchingForMatchNotification cb = new SearchingForMatchNotification(callback);
            object[] body = new object[] { matchMakerParams.GetBaseTypedObject() };
            this.InvokeWithCallback("matchmakerService", "attachToQueue", body, cb);
        }

        [AsyncStateMachine(typeof(<BanChampion>d__190))]
        public Task<object> BanChampion(int championId)
        {
            <BanChampion>d__190 d__;
            d__.<>4__this = this;
            d__.championId = championId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<BanChampion>d__190>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<BanObserverFromGame>d__189))]
        public Task<object> BanObserverFromGame(double gameId, double accountId)
        {
            <BanObserverFromGame>d__189 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.accountId = accountId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<BanObserverFromGame>d__189>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<BanUserFromGame>d__188))]
        public Task<object> BanUserFromGame(double gameId, double accountId)
        {
            <BanUserFromGame>d__188 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.accountId = accountId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<BanUserFromGame>d__188>(ref d__);
            return d__.<>t__builder.Task;
        }

        private void BeginReceive()
        {
            this.decodeThread = new Thread(delegate {
                try
                {
                    byte num;
                    List<byte> list;
                    int num2;
                    Dictionary<int, Packet> dictionary = new Dictionary<int, Packet>();
                    Dictionary<int, Packet> dictionary2 = new Dictionary<int, Packet>();
                    goto Label_06EE;
                Label_0011:
                    num2 = 0;
                    if ((num & 3) != 0)
                    {
                        num2 = num & 0x3f;
                        list.Add(num);
                    }
                    else if ((num & 1) != 0)
                    {
                        byte item = (byte) this.sslStream.ReadByte();
                        num2 = 0x40 + item;
                        list.Add(num);
                        list.Add(item);
                    }
                    else if ((num & 2) != 0)
                    {
                        byte num4 = (byte) this.sslStream.ReadByte();
                        byte num5 = (byte) this.sslStream.ReadByte();
                        list.Add(num);
                        list.Add(num4);
                        list.Add(num5);
                        num2 = (0x40 + num4) + (0x100 * num5);
                    }
                    int num6 = num & 0xc0;
                    int num7 = 0;
                    switch (num6)
                    {
                        case 0:
                            num7 = 12;
                            break;

                        case 0x40:
                            num7 = 8;
                            break;

                        case 0x80:
                            num7 = 4;
                            break;

                        case 0xc0:
                            num7 = 0;
                            break;
                    }
                    if (!dictionary2.ContainsKey(num2))
                    {
                        dictionary2.Add(num2, new Packet());
                    }
                    Packet packet = dictionary2[num2];
                    packet.AddToRaw(list.ToArray());
                    switch (num7)
                    {
                        case 12:
                        {
                            byte[] buffer = new byte[3];
                            for (int j = 0; j < 3; j++)
                            {
                                buffer[j] = (byte) this.sslStream.ReadByte();
                                packet.AddToRaw(buffer[j]);
                            }
                            byte[] buffer2 = new byte[3];
                            for (int k = 0; k < 3; k++)
                            {
                                buffer2[k] = (byte) this.sslStream.ReadByte();
                                packet.AddToRaw(buffer2[k]);
                            }
                            int size = 0;
                            for (int m = 0; m < 3; m++)
                            {
                                size = (size * 0x100) + (buffer2[m] & 0xff);
                            }
                            packet.SetSize(size);
                            int type = this.sslStream.ReadByte();
                            packet.AddToRaw((byte) type);
                            packet.SetType(type);
                            byte[] buffer3 = new byte[4];
                            for (int n = 0; n < 4; n++)
                            {
                                buffer3[n] = (byte) this.sslStream.ReadByte();
                                packet.AddToRaw(buffer3[n]);
                            }
                            break;
                        }
                        case 8:
                        {
                            byte[] buffer4 = new byte[3];
                            for (int num14 = 0; num14 < 3; num14++)
                            {
                                buffer4[num14] = (byte) this.sslStream.ReadByte();
                                packet.AddToRaw(buffer4[num14]);
                            }
                            byte[] buffer5 = new byte[3];
                            for (int num15 = 0; num15 < 3; num15++)
                            {
                                buffer5[num15] = (byte) this.sslStream.ReadByte();
                                packet.AddToRaw(buffer5[num15]);
                            }
                            int num16 = 0;
                            for (int num17 = 0; num17 < 3; num17++)
                            {
                                num16 = (num16 * 0x100) + (buffer5[num17] & 0xff);
                            }
                            packet.SetSize(num16);
                            int num18 = this.sslStream.ReadByte();
                            packet.AddToRaw((byte) num18);
                            packet.SetType(num18);
                            break;
                        }
                        case 4:
                        {
                            byte[] buffer6 = new byte[3];
                            for (int num19 = 0; num19 < 3; num19++)
                            {
                                buffer6[num19] = (byte) this.sslStream.ReadByte();
                                packet.AddToRaw(buffer6[num19]);
                            }
                            if (((packet.GetSize() == 0) && (packet.GetPacketType() == 0)) && dictionary.ContainsKey(num2))
                            {
                                packet.SetSize(dictionary[num2].GetSize());
                                packet.SetType(dictionary[num2].GetPacketType());
                            }
                            break;
                        }
                        default:
                            if (((num7 == 0) && (packet.GetSize() == 0)) && ((packet.GetPacketType() == 0) && dictionary.ContainsKey(num2)))
                            {
                                packet.SetSize(dictionary[num2].GetSize());
                                packet.SetType(dictionary[num2].GetPacketType());
                            }
                            break;
                    }
                    for (int i = 0; i < 0x80; i++)
                    {
                        byte b = (byte) this.sslStream.ReadByte();
                        packet.Add(b);
                        packet.AddToRaw(b);
                        if (packet.IsComplete())
                        {
                            break;
                        }
                    }
                    if (packet.IsComplete())
                    {
                        TypedObject obj2;
                        if (dictionary.ContainsKey(num2))
                        {
                            dictionary.Remove(num2);
                        }
                        dictionary.Add(num2, packet);
                        if (dictionary2.ContainsKey(num2))
                        {
                            dictionary2.Remove(num2);
                        }
                        RTMPSDecoder decoder = new RTMPSDecoder();
                        if (packet.GetPacketType() == 20)
                        {
                            obj2 = decoder.DecodeConnect(packet.GetData());
                        }
                        else
                        {
                            if (packet.GetPacketType() != 0x11)
                            {
                                goto Label_05FE;
                            }
                            obj2 = decoder.DecodeInvoke(packet.GetData());
                        }
                        int? nullable = obj2.GetInt("invokeId");
                        if (obj2["result"].Equals("_error"))
                        {
                            this.Error(this.GetErrorMessage(obj2), this.GetErrorCode(obj2), ErrorType.Receive);
                        }
                        if (obj2["result"].Equals("receive") && (obj2.GetTO("data") != null))
                        {
                            TypedObject to = obj2.GetTO("data");
                            if (to.ContainsKey("body") && (to["body"] is TypedObject))
                            {
                                new Thread(delegate {
                                    TypedObject result = (TypedObject) to["body"];
                                    if (result.type.Equals("com.riotgames.platform.game.GameDTO"))
                                    {
                                        this.MessageReceived(new GameDTO(result));
                                    }
                                    else if (result.type.Equals("com.riotgames.platform.game.PlayerCredentialsDto"))
                                    {
                                        this.MessageReceived(new PlayerCredentialsDto(result));
                                    }
                                    else if (result.type.Equals("com.riotgames.platform.game.message.GameNotification"))
                                    {
                                        this.MessageReceived(new GameNotification(result));
                                    }
                                    else if (result.type.Equals("com.riotgames.platform.matchmaking.SearchingForMatchNotification"))
                                    {
                                        this.MessageReceived(new SearchingForMatchNotification(result));
                                    }
                                    else if (result.type.Equals("com.riotgames.platform.messaging.StoreFulfillmentNotification"))
                                    {
                                        this.MessageReceived(new StoreFulfillmentNotification(result));
                                    }
                                    else if (result.type.Equals("com.riotgames.platform.messaging.StoreFulfillmentNotification"))
                                    {
                                        this.MessageReceived(new StoreAccountBalanceNotification(result));
                                    }
                                    else if (result.type.Equals("com.riotgames.platform.statistics.EndOfGameStats"))
                                    {
                                        this.MessageReceived(new EndOfGameStats(result));
                                    }
                                    else
                                    {
                                        this.MessageReceived(result);
                                    }
                                }).Start();
                            }
                        }
                        if (nullable.HasValue)
                        {
                            int? nullable2 = nullable;
                            if (!((nullable2.GetValueOrDefault() == 0) ? nullable2.HasValue : false))
                            {
                                if (this.callbacks.ContainsKey(nullable.Value))
                                {
                                    RiotGamesObject cb = this.callbacks[nullable.Value];
                                    this.callbacks.Remove(nullable.Value);
                                    if (cb != null)
                                    {
                                        TypedObject messageBody = obj2.GetTO("data").GetTO("body");
                                        new Thread(() => cb.DoCallback(messageBody)).Start();
                                    }
                                }
                                else
                                {
                                    this.results.Add(nullable.Value, obj2);
                                }
                            }
                            this.pendingInvokes.Remove(nullable.Value);
                        }
                    }
                    goto Label_06EE;
                Label_05FE:
                    if (packet.GetPacketType() == 6)
                    {
                        byte[] data = packet.GetData();
                        int num22 = 0;
                        for (int num23 = 0; num23 < 4; num23++)
                        {
                            num22 = (num22 * 0x100) + (data[num23] & 0xff);
                        }
                    }
                    else if (packet.GetPacketType() == 5)
                    {
                        byte[] buffer8 = packet.GetData();
                        int num24 = 0;
                        for (int num25 = 0; num25 < 4; num25++)
                        {
                            num24 = (num24 * 0x100) + (buffer8[num25] & 0xff);
                        }
                    }
                    else if (packet.GetPacketType() == 3)
                    {
                        byte[] buffer9 = packet.GetData();
                        int num26 = 0;
                        for (int num27 = 0; num27 < 4; num27++)
                        {
                            num26 = (num26 * 0x100) + (buffer9[num27] & 0xff);
                        }
                    }
                    else if (packet.GetPacketType() == 2)
                    {
                        packet.GetData();
                    }
                    else if (packet.GetPacketType() == 1)
                    {
                        packet.GetData();
                    }
                    goto Label_06EE;
                Label_06E3:
                    this.Disconnect();
                    goto Label_0011;
                Label_06EE:
                    num = (byte) this.sslStream.ReadByte();
                    list = new List<byte>();
                    if (num != 0xff)
                    {
                        goto Label_0011;
                    }
                    goto Label_06E3;
                }
                catch (Exception exception)
                {
                    if (this.IsConnected())
                    {
                        this.Error(exception.Message, ErrorType.Receive);
                    }
                }
            });
            this.decodeThread.Start();
        }

        [AsyncStateMachine(typeof(<Call>d__180))]
        public Task<object> Call(string GroupFinderUUID, string GameMode, string ProcedureCall, string Parameters)
        {
            <Call>d__180 d__;
            d__.<>4__this = this;
            d__.GroupFinderUUID = GroupFinderUUID;
            d__.GameMode = GameMode;
            d__.ProcedureCall = ProcedureCall;
            d__.Parameters = Parameters;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<Call>d__180>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<CallKudos>d__116))]
        public Task<LcdsResponseString> CallKudos(string arg0)
        {
            <CallKudos>d__116 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.<>t__builder = AsyncTaskMethodBuilder<LcdsResponseString>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CallKudos>d__116>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void CallKudos(string arg0, LcdsResponseString.Callback callback)
        {
            LcdsResponseString cb = new LcdsResponseString(callback);
            object[] body = new object[] { arg0 };
            this.InvokeWithCallback("clientFacadeService", "callKudos", body, cb);
        }

        [AsyncStateMachine(typeof(<callPersistenceMessaging>d__86))]
        public Task<object> callPersistenceMessaging(SimpleDialogMessageResponse response)
        {
            <callPersistenceMessaging>d__86 d__;
            d__.<>4__this = this;
            d__.response = response;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<callPersistenceMessaging>d__86>(ref d__);
            return d__.<>t__builder.Task;
        }

        private void Cancel(int id)
        {
            this.pendingInvokes.Remove(id);
            if (this.PeekResult(id) == null)
            {
                this.callbacks.Add(id, null);
                if (this.PeekResult(id) != null)
                {
                    this.callbacks.Remove(id);
                }
            }
        }

        [AsyncStateMachine(typeof(<CancelFromQueueIfPossible>d__152))]
        public Task<bool> CancelFromQueueIfPossible(int queueId)
        {
            <CancelFromQueueIfPossible>d__152 d__;
            d__.<>4__this = this;
            d__.queueId = queueId;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CancelFromQueueIfPossible>d__152>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<ChampionSelectCompleted>d__178))]
        public Task<object> ChampionSelectCompleted()
        {
            <ChampionSelectCompleted>d__178 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<ChampionSelectCompleted>d__178>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void Connect(string user, string password, Region region, string clientVersion)
        {
            if (!this.isConnected)
            {
                new Thread(delegate {
                    this.user = user;
                    this.password = password;
                    this.clientVersion = clientVersion;
                    this.server = RegionInfo.GetServerValue(region);
                    this.loginQueue = RegionInfo.GetLoginQueueValue(region);
                    this.locale = RegionInfo.GetLocaleValue(region);
                    this.useGarena = RegionInfo.GetUseGarenaValue(region);
                    try
                    {
                        this.client = new TcpClient(this.server, 0x833);
                        if ((!this.useGarena || this.GetGarenaToken()) && (this.GetAuthKey() && this.GetIpAddress()))
                        {
                            this.sslStream = new SslStream(this.client.GetStream(), false, new RemoteCertificateValidationCallback(this.AcceptAllCertificates));
                            IAsyncResult asyncResult = this.sslStream.BeginAuthenticateAsClient(this.server, null, null);
                            using (asyncResult.AsyncWaitHandle)
                            {
                                if (asyncResult.AsyncWaitHandle.WaitOne(-1))
                                {
                                    this.sslStream.EndAuthenticateAsClient(asyncResult);
                                }
                            }
                            if (this.Handshake())
                            {
                                this.BeginReceive();
                                if (this.SendConnect() && this.Login())
                                {
                                    this.StartHeartbeat();
                                }
                            }
                        }
                    }
                    catch
                    {
                        this.Error("Riots servers are currently unavailable.", ErrorType.AuthKey);
                        this.Disconnect();
                    }
                }).Start();
            }
        }

        [AsyncStateMachine(typeof(<CreateDefaultSummoner>d__104))]
        public Task<AllSummonerData> CreateDefaultSummoner(string summonerName)
        {
            <CreateDefaultSummoner>d__104 d__;
            d__.<>4__this = this;
            d__.summonerName = summonerName;
            d__.<>t__builder = AsyncTaskMethodBuilder<AllSummonerData>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CreateDefaultSummoner>d__104>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<CreatePlayer>d__103))]
        public Task<PlayerDTO> CreatePlayer()
        {
            <CreatePlayer>d__103 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<PlayerDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CreatePlayer>d__103>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void CreatePlayer(PlayerDTO.Callback callback)
        {
            PlayerDTO cb = new PlayerDTO(callback);
            this.InvokeWithCallback("summonerTeamService", "createPlayer", new object[0], cb);
        }

        [AsyncStateMachine(typeof(<CreatePracticeGame>d__165))]
        public Task<GameDTO> CreatePracticeGame(PracticeGameConfig practiceGameConfig)
        {
            <CreatePracticeGame>d__165 d__;
            d__.<>4__this = this;
            d__.practiceGameConfig = practiceGameConfig;
            d__.<>t__builder = AsyncTaskMethodBuilder<GameDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CreatePracticeGame>d__165>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void CreatePracticeGame(PracticeGameConfig practiceGameConfig, GameDTO.Callback callback)
        {
            GameDTO cb = new GameDTO(callback);
            object[] body = new object[] { practiceGameConfig.GetBaseTypedObject() };
            this.InvokeWithCallback("gameService", "createPracticeGame", body, cb);
        }

        [AsyncStateMachine(typeof(<CreateTeam>d__137))]
        public Task<TeamDTO> CreateTeam(string teamName, string tagName)
        {
            <CreateTeam>d__137 d__;
            d__.<>4__this = this;
            d__.teamName = teamName;
            d__.tagName = tagName;
            d__.<>t__builder = AsyncTaskMethodBuilder<TeamDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<CreateTeam>d__137>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void CreateTeam(string teamName, string tagName, TeamDTO.Callback callback)
        {
            TeamDTO cb = new TeamDTO(callback);
            object[] body = new object[] { teamName, tagName };
            this.InvokeWithCallback("summonerTeamService", "createTeam", body, cb);
        }

        [AsyncStateMachine(typeof(<DeclineObserverReconnect>d__183))]
        public Task<bool> DeclineObserverReconnect()
        {
            <DeclineObserverReconnect>d__183 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<DeclineObserverReconnect>d__183>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<DisbandTeam>d__133))]
        public Task<object> DisbandTeam(TeamId teamId)
        {
            <DisbandTeam>d__133 d__;
            d__.<>4__this = this;
            d__.teamId = teamId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<DisbandTeam>d__133>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void Disconnect()
        {
            new Thread(delegate {
                if (this.isConnected)
                {
                    object[] body = new object[] { this.authToken };
                    int id = this.Invoke("loginService", "logout", body);
                    this.Join(id);
                }
                this.isConnected = false;
                if (this.heartbeatThread != null)
                {
                    this.heartbeatThread.Abort();
                }
                if (this.decodeThread != null)
                {
                    this.decodeThread.Abort();
                }
                this.invokeID = 2;
                this.heartbeatCount = 1;
                this.pendingInvokes.Clear();
                this.callbacks.Clear();
                this.results.Clear();
                this.client = null;
                this.sslStream = null;
                if (this.OnDisconnect != null)
                {
                    this.OnDisconnect(this, EventArgs.Empty);
                }
            }).Start();
        }

        private void Error(string message, ErrorType type)
        {
            this.Error(message, "", type);
        }

        private void Error(string message, string errorCode, ErrorType type)
        {
            LoLLauncher.Error error = new LoLLauncher.Error {
                Type = type,
                Message = message,
                ErrorCode = errorCode
            };
            if (this.OnError != null)
            {
                this.OnError(this, error);
            }
        }

        [AsyncStateMachine(typeof(<FindPlayer>d__147))]
        public Task<PlayerDTO> FindPlayer(double summonerId)
        {
            <FindPlayer>d__147 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.<>t__builder = AsyncTaskMethodBuilder<PlayerDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<FindPlayer>d__147>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void FindPlayer(double summonerId, PlayerDTO.Callback callback)
        {
            PlayerDTO cb = new PlayerDTO(callback);
            object[] body = new object[] { summonerId };
            this.InvokeWithCallback("summonerTeamService", "findPlayer", body, cb);
        }

        [AsyncStateMachine(typeof(<FindTeamById>d__127))]
        public Task<TeamDTO> FindTeamById(TeamId teamId)
        {
            <FindTeamById>d__127 d__;
            d__.<>4__this = this;
            d__.teamId = teamId;
            d__.<>t__builder = AsyncTaskMethodBuilder<TeamDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<FindTeamById>d__127>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void FindTeamById(TeamId teamId, TeamDTO.Callback callback)
        {
            TeamDTO cb = new TeamDTO(callback);
            object[] body = new object[] { teamId.GetBaseTypedObject() };
            this.InvokeWithCallback("summonerTeamService", "findTeamById", body, cb);
        }

        [AsyncStateMachine(typeof(<GetAggregatedStats>d__123))]
        public Task<AggregatedStats> GetAggregatedStats(double summonerId, string gameMode, string season)
        {
            <GetAggregatedStats>d__123 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.gameMode = gameMode;
            d__.season = season;
            d__.<>t__builder = AsyncTaskMethodBuilder<AggregatedStats>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAggregatedStats>d__123>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetAggregatedStats(double summonerId, string gameMode, string season, AggregatedStats.Callback callback)
        {
            AggregatedStats cb = new AggregatedStats(callback);
            object[] body = new object[] { summonerId, gameMode, season };
            this.InvokeWithCallback("playerStatsService", "getAggregatedStats", body, cb);
        }

        [AsyncStateMachine(typeof(<GetAllLeaguesForPlayer>d__143))]
        public Task<SummonerLeaguesDTO> GetAllLeaguesForPlayer(double summonerId)
        {
            <GetAllLeaguesForPlayer>d__143 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.<>t__builder = AsyncTaskMethodBuilder<SummonerLeaguesDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAllLeaguesForPlayer>d__143>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetAllLeaguesForPlayer(double summonerId, SummonerLeaguesDTO.Callback callback)
        {
            SummonerLeaguesDTO cb = new SummonerLeaguesDTO(callback);
            object[] body = new object[] { summonerId };
            this.InvokeWithCallback("leaguesServiceProxy", "getAllLeaguesForPlayer", body, cb);
        }

        [AsyncStateMachine(typeof(<GetAllMyLeagues>d__109))]
        public Task<SummonerLeaguesDTO> GetAllMyLeagues()
        {
            <GetAllMyLeagues>d__109 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<SummonerLeaguesDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAllMyLeagues>d__109>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetAllMyLeagues(SummonerLeaguesDTO.Callback callback)
        {
            SummonerLeaguesDTO cb = new SummonerLeaguesDTO(callback);
            this.InvokeWithCallback("leaguesServiceProxy", "getAllMyLeagues", new object[0], cb);
        }

        [AsyncStateMachine(typeof(<GetAllPublicSummonerDataByAccount>d__145))]
        public Task<AllPublicSummonerDataDTO> GetAllPublicSummonerDataByAccount(double accountId)
        {
            <GetAllPublicSummonerDataByAccount>d__145 d__;
            d__.<>4__this = this;
            d__.accountId = accountId;
            d__.<>t__builder = AsyncTaskMethodBuilder<AllPublicSummonerDataDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAllPublicSummonerDataByAccount>d__145>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetAllPublicSummonerDataByAccount(double accountId, AllPublicSummonerDataDTO.Callback callback)
        {
            AllPublicSummonerDataDTO cb = new AllPublicSummonerDataDTO(callback);
            object[] body = new object[] { accountId };
            this.InvokeWithCallback("summonerService", "getAllPublicSummonerDataByAccount", body, cb);
        }

        [AsyncStateMachine(typeof(<GetAllSummonerDataByAccount>d__111))]
        public Task<AllSummonerData> GetAllSummonerDataByAccount(double accountId)
        {
            <GetAllSummonerDataByAccount>d__111 d__;
            d__.<>4__this = this;
            d__.accountId = accountId;
            d__.<>t__builder = AsyncTaskMethodBuilder<AllSummonerData>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAllSummonerDataByAccount>d__111>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetAllSummonerDataByAccount(double accountId, AllSummonerData.Callback callback)
        {
            AllSummonerData cb = new AllSummonerData(callback);
            object[] body = new object[] { accountId };
            this.InvokeWithCallback("summonerService", "getAllSummonerDataByAccount", body, cb);
        }

        private bool GetAuthKey()
        {
            try
            {
                int num;
                StringBuilder builder = new StringBuilder();
                string garenaToken = "user=" + this.user + ",password=" + this.password;
                string s = "payload=" + garenaToken;
                if (this.useGarena)
                {
                    garenaToken = this.garenaToken;
                }
                WebRequest request = WebRequest.Create(this.loginQueue + "login-queue/rest/queue/authenticate");
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(Encoding.ASCII.GetBytes(s), 0, Encoding.ASCII.GetByteCount(s));
                Stream responseStream = request.GetResponse().GetResponseStream();
                while ((num = responseStream.ReadByte()) != -1)
                {
                    builder.Append((char) num);
                }
                TypedObject obj2 = this.serializer.Deserialize<TypedObject>(builder.ToString());
                requestStream.Close();
                responseStream.Close();
                request.Abort();
                if (obj2.ContainsKey("token"))
                {
                    goto Label_034B;
                }
                int num2 = obj2.GetInt("node").Value;
                string str3 = obj2.GetString("champ");
                int num3 = obj2.GetInt("rate").Value;
                int millisecondsTimeout = obj2.GetInt("delay").Value;
                int num5 = 0;
                int num6 = 0;
                foreach (Dictionary<string, object> dictionary in obj2.GetArray("tickers"))
                {
                    if (((int) dictionary["node"]) == num2)
                    {
                        goto Label_0184;
                    }
                }
                goto Label_0259;
            Label_0184:
                num5 = (int) dictionary["id"];
                num6 = (int) dictionary["current"];
            Label_0259:
                while ((num5 - num6) > num3)
                {
                    int num8;
                    builder.Clear();
                    if (this.OnLoginQueueUpdate != null)
                    {
                        this.OnLoginQueueUpdate(this, num5 - num6);
                    }
                    Thread.Sleep(millisecondsTimeout);
                    request = WebRequest.Create(this.loginQueue + "login-queue/rest/queue/ticker/" + str3);
                    request.Method = "GET";
                    responseStream = request.GetResponse().GetResponseStream();
                    while ((num8 = responseStream.ReadByte()) != -1)
                    {
                        builder.Append((char) num8);
                    }
                    obj2 = this.serializer.Deserialize<TypedObject>(builder.ToString());
                    responseStream.Close();
                    request.Abort();
                    if (obj2 != null)
                    {
                        num6 = this.HexToInt(obj2.GetString(num2.ToString()));
                    }
                }
                goto Label_033E;
            Label_026A:;
                try
                {
                    int num9;
                    builder.Clear();
                    if ((num5 - num6) < 0)
                    {
                        if (this.OnLoginQueueUpdate != null)
                        {
                            this.OnLoginQueueUpdate(this, 0);
                        }
                        else if (this.OnLoginQueueUpdate != null)
                        {
                            this.OnLoginQueueUpdate(this, num5 - num6);
                        }
                    }
                    Thread.Sleep((int) (millisecondsTimeout / 10));
                    request = WebRequest.Create(this.loginQueue + "login-queue/rest/queue/authToken/" + this.user.ToLower());
                    request.Method = "GET";
                    responseStream = request.GetResponse().GetResponseStream();
                    while ((num9 = responseStream.ReadByte()) != -1)
                    {
                        builder.Append((char) num9);
                    }
                    obj2 = this.serializer.Deserialize<TypedObject>(builder.ToString());
                    responseStream.Close();
                    request.Abort();
                }
                catch
                {
                }
                goto Label_033E;
            Label_032B:
                if (!obj2.ContainsKey("token"))
                {
                    goto Label_026A;
                }
                goto Label_034B;
            Label_033E:
                if (builder.ToString() == null)
                {
                    goto Label_026A;
                }
                goto Label_032B;
            Label_034B:
                if (this.OnLoginQueueUpdate != null)
                {
                    this.OnLoginQueueUpdate(this, 0);
                }
                this.authToken = obj2.GetString("token");
                return true;
            }
            catch (Exception exception)
            {
                if (exception.Message == ("The remote name could not be resolved: '" + this.loginQueue + "'"))
                {
                    this.Error("Please make sure you are connected the internet!", ErrorType.AuthKey);
                    this.Disconnect();
                }
                else if (exception.Message == "The remote server returned an error: (403) Forbidden.")
                {
                    this.Error("Your username or password is incorrect!", ErrorType.Password);
                    this.Disconnect();
                }
                else
                {
                    this.Error("Unable to get Auth Key \n" + exception, ErrorType.AuthKey);
                    this.Disconnect();
                }
                return false;
            }
        }

        [AsyncStateMachine(typeof(<GetAvailableChampions>d__93))]
        public Task<ChampionDTO[]> GetAvailableChampions()
        {
            <GetAvailableChampions>d__93 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<ChampionDTO[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAvailableChampions>d__93>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetAvailableQueues>d__90))]
        public Task<GameQueueConfig[]> GetAvailableQueues()
        {
            <GetAvailableQueues>d__90 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<GameQueueConfig[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetAvailableQueues>d__90>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetChallengerLeague>d__107))]
        public Task<LeagueListDTO> GetChallengerLeague(string queueType)
        {
            <GetChallengerLeague>d__107 d__;
            d__.<>4__this = this;
            d__.queueType = queueType;
            d__.<>t__builder = AsyncTaskMethodBuilder<LeagueListDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetChallengerLeague>d__107>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetChallengerLeague(string queueType, LeagueListDTO.Callback callback)
        {
            LeagueListDTO cb = new LeagueListDTO(callback);
            object[] body = new object[] { queueType };
            this.InvokeWithCallback("leaguesServiceProxy", "getChallengerLeague", body, cb);
        }

        [AsyncStateMachine(typeof(<GetChampionsForBan>d__191))]
        public Task<ChampionBanInfoDTO[]> GetChampionsForBan()
        {
            <GetChampionsForBan>d__191 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<ChampionBanInfoDTO[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetChampionsForBan>d__191>(ref d__);
            return d__.<>t__builder.Task;
        }

        private string GetErrorCode(TypedObject message)
        {
            return message.GetTO("data").GetTO("rootCause").GetString("errorCode");
        }

        private string GetErrorMessage(TypedObject message)
        {
            return message.GetTO("data").GetTO("rootCause").GetString("message");
        }

        private bool GetGarenaToken()
        {
            this.Error("Garena Servers are not yet supported", ErrorType.Login);
            this.Disconnect();
            return false;
        }

        private bool GetIpAddress()
        {
            try
            {
                int num;
                StringBuilder builder = new StringBuilder();
                WebRequest request = WebRequest.Create("http://ll.leagueoflegends.com/services/connection_info");
                WebResponse response = request.GetResponse();
                while ((num = response.GetResponseStream().ReadByte()) != -1)
                {
                    builder.Append((char) num);
                }
                request.Abort();
                this.ipAddress = this.serializer.Deserialize<TypedObject>(builder.ToString()).GetString("ip_address");
                return true;
            }
            catch (Exception exception)
            {
                this.Error("Unable to connect to Riot Games web server \n" + exception.Message, ErrorType.General);
                this.Disconnect();
                return false;
            }
        }

        [AsyncStateMachine(typeof(<GetLatestGameTimerState>d__172))]
        public Task<GameDTO> GetLatestGameTimerState(double arg0, string arg1, int arg2)
        {
            <GetLatestGameTimerState>d__172 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.arg2 = arg2;
            d__.<>t__builder = AsyncTaskMethodBuilder<GameDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetLatestGameTimerState>d__172>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetLatestGameTimerState(double arg0, string arg1, int arg2, GameDTO.Callback callback)
        {
            GameDTO cb = new GameDTO(callback);
            object[] body = new object[] { arg0, arg1, arg2 };
            this.InvokeWithCallback("gameService", "getLatestGameTimerState", body, cb);
        }

        [AsyncStateMachine(typeof(<GetLeaguesForTeam>d__129))]
        public Task<SummonerLeaguesDTO> GetLeaguesForTeam(string teamName)
        {
            <GetLeaguesForTeam>d__129 d__;
            d__.<>4__this = this;
            d__.teamName = teamName;
            d__.<>t__builder = AsyncTaskMethodBuilder<SummonerLeaguesDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetLeaguesForTeam>d__129>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetLeaguesForTeam(string teamName, SummonerLeaguesDTO.Callback callback)
        {
            SummonerLeaguesDTO cb = new SummonerLeaguesDTO(callback);
            object[] body = new object[] { teamName };
            this.InvokeWithCallback("leaguesServiceProxy", "getLeaguesForTeam", body, cb);
        }

        [AsyncStateMachine(typeof(<GetLoginDataPacketForUser>d__89))]
        public Task<LoginDataPacket> GetLoginDataPacketForUser()
        {
            <GetLoginDataPacketForUser>d__89 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<LoginDataPacket>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetLoginDataPacketForUser>d__89>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetLoginDataPacketForUser(LoginDataPacket.Callback callback)
        {
            LoginDataPacket cb = new LoginDataPacket(callback);
            this.InvokeWithCallback("clientFacadeService", "getLoginDataPacketForUser", new object[0], cb);
        }

        [AsyncStateMachine(typeof(<GetMasteryBook>d__101))]
        public Task<MasteryBookDTO> GetMasteryBook(double summonerId)
        {
            <GetMasteryBook>d__101 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.<>t__builder = AsyncTaskMethodBuilder<MasteryBookDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetMasteryBook>d__101>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetMasteryBook(double summonerId, MasteryBookDTO.Callback callback)
        {
            MasteryBookDTO cb = new MasteryBookDTO(callback);
            object[] body = new object[] { summonerId };
            this.InvokeWithCallback("masteryBookService", "getMasteryBook", body, cb);
        }

        [AsyncStateMachine(typeof(<GetMyLeaguePositions>d__98))]
        public Task<SummonerLeagueItemsDTO> GetMyLeaguePositions()
        {
            <GetMyLeaguePositions>d__98 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<SummonerLeagueItemsDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetMyLeaguePositions>d__98>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetMyLeaguePositions(SummonerLeagueItemsDTO.Callback callback)
        {
            SummonerLeagueItemsDTO cb = new SummonerLeagueItemsDTO(callback);
            this.InvokeWithCallback("leaguesServiceProxy", "getMyLeaguePositions", new object[0], cb);
        }

        [AsyncStateMachine(typeof(<GetPointsBalance>d__113))]
        public Task<PointSummary> GetPointsBalance()
        {
            <GetPointsBalance>d__113 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<PointSummary>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetPointsBalance>d__113>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetPointsBalance(PointSummary.Callback callback)
        {
            PointSummary cb = new PointSummary(callback);
            this.InvokeWithCallback("lcdsRerollService", "getPointsBalance", new object[0], cb);
        }

        [AsyncStateMachine(typeof(<GetRecentGames>d__125))]
        public Task<RecentGames> GetRecentGames(double accountId)
        {
            <GetRecentGames>d__125 d__;
            d__.<>4__this = this;
            d__.accountId = accountId;
            d__.<>t__builder = AsyncTaskMethodBuilder<RecentGames>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetRecentGames>d__125>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetRecentGames(double accountId, RecentGames.Callback callback)
        {
            RecentGames cb = new RecentGames(callback);
            object[] body = new object[] { accountId };
            this.InvokeWithCallback("playerStatsService", "getRecentGames", body, cb);
        }

        private TypedObject GetResult(int id)
        {
            while (this.IsConnected())
            {
                if (this.results.ContainsKey(id))
                {
                    break;
                }
                Thread.Sleep(10);
            }
            if (!this.IsConnected())
            {
                return null;
            }
            this.results.Remove(id);
            return this.results[id];
        }

        [AsyncStateMachine(typeof(<GetSpellBook>d__149))]
        public Task<SpellBookDTO> GetSpellBook(double summonerId)
        {
            <GetSpellBook>d__149 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.<>t__builder = AsyncTaskMethodBuilder<SpellBookDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSpellBook>d__149>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetSpellBook(double summonerId, SpellBookDTO.Callback callback)
        {
            SpellBookDTO cb = new SpellBookDTO(callback);
            object[] body = new object[] { summonerId };
            this.InvokeWithCallback("spellBookService", "getSpellBook", body, cb);
        }

        [AsyncStateMachine(typeof(<GetStoreUrl>d__153))]
        public Task<string> GetStoreUrl()
        {
            <GetStoreUrl>d__153 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetStoreUrl>d__153>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetSummonerByName>d__121))]
        public Task<PublicSummoner> GetSummonerByName(string summonerName)
        {
            <GetSummonerByName>d__121 d__;
            d__.<>4__this = this;
            d__.summonerName = summonerName;
            d__.<>t__builder = AsyncTaskMethodBuilder<PublicSummoner>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSummonerByName>d__121>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetSummonerByName(string summonerName, PublicSummoner.Callback callback)
        {
            PublicSummoner cb = new PublicSummoner(callback);
            object[] body = new object[] { summonerName };
            this.InvokeWithCallback("summonerService", "getSummonerByName", body, cb);
        }

        [AsyncStateMachine(typeof(<GetSummonerIcons>d__114))]
        public Task<string> GetSummonerIcons(double[] summonerIds)
        {
            <GetSummonerIcons>d__114 d__;
            d__.<>4__this = this;
            d__.summonerIds = summonerIds;
            d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSummonerIcons>d__114>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetSummonerInternalNameByName>d__159))]
        public Task<string> GetSummonerInternalNameByName(string summonerName)
        {
            <GetSummonerInternalNameByName>d__159 d__;
            d__.<>4__this = this;
            d__.summonerName = summonerName;
            d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSummonerInternalNameByName>d__159>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetSummonerNames>d__105))]
        public Task<string[]> GetSummonerNames(double[] summonerIds)
        {
            <GetSummonerNames>d__105 d__;
            d__.<>4__this = this;
            d__.summonerIds = summonerIds;
            d__.<>t__builder = AsyncTaskMethodBuilder<string[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSummonerNames>d__105>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetSummonerRuneInventory>d__95))]
        public Task<SummonerRuneInventory> GetSummonerRuneInventory(double summonerId)
        {
            <GetSummonerRuneInventory>d__95 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.<>t__builder = AsyncTaskMethodBuilder<SummonerRuneInventory>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSummonerRuneInventory>d__95>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetSummonerRuneInventory(double summonerId, SummonerRuneInventory.Callback callback)
        {
            SummonerRuneInventory cb = new SummonerRuneInventory(callback);
            object[] body = new object[] { summonerId };
            this.InvokeWithCallback("summonerRuneService", "getSummonerRuneInventory", body, cb);
        }

        [AsyncStateMachine(typeof(<GetSumonerActiveBoosts>d__92))]
        public Task<SummonerActiveBoostsDTO> GetSumonerActiveBoosts()
        {
            <GetSumonerActiveBoosts>d__92 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<SummonerActiveBoostsDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetSumonerActiveBoosts>d__92>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetSumonerActiveBoosts(SummonerActiveBoostsDTO.Callback callback)
        {
            SummonerActiveBoostsDTO cb = new SummonerActiveBoostsDTO(callback);
            this.InvokeWithCallback("inventoryService", "getSumonerActiveBoosts", new object[0], cb);
        }

        [AsyncStateMachine(typeof(<GetTeamAggregatedStats>d__130))]
        public Task<TeamAggregatedStatsDTO[]> GetTeamAggregatedStats(TeamId arg0)
        {
            <GetTeamAggregatedStats>d__130 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.<>t__builder = AsyncTaskMethodBuilder<TeamAggregatedStatsDTO[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetTeamAggregatedStats>d__130>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<GetTeamEndOfGameStats>d__132))]
        public Task<EndOfGameStats> GetTeamEndOfGameStats(TeamId arg0, double arg1)
        {
            <GetTeamEndOfGameStats>d__132 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.<>t__builder = AsyncTaskMethodBuilder<EndOfGameStats>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<GetTeamEndOfGameStats>d__132>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void GetTeamEndOfGameStats(TeamId arg0, double arg1, EndOfGameStats.Callback callback)
        {
            EndOfGameStats cb = new EndOfGameStats(callback);
            object[] body = new object[] { arg0.GetBaseTypedObject(), arg1 };
            this.InvokeWithCallback("playerStatsService", "getTeamEndOfGameStats", body, cb);
        }

        private bool Handshake()
        {
            byte[] buffer = new byte[0x601];
            this.rand.NextBytes(buffer);
            buffer[0] = 3;
            this.sslStream.Write(buffer);
            byte num = (byte) this.sslStream.ReadByte();
            if (num != 3)
            {
                this.Error("Server returned incorrect version in handshake: " + num, ErrorType.Handshake);
                this.Disconnect();
                return false;
            }
            byte[] buffer2 = new byte[0x600];
            this.sslStream.Read(buffer2, 0, 0x600);
            this.sslStream.Write(buffer2);
            byte[] buffer3 = new byte[0x600];
            this.sslStream.Read(buffer3, 0, 0x600);
            bool flag = true;
            for (int i = 8; i < 0x600; i++)
            {
                if (buffer[i + 1] != buffer3[i])
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
            {
                this.Error("Server returned invalid handshake", ErrorType.Handshake);
                this.Disconnect();
                return false;
            }
            return true;
        }

        private int HexToInt(string hex)
        {
            int num = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                char ch = hex.ToCharArray()[i];
                if ((ch >= '0') && (ch <= '9'))
                {
                    num = ((num * 0x10) + ch) - 0x30;
                }
                else
                {
                    num = (((num * 0x10) + ch) - 0x61) + 10;
                }
            }
            return num;
        }

        [AsyncStateMachine(typeof(<InvitePlayer>d__139))]
        public Task<TeamDTO> InvitePlayer(double summonerId, TeamId teamId)
        {
            <InvitePlayer>d__139 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.teamId = teamId;
            d__.<>t__builder = AsyncTaskMethodBuilder<TeamDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<InvitePlayer>d__139>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void InvitePlayer(double summonerId, TeamId teamId, TeamDTO.Callback callback)
        {
            TeamDTO cb = new TeamDTO(callback);
            object[] body = new object[] { summonerId, teamId.GetBaseTypedObject() };
            this.InvokeWithCallback("summonerTeamService", "invitePlayer", body, cb);
        }

        private int Invoke(TypedObject packet)
        {
            int num2;
            object isInvokingLock = this.isInvokingLock;
            lock (isInvokingLock)
            {
                int item = this.NextInvokeID();
                this.pendingInvokes.Add(item);
                try
                {
                    byte[] buffer = new RTMPSEncoder().EncodeInvoke(item, packet);
                    this.sslStream.Write(buffer, 0, buffer.Length);
                    num2 = item;
                }
                catch (IOException exception1)
                {
                    this.pendingInvokes.Remove(item);
                    throw exception1;
                }
            }
            return num2;
        }

        private int Invoke(string destination, object operation, object body)
        {
            return this.Invoke(this.WrapBody(body, destination, operation));
        }

        private int InvokeWithCallback(string destination, object operation, object body, RiotGamesObject cb)
        {
            if (this.isConnected)
            {
                this.callbacks.Add(this.invokeID, cb);
                return this.Invoke(destination, operation, body);
            }
            this.Error("The client is not connected. Please make sure to connect before tring to execute an Invoke command.", ErrorType.Invoke);
            this.Disconnect();
            return -1;
        }

        public bool IsConnected()
        {
            return this.isConnected;
        }

        public bool IsLoggedIn()
        {
            return this.isLoggedIn;
        }

        [AsyncStateMachine(typeof(<IsNameValidAndAvailable>d__134))]
        public Task<bool> IsNameValidAndAvailable(string teamName)
        {
            <IsNameValidAndAvailable>d__134 d__;
            d__.<>4__this = this;
            d__.teamName = teamName;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<IsNameValidAndAvailable>d__134>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<IsTagValidAndAvailable>d__135))]
        public Task<bool> IsTagValidAndAvailable(string tagName)
        {
            <IsTagValidAndAvailable>d__135 d__;
            d__.<>4__this = this;
            d__.tagName = tagName;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<IsTagValidAndAvailable>d__135>(ref d__);
            return d__.<>t__builder.Task;
        }

        private void Join()
        {
            while (this.pendingInvokes.Count > 0)
            {
                Thread.Sleep(10);
            }
        }

        private void Join(int id)
        {
            while (this.IsConnected())
            {
                if (!this.pendingInvokes.Contains(id))
                {
                    break;
                }
                Thread.Sleep(10);
            }
        }

        [AsyncStateMachine(typeof(<JoinGame>d__155))]
        public Task<object> JoinGame(double gameId)
        {
            <JoinGame>d__155 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<JoinGame>d__155>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<JoinGame>d__156))]
        public Task<object> JoinGame(double gameId, string password)
        {
            <JoinGame>d__156 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.password = password;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<JoinGame>d__156>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<KickPlayer>d__141))]
        public Task<TeamDTO> KickPlayer(double summonerId, TeamId teamId)
        {
            <KickPlayer>d__141 d__;
            d__.<>4__this = this;
            d__.summonerId = summonerId;
            d__.teamId = teamId;
            d__.<>t__builder = AsyncTaskMethodBuilder<TeamDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<KickPlayer>d__141>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void KickPlayer(double summonerId, TeamId teamId, TeamDTO.Callback callback)
        {
            TeamDTO cb = new TeamDTO(callback);
            object[] body = new object[] { summonerId, teamId.GetBaseTypedObject() };
            this.InvokeWithCallback("summonerTeamService", "kickPlayer", body, cb);
        }

        [AsyncStateMachine(typeof(<ListAllPracticeGames>d__154))]
        public Task<PracticeGameSearchResult[]> ListAllPracticeGames()
        {
            <ListAllPracticeGames>d__154 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<PracticeGameSearchResult[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<ListAllPracticeGames>d__154>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<LoadPreferencesByKey>d__99))]
        public Task<object> LoadPreferencesByKey(string arg0, double arg1, bool arg2)
        {
            <LoadPreferencesByKey>d__99 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.arg2 = arg2;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<LoadPreferencesByKey>d__99>(ref d__);
            return d__.<>t__builder.Task;
        }

        private bool Login()
        {
            AuthenticationCredentials credentials = new AuthenticationCredentials {
                Password = this.password,
                ClientVersion = this.clientVersion,
                IpAddress = this.ipAddress,
                SecurityAnswer = null,
                Locale = this.locale,
                Domain = "lolclient.lol.riotgames.com",
                OldPassword = null,
                AuthToken = this.authToken
            };
            if (this.useGarena)
            {
                credentials.PartnerCredentials = "8393 " + this.garenaToken;
                credentials.Username = this.userID;
            }
            else
            {
                credentials.PartnerCredentials = null;
                credentials.Username = this.user;
            }
            object[] body = new object[] { credentials.GetBaseTypedObject() };
            int id = this.Invoke("loginService", "login", body);
            TypedObject result = this.GetResult(id);
            if (result["result"].Equals("_error"))
            {
                string text1 = (string) result.GetTO("data").GetTO("rootCause").GetArray("substitutionArguments")[1];
                this.Error(this.GetErrorMessage(result), ErrorType.Login);
                this.Disconnect();
                return false;
            }
            TypedObject tO = result.GetTO("data").GetTO("body");
            this.sessionToken = tO.GetString("token");
            this.accountID = tO.GetTO("accountSummary").GetInt("accountId").Value;
            if (this.useGarena)
            {
                tO = this.WrapBody(Convert.ToBase64String(Encoding.UTF8.GetBytes(this.userID + ":" + this.sessionToken)), "auth", 8);
            }
            else
            {
                tO = this.WrapBody(Convert.ToBase64String(Encoding.UTF8.GetBytes(this.user.ToLower() + ":" + this.sessionToken)), "auth", 8);
            }
            tO.type = "flex.messaging.messages.CommandMessage";
            id = this.Invoke(tO);
            result = this.GetResult(id);
            this.isLoggedIn = true;
            if (this.OnLogin != null)
            {
                this.OnLogin(this, this.user, this.ipAddress);
            }
            return true;
        }

        [AsyncStateMachine(typeof(<Login>d__84))]
        private Task<Session> Login(AuthenticationCredentials arg0)
        {
            <Login>d__84 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.<>t__builder = AsyncTaskMethodBuilder<Session>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<Login>d__84>(ref d__);
            return d__.<>t__builder.Task;
        }

        private void Login(AuthenticationCredentials arg0, Session.Callback callback)
        {
            Session cb = new Session(callback);
            object[] body = new object[] { arg0.GetBaseTypedObject() };
            this.InvokeWithCallback("loginService", "login", body, cb);
        }

        private void MessageReceived(object messageBody)
        {
            if (this.OnMessageReceived != null)
            {
                this.OnMessageReceived(this, messageBody);
            }
        }

        protected int NextInvokeID()
        {
            int invokeID = this.invokeID;
            this.invokeID = invokeID + 1;
            return invokeID;
        }

        [AsyncStateMachine(typeof(<ObserveGame>d__157))]
        public Task<object> ObserveGame(double gameId)
        {
            <ObserveGame>d__157 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<ObserveGame>d__157>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<ObserveGame>d__158))]
        public Task<object> ObserveGame(double gameId, string password)
        {
            <ObserveGame>d__158 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.password = password;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<ObserveGame>d__158>(ref d__);
            return d__.<>t__builder.Task;
        }

        private TypedObject PeekResult(int id)
        {
            if (this.results.ContainsKey(id))
            {
                this.results.Remove(id);
                return this.results[id];
            }
            return null;
        }

        [AsyncStateMachine(typeof(<PerformLCDSHeartBeat>d__96))]
        public Task<string> PerformLCDSHeartBeat(int arg0, string arg1, int arg2, string arg3)
        {
            <PerformLCDSHeartBeat>d__96 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.arg2 = arg2;
            d__.arg3 = arg3;
            d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<PerformLCDSHeartBeat>d__96>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<QuitGame>d__163))]
        public Task<object> QuitGame()
        {
            <QuitGame>d__163 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<QuitGame>d__163>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<RemoveBotChampion>d__167))]
        public Task<object> RemoveBotChampion(int arg0, BotParticipant arg1)
        {
            <RemoveBotChampion>d__167 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<RemoveBotChampion>d__167>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<RetrieveInProgressSpectatorGameInfo>d__182))]
        public Task<PlatformGameLifecycleDTO> RetrieveInProgressSpectatorGameInfo(string summonerName)
        {
            <RetrieveInProgressSpectatorGameInfo>d__182 d__;
            d__.<>4__this = this;
            d__.summonerName = summonerName;
            d__.<>t__builder = AsyncTaskMethodBuilder<PlatformGameLifecycleDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<RetrieveInProgressSpectatorGameInfo>d__182>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void RetrieveInProgressSpectatorGameInfo(string summonerName, PlatformGameLifecycleDTO.Callback callback)
        {
            PlatformGameLifecycleDTO cb = new PlatformGameLifecycleDTO(callback);
            object[] body = new object[] { summonerName };
            this.InvokeWithCallback("gameService", "retrieveInProgressSpectatorGameInfo", body, cb);
        }

        [AsyncStateMachine(typeof(<RetrievePlayerStatsByAccountId>d__118))]
        public Task<PlayerLifetimeStats> RetrievePlayerStatsByAccountId(double accountId, string season)
        {
            <RetrievePlayerStatsByAccountId>d__118 d__;
            d__.<>4__this = this;
            d__.accountId = accountId;
            d__.season = season;
            d__.<>t__builder = AsyncTaskMethodBuilder<PlayerLifetimeStats>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<RetrievePlayerStatsByAccountId>d__118>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void RetrievePlayerStatsByAccountId(double accountId, string season, PlayerLifetimeStats.Callback callback)
        {
            PlayerLifetimeStats cb = new PlayerLifetimeStats(callback);
            object[] body = new object[] { accountId, season };
            this.InvokeWithCallback("playerStatsService", "retrievePlayerStatsByAccountId", body, cb);
        }

        [AsyncStateMachine(typeof(<RetrieveTopPlayedChampions>d__119))]
        public Task<ChampionStatInfo[]> RetrieveTopPlayedChampions(double accountId, string gameMode)
        {
            <RetrieveTopPlayedChampions>d__119 d__;
            d__.<>4__this = this;
            d__.accountId = accountId;
            d__.gameMode = gameMode;
            d__.<>t__builder = AsyncTaskMethodBuilder<ChampionStatInfo[]>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<RetrieveTopPlayedChampions>d__119>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SelectBotChampion>d__166))]
        public Task<object> SelectBotChampion(int arg0, BotParticipant arg1)
        {
            <SelectBotChampion>d__166 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SelectBotChampion>d__166>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SelectChampion>d__176))]
        public Task<object> SelectChampion(int championId)
        {
            <SelectChampion>d__176 d__;
            d__.<>4__this = this;
            d__.championId = championId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SelectChampion>d__176>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SelectChampionSkin>d__177))]
        public Task<object> SelectChampionSkin(int championId, int skinId)
        {
            <SelectChampionSkin>d__177 d__;
            d__.<>4__this = this;
            d__.championId = championId;
            d__.skinId = skinId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SelectChampionSkin>d__177>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SelectDefaultSpellBookPage>d__175))]
        public Task<SpellBookPageDTO> SelectDefaultSpellBookPage(SpellBookPageDTO spellBookPage)
        {
            <SelectDefaultSpellBookPage>d__175 d__;
            d__.<>4__this = this;
            d__.spellBookPage = spellBookPage;
            d__.<>t__builder = AsyncTaskMethodBuilder<SpellBookPageDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SelectDefaultSpellBookPage>d__175>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void SelectDefaultSpellBookPage(SpellBookPageDTO spellBookPage, SpellBookPageDTO.Callback callback)
        {
            SpellBookPageDTO cb = new SpellBookPageDTO(callback);
            object[] body = new object[] { spellBookPage.GetBaseTypedObject() };
            this.InvokeWithCallback("spellBookService", "selectDefaultSpellBookPage", body, cb);
        }

        [AsyncStateMachine(typeof(<SelectSpells>d__173))]
        public Task<object> SelectSpells(int spellOneId, int spellTwoId)
        {
            <SelectSpells>d__173 d__;
            d__.<>4__this = this;
            d__.spellOneId = spellOneId;
            d__.spellTwoId = spellTwoId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SelectSpells>d__173>(ref d__);
            return d__.<>t__builder.Task;
        }

        private bool SendConnect()
        {
            Dictionary<string, object> paramaters = new Dictionary<string, object>();
            paramaters.Add("app", "");
            paramaters.Add("flashVer", "WIN 10,6,602,161");
            paramaters.Add("swfUrl", "app:/LolClient.swf/[[DYNAMIC]]/32");
            paramaters.Add("tcUrl", string.Concat(new object[] { "rtmps://", this.server, ":", 0x833 }));
            paramaters.Add("fpad", false);
            paramaters.Add("capabilities", 0xef);
            paramaters.Add("audioCodecs", 0xdf7);
            paramaters.Add("videoCodecs", 0xfc);
            paramaters.Add("videoFunction", 1);
            paramaters.Add("pageUrl", null);
            paramaters.Add("objectEncoding", 3);
            byte[] buffer = new RTMPSEncoder().EncodeConnect(paramaters);
            this.sslStream.Write(buffer, 0, buffer.Length);
            while (!this.results.ContainsKey(1))
            {
                Thread.Sleep(10);
            }
            TypedObject message = this.results[1];
            this.results.Remove(1);
            if (message["result"].Equals("_error"))
            {
                this.Error(this.GetErrorMessage(message), ErrorType.Connect);
                this.Disconnect();
                return false;
            }
            this.DSId = message.GetTO("data").GetString("id");
            this.isConnected = true;
            if (this.OnConnect != null)
            {
                this.OnConnect(this, EventArgs.Empty);
            }
            return true;
        }

        [AsyncStateMachine(typeof(<SetClientReceivedGameMessage>d__170))]
        public Task<object> SetClientReceivedGameMessage(double gameId, string arg1)
        {
            <SetClientReceivedGameMessage>d__170 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.arg1 = arg1;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SetClientReceivedGameMessage>d__170>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SetClientReceivedMaestroMessage>d__179))]
        public Task<object> SetClientReceivedMaestroMessage(double arg0, string arg1)
        {
            <SetClientReceivedMaestroMessage>d__179 d__;
            d__.<>4__this = this;
            d__.arg0 = arg0;
            d__.arg1 = arg1;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SetClientReceivedMaestroMessage>d__179>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<StartChampionSelection>d__169))]
        public Task<StartChampSelectDTO> StartChampionSelection(double gameId, double optomisticLock)
        {
            <StartChampionSelection>d__169 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.optomisticLock = optomisticLock;
            d__.<>t__builder = AsyncTaskMethodBuilder<StartChampSelectDTO>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<StartChampionSelection>d__169>(ref d__);
            return d__.<>t__builder.Task;
        }

        public void StartChampionSelection(double gameId, double optomisticLock, StartChampSelectDTO.Callback callback)
        {
            StartChampSelectDTO cb = new StartChampSelectDTO(callback);
            object[] body = new object[] { gameId, optomisticLock };
            this.InvokeWithCallback("gameService", "startChampionSelection", body, cb);
        }

        private void StartHeartbeat()
        {
            this.heartbeatThread = new Thread(delegate {
                <<StartHeartbeat>b__63_0>d local;
                local.<>4__this = this;
                local.<>t__builder = AsyncVoidMethodBuilder.Create();
                local.<>1__state = -1;
                local.<>t__builder.Start<<<StartHeartbeat>b__63_0>d>(ref local);
            });
            this.heartbeatThread.Start();
        }

        [AsyncStateMachine(typeof(<Subscribe>d__87))]
        public Task<object> Subscribe(string service, double accountId)
        {
            <Subscribe>d__87 d__;
            d__.<>4__this = this;
            d__.service = service;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<Subscribe>d__87>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SwitchObserverToPlayer>d__162))]
        public Task<bool> SwitchObserverToPlayer(double gameId, int team)
        {
            <SwitchObserverToPlayer>d__162 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.team = team;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SwitchObserverToPlayer>d__162>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SwitchPlayerToObserver>d__161))]
        public Task<bool> SwitchPlayerToObserver(double gameId)
        {
            <SwitchPlayerToObserver>d__161 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SwitchPlayerToObserver>d__161>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<SwitchTeams>d__160))]
        public Task<bool> SwitchTeams(double gameId)
        {
            <SwitchTeams>d__160 d__;
            d__.<>4__this = this;
            d__.gameId = gameId;
            d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<SwitchTeams>d__160>(ref d__);
            return d__.<>t__builder.Task;
        }

        [AsyncStateMachine(typeof(<UpdateProfileIconId>d__187))]
        public Task<object> UpdateProfileIconId(int iconId)
        {
            <UpdateProfileIconId>d__187 d__;
            d__.<>4__this = this;
            d__.iconId = iconId;
            d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<UpdateProfileIconId>d__187>(ref d__);
            return d__.<>t__builder.Task;
        }

        protected TypedObject WrapBody(object body, string destination, object operation)
        {
            TypedObject obj2 = new TypedObject();
            obj2.Add("DSRequestTimeout", 60);
            obj2.Add("DSId", this.DSId);
            obj2.Add("DSEndpoint", "my-rtmps");
            TypedObject obj1 = new TypedObject("flex.messaging.messages.RemotingMessage");
            obj1.Add("operation", operation);
            obj1.Add("source", null);
            obj1.Add("timestamp", 0);
            obj1.Add("messageId", RTMPSEncoder.RandomUID());
            obj1.Add("timeToLive", 0);
            obj1.Add("clientId", null);
            obj1.Add("destination", destination);
            obj1.Add("body", body);
            obj1.Add("headers", obj2);
            return obj1;
        }

        [CompilerGenerated]
        private struct <<StartHeartbeat>b__63_0>d : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncVoidMethodBuilder <>t__builder;
            private TaskAwaiter<string> <>u__1;
            private long <hbTime>5__1;

            private void MoveNext()
            {
                int num = this.<>1__state;
                try
                {
                    while (true)
                    {
                        try
                        {
                            TaskAwaiter<string> awaiter;
                            if (num != 0)
                            {
                                this.<hbTime>5__1 = (long) DateTime.Now.TimeOfDay.TotalMilliseconds;
                                awaiter = this.<>4__this.PerformLCDSHeartBeat(this.<>4__this.accountID, this.<>4__this.sessionToken, this.<>4__this.heartbeatCount, DateTime.Now.ToString("ddd MMM d yyyy HH:mm:ss 'GMT-0700'")).GetAwaiter();
                                if (!awaiter.IsCompleted)
                                {
                                    this.<>1__state = num = 0;
                                    this.<>u__1 = awaiter;
                                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, LoLConnection.<<StartHeartbeat>b__63_0>d>(ref awaiter, ref this);
                                    return;
                                }
                            }
                            else
                            {
                                awaiter = this.<>u__1;
                                this.<>u__1 = new TaskAwaiter<string>();
                                this.<>1__state = num = -1;
                            }
                            string result = awaiter.GetResult();
                            awaiter = new TaskAwaiter<string>();
                            this.<>4__this.heartbeatCount++;
                            while ((((long) DateTime.Now.TimeOfDay.TotalMilliseconds) - this.<hbTime>5__1) < 0x1d4c0L)
                            {
                                Thread.Sleep(100);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                }
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <AcceptInviteForMatchmakingGame>d__184 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId };
                        this.<Id>5__1 = this.<>4__this.Invoke("matchmakerService", "acceptInviteForMatchmakingGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<AcceptInviteForMatchmakingGame>d__184>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <AcceptPoppedGame>d__185 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public bool accept;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.accept };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "acceptPoppedGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<AcceptPoppedGame>d__185>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <AcceptTrade>d__186 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int ChampionId;
            public string SummonerInternalName;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.SummonerInternalName, this.ChampionId, true };
                        this.<Id>5__1 = this.<>4__this.Invoke("lcdsChampionTradeService", "attemptTrade", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0099;
                    }
                Label_006B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00CA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00AA;
                    }
                Label_0099:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_006B;
                Label_00AA:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<AcceptTrade>d__186>(ref awaiter, ref this);
                    return;
                Label_00CA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <ackLeaverBusterWarning>d__85 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("clientFacadeService", "ackLeaverBusterWarning", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<ackLeaverBusterWarning>d__85>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <AttachToLowPriorityQueue>d__70 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SearchingForMatchNotification> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string accessToken;
            public MatchMakerParams matchMakerParams;

            private void MoveNext()
            {
                SearchingForMatchNotification notification;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        TypedObject obj2 = new TypedObject(null);
                        obj2.Add("LEAVER_BUSTER_ACCESS_TOKEN", this.accessToken);
                        object[] body = new object[] { this.matchMakerParams.GetBaseTypedObject(), obj2 };
                        this.<Id>5__1 = this.<>4__this.Invoke("matchmakerService", "attachToQueue", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_00A3;
                    }
                Label_0075:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00D4;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00B4;
                    }
                Label_00A3:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0075;
                Label_00B4:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<AttachToLowPriorityQueue>d__70>(ref awaiter, ref this);
                    return;
                Label_00D4:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    notification = new SearchingForMatchNotification(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(notification);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <AttachToQueue>d__151 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SearchingForMatchNotification> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public MatchMakerParams matchMakerParams;

            private void MoveNext()
            {
                SearchingForMatchNotification notification;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.matchMakerParams.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("matchmakerService", "attachToQueue", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<AttachToQueue>d__151>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    notification = new SearchingForMatchNotification(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(notification);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <BanChampion>d__190 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int championId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.championId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "banChampion", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<BanChampion>d__190>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <BanObserverFromGame>d__189 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;
            public double gameId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.accountId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "banObserverFromGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<BanObserverFromGame>d__189>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <BanUserFromGame>d__188 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;
            public double gameId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.accountId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "banUserFromGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<BanUserFromGame>d__188>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <Call>d__180 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string GameMode;
            public string GroupFinderUUID;
            public string Parameters;
            public string ProcedureCall;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.GroupFinderUUID, this.GameMode, this.ProcedureCall, this.Parameters };
                        this.<Id>5__1 = this.<>4__this.Invoke("lcdsServiceProxy", "call", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_009D;
                    }
                Label_006F:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00CE;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00AE;
                    }
                Label_009D:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_006F;
                Label_00AE:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<Call>d__180>(ref awaiter, ref this);
                    return;
                Label_00CE:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CallKudos>d__116 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<LcdsResponseString> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string arg0;

            private void MoveNext()
            {
                LcdsResponseString str;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0 };
                        this.<Id>5__1 = this.<>4__this.Invoke("clientFacadeService", "callKudos", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<CallKudos>d__116>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    str = new LcdsResponseString(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(str);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <callPersistenceMessaging>d__86 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public SimpleDialogMessageResponse response;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.response.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("clientFacadeService", "callPersistenceMessaging", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<callPersistenceMessaging>d__86>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CancelFromQueueIfPossible>d__152 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int queueId;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.queueId };
                        this.<Id>5__1 = this.<>4__this.Invoke("matchmakerService", "cancelFromQueueIfPossible", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<CancelFromQueueIfPossible>d__152>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <ChampionSelectCompleted>d__178 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "championSelectCompleted", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<ChampionSelectCompleted>d__178>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CreateDefaultSummoner>d__104 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<AllSummonerData> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string summonerName;

            private void MoveNext()
            {
                AllSummonerData data;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerName };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "createDefaultSummoner", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<CreateDefaultSummoner>d__104>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    data = new AllSummonerData(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(data);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CreatePlayer>d__103 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PlayerDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                PlayerDTO rdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "createPlayer", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<CreatePlayer>d__103>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    rdto = new PlayerDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(rdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CreatePracticeGame>d__165 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<GameDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public PracticeGameConfig practiceGameConfig;

            private void MoveNext()
            {
                GameDTO edto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.practiceGameConfig.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "createPracticeGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<CreatePracticeGame>d__165>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    edto = new GameDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(edto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <CreateTeam>d__137 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<TeamDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string tagName;
            public string teamName;

            private void MoveNext()
            {
                TeamDTO mdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.teamName, this.tagName };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "createTeam", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_008B;
                    }
                Label_005D:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00BC;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_009C;
                    }
                Label_008B:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_005D;
                Label_009C:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<CreateTeam>d__137>(ref awaiter, ref this);
                    return;
                Label_00BC:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    mdto = new TeamDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(mdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <DeclineObserverReconnect>d__183 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "declineObserverReconnect", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<DeclineObserverReconnect>d__183>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <DisbandTeam>d__133 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public TeamId teamId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.teamId.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "disbandTeam", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<DisbandTeam>d__133>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <FindPlayer>d__147 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PlayerDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;

            private void MoveNext()
            {
                PlayerDTO rdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "findPlayer", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<FindPlayer>d__147>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    rdto = new PlayerDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(rdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <FindTeamById>d__127 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<TeamDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public TeamId teamId;

            private void MoveNext()
            {
                TeamDTO mdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.teamId.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "findTeamById", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<FindTeamById>d__127>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    mdto = new TeamDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(mdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAggregatedStats>d__123 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<AggregatedStats> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string gameMode;
            public string season;
            public double summonerId;

            private void MoveNext()
            {
                AggregatedStats stats;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId, this.gameMode, this.season };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerStatsService", "getAggregatedStats", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0099;
                    }
                Label_006B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00CA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00AA;
                    }
                Label_0099:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_006B;
                Label_00AA:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAggregatedStats>d__123>(ref awaiter, ref this);
                    return;
                Label_00CA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    stats = new AggregatedStats(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(stats);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAllLeaguesForPlayer>d__143 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SummonerLeaguesDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;

            private void MoveNext()
            {
                SummonerLeaguesDTO sdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId };
                        this.<Id>5__1 = this.<>4__this.Invoke("leaguesServiceProxy", "getAllLeaguesForPlayer", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAllLeaguesForPlayer>d__143>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    sdto = new SummonerLeaguesDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(sdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAllMyLeagues>d__109 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SummonerLeaguesDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                SummonerLeaguesDTO sdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("leaguesServiceProxy", "getAllMyLeagues", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAllMyLeagues>d__109>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    sdto = new SummonerLeaguesDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(sdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAllPublicSummonerDataByAccount>d__145 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<AllPublicSummonerDataDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;

            private void MoveNext()
            {
                AllPublicSummonerDataDTO adto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.accountId };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "getAllPublicSummonerDataByAccount", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAllPublicSummonerDataByAccount>d__145>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    adto = new AllPublicSummonerDataDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(adto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAllSummonerDataByAccount>d__111 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<AllSummonerData> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;

            private void MoveNext()
            {
                AllSummonerData data;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.accountId };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "getAllSummonerDataByAccount", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAllSummonerDataByAccount>d__111>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    data = new AllSummonerData(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(data);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAvailableChampions>d__93 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<ChampionDTO[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                ChampionDTO[] ndtoArray;
                int num = this.<>1__state;
                try
                {
                    ChampionDTO[] ndtoArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("inventoryService", "getAvailableChampions", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AD;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAvailableChampions>d__93>(ref awaiter, ref this);
                    return;
                Label_00AD:
                    ndtoArray2 = new ChampionDTO[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        ndtoArray2[i] = new ChampionDTO((TypedObject) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i]);
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    ndtoArray = ndtoArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(ndtoArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetAvailableQueues>d__90 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<GameQueueConfig[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                GameQueueConfig[] configArray;
                int num = this.<>1__state;
                try
                {
                    GameQueueConfig[] configArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("matchmakerService", "getAvailableQueues", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AD;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetAvailableQueues>d__90>(ref awaiter, ref this);
                    return;
                Label_00AD:
                    configArray2 = new GameQueueConfig[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        configArray2[i] = new GameQueueConfig((TypedObject) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i]);
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    configArray = configArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(configArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetChallengerLeague>d__107 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<LeagueListDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string queueType;

            private void MoveNext()
            {
                LeagueListDTO tdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.queueType };
                        this.<Id>5__1 = this.<>4__this.Invoke("leaguesServiceProxy", "getChallengerLeague", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetChallengerLeague>d__107>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    tdto = new LeagueListDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(tdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetChampionsForBan>d__191 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<ChampionBanInfoDTO[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                ChampionBanInfoDTO[] odtoArray;
                int num = this.<>1__state;
                try
                {
                    ChampionBanInfoDTO[] odtoArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "getChampionsForBan", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AD;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetChampionsForBan>d__191>(ref awaiter, ref this);
                    return;
                Label_00AD:
                    odtoArray2 = new ChampionBanInfoDTO[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        odtoArray2[i] = new ChampionBanInfoDTO((TypedObject) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i]);
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    odtoArray = odtoArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(odtoArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetLatestGameTimerState>d__172 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<GameDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double arg0;
            public string arg1;
            public int arg2;

            private void MoveNext()
            {
                GameDTO edto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0, this.arg1, this.arg2 };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "getLatestGameTimerState", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_009E;
                    }
                Label_0070:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00CF;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00AF;
                    }
                Label_009E:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0070;
                Label_00AF:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetLatestGameTimerState>d__172>(ref awaiter, ref this);
                    return;
                Label_00CF:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    edto = new GameDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(edto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetLeaguesForTeam>d__129 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SummonerLeaguesDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string teamName;

            private void MoveNext()
            {
                SummonerLeaguesDTO sdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.teamName };
                        this.<Id>5__1 = this.<>4__this.Invoke("leaguesServiceProxy", "getLeaguesForTeam", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetLeaguesForTeam>d__129>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    sdto = new SummonerLeaguesDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(sdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetLoginDataPacketForUser>d__89 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<LoginDataPacket> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                LoginDataPacket packet;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("clientFacadeService", "getLoginDataPacketForUser", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetLoginDataPacketForUser>d__89>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    packet = new LoginDataPacket(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(packet);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetMasteryBook>d__101 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<MasteryBookDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;

            private void MoveNext()
            {
                MasteryBookDTO kdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId };
                        this.<Id>5__1 = this.<>4__this.Invoke("masteryBookService", "getMasteryBook", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetMasteryBook>d__101>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    kdto = new MasteryBookDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(kdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetMyLeaguePositions>d__98 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SummonerLeagueItemsDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                SummonerLeagueItemsDTO sdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("leaguesServiceProxy", "getMyLeaguePositions", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetMyLeaguePositions>d__98>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    sdto = new SummonerLeagueItemsDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(sdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetPointsBalance>d__113 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PointSummary> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                PointSummary summary;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("lcdsRerollService", "getPointsBalance", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetPointsBalance>d__113>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    summary = new PointSummary(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(summary);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetRecentGames>d__125 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<RecentGames> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;

            private void MoveNext()
            {
                RecentGames games;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.accountId };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerStatsService", "getRecentGames", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetRecentGames>d__125>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    games = new RecentGames(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(games);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSpellBook>d__149 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SpellBookDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;

            private void MoveNext()
            {
                SpellBookDTO kdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId };
                        this.<Id>5__1 = this.<>4__this.Invoke("spellBookService", "getSpellBook", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSpellBook>d__149>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    kdto = new SpellBookDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(kdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetStoreUrl>d__153 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<string> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                string str;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("loginService", "getStoreUrl", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetStoreUrl>d__153>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    str = (string) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(str);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSummonerByName>d__121 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PublicSummoner> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string summonerName;

            private void MoveNext()
            {
                PublicSummoner summoner;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerName };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "getSummonerByName", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSummonerByName>d__121>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    summoner = new PublicSummoner(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(summoner);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSummonerIcons>d__114 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<string> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double[] summonerIds;

            private void MoveNext()
            {
                string str;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerIds.Cast<object>().ToArray<object>() };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "getSummonerIcons", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_008C;
                    }
                Label_005E:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00BD;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_009D;
                    }
                Label_008C:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_005E;
                Label_009D:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSummonerIcons>d__114>(ref awaiter, ref this);
                    return;
                Label_00BD:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    str = (string) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(str);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSummonerInternalNameByName>d__159 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<string> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string summonerName;

            private void MoveNext()
            {
                string str;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerName };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "getSummonerInternalNameByName", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSummonerInternalNameByName>d__159>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    str = (string) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(str);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSummonerNames>d__105 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<string[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double[] summonerIds;

            private void MoveNext()
            {
                string[] strArray;
                int num = this.<>1__state;
                try
                {
                    string[] strArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerIds.Cast<object>().ToArray<object>() };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "getSummonerNames", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_008C;
                    }
                Label_005E:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C0;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_009D;
                    }
                Label_008C:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_005E;
                Label_009D:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSummonerNames>d__105>(ref awaiter, ref this);
                    return;
                Label_00C0:
                    strArray2 = new string[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        strArray2[i] = (string) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i];
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    strArray = strArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(strArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSummonerRuneInventory>d__95 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SummonerRuneInventory> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;

            private void MoveNext()
            {
                SummonerRuneInventory inventory;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerRuneService", "getSummonerRuneInventory", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSummonerRuneInventory>d__95>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    inventory = new SummonerRuneInventory(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(inventory);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetSumonerActiveBoosts>d__92 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SummonerActiveBoostsDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                SummonerActiveBoostsDTO sdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("inventoryService", "getSumonerActiveBoosts", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetSumonerActiveBoosts>d__92>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    sdto = new SummonerActiveBoostsDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(sdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetTeamAggregatedStats>d__130 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<TeamAggregatedStatsDTO[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public TeamId arg0;

            private void MoveNext()
            {
                TeamAggregatedStatsDTO[] sdtoArray;
                int num = this.<>1__state;
                try
                {
                    TeamAggregatedStatsDTO[] sdtoArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerStatsService", "getTeamAggregatedStats", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00BB;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetTeamAggregatedStats>d__130>(ref awaiter, ref this);
                    return;
                Label_00BB:
                    sdtoArray2 = new TeamAggregatedStatsDTO[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        sdtoArray2[i] = new TeamAggregatedStatsDTO((TypedObject) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i]);
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    sdtoArray = sdtoArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(sdtoArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <GetTeamEndOfGameStats>d__132 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<EndOfGameStats> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public TeamId arg0;
            public double arg1;

            private void MoveNext()
            {
                EndOfGameStats stats;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0.GetBaseTypedObject(), this.arg1 };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerStatsService", "getTeamEndOfGameStats", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<GetTeamEndOfGameStats>d__132>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    stats = new EndOfGameStats(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(stats);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <InvitePlayer>d__139 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<TeamDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;
            public TeamId teamId;

            private void MoveNext()
            {
                TeamDTO mdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId, this.teamId.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "invitePlayer", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<InvitePlayer>d__139>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    mdto = new TeamDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(mdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <IsNameValidAndAvailable>d__134 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string teamName;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.teamName };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "isNameValidAndAvailable", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<IsNameValidAndAvailable>d__134>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <IsTagValidAndAvailable>d__135 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string tagName;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.tagName };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "isTagValidAndAvailable", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<IsTagValidAndAvailable>d__135>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <JoinGame>d__155 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[2];
                        body[0] = this.gameId;
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "joinGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<JoinGame>d__155>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <JoinGame>d__156 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;
            public string password;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.password };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "joinGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0090;
                    }
                Label_0062:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C1;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A1;
                    }
                Label_0090:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0062;
                Label_00A1:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<JoinGame>d__156>(ref awaiter, ref this);
                    return;
                Label_00C1:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <KickPlayer>d__141 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<TeamDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double summonerId;
            public TeamId teamId;

            private void MoveNext()
            {
                TeamDTO mdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerId, this.teamId.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerTeamService", "kickPlayer", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<KickPlayer>d__141>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    mdto = new TeamDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(mdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <ListAllPracticeGames>d__154 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PracticeGameSearchResult[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                PracticeGameSearchResult[] resultArray;
                int num = this.<>1__state;
                try
                {
                    PracticeGameSearchResult[] resultArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "listAllPracticeGames", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AD;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<ListAllPracticeGames>d__154>(ref awaiter, ref this);
                    return;
                Label_00AD:
                    resultArray2 = new PracticeGameSearchResult[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        resultArray2[i] = new PracticeGameSearchResult((TypedObject) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i]);
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    resultArray = resultArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(resultArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <LoadPreferencesByKey>d__99 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string arg0;
            public double arg1;
            public bool arg2;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0, this.arg1, this.arg2 };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerPreferencesService", "loadPreferencesByKey", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_009E;
                    }
                Label_0070:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00CF;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00AF;
                    }
                Label_009E:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0070;
                Label_00AF:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<LoadPreferencesByKey>d__99>(ref awaiter, ref this);
                    return;
                Label_00CF:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <Login>d__84 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<Session> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public AuthenticationCredentials arg0;

            private void MoveNext()
            {
                Session session;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("loginService", "login", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<Login>d__84>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    session = new Session(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(session);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <ObserveGame>d__157 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[2];
                        body[0] = this.gameId;
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "observeGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<ObserveGame>d__157>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <ObserveGame>d__158 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;
            public string password;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.password };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "observeGame", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0090;
                    }
                Label_0062:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C1;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A1;
                    }
                Label_0090:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0062;
                Label_00A1:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<ObserveGame>d__158>(ref awaiter, ref this);
                    return;
                Label_00C1:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <PerformLCDSHeartBeat>d__96 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<string> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int arg0;
            public string arg1;
            public int arg2;
            public string arg3;

            private void MoveNext()
            {
                string str;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0, this.arg1, this.arg2, this.arg3 };
                        this.<Id>5__1 = this.<>4__this.Invoke("loginService", "performLCDSHeartBeat", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_00A7;
                    }
                Label_0079:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00D8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00B8;
                    }
                Label_00A7:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0079;
                Label_00B8:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<PerformLCDSHeartBeat>d__96>(ref awaiter, ref this);
                    return;
                Label_00D8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    str = (string) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(str);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <QuitGame>d__163 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "quitGame", new object[0]);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0079;
                    }
                Label_004B:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00AA;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_008A;
                    }
                Label_0079:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_004B;
                Label_008A:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<QuitGame>d__163>(ref awaiter, ref this);
                    return;
                Label_00AA:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <RemoveBotChampion>d__167 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int arg0;
            public BotParticipant arg1;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0, this.arg1.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "removeBotChampion", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<RemoveBotChampion>d__167>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <RetrieveInProgressSpectatorGameInfo>d__182 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PlatformGameLifecycleDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string summonerName;

            private void MoveNext()
            {
                PlatformGameLifecycleDTO edto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.summonerName };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "retrieveInProgressSpectatorGameInfo", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0082;
                    }
                Label_0054:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B3;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0093;
                    }
                Label_0082:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0054;
                Label_0093:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<RetrieveInProgressSpectatorGameInfo>d__182>(ref awaiter, ref this);
                    return;
                Label_00B3:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    edto = new PlatformGameLifecycleDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(edto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <RetrievePlayerStatsByAccountId>d__118 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<PlayerLifetimeStats> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;
            public string season;

            private void MoveNext()
            {
                PlayerLifetimeStats stats;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.accountId, this.season };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerStatsService", "retrievePlayerStatsByAccountId", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0090;
                    }
                Label_0062:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C1;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A1;
                    }
                Label_0090:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0062;
                Label_00A1:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<RetrievePlayerStatsByAccountId>d__118>(ref awaiter, ref this);
                    return;
                Label_00C1:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    stats = new PlayerLifetimeStats(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(stats);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <RetrieveTopPlayedChampions>d__119 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<ChampionStatInfo[]> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double accountId;
            public string gameMode;

            private void MoveNext()
            {
                ChampionStatInfo[] infoArray;
                int num = this.<>1__state;
                try
                {
                    ChampionStatInfo[] infoArray2;
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.accountId, this.gameMode };
                        this.<Id>5__1 = this.<>4__this.Invoke("playerStatsService", "retrieveTopPlayedChampions", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0090;
                    }
                Label_0062:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C4;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A1;
                    }
                Label_0090:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0062;
                Label_00A1:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<RetrieveTopPlayedChampions>d__119>(ref awaiter, ref this);
                    return;
                Label_00C4:
                    infoArray2 = new ChampionStatInfo[this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length];
                    for (int i = 0; i < this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body").Length; i++)
                    {
                        infoArray2[i] = new ChampionStatInfo((TypedObject) this.<>4__this.results[this.<Id>5__1].GetTO("data").GetArray("body")[i]);
                    }
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    infoArray = infoArray2;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(infoArray);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SelectBotChampion>d__166 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int arg0;
            public BotParticipant arg1;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0, this.arg1.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "selectBotChampion", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SelectBotChampion>d__166>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SelectChampion>d__176 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int championId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.championId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "selectChampion", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SelectChampion>d__176>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SelectChampionSkin>d__177 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int championId;
            public int skinId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.championId, this.skinId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "selectChampionSkin", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SelectChampionSkin>d__177>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SelectDefaultSpellBookPage>d__175 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<SpellBookPageDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public SpellBookPageDTO spellBookPage;

            private void MoveNext()
            {
                SpellBookPageDTO edto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.spellBookPage.GetBaseTypedObject() };
                        this.<Id>5__1 = this.<>4__this.Invoke("spellBookService", "selectDefaultSpellBookPage", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SelectDefaultSpellBookPage>d__175>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    edto = new SpellBookPageDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(edto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SelectSpells>d__173 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int spellOneId;
            public int spellTwoId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.spellOneId, this.spellTwoId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "selectSpells", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SelectSpells>d__173>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SetClientReceivedGameMessage>d__170 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string arg1;
            public double gameId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.arg1 };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "setClientReceivedGameMessage", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0090;
                    }
                Label_0062:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C1;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A1;
                    }
                Label_0090:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0062;
                Label_00A1:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SetClientReceivedGameMessage>d__170>(ref awaiter, ref this);
                    return;
                Label_00C1:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SetClientReceivedMaestroMessage>d__179 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double arg0;
            public string arg1;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.arg0, this.arg1 };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "setClientReceivedMaestroMessage", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0090;
                    }
                Label_0062:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C1;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A1;
                    }
                Label_0090:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0062;
                Label_00A1:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SetClientReceivedMaestroMessage>d__179>(ref awaiter, ref this);
                    return;
                Label_00C1:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <StartChampionSelection>d__169 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<StartChampSelectDTO> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;
            public double optomisticLock;

            private void MoveNext()
            {
                StartChampSelectDTO tdto;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.optomisticLock };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "startChampionSelection", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<StartChampionSelection>d__169>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    tdto = new StartChampSelectDTO(this.<>4__this.results[this.<Id>5__1].GetTO("data").GetTO("body"));
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(tdto);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <Subscribe>d__87 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public string service;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        TypedObject packet = this.<>4__this.WrapBody(new TypedObject(), "messagingDestination", 0);
                        packet.type = "flex.messaging.messages.CommandMessage";
                        TypedObject tO = packet.GetTO("headers");
                        if (this.service == "bc")
                        {
                            tO.Add("DSSubtopic", "bc");
                        }
                        else
                        {
                            tO.Add("DSSubtopic", this.service + "-" + this.<>4__this.accountID);
                        }
                        tO.Remove("DSRequestTimeout");
                        packet["clientId"] = this.service + "-" + this.<>4__this.accountID;
                        this.<Id>5__1 = this.<>4__this.Invoke(packet);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0128;
                    }
                Label_00F9:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_015A;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0139;
                    }
                Label_0128:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_00F9;
                Label_0139:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<Subscribe>d__87>(ref awaiter, ref this);
                    return;
                Label_015A:
                    this.<>4__this.GetResult(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SwitchObserverToPlayer>d__162 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;
            public int team;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId, this.team };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "switchObserverToPlayer", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0095;
                    }
                Label_0067:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00C6;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_00A6;
                    }
                Label_0095:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0067;
                Label_00A6:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SwitchObserverToPlayer>d__162>(ref awaiter, ref this);
                    return;
                Label_00C6:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SwitchPlayerToObserver>d__161 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "switchPlayerToObserver", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SwitchPlayerToObserver>d__161>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <SwitchTeams>d__160 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<bool> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public double gameId;

            private void MoveNext()
            {
                bool flag;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.gameId };
                        this.<Id>5__1 = this.<>4__this.Invoke("gameService", "switchTeams", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<SwitchTeams>d__160>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    flag = (bool) this.<>4__this.results[this.<Id>5__1].GetTO("data")["body"];
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(flag);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        [CompilerGenerated]
        private struct <UpdateProfileIconId>d__187 : IAsyncStateMachine
        {
            public int <>1__state;
            public LoLConnection <>4__this;
            public AsyncTaskMethodBuilder<object> <>t__builder;
            private TaskAwaiter <>u__1;
            private int <Id>5__1;
            public int iconId;

            private void MoveNext()
            {
                object obj2;
                int num = this.<>1__state;
                try
                {
                    TaskAwaiter awaiter;
                    if (num != 0)
                    {
                        object[] body = new object[] { this.iconId };
                        this.<Id>5__1 = this.<>4__this.Invoke("summonerService", "updateProfileIconId", body);
                    }
                    else
                    {
                        awaiter = this.<>u__1;
                        this.<>u__1 = new TaskAwaiter();
                        this.<>1__state = num = -1;
                        goto Label_0087;
                    }
                Label_0059:
                    if (this.<>4__this.results.ContainsKey(this.<Id>5__1))
                    {
                        goto Label_00B8;
                    }
                    awaiter = Task.Delay(10).GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        goto Label_0098;
                    }
                Label_0087:
                    awaiter.GetResult();
                    awaiter = new TaskAwaiter();
                    goto Label_0059;
                Label_0098:
                    this.<>1__state = num = 0;
                    this.<>u__1 = awaiter;
                    this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LoLConnection.<UpdateProfileIconId>d__187>(ref awaiter, ref this);
                    return;
                Label_00B8:
                    this.<>4__this.results.Remove(this.<Id>5__1);
                    obj2 = null;
                }
                catch (Exception exception)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception);
                    return;
                }
                this.<>1__state = -2;
                this.<>t__builder.SetResult(obj2);
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        public delegate void OnConnectHandler(object sender, EventArgs e);

        public delegate void OnDisconnectHandler(object sender, EventArgs e);

        public delegate void OnErrorHandler(object sender, LoLLauncher.Error error);

        public delegate void OnLoginHandler(object sender, string username, string ipAddress);

        public delegate void OnLoginQueueUpdateHandler(object sender, int positionInLine);

        public delegate void OnMessageReceivedHandler(object sender, object message);
    }
}

