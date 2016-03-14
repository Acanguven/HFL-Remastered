namespace LoLLauncher
{
    using System;

    public class StringValue : Attribute
    {
        private string _value;

        public StringValue(string value)
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

