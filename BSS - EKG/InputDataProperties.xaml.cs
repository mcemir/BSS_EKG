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
using System.Windows.Shapes;

namespace BSS___EKG
{
    /// <summary>
    /// Interaction logic for InputDataProperties.xaml
    /// </summary>
    public partial class InputDataProperties : Window
    {
        public bool fc { get; private set; }
        public int channelToRead { get; private set; }
        public InputDataProperties(RecordDescription rd)
        {
            InitializeComponent();
            textBoxRecordID.Text = rd.signalID;
            textBoxSamplingFrequency.Text = Convert.ToString(rd.samplingFrequency);
            textBoxNumberSamples.Text = Convert.ToString(rd.numberOfSamples);
            textBoxNumberChannels.Text = Convert.ToString(rd.numberOfChannels);
            for (int i = 1; i <= rd.channels.Count; i++)
            {
                comboBoxChannel.Items.Add(i.ToString());
            }
            comboBoxChannel.SelectedIndex = 0;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.channelToRead = comboBoxChannel.SelectedIndex + 1;
            this.fc = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.channelToRead = -1;
            this.fc = false;
            this.Close();
        }
    }
}
