namespace LoLLauncher
{
    using System;

    public class UseGarenaValue : Attribute
    {
        private bool _value;

        public UseGarenaValue(bool value)
        {
            this._value = value;
        }

        public bool Value
        {
            get
            {
                return this._value;
            }
        }
    }
}

