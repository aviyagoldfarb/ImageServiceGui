using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    interface ILogsModel : INotifyPropertyChanged
    {
        // connection to the service 
        void Connect(string ip, int port);
        void Disconnect();
        void Start();

    }
}
