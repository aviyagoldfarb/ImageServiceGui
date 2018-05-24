using ImageServiceGui.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceGui.Communication
{
    public class ServiceTcpClient : ITcpClient
    {
        //private IPEndPoint ep;
        private TcpClient client;
        //private NetworkStream stream;

        public event EventHandler<MessageEventArgs> ConfigRecieved;
        public event EventHandler<MessageEventArgs> LogUdated;
        public event EventHandler<MessageEventArgs> RemovedHandler;

        private ServiceTcpClient()
        {
            this.client = new TcpClient();
            //this.client.Connect("127.0.0.1", 8000);
            
            try
            {
                this.client.Connect("127.0.0.1", 8000);
            }
            catch (SocketException)
            {

            }
            this.Read();
            Thread.Sleep(100);
        }

        private static ServiceTcpClient instance;

        public static ServiceTcpClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceTcpClient();
                }
                return instance;
            }
        }
        /*
        public void Connect(string ip, int port)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                this.client.Connect(ep);
                
                new Task(() =>
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    {
                        while (true)
                        {
                            // Get result from server
                            string result = reader.ReadString();
                            ProcessAndSend(result);
                        }
                    }
                }).Start();
            }
            catch (SocketException)
            {

            }
            
            //IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            //this.client.Connect(ep);
            //this.stream = client.GetStream();
            //this.Read();
            
        }
        */


        /*
        public void Write(string command)
        {
            //NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            // Send data to server     
            writer.Write(command);
            writer.Flush();
        }
        */



        public void Write(string command)
        {
            
            if (!this.client.Connected)
            {
                return;
            }
            new Task(() =>
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    {
                        // Send data to server
                        writer.Write(command);
                        writer.Flush();
                    }
                }
                catch (Exception)
                {

                }
            }).Start();
        }

        public void Read()
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                {
                    while (true)
                    {
                        // Get result from server
                        string result = reader.ReadString();
                        ProcessAndSend(result);
                    }
                }
            }).Start();
        }

        /*
        public string Read()
        {
            NetworkStream stream = client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            
            // Get result from server
            string result = reader.ReadString();
            return result;
        }
        */

        public bool Connected()
        {
            return this.client.Connected;
        }

        public void Disconnect()
        {
            client.Close();
        }

        private void ProcessAndSend(string message)
        {
            string[] commandAndArg = message.Split('#');
            MessageEventArgs m = new MessageEventArgs(commandAndArg[1]);
            switch (commandAndArg[0])
            {
                case "ConfigRecieved":
                    ConfigRecieved?.Invoke(this, m);
                    break;
                case "LogUpdated":
                    LogUdated?.Invoke(this, m);
                    break;
                case "RemovedHandler":
                    RemovedHandler?.Invoke(this, m);
                    break;
                default:
                    break;
            }
        }
    }
}
