using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BSS___EKG
{
    class Controller
    {
        public static Controller Instance;
                
        private List<int> R_peaks = new List<int>();
        private SoundPlayer localPlayer = new SoundPlayer();
        private int QRS_Threshold;

        public int HR_digits { get; set; }  // Number of decimal places of HR BPM
        public int Cycles { get; set; }   // Number of cycles used for HR calculation        
        public double HR { get; private set; }


        public Controller()
        {
            Instance = this;
            HR_digits = 1;
            Cycles = 2;
            QRS_Threshold = 1100;
            HR = 0;
        }


        public void QRS_Detect(List<decimal> data, int index)
        {
            if (data[index] > QRS_Threshold && 
                data[index] > data[index - 1] && 
                data[index] > data[index+1])
            {
                if (MainWindow.Instance.Sound_CheckBox.IsChecked ==  true)
                    playSound();

                
                updateHR(index);                
            }
        }


        private void updateHR(int index)
        {
            R_peaks.Add(index);

            if (R_peaks.Count > 2) {
                int time = R_peaks.Last() - R_peaks.First();
                HR = 60.0 * (R_peaks.Count-1) * SignalAcquisition.Instance.F / time;

                while (R_peaks.Count > Cycles)
                    R_peaks.RemoveAt(0);

                // Update HR TextBlock
                if (MainWindow.Instance.ShowHR_CheckBox.IsChecked == true)
                    MainWindow.Instance.hrTextBlock.Text = Math.Round(HR, HR_digits).ToString();
            }  
        }

        private void playSound()
        {
            localPlayer.SoundLocation = @"Assets\EKG_Sound_Effect.wav";
            localPlayer.Load();
            localPlayer.Play();
        }
    }
}
