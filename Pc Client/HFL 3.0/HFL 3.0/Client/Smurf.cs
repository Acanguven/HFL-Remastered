using HFL_3._0.Core;
using LoLLauncher;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Game.Message;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects.Platform.Trade;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using VoliBot.LoLLauncher.RiotObjects.Platform.Messaging;

namespace HFL_3._0.Client
{

    public class Smurf
    {
        //rotation variables
        public delegate void smurfDone(object sender);
        public SmurfData info;
        public LoginDataPacket loginDataPacket = new LoginDataPacket();
        public LoLConnection connection = new LoLConnection();
        public bool restartPending = false;
        public event smurfDone smurfCompleted;
        public DateTime smurfingLimit = DateTime.Now;
        public RotationType type;
        //Account and Summoner Infos
        public ChampionDTO[] myChampions;
        public bool firstTimeInLobby = true;
        public bool firstTimeInQueuePop = true;
        public bool firstTimeInCustom = true;
        public bool firstTimeInPostChampSelect = true;
        public int m_leaverBustedPenalty { get; set; }
        public string m_accessToken { get; set; }
        public QueueTypes QueueType { get; set; }
        public QueueTypes ActualQueueType { get; set; }
        public Process exeProcess;
        System.Timers.Timer dispatcherTimer;
        TimeSpan t;


        public Smurf(SmurfData _smurf, RotationType rotationType)
        {
            info = _smurf;

            connection.OnConnect += new LoLConnection.OnConnectHandler(connection_OnConnect);
            connection.OnError += new LoLConnection.OnErrorHandler(connection_OnError);
            connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(connection_OnMessageReceived);
            type = rotationType;
            connection.Connect(info.username, info.password, info.region);   
        }

        public Smurf(SmurfData _smurf, RotationType rotationType, int minutes)
        {
            info = _smurf;

            connection.OnConnect += new LoLConnection.OnConnectHandler(connection_OnConnect);
            connection.OnError += new LoLConnection.OnErrorHandler(connection_OnError);
            connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(connection_OnMessageReceived);
            type = rotationType;
            smurfingLimit.AddMinutes(minutes);
            connection.Connect(info.username, info.password, info.region);    
        }

        private async void connection_OnMessageReceived(object sender, object message)
        {
            if (message is GameDTO)
            {
                GameDTO game = message as GameDTO;
                Console.WriteLine("Message Type:" + game.GameState);
                switch (game.GameState)
                {
                    case "START_REQUESTED":
                        break;
                    case "FAILED_TO_START":
                        Console.WriteLine("Failed to Start!");
                        break;
                    case "CHAMP_SELECT":
                        firstTimeInCustom = true;
                        firstTimeInQueuePop = true;
                        if (firstTimeInLobby)
                        {
                            firstTimeInLobby = false;
                            object obj = await connection.SetClientReceivedGameMessage(game.Id, "CHAMP_SELECT_CLIENT");
                            if (QueueType != QueueTypes.ARAM)
                            {

                                int Spell1;
                                int Spell2;
                                var random = new Random();
                                var spellList = new List<int> { 13, 6, 7, 1, 11, 21, 12, 3, 14, 2, 4 };

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

                                await connection.SelectSpells(Spell1, Spell2);

                                var randAvailableChampsArray = myChampions.Shuffle();
                                await connection.SelectChampion(randAvailableChampsArray.First(champ => champ.Owned || champ.FreeToPlay).ChampionId);
                                await connection.ChampionSelectCompleted();
                            }
                            break;
                        }
                        else
                            break;
                    case "PRE_CHAMP_SELECT":
                        updateStatus(msgStatus.INFO, "Champion selection in progress");
                        break;
                    case "POST_CHAMP_SELECT":
                        firstTimeInLobby = false;
                        if (firstTimeInPostChampSelect)
                        {
                            firstTimeInPostChampSelect = false;
                            updateStatus(msgStatus.INFO, "Champion selection is done, waiting for game to start");
                            break;
                        }
                        else
                            break;
                    case "IN_QUEUE":
                        updateStatus(msgStatus.INFO, "In Queue");
                        break;
                    case "TERMINATED":
                        updateStatus(msgStatus.INFO, "Re-entering queue");
                        firstTimeInPostChampSelect = true;
                        firstTimeInQueuePop = true;
                        break;
                    case "JOINING_CHAMP_SELECT":
                        if (firstTimeInQueuePop && game.StatusOfParticipants.Contains("1"))
                        {
                            updateStatus(msgStatus.INFO, "Accepted Queue");
                            firstTimeInQueuePop = false;
                            firstTimeInLobby = true;
                            object obj = await connection.AcceptPoppedGame(true);
                            break;
                        }
                        else
                            break;
                    default:
                        updateStatus(msgStatus.INFO, "[DEFAULT]" + game.GameStateString);
                        break;
                }
            }
            else if (message.GetType() == typeof(TradeContractDTO))
            {
                TradeContractDTO tradeDto = message as TradeContractDTO;
                if (tradeDto != null)
                {
                    switch (tradeDto.State)
                    {
                        case "PENDING":
                            if (tradeDto != null)
                            {
                                object obj = await connection.AcceptTrade(tradeDto.RequesterInternalSummonerName, (int)tradeDto.RequesterChampionId);
                                break;
                            }
                            else
                                break;
                    }
                }
            }
            else if (message is PlayerCredentialsDto)
            {
                firstTimeInPostChampSelect = true;
                PlayerCredentialsDto dto = message as PlayerCredentialsDto;
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = FindLoLExe();
                startInfo.FileName = "League of Legends.exe";
                startInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"" + dto.ServerIp + " " +
                    dto.ServerPort + " " + dto.EncryptionKey + " " + dto.SummonerId + "\"";
                updateStatus(msgStatus.INFO, "Launching League of Legends");

                new Thread(() =>
                {
                    exeProcess = Process.Start(startInfo);
                    exeProcess.EnableRaisingEvents = true;
                    exeProcess.Exited += new EventHandler(exeProcess_Exited);
                    while (exeProcess.MainWindowHandle == IntPtr.Zero) { }
                    Console.WriteLine(exeProcess.MainWindowTitle);
                    Thread.Sleep(1000);
                    App.gameMask.addGame(info.username, info.region.ToString(), exeProcess);
                }).Start();
            }
            else if (!(message is GameNotification) && !(message is SearchingForMatchNotification))
            {
                if (message is EndOfGameStats)
                {
                    object obj4 = await connection.ackLeaverBusterWarning();
                    object obj5 = await connection.callPersistenceMessaging(new SimpleDialogMessageResponse()
                    {
                        AccountID = loginDataPacket.AllSummonerData.Summoner.SumId,
                        MsgID = loginDataPacket.AllSummonerData.Summoner.SumId,
                        Command = "ack"
                    });
                    MatchMakerParams matchParams = new MatchMakerParams();
                    checkAndUpdateQueueType();
                    if (QueueType == QueueTypes.INTRO_BOT)
                    {
                        matchParams.BotDifficulty = "INTRO";
                    }
                    else if (QueueType == QueueTypes.BEGINNER_BOT)
                    {
                        matchParams.BotDifficulty = "EASY";
                    }
                    else if (QueueType == QueueTypes.MEDIUM_BOT)
                    {
                        matchParams.BotDifficulty = "MEDIUM";
                    }
                    if (QueueType != 0)
                    {
                        matchParams.QueueIds = new Int32[1] { (int)QueueType };
                        SearchingForMatchNotification m = await connection.AttachToQueue(matchParams);

                        if (m.PlayerJoinFailures == null)
                        {
                            updateStatus(msgStatus.INFO, "In Queue: " + QueueType.ToString());
                        }
                        else
                        {
                            foreach (var failure in m.PlayerJoinFailures)
                            {
                                if (failure.ReasonFailed == "LEAVER_BUSTED")
                                {
                                    m_accessToken = failure.AccessToken;
                                    if (failure.LeaverPenaltyMillisRemaining > m_leaverBustedPenalty)
                                    {
                                        m_leaverBustedPenalty = failure.LeaverPenaltyMillisRemaining;
                                    }
                                }
                                Console.WriteLine("Start Failed:" + failure.ReasonFailed);
                            }

                            if (string.IsNullOrEmpty(m_accessToken))
                            {
                                foreach (var failure in m.PlayerJoinFailures)
                                {
                                    updateStatus(msgStatus.INFO, "Dodge Remaining Time: " + Convert.ToString((failure.DodgePenaltyRemainingTime / 1000 / (float)60)).Replace(",", ":") + "...");
                                }
                            }
                            else
                            {
                                double minutes = m_leaverBustedPenalty / 1000 / (float)60;
                                updateStatus(msgStatus.INFO, "Waiting out leaver buster: " + minutes + " minutes!");
                                t = TimeSpan.FromMinutes((int)minutes);
                                //Tick(); -> Enable to get visual time remaining
                                Thread.Sleep(TimeSpan.FromMilliseconds(m_leaverBustedPenalty));
                                m = await connection.AttachToLowPriorityQueue(matchParams, m_accessToken);
                                if (m.PlayerJoinFailures == null)
                                {
                                    updateStatus(msgStatus.INFO, "Succesfully joined lower priority queue!");
                                }
                                else
                                {
                                    updateStatus(msgStatus.ERROR, "There was an error in joining lower priority queue.\nDisconnecting.");
                                    connection.Disconnect();
                                }
                            }
                        }
                    }
                }
                else if (message.ToString().Contains("EndOfGameStats"))
                {
                    EndOfGameStats eog = new EndOfGameStats();
                    exeProcess.Exited -= new EventHandler(exeProcess_Exited);
                    exeProcess.Kill();
                    Thread.Sleep(500);
                    if (exeProcess.Responding) { 
                        Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                    }
                    loginDataPacket = await connection.GetLoginDataPacketForUser();
                    if(type == RotationType.SmurfDone)
                    {
                        if (info.desiredLevel > loginDataPacket.AllSummonerData.SummonerLevel.Level)
                        {
                            connection_OnMessageReceived(sender, eog);
                        }
                        else
                        {
                            connection.Disconnect();
                       }
                    }else
                    {
                        connection_OnMessageReceived(sender, eog);
                    }
                }
            }
        }

        public void Tick()
        {
            dispatcherTimer = new System.Timers.Timer(1000);
            dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = 1000;
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
        {
            t = t.Subtract(TimeSpan.FromSeconds(1));
            if (t.Seconds < 0)
            {
                dispatcherTimer.Stop();
                return;
            }
            int cMinutes = 0;
            int cSeconds = 0;
            foreach (char c in t.Minutes.ToString())
            {
                ++cMinutes;
            }
            foreach (char c in t.Seconds.ToString())
            {
                ++cSeconds;
            }
            string minutes = t.Minutes.ToString();
            string seconds = t.Seconds.ToString();
            if (cMinutes == 1)
            {
                minutes = "0" + t.Minutes;
            }
            if (cSeconds == 1)
            {
                seconds = "0" + t.Seconds;
            }
            //Reamining Time -> "Status: Waiting " + minutes + ":" + seconds, _summonerLevel.ToString()
        }

        void exeProcess_Exited(object sender, EventArgs e)
        {
            App.gameMask.removeGame(exeProcess);
            updateStatus(msgStatus.INFO, "Restart League of Legends.");
            Thread.Sleep(1000);
            if (loginDataPacket.ReconnectInfo != null && loginDataPacket.ReconnectInfo.Game != null)
            {
                connection_OnMessageReceived(sender, loginDataPacket.ReconnectInfo.PlayerCredentials);
            }
            else
            {
                connection_OnMessageReceived(sender, new EndOfGameStats());
            }
        }

        private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
        {

        }

        private void connection_OnLogin(object sender, string username, string ipAddress)
        {
            new Thread(async () =>
            {
                App.gameMask.findAndKillHflWindow(info.username, info.region.ToString());

                loginDataPacket = await connection.GetLoginDataPacketForUser();

                #region createSummoner
                if (loginDataPacket.AllSummonerData == null)
                {
                    Random random = new Random();
                    string summonerName = info.username;
                    if (summonerName.Length > 16)
                    {
                        summonerName = summonerName.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
                    }
                    await connection.CreateDefaultSummoner(summonerName);
                    updateStatus(msgStatus.INFO, "Created Summoner: " + summonerName);
                }
                #endregion

                loginDataPacket = await connection.GetLoginDataPacketForUser();

                #region registerMessages
                await connection.Subscribe("bc", loginDataPacket.AllSummonerData.Summoner.AcctId);
                await connection.Subscribe("cn", loginDataPacket.AllSummonerData.Summoner.AcctId);
                await connection.Subscribe("gn", loginDataPacket.AllSummonerData.Summoner.AcctId);
                #endregion

                #region ackDialogBusterWarning
                await connection.ackLeaverBusterWarning();
                await connection.callPersistenceMessaging(new SimpleDialogMessageResponse()
                {
                    AccountID = loginDataPacket.AllSummonerData.Summoner.SumId,
                    MsgID = loginDataPacket.AllSummonerData.Summoner.SumId,
                    Command = "ack"
                });
                #endregion

                #region fetchSummonerData
                info.summonerId = loginDataPacket.AllSummonerData.Summoner.SumId;
                info.currentLevel = loginDataPacket.AllSummonerData.SummonerLevel.Level;
                info.currentIp = loginDataPacket.IpBalance;
                info.expToNextLevel = loginDataPacket.AllSummonerData.SummonerLevel.ExpToNextLevel; ;
                info.currentXp = loginDataPacket.AllSummonerData.SummonerLevelAndPoints.ExpPoints;
                info.currentRp = loginDataPacket.RpBalance;
                info.summonerName = loginDataPacket.AllSummonerData.Summoner.Name;
                myChampions = await connection.GetAvailableChampions();
                await connection.CreatePlayer();
                sessionManager.smurfStart(info.username, info.region, info.currentXp, info.currentIp, info.currentLevel, info.expToNextLevel);
                updateStatus(msgStatus.INFO, "Logged in as " + info.summonerName + " @ level " + info.currentLevel);
                #endregion

                #region checkReconnectState
                if (loginDataPacket.ReconnectInfo != null && loginDataPacket.ReconnectInfo.Game != null)
                {
                    connection_OnMessageReceived(sender, loginDataPacket.ReconnectInfo.PlayerCredentials);
                    return;
                }
                #endregion

                #region startSoloSequence
                if (type == RotationType.SmurfDone)
                {
                    if(info.desiredLevel <= loginDataPacket.AllSummonerData.SummonerLevel.Level)
                    {
                        connection.Disconnect();
                        return;
                    }
                }

                object obj4 = await connection.ackLeaverBusterWarning();
                object obj5 = await connection.callPersistenceMessaging(new SimpleDialogMessageResponse()
                {
                    AccountID = loginDataPacket.AllSummonerData.Summoner.SumId,
                    MsgID = loginDataPacket.AllSummonerData.Summoner.SumId,
                    Command = "ack"
                });
                MatchMakerParams matchParams = new MatchMakerParams();
                checkAndUpdateQueueType();
                if (QueueType == QueueTypes.INTRO_BOT)
                {
                    matchParams.BotDifficulty = "INTRO";
                }
                else if (QueueType == QueueTypes.BEGINNER_BOT)
                {
                    matchParams.BotDifficulty = "EASY";
                }
                else if (QueueType == QueueTypes.MEDIUM_BOT)
                {
                    matchParams.BotDifficulty = "MEDIUM";
                }
                updateStatus(msgStatus.INFO, QueueType.ToString());
                if (QueueType != 0)
                {
                    matchParams.QueueIds = new int[1] { (int)QueueType };
                    SearchingForMatchNotification m = await connection.AttachToQueue(matchParams);

                    if (m.PlayerJoinFailures == null)
                    {
                        updateStatus(msgStatus.INFO, "In Queue: " + QueueType.ToString());
                    }else
                    {
                        foreach (var failure in m.PlayerJoinFailures)
                        {
                            if (failure.ReasonFailed == "LEAVER_BUSTED")
                            {
                                m_accessToken = failure.AccessToken;
                                if (failure.LeaverPenaltyMillisRemaining > m_leaverBustedPenalty)
                                {
                                    m_leaverBustedPenalty = failure.LeaverPenaltyMillisRemaining;
                                }
                            }
                        }

                        if (String.IsNullOrEmpty(m_accessToken))
                        {
                            foreach (var failure in m.PlayerJoinFailures)
                            {
                                updateStatus(msgStatus.INFO, "Dodge Remaining Time: " + Convert.ToString((failure.DodgePenaltyRemainingTime / 1000 / (float)60)).Replace(",", ":") + "...");
                            }
                        }
                        else
                        {
                            double minutes = m_leaverBustedPenalty / 1000 / (float)60;
                            updateStatus(msgStatus.INFO, "Waiting out leaver buster: " + minutes + " minutes!");
                            t = TimeSpan.FromMinutes((int)minutes);
                            //Tick(); ->Visual Timer
                            Thread.Sleep(TimeSpan.FromMilliseconds(m_leaverBustedPenalty));
                            m = await connection.AttachToLowPriorityQueue(matchParams, m_accessToken);
                            if (m.PlayerJoinFailures == null)
                            {
                                updateStatus(msgStatus.INFO, "Succesfully joined lower priority queue!");
                            }
                            else
                            {
                                updateStatus(msgStatus.ERROR, "There was an error in joining lower priority queue.\nDisconnecting.");
                                connection.Disconnect();
                            }
                        }
                    }
                }
                #endregion
            }).Start();
        }

        private void connection_OnError(object sender, Error error)
        {
            if (error.Type == ErrorType.Password)
            {
                Console.WriteLine("Wrong password");
                return;
            }
            if (error.Type == ErrorType.AuthKey)
            {
                Console.WriteLine("Auth Key");
                Reconnect(2);
                return;
            }
            switch (error.ErrorCode)
            {
                case "LOGIN-0001":
                    Console.WriteLine("Client version is not matching with the updated one.");
                    return;
                    break;
            }
            if (error.Type == ErrorType.Receive)
            {
                return;
            }
            Console.WriteLine("Unhandled message recieved from server:" + error.ErrorCode + " | " + error.Message + " | " + error.Type.ToString());
        }

        private void connection_OnDisconnect(object sender, EventArgs e)
        {
            completeSmurf();
        }

        private void connection_OnConnect(object sender, object message)
        {
            connection.OnLogin += new LoLConnection.OnLoginHandler(connection_OnLogin);
            connection.OnLoginQueueUpdate += new LoLConnection.OnLoginQueueUpdateHandler(connection_OnLoginQueueUpdate);
            connection.OnDisconnect += new LoLConnection.OnDisconnectHandler(connection_OnDisconnect);
        }

        public void Reconnect(int seconds)
        {
            Thread.Sleep(seconds * 1000);
            Console.WriteLine("Restarting:" + info.username);
            connection.Connect(info.username, info.password, info.region);
        }

        private string FindLoLExe()
        {
            try
            {
                string installPath = Enumerable.Last(Enumerable.OrderBy(Directory.EnumerateDirectories((App.settings.gameFolder ?? "").Split(new string[] { "lol.launcher.exe" }, StringSplitOptions.None)[0] + "RADS\\solutions\\lol_game_client_sln\\releases\\"), f => new DirectoryInfo(f).CreationTime)) + "\\deploy\\";
                return installPath;
            }
            catch (DirectoryNotFoundException)
            {
                updateStatus(msgStatus.ERROR, "[Options][" + info.username + "] Launcher Path error! Directory not found.");
                connection.Disconnect();
                return "";
            }
        }

        public void setRestartState(bool s)
        {
            restartPending = s;
        }

        private void checkAndUpdateQueueType()
        {
            try
            {
                if (info.queue != "")
                {
                    if (info.queue == ((int)(QueueTypes.NORMAL_5x5)).ToString())
                    {
                        QueueType = QueueTypes.NORMAL_5x5;
                    }
                    else if (info.queue == ((int)(QueueTypes.NORMAL_3x3)).ToString())
                    {
                        QueueType = QueueTypes.NORMAL_3x3;
                    }
                    else if (info.queue == ((int)(QueueTypes.ARAM)).ToString())
                    {
                        QueueType = QueueTypes.ARAM;
                    }
                    else if (info.queue == ((int)(QueueTypes.INTRO_BOT)).ToString())
                    {
                        QueueType = QueueTypes.INTRO_BOT;
                    }
                    else if (info.queue == ((int)(QueueTypes.BEGINNER_BOT)).ToString())
                    {
                        QueueType = QueueTypes.BEGINNER_BOT;
                    }
                    else if (info.queue == ((int)(QueueTypes.MEDIUM_BOT)).ToString())
                    {
                        QueueType = QueueTypes.MEDIUM_BOT;
                    }else
                    {
                        connection.Disconnect();
                    }
                }
                else
                {
                    updateStatus(msgStatus.ERROR, "You have to select a queue");
                    connection.Disconnect();
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //SetUp
                if (loginDataPacket.AllSummonerData.SummonerLevel.Level < 3.0 && QueueType == QueueTypes.NORMAL_5x5)
                {
                    updateStatus(msgStatus.INFO, "Need to be Level 3 before NORMAL_5x5 queue.");
                    updateStatus(msgStatus.INFO, "Joins Co-Op vs AI (Beginner) queue until 3");
                    QueueType = QueueTypes.BEGINNER_BOT;
                    ActualQueueType = QueueTypes.NORMAL_5x5;
                }
                else if (loginDataPacket.AllSummonerData.SummonerLevel.Level < 6.0 && QueueType == QueueTypes.ARAM)
                {
                    updateStatus(msgStatus.INFO, "Need to be Level 6 before ARAM queue.");
                    updateStatus(msgStatus.INFO, "Joins Co-Op vs AI (Beginner) queue until 6");
                    QueueType = QueueTypes.BEGINNER_BOT;
                    ActualQueueType = QueueTypes.ARAM;
                }
                else if (loginDataPacket.AllSummonerData.SummonerLevel.Level < 7.0 && QueueType == QueueTypes.NORMAL_3x3)
                {
                    updateStatus(msgStatus.INFO, "Need to be Level 7 before NORMAL_3x3 queue.");
                    updateStatus(msgStatus.INFO, "Joins Co-Op vs AI (Beginner) queue until 7");
                    QueueType = QueueTypes.BEGINNER_BOT;
                    ActualQueueType = QueueTypes.NORMAL_3x3;
                }
                //Check if is available to join queue.
                if (loginDataPacket.AllSummonerData.SummonerLevel.Level == 3 && ActualQueueType == QueueTypes.NORMAL_5x5)
                {
                    QueueType = ActualQueueType;
                }
                else if (loginDataPacket.AllSummonerData.SummonerLevel.Level == 6 && ActualQueueType == QueueTypes.ARAM)
                {
                    QueueType = ActualQueueType;
                }
                else if (loginDataPacket.AllSummonerData.SummonerLevel.Level == 7 && ActualQueueType == QueueTypes.NORMAL_3x3)
                {
                    QueueType = ActualQueueType;
                }
            }
            catch (Exception)
            {

            }
        }

        public void completeSmurf()
        {
            sessionManager.smurfEnd(info.username, info.region, info.currentXp, info.currentIp, info.currentLevel, info.expToNextLevel);
            smurfCompleted(this);
        }

        #region messageLogs
        private void updateStatus(msgStatus type, string msg)
        {
            Console.WriteLine(info.username + " | " + msg);
            /*switch (type)
            {

                case msgStatus.DEBUG:
                    var string = 
                    richTextBox1.AppendText("[" + type + "]", Color.Pink);
                    break;
                case msgStatus.ERROR:
                    richTextBox1.AppendText("[" + type + "]", Color.Red);
                    break;
                case msgStatus.INFO:
                    richTextBox1.AppendText("[" + type + "]", Color.Blue);
                    break;
                default:
                    richTextBox1.AppendText("[" + type + "]", Color.Aqua);
                    break;
            }
            richTextBox1.AppendText("[" + DateTime.Now.ToShortTimeString() + "]", Color.Blue);
            richTextBox1.AppendText(" ", Color.Black);
            richTextBox1.AppendText(_username, Color.DarkBlue);
            richTextBox1.AppendText(": ", Color.Black);
            richTextBox1.AppendText(msg, Color.Black);
            richTextBox1.AppendText(Environment.NewLine, Color.Black);
            */
        }

        private void updateStatus(msgStatus type, string msg, Color msgClr)
        {
            Console.WriteLine(info.username + " | " + msg);
        }

        public void Log(string text)
        {
            Console.WriteLine(text);
        }
        #endregion
    }

    public enum msgStatus
    {
        ERROR,
        INFO,
        DEBUG,
        UNDEFINED
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
