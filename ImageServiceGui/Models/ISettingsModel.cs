using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Models
{
    public interface ISettingsModel : INotifyPropertyChanged
    {
        ObservableCollection<KeyValuePair<string, string>> Settings { set; get; }
        ObservableCollection<string> Handlers { set; get; }
        string SelectedHandler { set; get; }

        void Disconnect();
        void Start();
        void RemoveHandler(string handlerPath);
    }
}
