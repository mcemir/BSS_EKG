using Microsoft.Win32;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace BSS___EKG
{
    class Controller
    {                
        private List<int> R_peaks = new List<int>();
        private SoundPlayer localPlayer = new SoundPlayer();
        private int QRS_Threshold;


        private InputBuffer inputBuffer = new InputBuffer();
        private DispatcherTimer dispatcherTimer;
        private OxyPlot.Series.LineSeries lineSeries = new OxyPlot.Series.LineSeries(); // Lines for the plot

        public int Duration { get; set; }   // Number of samples to be shown on the graph
        public int F { get; set; }    // Sampling frequency

        public int HR_digits { get; set; }  // Number of decimal places of HR BPM
        public int Cycles { get; set; }   // Number of cycles used for HR calculation        
        public double HR { get; private set; }


        public Controller()
        {
            HR_digits = 1;
            Cycles = 2;
            QRS_Threshold = 1100;
            HR = 0;
            Duration = 500;
        }




        public void LoadSignal()
        {

            // Open the file dialog to show the file
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";                            // Default file extension
            dlg.Filter = "Text Documents (*.txt)|*.txt";        // Filter files by extension
            dlg.Title = "Select PhysioBank MIT-BIH Arrhytmia Signal";


            // Show open file dialog box
            bool result = (bool)dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Get file informations 
                string filePath = dlg.FileName;
                string fileName = System.IO.Path.GetFileName(filePath);
                string fileExtension = System.IO.Path.GetExtension(filePath);
                string directoryPath = System.IO.Path.GetDirectoryName(filePath);
                string headerFile = directoryPath + "\\" + fileName.Remove(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.')) + ".hea";


                /*
                // Read the header file
                using (TextReader reader = File.OpenText(headerFile))
                {
                    int signalsCount;       // The number of signals present in the file        
                    int samples;            // The number of samples  
                    string[] bits = reader.ReadLine().Split(' ');
                    signalsCount = Int32.Parse(bits[1]);
                    F = Int32.Parse(bits[2]);
                    samples = Int32.Parse(bits[3]);

                    // Update main windows
                    MainWindow.Instance.FrequencyTextBlock.Text = F.ToString();
                }
                */

                // Determine file type
                FileType type = FileType.BINARY;
                if (fileExtension == ".txt" || fileExtension == ".TXT")
                    type = FileType.TEXT;


                // Open the buffer
                inputBuffer.Open(filePath, 1, type);
            }
            else
                return;


            


            // Init timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)TimeSpan.FromSeconds(1.0/F).TotalMilliseconds);


            // Init plot
            PlotModel model = new PlotModel { Title = "EKG Signal" };
                model.Series.Add(lineSeries);
            MainWindow.Instance.EKG_Plot.Model = model;

            
            // Add default data to plot
            for (int i = -Duration; i < 0; i++)
                lineSeries.Points.Add(new DataPoint(i * 1.0 / 10000, 0));
             

            //lineSeries.Smooth = true;
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
                HR = 60.0 * (R_peaks.Count-1) * F / time;

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




        public void Play()
        {
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
        }


        public void Stop()
        {
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            lineSeries.Points.RemoveAt(0);
            decimal value = inputBuffer.ReadOne();
            decimal time = inputBuffer.ReadOneTimeCurrent();

            lineSeries.Points.Add(new DataPoint((double)time, (double)value));

            
            //QRS_Detect(data, index); // Check if new value is a R peak

            MainWindow.Instance.EKG_Plot.InvalidatePlot();      // This updates the plot
        }
    }
}
