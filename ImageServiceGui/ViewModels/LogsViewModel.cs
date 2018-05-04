﻿using ImageServiceGui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModels
{
    class LogsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ILogsModel model;
        public LogsViewModel(ILogsModel model)
        {
            this.model = model;
            model.PropertyChanged += 
                delegate (Object sender, PropertyChangedEventArgs e) 
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                }; 
        }
        private ObservableCollection<KeyValuePair<string, string>> vm_logs/* = new ObservableCollection<KeyValuePair<string, string>>()*/;
        public ObservableCollection<KeyValuePair<string, string>> VM_Logs
        {
            get { return model.Logs; }
            set
            {
                model.Logs = value;
                NotifyPropertyChanged("VM_Logs");
            }
        }
    }
}
