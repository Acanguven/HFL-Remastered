namespace LoLLauncher
{
    using System;

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string str = null;
            StringValue[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (customAttributes.Length != 0)
            {
                str = customAttributes[0].Value;
            }
            return str;
        }
    }
}

