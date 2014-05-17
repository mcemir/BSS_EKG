using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace BSS___EKG
{
    class SignalProcessor
    {
        private List<double> R_peaks = new List<double>();
        private SoundPlayer localPlayer = new SoundPlayer();
        private double QRS_Threshold;
        private List<decimal> data = new List<decimal>();

        public int HR_digits { get; set; }  // Number of decimal places of HR BPM
        public int Cycles { get; set; }   // Number of cycles used for HR calculation        
        public double HR { get; private set; }


        public SignalProcessor()
        {
            HR_digits = 1;
            Cycles = 2;
            QRS_Threshold = 0.6;
            HR = 0;

            // Load player
            localPlayer.SoundLocation = @"Assets\EKG_Sound_Effect.wav";
            localPlayer.Load();
        }



        public void QRS_Detect(decimal v, decimal t)
        {
            data.Add(v);


            if (data.Count > 3  &&
                data[data.Count - 2] > (decimal)QRS_Threshold &&
                data[data.Count - 2] > data[data.Count - 1] &&
                data[data.Count - 2] > data[data.Count - 3])
            {
                if (MainWindow.Instance.Sound_CheckBox.IsChecked == true)
                    playSound();


                updateHR((double)t);
            }

            while (data.Count > 5)
                data.RemoveAt(0);
        }


        private void updateHR(double index)
        {
            R_peaks.Add(index);

            if (R_peaks.Count > Cycles)
            {
                double seconds = R_peaks.Last() - R_peaks.First();
                HR = 60.0 * (R_peaks.Count - 1)/seconds;

                while (R_peaks.Count > Cycles)
                    R_peaks.RemoveAt(0);

                // Update HR TextBlock
                if (MainWindow.Instance.ShowHR_CheckBox.IsChecked == true)
                    MainWindow.Instance.hrTextBlock.Text = Math.Round(HR, HR_digits).ToString();
            }
        }

        private void playSound()
        {            
            localPlayer.Play();
        }
    }
}
