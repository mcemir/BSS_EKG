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
        private InputBuffer inputBuffer = new InputBuffer();
        private SignalProcessor signalProcessor = new SignalProcessor();
        private DispatcherTimer dispatcherTimer;
        private OxyPlot.Series.LineSeries lineSeries = new OxyPlot.Series.LineSeries(); // Lines for the plot

        private decimal lastTime = 0;


        // SIGNAL PROCESSOR INTERFACE
        public int HR_digits { // Number of decimal places of HR BPM
            get { return signalProcessor.HR_digits; } 
            set {signalProcessor.HR_digits = value;} 
        }  
        public int Cycles { // Number of cycles used for HR calculation    
            get { return signalProcessor.Cycles; }
            set { signalProcessor.Cycles = value; }
        }       
        public double HR {
            get { return signalProcessor.HR; } 
        }




        public int Duration { get; set; }   // Number of samples to be shown on the graph
        


        public Controller()
        {
            Duration = 2000;
        }




        public void LoadSignal()
        {

            // Open the file dialog to show the file
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";                            // Default file extension
            dlg.Filter = "MIT Arrhythmia File (*.txt, *.dat, *.mat)|*.txt; *.dat; *.mat";        // Filter files by extension
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
            if (lineSeries.Points.Count>3)
                while (lineSeries.Points.Last().X - lineSeries.Points.First().X > Duration/1000.0)
                    lineSeries.Points.RemoveAt(0);

            decimal value = inputBuffer.ReadOne();
            decimal time = inputBuffer.ReadOneTimeCurrent();

            lineSeries.Points.Add(new DataPoint((double)time, (double)value));
            
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)TimeSpan.FromSeconds((double)(time - lastTime)).TotalMilliseconds);

            signalProcessor.QRS_Detect(value, time);

            lastTime = time;
            MainWindow.Instance.EKG_Plot.InvalidatePlot();      // This updates the plot
        }
    }
}
