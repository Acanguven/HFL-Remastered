namespace LoLLauncher
{
    using System;

    public enum Region
    {
        [LocaleValue("pt_BR"), UseGarenaValue(false), ServerValue("prod.br.lol.riotgames.com"), LoginQueueValue("https://lq.br.lol.riotgames.com/")]
        BR = 4,
        [UseGarenaValue(false), ServerValue("prod.cs.lol.riotgames.com"), LoginQueueValue("https://lq.cs.lol.riotgames.com/"), LocaleValue("en_US")]
        CS = 0x12,
        [UseGarenaValue(false), LoginQueueValue("https://lq.eun1.lol.riotgames.com/"), LocaleValue("en_GB"), ServerValue("prod.eun1.lol.riotgames.com")]
        EUN = 2,
        [ServerValue("prod.euw1.lol.riotgames.com"), UseGarenaValue(false), LoginQueueValue("https://lq.euw1.lol.riotgames.com/"), LocaleValue("en_GB")]
        EUW = 1,
        [UseGarenaValue(false), ServerValue("prod.kr.lol.riotgames.com"), LoginQueueValue("https://lq.kr.lol.riotgames.com/"), LocaleValue("ko_KR")]
        KR = 3,
        [LocaleValue("es_MX"), UseGarenaValue(false), ServerValue("prod.la1.lol.riotgames.com"), LoginQueueValue("https://lq.la1.lol.riotgames.com/")]
        LA1 = 7,
        [LocaleValue("es_MX"), UseGarenaValue(false), ServerValue("prod.la2.lol.riotgames.com"), LoginQueueValue("https://lq.la2.lol.riotgames.com/")]
        LA2 = 8,
        [UseGarenaValue(true), ServerValue("prod.lol.garenanow.com"), LocaleValue("en_US"), LoginQueueValue("https://lq.lol.garenanow.com/")]
        MY = 11,
        [UseGarenaValue(false), ServerValue("prod.na2.lol.riotgames.com"), LoginQueueValue("https://lq.na2.lol.riotgames.com/"), LocaleValue("en_US")]
        NA = 0,
        [LocaleValue("en_US"), UseGarenaValue(false), ServerValue("prod.oc1.lol.riotgames.com"), LoginQueueValue("https://lq.oc1.lol.riotgames.com/")]
        OCE = 0x11,
        [LoginQueueValue("https://lq.pbe1.lol.riotgames.com/"), UseGarenaValue(false), ServerValue("prod.pbe1.lol.riotgames.com"), LocaleValue("en_US")]
        PBE = 9,
        [UseGarenaValue(true), LocaleValue("en_US"), LoginQueueValue("https://lqph.lol.garenanow.com/"), ServerValue("prodph.lol.garenanow.com")]
        PH = 15,
        [LocaleValue("en_US"), LoginQueueValue("https://lq.ru.lol.riotgames.com/"), UseGarenaValue(false), ServerValue("prod.ru.lol.riotgames.com")]
        RU = 6,
        [ServerValue("prod.lol.garenanow.com"), LoginQueueValue("https://lq.lol.garenanow.com/"), LocaleValue("en_US"), UseGarenaValue(true)]
        SG = 10,
        [LocaleValue("en_US"), ServerValue("prod.lol.garenanow.com"), LoginQueueValue("https://lq.lol.garenanow.com/"), UseGarenaValue(true)]
        SGMY = 12,
        [ServerValue("prodth.lol.garenanow.com"), LocaleValue("en_US"), LoginQueueValue("https://lqth.lol.garenanow.com/"), UseGarenaValue(true)]
        TH = 14,
        [LocaleValue("pt_BR"), UseGarenaValue(false), LoginQueueValue("https://lq.tr.lol.riotgames.com/"), ServerValue("prod.tr.lol.riotgames.com")]
        TR = 5,
        [ServerValue("prodtw.lol.garenanow.com"), UseGarenaValue(true), LocaleValue("en_US"), LoginQueueValue("https://loginqueuetw.lol.garenanow.com/")]
        TW = 13,
        [LocaleValue("en_US"), UseGarenaValue(true), LoginQueueValue("https://lqvn.lol.garenanow.com/"), ServerValue("prodvn.lol.garenanow.com")]
        VN = 0x10
    }
}

