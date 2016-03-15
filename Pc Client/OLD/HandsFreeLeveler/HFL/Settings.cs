using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;

namespace HandsFreeLeveler
{

    public static class Settings
    {
        public delegate void EventAction();
        public static event EventAction EventActions;
        public static int maxSmurf { get; set; }
        public static bool disableGpu { get; set; }
        public static bool buyBoost { get; set; }
        public static bool firstTime { get; set; }
        public static string bolPath { get; set; }
        public static string gamePath { get; set; }
        public static LoLLauncher.QueueTypes queueType = LoLLauncher.QueueTypes.INTRO_BOT;
        public static string dllPath { get; set; }
        public static bool reconnect = true;
        public static bool disableSpec { get; set; }
        public static bool mInject = false;
        public static string championId = "";
        public static string spell1 = "GHOST";
        public static string spell2 = "HEAL";
        public static bool smurfBreak = false;
        public static int smurfTimeoutAfter { get; set; }
        public static int smurfSleep { get; set; }
        public static bool rndSpell = false;
        public static string language = "English";

        public static void update()
        {

            var xs = new XmlSerializer(typeof(ObservableCollection<Smurf>));

            // serialize to disk
            using (Stream s = File.Create("Smurfs.xml"))
            {
                xs.Serialize(s, App.smurfList);
            }

            XDocument settings = new XDocument(
                   new XElement("HFL",
                       new XElement("Account",
                           new XElement("Username", User.username),
                           new XElement("Password", User.password)
                       ),
                       new XElement("Paths",
                           new XElement("BolPath", bolPath),
                           new XElement("GamePath", gamePath)
                       ),
                       new XElement("Settings",
                           new XElement("MaxSmurf", maxSmurf),
                           new XElement("DisableGpu", disableGpu),
                           new XElement("BuyBoost", buyBoost),
                           new XElement("FirstTime", firstTime),
                           new XElement("smurfBreak", smurfBreak),
                           new XElement("smurfTimeoutAfter", smurfTimeoutAfter),
                           new XElement("smurfSleep", smurfSleep),
                           new XElement("reconnect", reconnect),
                           new XElement("mInject", mInject),
                           new XElement("disableSpec", disableSpec),
                           new XElement("language", language)
                       ),
                       new XElement("Smurfs", App.smurfList)
                   )
               );
            settings.Save("settings.xml");
        }

        public static void updateLang(string lang)
        {
            language = lang;
            update();
            if (EventActions != null)
            {
                EventActions();
            }
        }

        public static void ReplaceGameConfig()
        {
            try
            {
                string Config = Settings.gamePath.Split(new string[] { "lol.launcher.exe" }, StringSplitOptions.None)[0] + "Config\\";

                if (!File.Exists(Config + "oldgame.cfg"))
                {
                    if (File.Exists(Config + "game.cfg"))
                    {
                        File.Copy(Config + "game.cfg", Config + "oldgame.cfg");
                    }
                }
                if (!File.Exists(Config + "game.cfg"))
                {
                    FileStream newConfig = File.Create(Config + "game.cfg");
                    newConfig.Close();
                }
                FileInfo info = new FileInfo(Config + "game.cfg")
                {
                    IsReadOnly = false
                };
                info.Refresh();
                while (IsFileLocked(info)) { }
                string str = "[General]\nGameMouseSpeed=9\nEnableAudio=0\nUserSetResolution=1\nBindSysKeys=0\nSnapCameraOnRespawn=1\nOSXMouseAcceleration=1\nAutoAcquireTarget=1\nEnableLightFx=0\nWindowMode=1\nShowTurretRangeIndicators=0\nPredictMovement=0\nWaitForVerticalSync=0\nColors=16\nHeight=200\nWidth=200\nSystemMouseSpeed=0\nCfgVersion=4.13.265\n\n[HUD]\nShowNeutralCamps=0\nDrawHealthBars=0\nAutoDisplayTarget=0\nMinimapMoveSelf=0\nItemShopPrevY=19\nItemShopPrevX=117\nShowAllChannelChat=0\nShowTimestamps=0\nObjectTooltips=0\nFlashScreenWhenDamaged=0\nNameTagDisplay=1\nShowChampionIndicator=0\nShowSummonerNames=0\nScrollSmoothingEnabled=0\nMiddleMouseScrollSpeed=0.5000\nMapScrollSpeed=0.5000\nShowAttackRadius=0\nNumericCooldownFormat=3\nSmartCastOnKeyRelease=0\nEnableLineMissileVis=0\nFlipMiniMap=0\nItemShopResizeHeight=47\nItemShopResizeWidth=455\nItemShopPrevResizeHeight=200\nItemShopPrevResizeWidth=300\nItemShopItemDisplayMode=1\nItemShopStartPane=1\n\n[Performance]\nShadowsEnabled=0\nEnableHUDAnimations=0\nPerPixelPointLighting=0\nEnableParticleOptimizations=0\nBudgetOverdrawAverage=10\nBudgetSkinnedVertexCount=10\nBudgetSkinnedDrawCallCount=10\nBudgetTextureUsage=10\nBudgetVertexCount=10\nBudgetTriangleCount=10\nBudgetDrawCallCount=1000\nEnableGrassSwaying=0\nEnableFXAA=0\nAdvancedShader=0\nFrameCapType=3\nGammaEnabled=1\nFull3DModeEnabled=0\nAutoPerformanceSettings=0\n=0\nEnvironmentQuality=0\nEffectsQuality=0\nShadowQuality=0\nGraphicsSlider=0\n\n[Volume]\nMasterVolume=1\nMusicMute=0\n\n[LossOfControl]\nShowSlows=0\n\n[ColorPalette]\nColorPalette=0\n\n[FloatingText]\nCountdown_Enabled=0\nEnemyTrueDamage_Enabled=0\nEnemyMagicalDamage_Enabled=0\nEnemyPhysicalDamage_Enabled=0\nTrueDamage_Enabled=0\nMagicalDamage_Enabled=0\nPhysicalDamage_Enabled=0\nScore_Enabled=0\nDisable_Enabled=0\nLevel_Enabled=0\nGold_Enabled=0\nDodge_Enabled=0\nHeal_Enabled=0\nSpecial_Enabled=0\nInvulnerable_Enabled=0\nDebug_Enabled=1\nAbsorbed_Enabled=1\nOMW_Enabled=1\nEnemyCritical_Enabled=0\nQuestComplete_Enabled=0\nQuestReceived_Enabled=0\nMagicCritical_Enabled=0\nCritical_Enabled=1\n\n[Replay]\nEnableHelpTip=0";
                System.IO.StreamWriter file = new System.IO.StreamWriter(Config + "game.cfg");
                file.WriteLine(str);
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't replace config, please check your game path.");
            }
        }

        public static void restoreCfg()
        {
            try
            {
                if (File.Exists(Settings.gamePath))
                {
                    string oldConfig = Settings.gamePath.Split(new string[] { "lol.launcher.exe" }, StringSplitOptions.None)[0] + "Config\\";
                    if (File.Exists(oldConfig + "oldgame.cfg"))
                    {
                        if (File.Exists(oldConfig + "game.cfg"))
                        {
                            File.Delete(oldConfig + "game.cfg");
                            File.Move(oldConfig + "oldgame.cfg", oldConfig + "game.cfg");
                        }
                        else
                        {
                            File.Move(oldConfig + "oldgame.cfg", oldConfig + "game.cfg");
                        }
                        MessageBox.Show("Your settings restored succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("You never used the Disable Gpu option, so there is no backup for your settings.");
                    }
                }
                else
                {
                    MessageBox.Show("Update your game folder settings first.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't restore config, please check your game path.");
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }

}
