namespace RitoBot.Utils
{
    using System;
    using System.IO;
    using System.Text;
    using VoliBot.Utils;

    internal class Basic
    {
        private static string _consonants = "bcdfghjklmnpqrstvwxz";
        private static string _vowels = "aeiouy";
        private static Random randomizer = new Random();

        public static void ChangeTitel()
        {
            Console.Title = "VoliBot | Current Connected: " + Config.connectedAccs;
        }

        public static int Rand(int min, int max)
        {
            return randomizer.Next(min, max + 1);
        }

        public static string RandomName()
        {
            string str = GetConsonant.ToUpper() + GetVowel;
            if (Rand(0, 1) == 0)
            {
                str = str + GetVowel;
            }
            str = str + GetConsonant;
            if (Rand(0, 1) == 0)
            {
                str = str + str[str.Length - 1].ToString();
            }
            str = str + GetVowel;
            if (Rand(0, 1) == 0)
            {
                str = str + GetConsonant + GetVowel;
            }
            if (Rand(0, 1) == 0)
            {
                str = str + Rand(0, 8);
                if (Rand(0, 1) == 0)
                {
                    str = str + Rand(0, 8);
                }
            }
            return str;
        }

        public static void ReplaceGameConfig()
        {
            try
            {
                FileInfo info = new FileInfo(Config.LauncherPath + @"Config\\game.cfg") {
                    IsReadOnly = false
                };
                info.Refresh();
                string str = "[General]\nGameMouseSpeed=9\nEnableAudio=0\nUserSetResolution=1\nBindSysKeys=0\nSnapCameraOnRespawn=1\nOSXMouseAcceleration=1\nAutoAcquireTarget=1\nEnableLightFx=0\nWindowMode=1\nShowTurretRangeIndicators=0\nPredictMovement=0\nWaitForVerticalSync=0\nColors=16\nHeight=200\nWidth=300\nSystemMouseSpeed=0\nCfgVersion=4.13.265\n\n[HUD]\nShowNeutralCamps=0\nDrawHealthBars=0\nAutoDisplayTarget=0\nMinimapMoveSelf=0\nItemShopPrevY=19\nItemShopPrevX=117\nShowAllChannelChat=0\nShowTimestamps=0\nObjectTooltips=0\nFlashScreenWhenDamaged=0\nNameTagDisplay=1\nShowChampionIndicator=0\nShowSummonerNames=0\nScrollSmoothingEnabled=0\nMiddleMouseScrollSpeed=0.5000\nMapScrollSpeed=0.5000\nShowAttackRadius=0\nNumericCooldownFormat=3\nSmartCastOnKeyRelease=0\nEnableLineMissileVis=0\nFlipMiniMap=0\nItemShopResizeHeight=47\nItemShopResizeWidth=455\nItemShopPrevResizeHeight=200\nItemShopPrevResizeWidth=300\nItemShopItemDisplayMode=1\nItemShopStartPane=1\n\n[Performance]\nShadowsEnabled=0\nEnableHUDAnimations=0\nPerPixelPointLighting=0\nEnableParticleOptimizations=0\nBudgetOverdrawAverage=10\nBudgetSkinnedVertexCount=10\nBudgetSkinnedDrawCallCount=10\nBudgetTextureUsage=10\nBudgetVertexCount=10\nBudgetTriangleCount=10\nBudgetDrawCallCount=1000\nEnableGrassSwaying=0\nEnableFXAA=0\nAdvancedShader=0\nFrameCapType=3\nGammaEnabled=1\nFull3DModeEnabled=0\nAutoPerformanceSettings=0\n=0\nEnvironmentQuality=0\nEffectsQuality=0\nShadowQuality=0\nGraphicsSlider=0\n\n[Volume]\nMasterVolume=1\nMusicMute=0\n\n[LossOfControl]\nShowSlows=0\n\n[ColorPalette]\nColorPalette=0\n\n[FloatingText]\nCountdown_Enabled=0\nEnemyTrueDamage_Enabled=0\nEnemyMagicalDamage_Enabled=0\nEnemyPhysicalDamage_Enabled=0\nTrueDamage_Enabled=0\nMagicalDamage_Enabled=0\nPhysicalDamage_Enabled=0\nScore_Enabled=0\nDisable_Enabled=0\nLevel_Enabled=0\nGold_Enabled=0\nDodge_Enabled=0\nHeal_Enabled=0\nSpecial_Enabled=0\nInvulnerable_Enabled=0\nDebug_Enabled=1\nAbsorbed_Enabled=1\nOMW_Enabled=1\nEnemyCritical_Enabled=0\nQuestComplete_Enabled=0\nQuestReceived_Enabled=0\nMagicCritical_Enabled=0\nCritical_Enabled=1\n\n[Replay]\nEnableHelpTip=0";
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(str);
                using (StreamWriter writer = new StreamWriter(Config.LauncherPath + @"Config\game.cfg"))
                {
                    writer.Write(builder.ToString());
                }
                info.IsReadOnly = true;
                info.Refresh();
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("game.cfg Error: If using VMWare Shared Folder, make sure it is not set to Read-Only.\nException:" + exception.Message);
            }
        }

        public static string GetConsonant
        {
            get
            {
                return _consonants.Substring(Rand(0, _consonants.Length - 1), 1);
            }
        }

        public static string GetVowel
        {
            get
            {
                return _vowels.Substring(Rand(0, _vowels.Length - 1), 1);
            }
        }
    }
}

