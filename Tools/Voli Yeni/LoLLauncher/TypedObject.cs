namespace LoLLauncher
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TypedObject : Dictionary<string, object>
    {
        private static long serialVersionUID = 0x11468446e7eabd77L;
        public string type;

        public TypedObject()
        {
            this.type = null;
        }

        public TypedObject(string type)
        {
            this.type = type;
        }

        public object[] GetArray(string key)
        {
            if ((base[key] is TypedObject) && this.GetTO(key).type.Equals("flex.messaging.io.ArrayCollection"))
            {
                return (object[]) this.GetTO(key)["array"];
            }
            return (object[]) base[key];
        }

        public bool GetBool(string key)
        {
            return (bool) base[key];
        }

        public double? GetDouble(string key)
        {
            object obj2 = base[key];
            if (obj2 == null)
            {
                return null;
            }
            if (obj2 is double)
            {
                return new double?((double) obj2);
            }
            return new double?(Convert.ToDouble((int) obj2));
        }

        public int? GetInt(string key)
        {
            object obj2 = base[key];
            if (obj2 == null)
            {
                return null;
            }
            if (obj2 is int)
            {
                return new int?((int) obj2);
            }
            return new int?(Convert.ToInt32((double) obj2));
        }

        public string GetString(string key)
        {
            return (string) base[key];
        }

        public TypedObject GetTO(string key)
        {
            if (base.ContainsKey(key) && (base[key] is TypedObject))
            {
                return (TypedObject) base[key];
            }
            return null;
        }

        public static TypedObject MakeArrayCollection(object[] data)
        {
            TypedObject obj1 = new TypedObject("flex.messaging.io.ArrayCollection");
            obj1.Add("array", data);
            return obj1;
        }

        public override string ToString()
        {
            if (this.type == null)
            {
                return base.ToString();
            }
            if (this.type.Equals("flex.messaging.io.ArrayCollection"))
            {
                StringBuilder builder = new StringBuilder();
                object[] objArray = (object[]) base["array"];
                builder.Append("ArrayCollection[");
                for (int i = 0; i < objArray.Length; i++)
                {
                    builder.Append(objArray[i]);
                    if (i < (objArray.Length - 1))
                    {
                        builder.Append(", ");
                    }
                }
                builder.Append(']');
                return builder.ToString();
            }
            string str = "";
            foreach (KeyValuePair<string, object> pair in this)
            {
                object[] objArray1 = new object[] { str, pair.Key, " : ", pair.Value, "\n" };
                str = string.Concat(objArray1);
            }
            return (str + this.type + ":" + base.ToString());
        }
    }
}

