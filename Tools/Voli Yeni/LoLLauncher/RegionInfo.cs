namespace LoLLauncher
{
    using System;

    public static class RegionInfo
    {
        public static string GetLocaleValue(Enum value)
        {
            string str = null;
            LocaleValue[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(LocaleValue), false) as LocaleValue[];
            if (customAttributes.Length != 0)
            {
                str = customAttributes[0].Value;
            }
            return str;
        }

        public static string GetLoginQueueValue(Enum value)
        {
            string str = null;
            LoginQueueValue[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(LoginQueueValue), false) as LoginQueueValue[];
            if (customAttributes.Length != 0)
            {
                str = customAttributes[0].Value;
            }
            return str;
        }

        public static string GetServerValue(Enum value)
        {
            string str = null;
            ServerValue[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(ServerValue), false) as ServerValue[];
            if (customAttributes.Length != 0)
            {
                str = customAttributes[0].Value;
            }
            return str;
        }

        public static bool GetUseGarenaValue(Enum value)
        {
            bool flag = false;
            UseGarenaValue[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(UseGarenaValue), false) as UseGarenaValue[];
            if (customAttributes.Length != 0)
            {
                flag = customAttributes[0].Value;
            }
            return flag;
        }
    }
}

