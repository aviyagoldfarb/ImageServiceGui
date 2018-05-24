using ImageServiceGui.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Communication
{
    interface ITcpClient
    {
        event EventHandler<MessageEventArgs> ConfigRecieved;
        event EventHandler<MessageEventArgs> LogUdated;
        event EventHandler<MessageEventArgs> RemovedHandler;

        //void Connect(string ip, int port);
        bool Connected();
        void Write(string command);
        //string Read();  // blocking call
        void Read();  // blocking call
        void Disconnect();
    }
}
