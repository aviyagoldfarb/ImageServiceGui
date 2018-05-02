﻿using ImageServiceGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel model;
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model; 
            //model.PropertyChanged+=... 
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //public void NotifyPropertyChanged(string propName) {...}

    }
}