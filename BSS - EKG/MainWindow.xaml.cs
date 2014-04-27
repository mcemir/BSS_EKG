using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace BSS___EKG
{
    public partial class MainWindow : Window
    {
        SignalAcquisition acquisition = new SignalAcquisition();

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void LoadSignal_Click(object sender, RoutedEventArgs e)
        {
            acquisition.Init(plot);

            
        }

        private void playSignalButton_Click(object sender, RoutedEventArgs e)
        {
            acquisition.Play();
        }

        private void stopSignalButton_Click(object sender, RoutedEventArgs e)
        {
            acquisition.Stop();
        }



      
    }
}
