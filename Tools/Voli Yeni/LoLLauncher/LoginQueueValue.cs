namespace LoLLauncher
{
    using System;

    public class LoginQueueValue : Attribute
    {
        private string _value;

        public LoginQueueValue(string value)
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

