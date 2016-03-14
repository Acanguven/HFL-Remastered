namespace LoLLauncher
{
    using System;
    using System.Collections.Generic;

    public class ClassDefinition
    {
        public bool dynamic;
        public bool externalizable;
        public List<string> members = new List<string>();
        public string type;
    }
}

