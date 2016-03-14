namespace LoLLauncher
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Script.Serialization;

    public class RTMPSDecoder
    {
        private List<ClassDefinition> classDefinitions = new List<ClassDefinition>();
        private byte[] dataBuffer;
        private int dataPos;
        private List<object> objectReferences = new List<object>();
        private List<string> stringReferences = new List<string>();

        private string ByteArrayToID(byte[] data)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                switch (i)
                {
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                        builder.Append('-');
                        break;
                }
                builder.AppendFormat("{0:X2}", data[i]);
            }
            return builder.ToString();
        }

        private object Decode()
        {
            byte num = this.ReadByte();
            switch (num)
            {
                case 0:
                    throw new Exception("Undefined data type");

                case 1:
                    return null;

                case 2:
                    return false;

                case 3:
                    return true;

                case 4:
                    return this.ReadInt();

                case 5:
                    return this.ReadDouble();

                case 6:
                    return this.ReadString();

                case 7:
                    return this.ReadXML();

                case 8:
                    return this.ReadDate();

                case 9:
                    return this.ReadArray();

                case 10:
                    return this.ReadObject();

                case 11:
                    return this.ReadXMLString();

                case 12:
                    return this.ReadByteArray();
            }
            throw new Exception("Unexpected AMF3 data type: " + num);
        }

        public object Decode(byte[] data)
        {
            this.dataBuffer = data;
            this.dataPos = 0;
            object obj2 = this.Decode();
            if (this.dataPos != this.dataBuffer.Length)
            {
                object[] objArray1 = new object[] { "Did not consume entire buffer: ", this.dataPos, " of ", this.dataBuffer.Length };
                throw new Exception(string.Concat(objArray1));
            }
            return obj2;
        }

        private object DecodeAMF0()
        {
            int num = this.ReadByte();
            switch (num)
            {
                case 0:
                    return this.ReadIntAMF0();

                case 2:
                    return this.ReadStringAMF0();

                case 3:
                    return this.ReadObjectAMF0();

                case 5:
                    return null;

                case 0x11:
                    return this.Decode();
            }
            throw new NotImplementedException("AMF0 type not supported: " + num);
        }

        public TypedObject DecodeConnect(byte[] data)
        {
            this.Reset();
            this.dataBuffer = data;
            this.dataPos = 0;
            TypedObject obj2 = new TypedObject("Invoke");
            obj2.Add("result", this.DecodeAMF0());
            obj2.Add("invokeId", this.DecodeAMF0());
            obj2.Add("serviceCall", this.DecodeAMF0());
            obj2.Add("data", this.DecodeAMF0());
            if (this.dataPos != this.dataBuffer.Length)
            {
                for (int i = this.dataPos; i < data.Length; i++)
                {
                    if (this.ReadByte() != 0)
                    {
                        throw new Exception("There is other data in the buffer!");
                    }
                }
            }
            if (this.dataPos != this.dataBuffer.Length)
            {
                object[] objArray1 = new object[] { "Did not consume entire buffer: ", this.dataPos, " of ", this.dataBuffer.Length };
                throw new Exception(string.Concat(objArray1));
            }
            return obj2;
        }

        public TypedObject DecodeInvoke(byte[] data)
        {
            this.Reset();
            this.dataBuffer = data;
            this.dataPos = 0;
            TypedObject obj2 = new TypedObject("Invoke");
            if (this.dataBuffer[0] == 0)
            {
                this.dataPos++;
                obj2.Add("version", 0);
            }
            obj2.Add("result", this.DecodeAMF0());
            obj2.Add("invokeId", this.DecodeAMF0());
            obj2.Add("serviceCall", this.DecodeAMF0());
            obj2.Add("data", this.DecodeAMF0());
            if (this.dataPos != this.dataBuffer.Length)
            {
                object[] objArray1 = new object[] { "Did not consume entire buffer: ", this.dataPos, " of ", this.dataBuffer.Length };
                throw new Exception(string.Concat(objArray1));
            }
            return obj2;
        }

        private object[] ReadArray()
        {
            int num = this.ReadInt();
            num = num >> 1;
            if ((num & 1) <= 0)
            {
                return (object[]) this.objectReferences[num];
            }
            string str = this.ReadString();
            if ((str != null) && !str.Equals(""))
            {
                throw new NotImplementedException("Associative arrays are not supported");
            }
            object[] item = new object[num];
            this.objectReferences.Add(item);
            for (int i = 0; i < num; i++)
            {
                item[i] = this.Decode();
            }
            return item;
        }

        private byte ReadByte()
        {
            this.dataPos++;
            return this.dataBuffer[this.dataPos];
        }

        private byte[] ReadByteArray()
        {
            int length = this.ReadInt();
            length = length >> 1;
            if ((length & 1) > 0)
            {
                byte[] item = this.ReadBytes(length);
                this.objectReferences.Add(item);
                return item;
            }
            return (byte[]) this.objectReferences[length];
        }

        private int ReadByteAsInt()
        {
            int num = this.ReadByte();
            if (num < 0)
            {
                num += 0x100;
            }
            return num;
        }

        private byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = this.dataBuffer[this.dataPos];
                this.dataPos++;
            }
            return buffer;
        }

        private DateTime ReadDate()
        {
            int num1 = this.ReadInt();
            if ((num1 & 1) > 0)
            {
                long num = (long) this.ReadDouble();
                DateTime item = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
                item = item.AddSeconds((double) (num / 0x3e8L));
                this.objectReferences.Add(item);
                return item;
            }
            return DateTime.MinValue;
        }

        private double ReadDouble()
        {
            long num = 0L;
            for (int i = 0; i < 8; i++)
            {
                num = (num << 8) + this.ReadByteAsInt();
            }
            return BitConverter.Int64BitsToDouble(num);
        }

        private TypedObject ReadDSA()
        {
            int num;
            TypedObject obj2 = new TypedObject("DSA");
            List<int> list = this.ReadFlags();
            for (int i = 0; i < list.Count; i++)
            {
                num = list[i];
                int bits = 0;
                switch (i)
                {
                    case 0:
                        if ((num & 1) != 0)
                        {
                            obj2.Add("body", this.Decode());
                        }
                        if ((num & 2) != 0)
                        {
                            obj2.Add("clientId", this.Decode());
                        }
                        if ((num & 4) != 0)
                        {
                            obj2.Add("destination", this.Decode());
                        }
                        if ((num & 8) != 0)
                        {
                            obj2.Add("headers", this.Decode());
                        }
                        if ((num & 0x10) != 0)
                        {
                            obj2.Add("messageId", this.Decode());
                        }
                        if ((num & 0x20) != 0)
                        {
                            obj2.Add("timeStamp", this.Decode());
                        }
                        if ((num & 0x40) != 0)
                        {
                            obj2.Add("timeToLive", this.Decode());
                        }
                        bits = 7;
                        break;

                    case 1:
                        if ((num & 1) != 0)
                        {
                            this.ReadByte();
                            byte[] buffer = this.ReadByteArray();
                            obj2.Add("clientIdBytes", buffer);
                            obj2.Add("clientId", this.ByteArrayToID(buffer));
                        }
                        if ((num & 2) != 0)
                        {
                            this.ReadByte();
                            byte[] buffer2 = this.ReadByteArray();
                            obj2.Add("messageIdBytes", buffer2);
                            obj2.Add("messageId", this.ByteArrayToID(buffer2));
                        }
                        bits = 2;
                        break;
                }
                this.ReadRemaining(num, bits);
            }
            list = this.ReadFlags();
            for (int j = 0; j < list.Count; j++)
            {
                num = list[j];
                int num5 = 0;
                if (j == 0)
                {
                    if ((num & 1) != 0)
                    {
                        obj2.Add("correlationId", this.Decode());
                    }
                    if ((num & 2) != 0)
                    {
                        this.ReadByte();
                        byte[] buffer3 = this.ReadByteArray();
                        obj2.Add("correlationIdBytes", buffer3);
                        obj2.Add("correlationId", this.ByteArrayToID(buffer3));
                    }
                    num5 = 2;
                }
                this.ReadRemaining(num, num5);
            }
            return obj2;
        }

        private TypedObject ReadDSK()
        {
            TypedObject obj2 = this.ReadDSA();
            obj2.type = "DSK";
            List<int> list = this.ReadFlags();
            for (int i = 0; i < list.Count; i++)
            {
                this.ReadRemaining(list[i], 0);
            }
            return obj2;
        }

        private List<int> ReadFlags()
        {
            int num;
            List<int> list = new List<int>();
            do
            {
                num = this.ReadByteAsInt();
                list.Add(num);
            }
            while ((num & 0x80) != 0);
            return list;
        }

        private int ReadInt()
        {
            int num = this.ReadByteAsInt();
            if (num < 0x80)
            {
                return num;
            }
            num = (num & 0x7f) << 7;
            int num2 = this.ReadByteAsInt();
            if (num2 < 0x80)
            {
                num |= num2;
            }
            else
            {
                num = (num | (num2 & 0x7f)) << 7;
                num2 = this.ReadByteAsInt();
                if (num2 < 0x80)
                {
                    num |= num2;
                }
                else
                {
                    num = (num | (num2 & 0x7f)) << 8;
                    num2 = this.ReadByteAsInt();
                    num |= num2;
                }
            }
            return (-(num & 0x10000000) | num);
        }

        private int ReadIntAMF0()
        {
            return (int) this.ReadDouble();
        }

        private List<object> ReadList()
        {
            List<object> list;
            int num = this.ReadInt();
            num = num >> 1;
            if ((num & 1) <= 0)
            {
                return (List<object>) this.objectReferences[num];
            }
            string str = this.ReadString();
            if ((str != null) && !str.Equals(""))
            {
                throw new NotImplementedException("Associative arrays are not supported");
            }
            list = new List<object> {
                list
            };
            for (int i = 0; i < num; i++)
            {
                list.Add(this.Decode());
            }
            return list;
        }

        private object ReadObject()
        {
            ClassDefinition definition;
            int num = this.ReadInt();
            num = num >> 1;
            if ((num & 1) <= 0)
            {
                return this.objectReferences[num];
            }
            num = num >> 1;
            if ((num & 1) > 0)
            {
                definition = new ClassDefinition {
                    type = this.ReadString(),
                    externalizable = (num & 1) > 0
                };
                num = num >> 1;
                definition.dynamic = (num & 1) > 0;
                num = num >> 1;
                for (int j = 0; j < num; j++)
                {
                    definition.members.Add(this.ReadString());
                }
                this.classDefinitions.Add(definition);
            }
            else
            {
                definition = this.classDefinitions[num];
            }
            TypedObject item = new TypedObject(definition.type);
            this.objectReferences.Add(item);
            if (definition.externalizable)
            {
                if (definition.type.Equals("DSK"))
                {
                    return this.ReadDSK();
                }
                if (definition.type.Equals("DSA"))
                {
                    return this.ReadDSA();
                }
                if (definition.type.Equals("flex.messaging.io.ArrayCollection"))
                {
                    return TypedObject.MakeArrayCollection((object[]) this.Decode());
                }
                if (!definition.type.Equals("com.riotgames.platform.systemstate.ClientSystemStatesNotification") && !definition.type.Equals("com.riotgames.platform.broadcast.BroadcastNotification"))
                {
                    throw new NotImplementedException("Externalizable not handled for " + definition.type);
                }
                int length = 0;
                for (int k = 0; k < 4; k++)
                {
                    length = (length * 0x100) + this.ReadByteAsInt();
                }
                byte[] buffer = this.ReadBytes(length);
                StringBuilder builder = new StringBuilder();
                for (int m = 0; m < buffer.Length; m++)
                {
                    builder.Append(Convert.ToChar(buffer[m]));
                }
                item = new JavaScriptSerializer().Deserialize<TypedObject>(builder.ToString());
                item.type = definition.type;
                return item;
            }
            for (int i = 0; i < definition.members.Count; i++)
            {
                string key = definition.members[i];
                object obj3 = this.Decode();
                item.Add(key, obj3);
            }
            if (definition.dynamic)
            {
                string str2;
                while ((str2 = this.ReadString()).Length != 0)
                {
                    object obj4 = this.Decode();
                    item.Add(str2, obj4);
                }
            }
            return item;
        }

        private TypedObject ReadObjectAMF0()
        {
            string str;
            TypedObject obj2 = new TypedObject("Body");
            while (!(str = this.ReadStringAMF0()).Equals(""))
            {
                byte num = this.ReadByte();
                switch (num)
                {
                    case 0:
                    {
                        obj2.Add(str, this.ReadDouble());
                        continue;
                    }
                    case 2:
                    {
                        obj2.Add(str, this.ReadStringAMF0());
                        continue;
                    }
                }
                if (num != 5)
                {
                    throw new NotImplementedException("AMF0 type not supported: " + num);
                }
                obj2.Add(str, null);
            }
            this.ReadByte();
            return obj2;
        }

        private void ReadRemaining(int flag, int bits)
        {
            if ((flag >> bits) != 0)
            {
                for (int i = bits; i < 6; i++)
                {
                    if (((flag >> i) & 1) != 0)
                    {
                        this.Decode();
                    }
                }
            }
        }

        private string ReadString()
        {
            string str;
            int length = this.ReadInt();
            length = length >> 1;
            if ((length & 1) <= 0)
            {
                return this.stringReferences[length];
            }
            if (length == 0)
            {
                return "";
            }
            byte[] bytes = this.ReadBytes(length);
            try
            {
                str = new UTF8Encoding().GetString(bytes);
            }
            catch (Exception exception)
            {
                object[] objArray1 = new object[] { "Error parsing AMF3 string from ", bytes, "\n", exception.Message };
                throw new Exception(string.Concat(objArray1));
            }
            this.stringReferences.Add(str);
            return str;
        }

        private string ReadStringAMF0()
        {
            string str;
            int length = (this.ReadByteAsInt() << 8) + this.ReadByteAsInt();
            if (length == 0)
            {
                return "";
            }
            byte[] bytes = this.ReadBytes(length);
            try
            {
                str = new UTF8Encoding().GetString(bytes);
            }
            catch (Exception exception)
            {
                object[] objArray1 = new object[] { "Error parsing AMF0 string from ", bytes, "\n", exception.Message };
                throw new Exception(string.Concat(objArray1));
            }
            return str;
        }

        private string ReadXML()
        {
            throw new NotImplementedException("Reading of XML is not implemented");
        }

        private string ReadXMLString()
        {
            throw new NotImplementedException("Reading of XML strings is not implemented");
        }

        private void Reset()
        {
            this.stringReferences.Clear();
            this.objectReferences.Clear();
            this.classDefinitions.Clear();
        }
    }
}

