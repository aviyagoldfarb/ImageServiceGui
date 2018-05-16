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

        void Disconnect();
        void Start();

    }
}
