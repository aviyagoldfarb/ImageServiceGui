using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    public interface ILogsModel : INotifyPropertyChanged
    {
        ObservableCollection<KeyValuePair<string, string>> Logs { set; get; }

        // connection to the service 
        //void Connect(string ip, int port);
        void Disconnect();
        void Start();

    }
}
