namespace LoLLauncher
{
    using System;
    using System.Collections.Generic;

    public class Packet
    {
        private byte[] dataBuffer;
        private int dataPos;
        private int dataSize;
        private int packetType;
        private List<byte> rawPacketBytes = new List<byte>();

        public void Add(byte b)
        {
            int dataPos = this.dataPos;
            this.dataPos = dataPos + 1;
            this.dataBuffer[dataPos] = b;
        }

        public void AddToRaw(byte b)
        {
            this.rawPacketBytes.Add(b);
        }

        public void AddToRaw(byte[] b)
        {
            this.rawPacketBytes.AddRange(b);
        }

        public byte[] GetData()
        {
            return this.dataBuffer;
        }

        public int GetPacketType()
        {
            return this.packetType;
        }

        public byte[] GetRawData()
        {
            return this.rawPacketBytes.ToArray();
        }

        public int GetSize()
        {
            return this.dataSize;
        }

        public bool IsComplete()
        {
            return (this.dataPos == this.dataSize);
        }

        public void SetSize(int size)
        {
            this.dataSize = size;
            this.dataBuffer = new byte[this.dataSize];
        }

        public void SetType(int type)
        {
            this.packetType = type;
        }
    }
}

