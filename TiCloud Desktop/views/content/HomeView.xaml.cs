﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TiCloud_Desktop.core.data;
using TiCloud_Desktop.views.controls;

namespace TiCloud_Desktop.views.content
{
    /// <summary>
    /// Logika interakcji dla klasy HomeView.xaml
    /// </summary>
    public partial class HomeView : System.Windows.Controls.UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            


        }

        private void UpdateTile_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("SDDDD");
        }
    }
}
