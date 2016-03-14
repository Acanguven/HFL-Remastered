namespace RitoBot
{
    using System;

    internal class Enums
    {
        public static object[] champions = new object[] { 
            "AATROX", "AHRI", "AKALI", "ALISTAR", "AMUMU", "ANIVIA", "ANNIE", "ASHE", "AZIR", "BLITZCRANK", "BRAND", "BRAUM", "CAITLYN", "CASSIOPEIA", "CHOGATH", "CORKI", 
            "DARIUS", "DIANA", "MUNDO", "DRAVEN", "ELISE", "EVELYNN", "EZREAL", "FIDDLESTICKS", "FIORA", "FIZZ", "GALIO", "GANGPLANK", "GAREN", "GNAR", "GRAGAS", "GRAVES", 
            "HECARIM", "HEIMERDIGER", "IRELIA", "JANNA", "JARVAN", "JAX", "JAYCE", "JINX", "KALISTA", "KARMA", "KARTHUS", "KASSADIN", "KATARINA", "KAYLE", "KENNEN", "KHAZIX", 
            "KOGMAW", "LEBLANC", "LEESIN", "LEONA", "LISSANDRA", "LUCIAN", "LULU", "LUX", "MALPHITE", "MALZAHAR", "MAOKAI", "MASTERYI", "MISSFORTUNE", "MORDEKAISER", "MORGANA", "NAMI", 
            "NASUS", "NAUTILUS", "NIDALEE", "NOCTURNE", "NUNU", "OLAF", "ORIANNA", "PANTHEON", "POPPY", "QUINN", "REKSAI", "RAMMUS", "RENEKTON", "RENGAR", "RIVEN", "RUMBLE", 
            "RYZE", "SEJUANI", "SHACO", "SHEN", "SHYVANA", "SINGED", "SION", "SIVIR", "SKARNER", "SONA", "SORAKA", "SWAIN", "SYNDRA", "TALON", "TARIC", "TEEMO", 
            "THRESH", "TRISTANA", "TRUNDLE", "TRYNDAMERE", "TWISTEDFATE", "TWITCH", "UDYR", "URGOT", "VARUS", "VAYNE", "VEIGAR", "VELKOZ", "VI", "VIKTOR", "VLADIMIR", "VOLIBEAR", 
            "WARWICK", "WUKONG", "XERATH", "XINZHAO", "YASUO", "YORICK", "ZAC", "ZED", "ZIGGS", "ZILEAN", "ZYRA"
         };
        public static object[] queues = new object[] { "NORMAL_5x5", "NORMAL_3x3", "INTRO_BOT", "BEGINNER_BOT", "MEDIUM_BOT", "ARAM" };
        public static object[] regions = new object[] { "NA", "EUW", "EUNE", "OCE", "LAN", "LAS", "BR", "TR", "RU", "QQ" };
        public static object[] spells = new object[] { "BARRIER", "CLAIRVOYANCE", "CLARITY", "CLEANSE", "EXHAUST", "FLASH", "GARRISON", "GHOST", "HEAL", "IGNITE", "REVIVE", "SMITE", "TELEPORT" };

        public static int championToId(string name)
        {
            uint num = <PrivateImplementationDetails><VoliBot.exe>.ComputeStringHash(name);
            if (num <= 0x7f25d76a)
            {
                if (num <= 0x379325e0)
                {
                    if (num <= 0x20a88ddc)
                    {
                        if (num <= 0xcb0da2e)
                        {
                            if (num <= 0x48c4438)
                            {
                                switch (num)
                                {
                                    case 0x845dba:
                                        if (!(name == "RAMMUS"))
                                        {
                                            goto Label_104C;
                                        }
                                        return 0x21;

                                    case 0x40cd233:
                                        if (!(name == "WARWICK"))
                                        {
                                            goto Label_104C;
                                        }
                                        return 0x13;
                                }
                                if ((num == 0x48c4438) && (name == "WUKONG"))
                                {
                                    return 0x3e;
                                }
                            }
                            else if (num <= 0xb6a7d9f)
                            {
                                if (num == 0xac00ba7)
                                {
                                    if (name == "KARMA")
                                    {
                                        return 0x2b;
                                    }
                                }
                                else if ((num == 0xb6a7d9f) && (name == "SHACO"))
                                {
                                    return 0x23;
                                }
                            }
                            else if (num == 0xbfbcb14)
                            {
                                if (name == "YASUO")
                                {
                                    return 0x9d;
                                }
                            }
                            else if ((num == 0xcb0da2e) && (name == "SONA"))
                            {
                                return 0x25;
                            }
                        }
                        else if (num <= 0x1bc18286)
                        {
                            if (num <= 0x11182ccb)
                            {
                                if (num == 0x10d0e5e5)
                                {
                                    if (name == "ZAC")
                                    {
                                        return 0x9a;
                                    }
                                }
                                else if ((num == 0x11182ccb) && (name == "GALIO"))
                                {
                                    return 3;
                                }
                            }
                            else if (num == 0x18a2d464)
                            {
                                if (name == "JAX")
                                {
                                    return 0x18;
                                }
                            }
                            else if ((num == 0x1bc18286) && (name == "AATROX"))
                            {
                                return 0x10a;
                            }
                        }
                        else if (num <= 0x1e9f9d03)
                        {
                            if (num == 0x1d5ad11a)
                            {
                                if (name == "PANTHEON")
                                {
                                    return 80;
                                }
                            }
                            else if ((num == 0x1e9f9d03) && (name == "DRAVEN"))
                            {
                                return 0x77;
                            }
                        }
                        else if (num == 0x202c716c)
                        {
                            if (name == "LISSANDRA")
                            {
                                return 0x7f;
                            }
                        }
                        else if ((num == 0x20a88ddc) && (name == "ZILEAN"))
                        {
                            return 0x1a;
                        }
                    }
                    else if (num <= 0x2ac179bc)
                    {
                        if (num <= 0x22bb41a2)
                        {
                            switch (num)
                            {
                                case 0x2115086b:
                                    if (!(name == "RENEKTON"))
                                    {
                                        goto Label_104C;
                                    }
                                    return 0x3a;

                                case 0x225b5055:
                                    if (!(name == "LULU"))
                                    {
                                        goto Label_104C;
                                    }
                                    return 0x75;
                            }
                            if ((num == 0x22bb41a2) && (name == "SIVIR"))
                            {
                                return 15;
                            }
                        }
                        else if (num <= 0x26cfaab4)
                        {
                            if (num == 0x24bff5da)
                            {
                                if (name == "GAREN")
                                {
                                    return 0x56;
                                }
                            }
                            else if ((num == 0x26cfaab4) && (name == "KALISTA"))
                            {
                                return 0x1ad;
                            }
                        }
                        else if (num == 0x29b5f6c6)
                        {
                            if (name == "SORAKA")
                            {
                                return 0x10;
                            }
                        }
                        else if ((num == 0x2ac179bc) && (name == "SION"))
                        {
                            return 14;
                        }
                    }
                    else if (num <= 0x342803d3)
                    {
                        if (num <= 0x2f8d64ca)
                        {
                            if (num == 0x2c08e9f4)
                            {
                                if (name == "REKSAI")
                                {
                                    return 0x1a5;
                                }
                            }
                            else if ((num == 0x2f8d64ca) && (name == "TWITCH"))
                            {
                                return 0x1d;
                            }
                        }
                        else if (num == 0x303f0149)
                        {
                            if (name == "SKARNER")
                            {
                                return 0x48;
                            }
                        }
                        else if ((num == 0x342803d3) && (name == "GRAVES"))
                        {
                            return 0x68;
                        }
                    }
                    else if (num <= 0x3515f855)
                    {
                        if (num == 0x34648d19)
                        {
                            if (name == "AZIR")
                            {
                                return 0x10c;
                            }
                        }
                        else if ((num == 0x3515f855) && (name == "RIVEN"))
                        {
                            return 0x5c;
                        }
                    }
                    else if (num == 0x3614bcf5)
                    {
                        if (name == "KASSADIN")
                        {
                            return 0x26;
                        }
                    }
                    else if ((num == 0x379325e0) && (name == "GRAGAS"))
                    {
                        return 0x4f;
                    }
                }
                else if (num <= 0x59907d34)
                {
                    if (num <= 0x4c78ed63)
                    {
                        if (num <= 0x45ad8860)
                        {
                            switch (num)
                            {
                                case 0x3e09055b:
                                    if (!(name == "JAYCE"))
                                    {
                                        goto Label_104C;
                                    }
                                    return 0x7e;

                                case 0x455800f3:
                                    if (!(name == "CHOGATH"))
                                    {
                                        goto Label_104C;
                                    }
                                    return 0x1f;
                            }
                            if ((num == 0x45ad8860) && (name == "RENGAR"))
                            {
                                return 0x6b;
                            }
                        }
                        else if (num <= 0x479f2d75)
                        {
                            if (num == 0x471e0b47)
                            {
                                if (name == "BLITZCRANK")
                                {
                                    return 0x35;
                                }
                            }
                            else if ((num == 0x479f2d75) && (name == "MASTERYI"))
                            {
                                return 11;
                            }
                        }
                        else if (num == 0x4b35ec81)
                        {
                            if (name == "KARTHUS")
                            {
                                return 30;
                            }
                        }
                        else if ((num == 0x4c78ed63) && (name == "ZIGGS"))
                        {
                            return 0x73;
                        }
                    }
                    else if (num <= 0x5615759a)
                    {
                        if (num <= 0x50064fc5)
                        {
                            if (num == 0x4f48f7f6)
                            {
                                if (name == "HEIMERDIGER")
                                {
                                    return 0x4a;
                                }
                            }
                            else if ((num == 0x50064fc5) && (name == "TEEMO"))
                            {
                                return 0x11;
                            }
                        }
                        else if (num == 0x51082254)
                        {
                            if (name == "TRYNDAMERE")
                            {
                                return 0x17;
                            }
                        }
                        else if ((num == 0x5615759a) && (name == "MISSFORTUNE"))
                        {
                            return 0x15;
                        }
                    }
                    else if (num <= 0x58fad79a)
                    {
                        if (num == 0x5687a357)
                        {
                            if (name == "DARIUS")
                            {
                                return 0x7a;
                            }
                        }
                        else if ((num == 0x58fad79a) && (name == "HECARIM"))
                        {
                            return 120;
                        }
                    }
                    else if (num == 0x5943a146)
                    {
                        if (name == "MUNDO")
                        {
                            return 0x24;
                        }
                    }
                    else if ((num == 0x59907d34) && (name == "RUMBLE"))
                    {
                        return 0x44;
                    }
                }
                else if (num <= 0x6744003b)
                {
                    if (num <= 0x5bfa3c60)
                    {
                        if (num <= 0x5a3509d8)
                        {
                            if (num == 0x59bd12b2)
                            {
                                if (name == "EVELYNN")
                                {
                                    return 0x1c;
                                }
                            }
                            else if ((num == 0x5a3509d8) && (name == "VELKOZ"))
                            {
                                return 0xa1;
                            }
                        }
                        else if (num == 0x5aee327e)
                        {
                            if (name == "EZREAL")
                            {
                                return 0x51;
                            }
                        }
                        else if ((num == 0x5bfa3c60) && (name == "VI"))
                        {
                            return 0xfe;
                        }
                    }
                    else if (num <= 0x65d47752)
                    {
                        if (num == 0x5eba9d0a)
                        {
                            if (name == "NAMI")
                            {
                                return 0x10b;
                            }
                        }
                        else if ((num == 0x65d47752) && (name == "ANNIE"))
                        {
                            return 1;
                        }
                    }
                    else if (num == 0x665553a8)
                    {
                        if (name == "AMUMU")
                        {
                            return 0x20;
                        }
                    }
                    else if ((num == 0x6744003b) && (name == "KAYLE"))
                    {
                        return 10;
                    }
                }
                else if (num <= 0x75fb3557)
                {
                    if (num <= 0x6a6e4ed6)
                    {
                        if (num == 0x67a74f85)
                        {
                            if (name == "IRELIA")
                            {
                                return 0x27;
                            }
                        }
                        else if ((num == 0x6a6e4ed6) && (name == "BRAND"))
                        {
                            return 0x3f;
                        }
                    }
                    else if (num == 0x6d62b277)
                    {
                        if (name == "TALON")
                        {
                            return 0x5b;
                        }
                    }
                    else if ((num == 0x75fb3557) && (name == "KOGMAW"))
                    {
                        return 0x60;
                    }
                }
                else if (num <= 0x7ba41929)
                {
                    if (num == 0x79a148a9)
                    {
                        if (name == "TRISTANA")
                        {
                            return 0x12;
                        }
                    }
                    else if ((num == 0x7ba41929) && (name == "OLAF"))
                    {
                        return 2;
                    }
                }
                else if (num == 0x7dad5db9)
                {
                    if (name == "SHYVANA")
                    {
                        return 0x66;
                    }
                }
                else if ((num == 0x7f25d76a) && (name == "KENNEN"))
                {
                    return 0x55;
                }
            }
            else if (num <= 0xbab1b1e4)
            {
                if (num <= 0x9f4df966)
                {
                    if (num <= 0x8911d12d)
                    {
                        if (num <= 0x81f13e2b)
                        {
                            switch (num)
                            {
                                case 0x7f725fb0:
                                    if (!(name == "MORGANA"))
                                    {
                                        goto Label_104C;
                                    }
                                    return 0x19;

                                case 0x80c47116:
                                    if (!(name == "YORICK"))
                                    {
                                        goto Label_104C;
                                    }
                                    return 0x53;
                            }
                            if ((num == 0x81f13e2b) && (name == "AKALI"))
                            {
                                return 0x54;
                            }
                        }
                        else if (num <= 0x8674d489)
                        {
                            if (num == 0x863c4f66)
                            {
                                if (name == "GANGPLANK")
                                {
                                    return 0x29;
                                }
                            }
                            else if ((num == 0x8674d489) && (name == "NUNU"))
                            {
                                return 20;
                            }
                        }
                        else if (num == 0x876c17a6)
                        {
                            if (name == "LUX")
                            {
                                return 0x63;
                            }
                        }
                        else if ((num == 0x8911d12d) && (name == "RYZE"))
                        {
                            return 13;
                        }
                    }
                    else if (num <= 0x9358c912)
                    {
                        if (num <= 0x8ecf106f)
                        {
                            if (num == 0x8af6a0df)
                            {
                                if (name == "MALPHITE")
                                {
                                    return 0x36;
                                }
                            }
                            else if ((num == 0x8ecf106f) && (name == "NOCTURNE"))
                            {
                                return 0x38;
                            }
                        }
                        else if (num == 0x8fe4fdb1)
                        {
                            if (name == "ZYRA")
                            {
                                return 0x8f;
                            }
                        }
                        else if ((num == 0x9358c912) && (name == "BRAUM"))
                        {
                            return 0xc9;
                        }
                    }
                    else if (num <= 0x9aaa1902)
                    {
                        if (num == 0x95d37ce2)
                        {
                            if (name == "KATARINA")
                            {
                                return 0x37;
                            }
                        }
                        else if ((num == 0x9aaa1902) && (name == "QUINN"))
                        {
                            return 0x85;
                        }
                    }
                    else if (num == 0x9efcc5dc)
                    {
                        if (name == "KHAZIX")
                        {
                            return 0x79;
                        }
                    }
                    else if ((num == 0x9f4df966) && (name == "VARUS"))
                    {
                        return 110;
                    }
                }
                else if (num <= 0xac1a69b7)
                {
                    if (num <= 0xa405df89)
                    {
                        if (num <= 0xa1252a15)
                        {
                            if (num == 0x9fb33f5d)
                            {
                                if (name == "SHEN")
                                {
                                    return 0x62;
                                }
                            }
                            else if ((num == 0xa1252a15) && (name == "NASUS"))
                            {
                                return 0x4b;
                            }
                        }
                        else if (num == 0xa2568d6a)
                        {
                            if (name == "JINX")
                            {
                                return 0xde;
                            }
                        }
                        else if ((num == 0xa405df89) && (name == "AHRI"))
                        {
                            return 0x67;
                        }
                    }
                    else if (num <= 0xa8995680)
                    {
                        if (num == 0xa7dae946)
                        {
                            if (name == "LEONA")
                            {
                                return 0x59;
                            }
                        }
                        else if ((num == 0xa8995680) && (name == "URGOT"))
                        {
                            return 6;
                        }
                    }
                    else if (num == 0xa9f54883)
                    {
                        if (name == "VEIGAR")
                        {
                            return 0x2d;
                        }
                    }
                    else if ((num == 0xac1a69b7) && (name == "UDYR"))
                    {
                        return 0x4d;
                    }
                }
                else if (num <= 0xb46ae856)
                {
                    if (num <= 0xaf56366b)
                    {
                        if (num == 0xacce0017)
                        {
                            if (name == "THRESH")
                            {
                                return 0x19c;
                            }
                        }
                        else if ((num == 0xaf56366b) && (name == "ELISE"))
                        {
                            return 60;
                        }
                    }
                    else if (num == 0xb3bb5bb5)
                    {
                        if (name == "LEESIN")
                        {
                            return 0x40;
                        }
                    }
                    else if ((num == 0xb46ae856) && (name == "CASSIOPEIA"))
                    {
                        return 0x45;
                    }
                }
                else if (num <= 0xb7e89133)
                {
                    if (num == 0xb5e98766)
                    {
                        if (name == "VIKTOR")
                        {
                            return 0x70;
                        }
                    }
                    else if ((num == 0xb7e89133) && (name == "XERATH"))
                    {
                        return 0x65;
                    }
                }
                else if (num == 0xb8835046)
                {
                    if (name == "XINZHAO")
                    {
                        return 5;
                    }
                }
                else if ((num == 0xbab1b1e4) && (name == "TARIC"))
                {
                    return 0x2c;
                }
            }
            else if (num <= 0xdffc86f3)
            {
                if (num <= 0xce4e718d)
                {
                    if (num <= 0xc03c64e6)
                    {
                        switch (num)
                        {
                            case 0xbad8826f:
                                if (!(name == "MAOKAI"))
                                {
                                    goto Label_104C;
                                }
                                return 0x39;

                            case 0xbcb8180f:
                                if (!(name == "VOLIBEAR"))
                                {
                                    goto Label_104C;
                                }
                                return 0x6a;
                        }
                        if ((num == 0xc03c64e6) && (name == "VAYNE"))
                        {
                            return 0x43;
                        }
                    }
                    else if (num <= 0xc7060eb5)
                    {
                        if (num == 0xc600f958)
                        {
                            if (name == "DIANA")
                            {
                                return 0x83;
                            }
                        }
                        else if ((num == 0xc7060eb5) && (name == "SWAIN"))
                        {
                            return 50;
                        }
                    }
                    else if (num == 0xcd733707)
                    {
                        if (name == "MALZAHAR")
                        {
                            return 90;
                        }
                    }
                    else if ((num == 0xce4e718d) && (name == "ORIANNA"))
                    {
                        return 0x3d;
                    }
                }
                else if (num <= 0xd9614f7b)
                {
                    if (num <= 0xd64d6cdf)
                    {
                        if (num == 0xd1a4e3f3)
                        {
                            if (name == "JANNA")
                            {
                                return 40;
                            }
                        }
                        else if ((num == 0xd64d6cdf) && (name == "CORKI"))
                        {
                            return 0x2a;
                        }
                    }
                    else if (num == 0xd82f8cc7)
                    {
                        if (name == "ALISTAR")
                        {
                            return 12;
                        }
                    }
                    else if ((num == 0xd9614f7b) && (name == "POPPY"))
                    {
                        return 0x4e;
                    }
                }
                else if (num <= 0xdc1c408b)
                {
                    if (num == 0xdbca3f78)
                    {
                        if (name == "NAUTILUS")
                        {
                            return 0x6f;
                        }
                    }
                    else if ((num == 0xdc1c408b) && (name == "GNAR"))
                    {
                        return 150;
                    }
                }
                else if (num == 0xdc5930d2)
                {
                    if (name == "SEJUANI")
                    {
                        return 0x71;
                    }
                }
                else if ((num == 0xdffc86f3) && (name == "TRUNDLE"))
                {
                    return 0x30;
                }
            }
            else if (num <= 0xf4850158)
            {
                if (num <= 0xe819b2b3)
                {
                    if (num <= 0xe3ecd821)
                    {
                        if (num == 0xe0646517)
                        {
                            if (name == "TWISTEDFATE")
                            {
                                return 4;
                            }
                        }
                        else if ((num == 0xe3ecd821) && (name == "LUCIAN"))
                        {
                            return 0xec;
                        }
                    }
                    else if (num == 0xe510faec)
                    {
                        if (name == "FIZZ")
                        {
                            return 0x69;
                        }
                    }
                    else if ((num == 0xe819b2b3) && (name == "NIDALEE"))
                    {
                        return 0x4c;
                    }
                }
                else if (num <= 0xebed7502)
                {
                    if (num == 0xeafea27f)
                    {
                        if (name == "CAITLYN")
                        {
                            return 0x33;
                        }
                    }
                    else if ((num == 0xebed7502) && (name == "FIDDLESTICKS"))
                    {
                        return 9;
                    }
                }
                else if (num == 0xec19936e)
                {
                    if (name == "ASHE")
                    {
                        return 0x16;
                    }
                }
                else if ((num == 0xf4850158) && (name == "LEBLANC"))
                {
                    return 7;
                }
            }
            else if (num <= 0xf711409f)
            {
                if (num <= 0xf5ce2b4b)
                {
                    if (num == 0xf4d725e4)
                    {
                        if (name == "FIORA")
                        {
                            return 0x72;
                        }
                    }
                    else if ((num == 0xf5ce2b4b) && (name == "ANIVIA"))
                    {
                        return 0x22;
                    }
                }
                else if (num == 0xf6684ba5)
                {
                    if (name == "MORDEKAISER")
                    {
                        return 0x52;
                    }
                }
                else if ((num == 0xf711409f) && (name == "SINGED"))
                {
                    return 0x1b;
                }
            }
            else if (num <= 0xf9995dfe)
            {
                if (num == 0xf95b869b)
                {
                    if (name == "JARVAN")
                    {
                        return 0x3b;
                    }
                }
                else if ((num == 0xf9995dfe) && (name == "SYNDRA"))
                {
                    return 0x86;
                }
            }
            else if (num != 0xf9c7c754)
            {
                if ((num == 0xffb645b7) && (name == "VLADIMIR"))
                {
                    return 8;
                }
            }
            else if (name == "ZED")
            {
                return 0xee;
            }
        Label_104C:
            return 0;
        }

        public static int spellToId(string name)
        {
            uint num = <PrivateImplementationDetails><VoliBot.exe>.ComputeStringHash(name);
            if (num <= 0x50fce17f)
            {
                if (num > 0x19181530)
                {
                    switch (num)
                    {
                        case 0x1faf7efe:
                            if (!(name == "TELEPORT"))
                            {
                                goto Label_0182;
                            }
                            return 12;

                        case 0x385a8c5b:
                            if (!(name == "HEAL"))
                            {
                                goto Label_0182;
                            }
                            return 7;
                    }
                    if ((num == 0x50fce17f) && (name == "CLAIRVOYANCE"))
                    {
                        return 2;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0x7a6cda7:
                            if (!(name == "SMITE"))
                            {
                                goto Label_0182;
                            }
                            return 11;

                        case 0x1562ebbf:
                            if (!(name == "IGNITE"))
                            {
                                goto Label_0182;
                            }
                            return 14;
                    }
                    if ((num == 0x19181530) && (name == "GHOST"))
                    {
                        return 6;
                    }
                }
            }
            else if (num <= 0x58d45aa5)
            {
                switch (num)
                {
                    case 0x5222a5e0:
                        if (!(name == "GARRISON"))
                        {
                            goto Label_0182;
                        }
                        return 0x11;

                    case 0x54b95ec0:
                        if (!(name == "BARRIER"))
                        {
                            goto Label_0182;
                        }
                        return 0x15;
                }
                if ((num == 0x58d45aa5) && (name == "EXHAUST"))
                {
                    return 3;
                }
            }
            else if (num <= 0xbed00a70)
            {
                if (num == 0x92d88c49)
                {
                    if (name == "FLASH")
                    {
                        return 4;
                    }
                }
                else if ((num == 0xbed00a70) && (name == "REVIVE"))
                {
                    return 10;
                }
            }
            else if (num != 0xd0f7f736)
            {
                if ((num == 0xdd016bdf) && (name == "CLARITY"))
                {
                    return 13;
                }
            }
            else if (name == "CLEANSE")
            {
                return 1;
            }
        Label_0182:
            return 0;
        }
    }
}

