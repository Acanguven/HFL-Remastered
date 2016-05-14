using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HFL_3._0
{
    public delegate void luaRecievedHandler(Socket client, LuaMessage msg);

    public class LuaMessage
    {
        public int ProcessID { get; set; }
        public string command { get; set; }
    }

    public class Bridge
    {
        private byte[] data = new byte[1024];
        private int size = 1024;
        private Socket server;

        public event luaRecievedHandler luaRecieved;

        public Bridge()
        {
            server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444);
            server.Bind(iep);
            server.Listen(5);
            server.BeginAccept(new AsyncCallback(AcceptConn), server);
        }

        void AcceptConn(IAsyncResult iar)
        {
            try
            {
                Socket oldserver = (Socket)iar.AsyncState;
                Socket client = oldserver.EndAccept(iar);
                client.BeginReceive(data, 0, size, SocketFlags.None,
                            new AsyncCallback(ReceiveData), client);
            }
            catch (Exception ex)
            {

            }
        }

        public void SendData(IAsyncResult iar)
        {
            try
            {
                Socket client = (Socket)iar.AsyncState;
                int sent = client.EndSend(iar);
                client.BeginReceive(data, 0, size, SocketFlags.None,
                            new AsyncCallback(ReceiveData), client);
            }
            catch (Exception ex)
            {

            }
        }

        public void SendData2(IAsyncResult iar)
        {
            try
            {
                Socket client = (Socket)iar.AsyncState;
                int sent = client.EndSend(iar);
            }
            catch (Exception ex)
            {

            }
        }

        public void sendAnswer(Socket client,string text)
        {
            byte[] message = Encoding.ASCII.GetBytes(text);
            client.BeginSend(message, 0, message.Length, SocketFlags.None,
                        new AsyncCallback(SendData2), client);
        }

        void ReceiveData(IAsyncResult iar)
        {

            Socket client = (Socket)iar.AsyncState;
            bool readNoProblem = true;
            int recv = new int();
            try
            {
                recv = client.EndReceive(iar);
            }
            catch (Exception ex)
            {
                readNoProblem = false;
            }
            if (readNoProblem)
            {
                if (recv == 0)
                {
                    client.Close();
                    server.BeginAccept(new AsyncCallback(AcceptConn), server);
                    return;
                }
                string receivedData = Encoding.ASCII.GetString(data, 0, recv);

                string[] words = receivedData.Split('|');
                LuaMessage cmd = new LuaMessage();
                cmd.ProcessID = int.Parse(words[0]);
                cmd.command = words[1];
                luaRecieved(client, cmd);
            }

            try
            {
                client.BeginReceive(data, 0, size, SocketFlags.None,
                        new AsyncCallback(ReceiveData), client);
            }
            catch (Exception ex)
            {

            }


        }
    }
}