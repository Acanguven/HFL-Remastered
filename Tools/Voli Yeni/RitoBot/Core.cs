namespace RitoBot
{
    using Ini;
    using LoLLauncher;
    using RitoBot.Utils;
    using RitoBot.Utils.Region;
    using System;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using VoliBot.Utils;

    public class Core
    {
        public static int accountIndex = 0;
        private static string Header1 = "VoliBot - Auto Queue | V2.0.0";
        private static string Header2 = "For League of Legends Version 5.13 by Maufeat";
        private static string Header3 = "www.VoliBot.com";
        private static string Header4 = "facebook.com/volibot";

        public static void getColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public static string getTimestamp()
        {
            return ("[" + DateTime.Now + "] ");
        }

        public static void loadAccounts()
        {
            string str;
            TextReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "accounts.txt");
            while ((str = reader.ReadLine()) != null)
            {
                Config.accounts.Add(str);
                Config.accountsNew.Add(str);
            }
            reader.Close();
        }

        public static void loadConfiguration()
        {
            try
            {
                IniFile file1 = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "config.ini");
                Config.LauncherPath = file1.IniReadValue("General", "LauncherPath");
                Config.maxBots = Convert.ToInt32(file1.IniReadValue("General", "MaxBots"));
                Config.maxLevel = Convert.ToInt32(file1.IniReadValue("General", "MaxLevel"));
                Config.replaceConfig = Convert.ToBoolean(file1.IniReadValue("General", "ReplaceConfig"));
                Config.Region = BaseRegion.GetRegion(file1.IniReadValue("Account", "Region").ToUpper());
                Config.buyBoost = Convert.ToBoolean(file1.IniReadValue("Account", "BuyBoost"));
                Config.rndIcon = Convert.ToBoolean(file1.IniReadValue("Account", "RndIcon"));
                Config.championToPick = file1.IniReadValue("Other", "ChampionPick").ToUpper();
                Config.rndSpell = Convert.ToBoolean(file1.IniReadValue("Other", "RndSpell"));
                Config.spell1 = file1.IniReadValue("Other", "Spell1").ToUpper();
                Config.spell2 = file1.IniReadValue("Other", "Spell2").ToUpper();
            }
            catch (Exception exception1)
            {
                Console.WriteLine(exception1.Message);
                Thread.Sleep(0x2710);
                Application.Exit();
            }
        }

        public static void lognNewAccount()
        {
            Config.accountsNew = Config.accounts;
            Config.accounts.RemoveAt(0);
            int num = 0;
            if (Config.accounts.Count == 0)
            {
                Console.WriteLine(getTimestamp() + "No more accounts to login.");
            }
            foreach (string str in Config.accounts)
            {
                string[] separator = new string[] { "|" };
                string[] strArray2 = str.Split(separator, StringSplitOptions.None);
                num++;
                if (strArray2[2] != null)
                {
                    QueueTypes queueType = (QueueTypes) Enum.Parse(typeof(QueueTypes), strArray2[2]);
                    new VoliBot(strArray2[0], strArray2[1], Config.Region, Config.LauncherPath, queueType);
                }
                else
                {
                    new VoliBot(strArray2[0], strArray2[1], Config.Region, Config.LauncherPath, QueueTypes.ARAM);
                }
                Console.Title = " Current Connected: " + Config.connectedAccs;
                if (num == Config.maxBots)
                {
                    break;
                }
            }
        }

        private static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth + 5, Console.WindowHeight);
            Console.Title = "VoliBot - Auto Queue";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("====================================================================================\n");
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (Header1.Length / 2)) + "}", Header1));
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (Header2.Length / 2)) + "}", Header2));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (Header3.Length / 2)) + "}", Header3));
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (Header3.Length / 2)) + "}", Header4));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("====================================================================================\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(getTimestamp() + "Loading config.ini");
            loadConfiguration();
            if (Config.replaceConfig)
            {
                Console.WriteLine(getTimestamp() + "Replacing Config");
                Basic.ReplaceGameConfig();
            }
            Console.WriteLine(getTimestamp() + "Loading accounts.txt");
            loadAccounts();
            int num = 0;
            foreach (string str in Config.accounts)
            {
                try
                {
                    Config.accountsNew.RemoveAt(0);
                    string str2 = str;
                    string[] separator = new string[] { "|" };
                    string[] strArray2 = str2.Split(separator, StringSplitOptions.None);
                    num++;
                    if (strArray2[2] != null)
                    {
                        QueueTypes queueType = (QueueTypes) Enum.Parse(typeof(QueueTypes), strArray2[2]);
                        new VoliBot(strArray2[0], strArray2[1], Config.Region, Config.LauncherPath, queueType);
                    }
                    else
                    {
                        new VoliBot(strArray2[0], strArray2[1], Config.Region, Config.LauncherPath, QueueTypes.ARAM);
                    }
                    Console.Title = " Current Connected: " + Config.connectedAccs;
                    if (num == Config.maxBots)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CountAccError: You may have an issue in your accounts.txt");
                }
            }
            Console.ReadKey();
        }
    }
}

