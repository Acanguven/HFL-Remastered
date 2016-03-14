namespace LoLLauncher.RiotObjects
{
    using System;
    using System.Runtime.CompilerServices;

    public class InternalNameAttribute : Attribute
    {
        public InternalNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}

