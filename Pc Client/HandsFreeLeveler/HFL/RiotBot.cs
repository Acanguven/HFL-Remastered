﻿
using LoLLauncher;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Game.Message;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects;
using LoLLauncher.RiotObjects.Leagues.Pojo;
using LoLLauncher.RiotObjects.Platform.Game.Practice;
using LoLLauncher.RiotObjects.Platform.Harassment;
using LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto;
using LoLLauncher.RiotObjects.Platform.Login;
using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
using LoLLauncher.RiotObjects.Platform.Statistics.Team;
using LoLLauncher.RiotObjects.Platform.Summoner;
using LoLLauncher.RiotObjects.Platform.Summoner.Boost;
using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
using LoLLauncher.RiotObjects.Platform.Summoner.Runes;
using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
using LoLLauncher.RiotObjects.Team;
using LoLLauncher.RiotObjects.Team.Dto;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using LoLLauncher.RiotObjects.Platform.Game.Map;
using System.Net;
using System.Net.Http;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using LoLLauncher.RiotObjects.Platform.Summoner.Icon;
using System.Windows.Interop;
using LoLLauncher.RiotObjects.Platform.Catalog.Icon;
using System.Timers;

namespace HandsFreeLeveler
{
    public class RiotBot
    {
        public LoginDataPacket loginPacket = new LoginDataPacket();
        public GameDTO currentGame = new GameDTO();
        public LoLConnection connection = new LoLConnection();
        public List<ChampionDTO> availableChamps = new List<ChampionDTO>();
        public LoLLauncher.RiotObjects.Platform.Catalog.Champion.ChampionDTO[] availableChampsArray;
        public bool firstTimeInLobby = true;
        public bool firstTimeInQueuePop = true;
        public bool firstTimeInCustom = true;
        public Process exeProcess;
        public string ipath;
        public string Accountname;
        public string Password;
        public double AccMaxLevel { get; set; }
        public int m_leaverBustedPenalty { get; set; }
        public string m_accessToken { get; set; }
        public double sumLevel { get; set; }
        public double archiveSumLevel { get; set; }
        public double rpBalance { get; set; }
        public QueueTypes queueType { get; set; }
        public QueueTypes actualQueueType { get; set; }
        public string region { get; set; }
        public string regionURL;
        public string clientMask { get; set; }
        public bool QueueFlag;
        public int LastAntiBusterAttempt = 0;
        public bool stopForced = false;
        public Smurf Owner { get; set; }
        public Thread processStarter { get; set; }


        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr Handle, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public RiotBot(string username, string password, int accmaxlevel, string reg, string path, QueueTypes QueueType, string cMask, Smurf own)
        {
            ipath = path;
            Accountname = username;
            Password = password;
            clientMask = cMask;
            AccMaxLevel = Convert.ToInt32(accmaxlevel);
            queueType = QueueType;
            region = reg;
            Owner = own;

            this.updateStatus("Trying to login...", Accountname);

            connection.OnError += new LoLConnection.OnErrorHandler(this.connection_OnError);
            connection.OnLogin += new LoLConnection.OnLoginHandler(this.connection_OnLogin);
            connection.OnLoginQueueUpdate += new LoLConnection.OnLoginQueueUpdateHandler(this.connection_OnLoginQueueUpdate);
            connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(this.connection_OnMessageReceived);

            init(region, username, password);
        }

        public async void init(String region, String username, String password)
        {
            if (stopForced)
            {
                return;
            }
            switch (region)
            {
                case "EUW":
                    connection.Connect(username, password, LoLLauncher.Region.EUW, clientMask);
                    break;
                case "EUNE":
                    connection.Connect(username, password, LoLLauncher.Region.EUN, clientMask);
                    break;
                case "NA":
                    connection.Connect(username, password, LoLLauncher.Region.NA, clientMask);
                    regionURL = "NA1";
                    break;
                case "KR":
                    connection.Connect(username, password, LoLLauncher.Region.KR, clientMask);
                    break;
                case "BR":
                    connection.Connect(username, password, LoLLauncher.Region.BR, clientMask);
                    break;
                case "OCE":
                    connection.Connect(username, password, LoLLauncher.Region.OCE, clientMask);
                    break;
                case "RU":
                    connection.Connect(username, password, LoLLauncher.Region.RU, clientMask);
                    break;
                case "TR":
                    connection.Connect(username, password, LoLLauncher.Region.TR, clientMask);
                    break;
                case "LAS":
                    connection.Connect(username, password, LoLLauncher.Region.LAS, clientMask);
                    break;
                case "LAN":
                    connection.Connect(username, password, LoLLauncher.Region.LAN, clientMask);
                    break;
                case "TH":
                    connection.Connect(username, password, LoLLauncher.Region.TH, clientMask);
                    break;
                case "SGMY":
                    connection.Connect(username, password, LoLLauncher.Region.SGMY, clientMask);
                    break;
                case "TW":
                    connection.Connect(username, password, LoLLauncher.Region.TW, clientMask);
                    break;
                case "PH":
                    connection.Connect(username, password, LoLLauncher.Region.PH, clientMask);
                    break;
                case "VN":
                    connection.Connect(username, password, LoLLauncher.Region.VN, clientMask);
                    break;
                case "ID":
                    connection.Connect(username, password, LoLLauncher.Region.ID, clientMask);
                    break;
            }
        }

        public async void AntiBuster(MatchMakerParams matchParams)
        {
            //Thx to mah niggah Everance
            //who made this possible

            if (QueueFlag)
            {
                Console.Out.WriteLine("Something wrong with smurf list");
                connection.Disconnect();
            }
            else
            {
                this.updateStatus("You are leaver busted.", Accountname);
                connection.Disconnect();
            }
        }

        public async void connection_OnMessageReceived(object sender, object message)
        {
            if (message is GameDTO)
            {
                GameDTO game = message as GameDTO;
                switch (game.GameState)
                {
                    case "TEAM_SELECT":
                        int totalPlayers = game.TeamOne.Count + game.TeamTwo.Count;
                        this.updateStatus("In custom lobby, playerCount:" + totalPlayers, Accountname);
                        if (totalPlayers == 6 && game.OwnerSummary.AccountId == this.connection.AccountID())
                        {
                            await connection.StartChampionSelection(game.Id, game.OptimisticLock);
                        }
                        break;
                    case "CHAMP_SELECT":
                        if (this.firstTimeInLobby)
                        {
                            QueueFlag = true;
                            firstTimeInLobby = false;
                            updateStatus("In Champion Selection", Accountname);
                            object obj = await connection.SetClientReceivedGameMessage(game.Id, "CHAMP_SELECT_CLIENT");
                            if (queueType != QueueTypes.ARAM)
                            {
                                if (Settings.championId != "" && Settings.championId != "RANDOM")
                                {

                                    int Spell1;
                                    int Spell2;
                                    if (!Settings.rndSpell)
                                    {
                                        Spell1 = Enums.spellToId(Settings.spell1);
                                        Spell2 = Enums.spellToId(Settings.spell2);
                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                                        int index = random.Next(spellList.Count);
                                        int index2 = random.Next(spellList.Count);

                                        int randomSpell1 = spellList[index];
                                        int randomSpell2 = spellList[index2];

                                        if (randomSpell1 == randomSpell2)
                                        {
                                            int index3 = random.Next(spellList.Count);
                                            randomSpell2 = spellList[index3];
                                        }

                                        Spell1 = Convert.ToInt32(randomSpell1);
                                        Spell2 = Convert.ToInt32(randomSpell2);
                                    }

                                    await connection.SelectSpells(Spell1, Spell2);

                                    await connection.SelectChampion(Enums.championToId(Settings.championId));
                                    await connection.ChampionSelectCompleted();
                                }
                                else if (Settings.championId == "RANDOM")
                                {

                                    int Spell1;
                                    int Spell2;
                                    if (!Settings.rndSpell)
                                    {
                                        Spell1 = Enums.spellToId(Settings.spell1);
                                        Spell2 = Enums.spellToId(Settings.spell2);
                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                                        int index = random.Next(spellList.Count);
                                        int index2 = random.Next(spellList.Count);

                                        int randomSpell1 = spellList[index];
                                        int randomSpell2 = spellList[index2];

                                        if (randomSpell1 == randomSpell2)
                                        {
                                            int index3 = random.Next(spellList.Count);
                                            randomSpell2 = spellList[index3];
                                        }

                                        Spell1 = Convert.ToInt32(randomSpell1);
                                        Spell2 = Convert.ToInt32(randomSpell2);
                                    }

                                    await connection.SelectSpells(Spell1, Spell2);

                                    var randAvailableChampsArray = availableChampsArray.Shuffle();
                                    int randomAdc = randAvailableChampsArray.First(champ => (champ.Owned || champ.FreeToPlay) && (champ.ChampionId == 22 || champ.ChampionId == 51 || champ.ChampionId == 42 || champ.ChampionId == 119 || champ.ChampionId == 81 || champ.ChampionId == 104 || champ.ChampionId == 222 || champ.ChampionId == 429 || champ.ChampionId == 96 || champ.ChampionId == 236 || champ.ChampionId == 21 || champ.ChampionId == 133 || champ.ChampionId == 15 || champ.ChampionId == 18 || champ.ChampionId == 29 || champ.ChampionId == 110 || champ.ChampionId == 67)).ChampionId;
                                    await connection.SelectChampion(randomAdc);


                                    await connection.ChampionSelectCompleted();

                                }
                                else
                                {

                                    int Spell1;
                                    int Spell2;
                                    if (!Settings.rndSpell)
                                    {
                                        Spell1 = Enums.spellToId(Settings.spell1);
                                        Spell2 = Enums.spellToId(Settings.spell2);
                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                                        int index = random.Next(spellList.Count);
                                        int index2 = random.Next(spellList.Count);

                                        int randomSpell1 = spellList[index];
                                        int randomSpell2 = spellList[index2];

                                        if (randomSpell1 == randomSpell2)
                                        {
                                            int index3 = random.Next(spellList.Count);
                                            randomSpell2 = spellList[index3];
                                        }

                                        Spell1 = Convert.ToInt32(randomSpell1);
                                        Spell2 = Convert.ToInt32(randomSpell2);
                                    }

                                    await connection.SelectSpells(Spell1, Spell2);

                                    var randAvailableChampsArray = availableChampsArray.Shuffle();
                                    int randomAdc = randAvailableChampsArray.First(champ => (champ.Owned || champ.FreeToPlay) && (champ.ChampionId == 22 || champ.ChampionId == 51 || champ.ChampionId == 42 || champ.ChampionId == 119 || champ.ChampionId == 81 || champ.ChampionId == 104 || champ.ChampionId == 222 || champ.ChampionId == 429 || champ.ChampionId == 96 || champ.ChampionId == 236 || champ.ChampionId == 21 || champ.ChampionId == 133 || champ.ChampionId == 15 || champ.ChampionId == 18 || champ.ChampionId == 29 || champ.ChampionId == 110 || champ.ChampionId == 67)).ChampionId;

                                    await connection.SelectChampion(randomAdc);

                                    await connection.ChampionSelectCompleted();
                                }
                            }
                            break;
                        }
                        else
                            break;
                    case "POST_CHAMP_SELECT":
                        firstTimeInLobby = false;
                        this.updateStatus("Last 10 seconds to start game", Accountname);
                        break;
                    case "PRE_CHAMP_SELECT":
                        this.updateStatus("Last seconds to set champion", Accountname);
                        break;
                    case "GAME_START_CLIENT":
                        this.updateStatus("In Game", Accountname);
                        break;
                    case "GameClientConnectedToServer":
                        this.updateStatus("Connected to server", Accountname);
                        break;
                    case "IN_QUEUE":
                        this.updateStatus("In Queue", Accountname);
                        QueueFlag = true;
                        break;
                    case "TERMINATED":
                        this.updateStatus("Re entering to queue", Accountname);
                        this.firstTimeInQueuePop = true;
                        if (queueType == QueueTypes.CUSTOM)
                        {
                            CreatePracticeGame();
                        }
                        break;
                    case "JOINING_CHAMP_SELECT":
                        if (this.firstTimeInQueuePop && game.StatusOfParticipants.Contains("1"))
                        {
                            this.updateStatus("Game accepted", Accountname);
                            this.firstTimeInQueuePop = false;
                            this.firstTimeInLobby = true;
                            object obj = await this.connection.AcceptPoppedGame(true);
                            break;
                        }
                        else
                            break;
                    case "LEAVER_BUSTED":
                        this.updateStatus("You are leaver busted", Accountname);
                        break;
                }
            }
            else if (message is PlayerCredentialsDto)
            {
                string str = Enumerable.Last<string>((IEnumerable<string>)Enumerable.OrderBy<string, DateTime>(Directory.EnumerateDirectories((this.ipath ?? "").Split(new string[] { "lol.launcher.exe" }, StringSplitOptions.None)[0] + "RADS\\solutions\\lol_game_client_sln\\releases\\"), (Func<string, DateTime>)(f => new DirectoryInfo(f).CreationTime))) + "\\deploy\\";
                LoLLauncher.RiotObjects.Platform.Game.PlayerCredentialsDto credentials = message as PlayerCredentialsDto;
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.WorkingDirectory = str;
                if (Settings.disableGpu)
                {
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
                startInfo.FileName = "League of Legends.exe";
                startInfo.Arguments = string.Concat(new object[] { "\"8394\" \"LoLLauncher.exe\" \"\" \"", credentials.ServerIp, " ", credentials.ServerPort, " ", credentials.EncryptionKey, " ", credentials.SummonerId, "\"" });
                updateStatus("Starting Game", Accountname);
                startProcessor(startInfo);
            }
            else if (!(message is GameNotification) && !(message is SearchingForMatchNotification))
            {

                if (message is EndOfGameStats)
                {
                    EndOfGameStats eog = message as EndOfGameStats;
                    this.joinQueue();
                }
                else
                {

                    if (message.ToString().Contains("EndOfGameStats"))
                    {
                        updateStatus("Game ending, calculating results", Accountname);
                        EndOfGameStats eog = new EndOfGameStats();
                        connection_OnMessageReceived(sender, eog);
                        exeProcess.Exited -= exeProcess_Exited;
                        exeProcess.Kill();
                        Thread.Sleep(500);
                        if (exeProcess.Responding)
                        {
                            Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                        }
                        loginPacket = await this.connection.GetLoginDataPacketForUser();
                        archiveSumLevel = sumLevel;
                        sumLevel = loginPacket.AllSummonerData.SummonerLevel.Level;
                        Owner.level = (int)sumLevel;
                        if (sumLevel != archiveSumLevel)
                        {
                            levelUp();
                        }
                    }
                }
            }
        }

        public void startProcessor(ProcessStartInfo startInfo)
        {
            processStarter = new Thread((ThreadStart)(() =>
            {
                exeProcess = Process.Start(startInfo);
                exeProcess.Exited += exeProcess_Exited;
                while (exeProcess.MainWindowHandle == IntPtr.Zero) ;
                exeProcess.PriorityClass = ProcessPriorityClass.Idle;
                exeProcess.EnableRaisingEvents = true;
                if (Settings.disableGpu)
                {
                    App.gameContainer.Dispatcher.Invoke(new Action(() => {
                        App.gameContainer.addWindow(exeProcess, Accountname);
                    }), DispatcherPriority.ContextIdle);
                }
                Thread.Sleep(3000);
                if (Settings.mInject)
                {
                    BasicInject.Inject(exeProcess, Settings.dllPath);
                }
            }));
            processStarter.Start();
        }

        public async void joinQueue()
        {
            if (queueType == QueueTypes.CUSTOM)
            {
                CreatePracticeGame();
            }
            else
            {
                LoLLauncher.RiotObjects.Platform.Matchmaking.MatchMakerParams matchParams = new LoLLauncher.RiotObjects.Platform.Matchmaking.MatchMakerParams();
                if (queueType == QueueTypes.INTRO_BOT)
                {
                    matchParams.BotDifficulty = "INTRO";
                }
                else if (queueType == QueueTypes.BEGINNER_BOT)
                {
                    matchParams.BotDifficulty = "EASY";
                }
                else if (queueType == QueueTypes.MEDIUM_BOT)
                {
                    matchParams.BotDifficulty = "MEDIUM";
                }

                if (sumLevel == 3 && actualQueueType == QueueTypes.NORMAL_5x5)
                {
                    queueType = actualQueueType;
                }
                else if (sumLevel == 6 && actualQueueType == QueueTypes.ARAM)
                {
                    queueType = actualQueueType;
                }
                else if (sumLevel == 7 && actualQueueType == QueueTypes.NORMAL_3x3)
                {
                    queueType = actualQueueType;
                }

                matchParams.QueueIds = new Int32[1] { (int)queueType };

                LoLLauncher.RiotObjects.Platform.Matchmaking.SearchingForMatchNotification m = await connection.AttachToQueue(matchParams);
                this.updateStatus("Trying to join queue", Accountname);
                if (m.PlayerJoinFailures == null)
                {
                    this.updateStatus("In queue for " + queueType.ToString(), Accountname);
                }
                else
                {
                    List<QueueDodger>.Enumerator enumerator = m.PlayerJoinFailures.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            QueueDodger current = enumerator.Current;
                            if (current.ReasonFailed == "LEAVER_BUSTED")
                            {
                                this.m_accessToken = current.AccessToken;
                                if (current.LeaverPenaltyMillisRemaining > this.m_leaverBustedPenalty)
                                {
                                    this.m_leaverBustedPenalty = current.LeaverPenaltyMillisRemaining;
                                }
                            }
                        }
                    }
                    finally
                    {
                        enumerator.Dispose();
                    }
                    if (string.IsNullOrEmpty(this.m_accessToken))
                    {
                        List<QueueDodger>.Enumerator enumerator2 = m.PlayerJoinFailures.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                QueueDodger dodger2 = enumerator2.Current;
                                this.updateStatus("Dodge Remaining Time: " + Convert.ToString((float)(((float)(dodger2.DodgePenaltyRemainingTime / 0x3e8)) / 60f)).Replace(",", ":") + "...", Accountname);

                                if (dodger2.DodgePenaltyRemainingTime == 0 || dodger2.LeaverPenaltyMillisRemaining == 0)
                                {
                                    this.updateStatus("You need login to your account using the game client and type 'I accept' to use HFL", Accountname);
                                    connection.Disconnect();
                                }
                            }
                            return;
                        }
                        finally
                        {
                            enumerator2.Dispose();
                        }
                    }
                    double minutes = ((float)(this.m_leaverBustedPenalty / 0x3e8)) / 60f;
                    this.updateStatus("Waiting out leaver buster: " + minutes + " minutes!", Accountname);
                    Thread.Sleep(TimeSpan.FromMilliseconds((double)this.m_leaverBustedPenalty));
                    m = await this.connection.AttachToLowPriorityQueue(matchParams, this.m_accessToken);
                    if (m.PlayerJoinFailures == null)
                    {
                        this.updateStatus("Succesfully joined lower priority queue!", Accountname);
                    }
                    else
                    {
                        this.updateStatus("There was an error in joining lower priority queue.Disconnecting...", Accountname);
                        this.connection.Disconnect();
                    }
                }
            }
        }

        async void CreatePracticeGame()
        {
            this.updateStatus("Looking for HFL games", Accountname);
            PracticeGameSearchResult[] gameList = await connection.ListAllPracticeGames();
            dynamic gameFound = gameList.FirstOrDefault(_ => (_.Name.Contains("thelawkings") && (_.Team1Count < 3 || _.Team2Count < 3)));
            if (gameFound != null)
            {
                this.updateStatus("Joining to custom game", Accountname);
                await connection.JoinGame(gameFound.Id, "hflthelawtheking");
            }
            else
            {
                this.updateStatus("Creating custom game", Accountname);
                LoLLauncher.RiotObjects.Platform.Game.PracticeGameConfig cfg = new LoLLauncher.RiotObjects.Platform.Game.PracticeGameConfig();
                cfg.GameName = "thelawkings" + new Random().Next().ToString();
                LoLLauncher.RiotObjects.Platform.Game.Map.GameMap map = new LoLLauncher.RiotObjects.Platform.Game.Map.GameMap();
                map.Description = "desc";
                map.DisplayName = "dummy";
                map.TotalPlayers = 2;
                map.Name = "dummy";
                map.MapId = (int)GameMode.TwistedTreeline;
                map.MinCustomPlayers = 1;
                cfg.GameMap = map;
                cfg.MaxNumPlayers = 6;
                cfg.GamePassword = "hflthelawtheking";
                cfg.GameTypeConfig = 1;
                cfg.AllowSpectators = "NONE";
                cfg.GameMode = StringEnum.GetStringValue(GameMode.TwistedTreeline);
                GameDTO game = await connection.CreatePracticeGame(cfg);
                if (game.Id == 0)
                {
                    CreatePracticeGame();
                }
                else
                {
                    this.updateStatus("Game (" + game.Id + ") created.", Accountname);
                }
            }
        }
        void exeProcess_Exited(object sender, EventArgs e)
        {
            if (Owner.thread != null)
            {
                updateStatus("Restarting game", Accountname);
                Thread.Sleep(1000);
                if (this.loginPacket.ReconnectInfo != null && this.loginPacket.ReconnectInfo.Game != null)
                {
                    this.connection_OnMessageReceived(sender, (object)this.loginPacket.ReconnectInfo.PlayerCredentials);
                }
                else
                {
                    this.connection_OnMessageReceived(sender, (object)new EndOfGameStats());
                }
            }
        }



        private void updateStatus(string status, string accname)
        {
            Owner.log(status);
        }

        private async void RegisterNotifications()
        {
            object obj1 = await this.connection.Subscribe("bc", this.connection.AccountID());
            object obj2 = await this.connection.Subscribe("cn", this.connection.AccountID());
            object obj3 = await this.connection.Subscribe("gn", this.connection.AccountID());
        }

        private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
        {
            if (positionInLine <= 0)
                return;
            this.updateStatus("Position to login: " + (object)positionInLine, Accountname);
        }

        private void connection_OnLogin(object sender, string username, string ipAddress)
        {
            new Thread((ThreadStart)(async () =>
            {
                updateStatus("Connecting to Riot servers", Accountname);
                this.RegisterNotifications();
                this.loginPacket = await this.connection.GetLoginDataPacketForUser();
                Owner.updateExpLevel(this.loginPacket.AllSummonerData.SummonerLevel.ExpToNextLevel, this.loginPacket.AllSummonerData.SummonerLevelAndPoints.ExpPoints);
                if (loginPacket.AllSummonerData == null)
                {
                    Random rnd = new Random();
                    String summonerName = Accountname;
                    if (summonerName.Length > 16)
                        summonerName = summonerName.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
                    LoLLauncher.RiotObjects.Platform.Summoner.AllSummonerData sumData = await connection.CreateDefaultSummoner(summonerName);
                    loginPacket.AllSummonerData = sumData;
                    updateStatus("Created Summonername " + summonerName, Accountname);
                }
                sumLevel = loginPacket.AllSummonerData.SummonerLevel.Level;
                Owner.level = (int)sumLevel;
                string sumName = loginPacket.AllSummonerData.Summoner.Name;
                double sumId = loginPacket.AllSummonerData.Summoner.SumId;
                rpBalance = loginPacket.RpBalance;
                if (sumLevel > AccMaxLevel || sumLevel == AccMaxLevel)
                {
                    connection.Disconnect();
                    updateStatus("Smurfing done with this account", Accountname);
                    return;
                }
                if (rpBalance == 400.0 && Settings.buyBoost)
                {
                    updateStatus("Buying XP Boost", Accountname);
                    try
                    {
                        Task t = new Task(buyBoost);
                        t.Start();
                    }
                    catch (Exception exception)
                    {
                        updateStatus("Couldn't buy RP Boost.\n" + exception, Accountname);
                    }
                }
                if (sumLevel < 3.0 && queueType == QueueTypes.NORMAL_5x5)
                {
                    this.updateStatus("Need to be Level 3 before NORMAL_5x5 queue.", Accountname);
                    this.updateStatus("Joins Co-Op vs AI (Beginner) queue until 3", Accountname);
                    queueType = QueueTypes.BEGINNER_BOT;
                    actualQueueType = QueueTypes.NORMAL_5x5;
                }
                else if (sumLevel < 6.0 && queueType == QueueTypes.ARAM)
                {

                    this.updateStatus("Need to be Level 6 before ARAM queue.", Accountname);
                    this.updateStatus("Joins Co-Op vs AI (Beginner) queue until 6", Accountname);
                    queueType = QueueTypes.INTRO_BOT;
                    actualQueueType = QueueTypes.ARAM;

                }
                else if (sumLevel < 7.0 && queueType == QueueTypes.NORMAL_3x3)
                {
                    this.updateStatus("Need to be Level 7 before NORMAL_3x3 queue.", Accountname);
                    this.updateStatus("Joins Co-Op vs AI (Beginner) queue until 7", Accountname);
                    queueType = QueueTypes.BEGINNER_BOT;
                    actualQueueType = QueueTypes.NORMAL_3x3;
                }
                /* Should be randomize the summonericon on every login,
                 * but only works with extra icons, so it crashes if you only got the standards.
                double[] ids = new double[Convert.ToInt32(sumId)];
                string icons = await connection.GetSummonerIcons(ids);
                List<int> availableIcons = new List<int> { };
                var random = new Random();
                foreach (var id in icons)
                {
                    availableIcons.Add(Convert.ToInt32(id));
                    Console.Out.WriteLine("[DEBUG]: Added Icon: " + id);
                }
                int index = random.Next(availableIcons.Count);
                Console.Out.WriteLine(" | Random Icon: " + index);
                int randomIcon = availableIcons[index];
                Console.Out.WriteLine(" | Choose from List: " + randomIcon);
                await connection.UpdateProfileIconId(randomIcon);*/
                updateStatus("Logged in to account level:" + loginPacket.AllSummonerData.SummonerLevel.Level, Accountname);
                availableChampsArray = await connection.GetAvailableChampions();

                var randAvailableChampsArray = availableChampsArray.Shuffle();

                LoLLauncher.RiotObjects.Team.Dto.PlayerDTO player = await connection.CreatePlayer();
                if (this.loginPacket.ReconnectInfo != null && this.loginPacket.ReconnectInfo.Game != null)
                {
                    this.connection_OnMessageReceived(sender, (object)this.loginPacket.ReconnectInfo.PlayerCredentials);
                }
                else
                {
                    this.connection_OnMessageReceived(sender, (object)new EndOfGameStats());
                }
            })).Start();
        }

        private void connection_OnError(object sender, LoLLauncher.Error error)
        {
            if (error.Message.Contains("is not owned by summoner"))
            {
                return;
            }
            else if (error.Message.Contains("Your summoner level is too low to select the spell"))
            {
                var random = new Random();
                var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                int index = random.Next(spellList.Count);
                int index2 = random.Next(spellList.Count);

                int randomSpell1 = spellList[index];
                int randomSpell2 = spellList[index2];

                if (randomSpell1 == randomSpell2)
                {
                    int index3 = random.Next(spellList.Count);
                    randomSpell2 = spellList[index3];
                }

                int Spell1 = Convert.ToInt32(randomSpell1);
                int Spell2 = Convert.ToInt32(randomSpell2);
                return;
            }
        }



        public void levelUp()
        {
            updateStatus("Congratz, levelup:" + sumLevel, Accountname);
            rpBalance = loginPacket.RpBalance;
            if (sumLevel >= AccMaxLevel)
            {
                connection.Disconnect();
            }
            else
            {
                this.joinQueue();
            }
            if (rpBalance == 400.0 && Settings.buyBoost)
            {
                updateStatus("Buying XP Boost", Accountname);
                try
                {
                    Task t = new Task(buyBoost);
                    t.Start();
                }
                catch (Exception exception)
                {
                    updateStatus("Couldn't buy RP Boost.\n" + exception, Accountname);
                }
            }

        }
        async void buyBoost()
        {
            try
            {
                if (region == "EUW")
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.Out.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.ToLower() + "1.lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.ToLower() + "1.lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
                else if (region == "EUNE")
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.Out.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.Substring(0, 3).ToLower() + "1.lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.Substring(0, 3).ToLower() + "1.lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
                else if (region == "NA")
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.Out.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.ToLower() + "2.lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.ToLower() + "2.lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
                else
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.Out.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.ToLower() + ".lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.ToLower() + ".lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            List<T> buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
