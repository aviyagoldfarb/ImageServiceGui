using ImageServiceGui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageServiceGui.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ISettingsModel model;
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }
        private ObservableCollection<KeyValuePair<string, string>> vm_settings;
        public ObservableCollection<KeyValuePair<string, string>> VM_Settings
        {
            get { return model.Settings; }
        }
        private ObservableCollection<string> vm_handlers;
        public ObservableCollection<string> VM_Handlers
        {
            get { return model.Handlers; }
        }
    }
}
