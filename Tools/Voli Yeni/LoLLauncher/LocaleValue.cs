namespace LoLLauncher
{
    using System;

    public class LocaleValue : Attribute
    {
        private string _value;

        public LocaleValue(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get
            {
                return this._value;
            }
        }
    }
}

