namespace LoLLauncher
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RTMPSEncoder
    {
        public long startTime = ((long) DateTime.Now.TimeOfDay.TotalMilliseconds);

        public byte[] AddHeaders(byte[] data)
        {
            List<byte> list = new List<byte> { 3 };
            long num = ((long) DateTime.Now.TimeOfDay.TotalMilliseconds) - this.startTime;
            list.Add((byte) ((num & 0xff0000L) >> 0x10));
            list.Add((byte) ((num & 0xff00L) >> 8));
            list.Add((byte) (num & 0xffL));
            list.Add((byte) ((data.Length & 0xff0000) >> 0x10));
            list.Add((byte) ((data.Length & 0xff00) >> 8));
            list.Add((byte) (data.Length & 0xff));
            list.Add(0x11);
            list.Add(0);
            list.Add(0);
            list.Add(0);
            list.Add(0);
            for (int i = 0; i < data.Length; i++)
            {
                list.Add(data[i]);
                if (((i % 0x80) == 0x7f) && (i != (data.Length - 1)))
                {
                    list.Add(0xc3);
                }
            }
            byte[] buffer = new byte[list.Count];
            for (int j = 0; j < buffer.Length; j++)
            {
                buffer[j] = list[j];
            }
            return buffer;
        }

        public byte[] Encode(object obj)
        {
            List<byte> ret = new List<byte>();
            this.Encode(ret, obj);
            byte[] buffer = new byte[ret.Count];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = ret[i];
            }
            return buffer;
        }

        public void Encode(List<byte> ret, object obj)
        {
            if (obj == null)
            {
                ret.Add(1);
            }
            else if (obj is bool)
            {
                if ((bool) obj)
                {
                    ret.Add(3);
                }
                else
                {
                    ret.Add(2);
                }
            }
            else if (obj is int)
            {
                ret.Add(4);
                this.WriteInt(ret, (int) obj);
            }
            else if (obj is double)
            {
                ret.Add(5);
                this.WriteDouble(ret, (double) obj);
            }
            else if (obj is string)
            {
                ret.Add(6);
                this.WriteString(ret, (string) obj);
            }
            else if (obj is DateTime)
            {
                ret.Add(8);
                this.WriteDate(ret, (DateTime) obj);
            }
            else if (obj is byte[])
            {
                ret.Add(12);
                this.WriteByteArray(ret, (byte[]) obj);
            }
            else if (obj is object[])
            {
                ret.Add(9);
                this.WriteArray(ret, (object[]) obj);
            }
            else if (obj is TypedObject)
            {
                ret.Add(10);
                this.WriteObject(ret, (TypedObject) obj);
            }
            else
            {
                if (!(obj is Dictionary<string, object>))
                {
                    throw new Exception("Unexpected object type: " + obj.GetType().FullName);
                }
                ret.Add(9);
                this.WriteAssociativeArray(ret, (Dictionary<string, object>) obj);
            }
        }

        public byte[] EncodeConnect(Dictionary<string, object> paramaters)
        {
            List<byte> ret = new List<byte>();
            this.WriteStringAMF0(ret, "connect");
            this.WriteIntAMF0(ret, 1);
            ret.Add(0x11);
            ret.Add(9);
            this.WriteAssociativeArray(ret, paramaters);
            ret.Add(1);
            ret.Add(0);
            this.WriteStringAMF0(ret, "nil");
            this.WriteStringAMF0(ret, "");
            TypedObject obj2 = new TypedObject("flex.messaging.messages.CommandMessage");
            obj2.Add("operation", 5);
            obj2.Add("correlationId", "");
            obj2.Add("timestamp", 0);
            obj2.Add("messageId", RandomUID());
            obj2.Add("body", new TypedObject(null));
            obj2.Add("destination", "");
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("DSMessagingVersion", 1.0);
            dictionary.Add("DSId", "my-rtmps");
            obj2.Add("headers", dictionary);
            obj2.Add("clientId", null);
            obj2.Add("timeToLive", 0);
            ret.Add(0x11);
            this.Encode(ret, obj2);
            byte[] data = new byte[ret.Count];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = ret[i];
            }
            data = this.AddHeaders(data);
            data[7] = 20;
            return data;
        }

        public byte[] EncodeInvoke(int id, object data)
        {
            List<byte> ret = new List<byte> { 0, 5 };
            this.WriteIntAMF0(ret, id);
            ret.Add(5);
            ret.Add(0x11);
            this.Encode(ret, data);
            byte[] buffer = new byte[ret.Count];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = ret[i];
            }
            return this.AddHeaders(buffer);
        }

        public static string RandomUID()
        {
            byte[] buffer = new byte[0x10];
            new Random().NextBytes(buffer);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
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
                builder.AppendFormat("{0:X2}", buffer[i]);
            }
            return builder.ToString();
        }

        private void WriteArray(List<byte> ret, object[] val)
        {
            this.WriteInt(ret, (val.Length << 1) | 1);
            ret.Add(1);
            foreach (object obj2 in val)
            {
                this.Encode(ret, obj2);
            }
        }

        private void WriteAssociativeArray(List<byte> ret, Dictionary<string, object> val)
        {
            ret.Add(1);
            foreach (string str in val.Keys)
            {
                this.WriteString(ret, str);
                this.Encode(ret, val[str]);
            }
            ret.Add(1);
        }

        private void WriteByteArray(List<byte> ret, byte[] val)
        {
            throw new NotImplementedException("Encoding byte arrays is not implemented");
        }

        private void WriteDate(List<byte> ret, DateTime val)
        {
            ret.Add(1);
            this.WriteDouble(ret, val.TimeOfDay.TotalMilliseconds);
        }

        private void WriteDouble(List<byte> ret, double val)
        {
            if (double.IsNaN(val))
            {
                ret.Add(0x7f);
                ret.Add(0xff);
                ret.Add(0xff);
                ret.Add(0xff);
                ret.Add(0xe0);
                ret.Add(0);
                ret.Add(0);
                ret.Add(0);
            }
            else
            {
                byte[] bytes = BitConverter.GetBytes(val);
                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    ret.Add(bytes[i]);
                }
            }
        }

        private void WriteInt(List<byte> ret, int val)
        {
            if ((val >= 0) && (val < 0x200000))
            {
                if (val >= 0x4000)
                {
                    ret.Add((byte) (((val >> 14) & 0x7f) | 0x80));
                }
                if (val >= 0x80)
                {
                    ret.Add((byte) (((val >> 7) & 0x7f) | 0x80));
                }
                ret.Add((byte) (val & 0x7f));
            }
            else
            {
                ret.Add((byte) (((val >> 0x16) & 0x7f) | 0x80));
                ret.Add((byte) (((val >> 15) & 0x7f) | 0x80));
                ret.Add((byte) (((val >> 8) & 0x7f) | 0x80));
                ret.Add((byte) (val & 0xff));
            }
        }

        private void WriteIntAMF0(List<byte> ret, int val)
        {
            ret.Add(0);
            byte[] bytes = BitConverter.GetBytes((double) val);
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                ret.Add(bytes[i]);
            }
        }

        private void WriteObject(List<byte> ret, TypedObject val)
        {
            if ((val.type != null) && !val.type.Equals(""))
            {
                if (val.type.Equals("flex.messaging.io.ArrayCollection"))
                {
                    ret.Add(7);
                    this.WriteString(ret, val.type);
                    this.Encode(ret, val["array"]);
                }
                else
                {
                    this.WriteInt(ret, (val.Count << 4) | 3);
                    this.WriteString(ret, val.type);
                    List<string> list = new List<string>();
                    foreach (string str2 in val.Keys)
                    {
                        this.WriteString(ret, str2);
                        list.Add(str2);
                    }
                    foreach (string str3 in list)
                    {
                        this.Encode(ret, val[str3]);
                    }
                }
            }
            else
            {
                ret.Add(11);
                ret.Add(1);
                foreach (string str in val.Keys)
                {
                    this.WriteString(ret, str);
                    this.Encode(ret, val[str]);
                }
                ret.Add(1);
            }
        }

        private void WriteString(List<byte> ret, string val)
        {
            byte[] bytes = null;
            try
            {
                bytes = new UTF8Encoding().GetBytes(val);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to encode string as UTF-8: " + val + "\n" + exception.Message);
            }
            this.WriteInt(ret, (bytes.Length << 1) | 1);
            foreach (byte num2 in bytes)
            {
                ret.Add(num2);
            }
        }

        private void WriteStringAMF0(List<byte> ret, string val)
        {
            byte[] bytes = null;
            try
            {
                bytes = new UTF8Encoding().GetBytes(val);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to encode string as UTF-8: " + val + "\n" + exception.Message);
            }
            ret.Add(2);
            ret.Add((byte) ((bytes.Length & 0xff00) >> 8));
            ret.Add((byte) (bytes.Length & 0xff));
            foreach (byte num2 in bytes)
            {
                ret.Add(num2);
            }
        }
    }
}

