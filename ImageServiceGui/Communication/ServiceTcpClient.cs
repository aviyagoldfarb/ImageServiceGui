﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Communication
{
    class ServiceTcpClient : ITcpClient
    {
        //private IPEndPoint ep;
        private TcpClient client;

        private ServiceTcpClient()
        {
            this.client = new TcpClient();
            this.client.Connect("127.0.0.1", 8000);
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

        public void Connect(string ip, int port)
        {
            //IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client.Connect(ep);
        }

        public void Write(string command)
        {
            using (NetworkStream stream = client.GetStream())
            //using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server     
                writer.Write(command);   
            }
        }

        public string Read()
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            //using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Get result from server
                string result = reader.ReadString();
                return result;
            }
        }

        public void Disconnect()
        {
            client.Close();
        }
    }
}
