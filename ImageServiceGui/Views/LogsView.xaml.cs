﻿using ImageServiceGui.Communication;
using ImageServiceGui.Models;
using ImageServiceGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGui
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        LogsViewModel vm;
        /// <summary>
        /// constructor
        /// </summary>
        public LogsView()
        {
            InitializeComponent();
            vm = new LogsViewModel(new LogsModel());
            DataContext = vm;
        }
    }
}
