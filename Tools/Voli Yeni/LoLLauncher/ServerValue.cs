namespace LoLLauncher
{
    using System;

    public class ServerValue : Attribute
    {
        private string _value;

        public ServerValue(string value)
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

