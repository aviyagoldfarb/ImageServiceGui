using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Communication
{
    interface ITcpClient
    {
        void Connect(string ip, int port);
        bool Connected();
        void Write(string command);
        string Read();  // blocking call 
        void Disconnect();
    }
}
