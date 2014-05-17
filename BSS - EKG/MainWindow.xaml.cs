using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
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
        public static MainWindow Instance;

        private Controller controller = new Controller();
        
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void LoadSignal_Click(object sender, RoutedEventArgs e)
        {
            controller.LoadSignal();
        }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            if (PreferencesPanel.Visibility == Visibility.Visible)
                PreferencesPanel.Visibility = Visibility.Collapsed;
            else
                PreferencesPanel.Visibility = Visibility.Visible;
        }

        private void playSignalButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Play();
        }

        private void stopSignalButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            applyPreferences();
        }

        private void applyPreferences()
        {
            controller.Duration = Convert.ToInt32(PreviewDurationTextBlock.Text);
            controller.Cycles = Convert.ToInt32(CyclesTextBlock.Text);
            controller.HR_digits = Convert.ToInt32(DecimalPlacesTextBlock.Text);

            if (ShowHR_CheckBox.IsChecked == true)
                hrStackPanel.Visibility = Visibility.Visible;
            else
                hrStackPanel.Visibility = Visibility.Collapsed;
        }

        private void PreferencesApply_Click(object sender, RoutedEventArgs e)
        {
            applyPreferences();
        }      
    }
}
