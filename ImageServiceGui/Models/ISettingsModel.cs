using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        ObservableCollection<KeyValuePair<string, string>> Settings { set; get; }
        
        // connection to the service 
        void Connect(string ip, int port);
        void Disconnect();
        void Start();

    }
}
