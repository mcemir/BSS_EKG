using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace BSS___EKG
{
    class SignalAcquisition
    {
        public static SignalAcquisition Instance { get; set;}

        private int signalsCount;       // The number of signals present in the file        
        private int samples;            // The number of samples        
        private int index = 0;          // Index of the current element        
        private List<decimal> data = new List<decimal>();
        private OxyPlot.Series.LineSeries lineSeries = new OxyPlot.Series.LineSeries(); // Lines for the plot

        private DispatcherTimer dispatcherTimer;

        public int F {get; set;}    // Sampling frequency
        public int Duration { get; set; }   // Number of samples to be shown on the graph

        public IList<DataPoint> Points { get; private set; }    // List of points that should be drawn
        
        public SignalAcquisition()
        {
            Instance = this;
            Duration = 500;
        }


        public void Init()
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
                string directoryPath = System.IO.Path.GetDirectoryName(filePath);
                string headerFile = directoryPath + "\\" + fileName.Remove(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.')) + ".hea";
                InputBuffer ib1 = new InputBuffer();
                ib1.Open(filePath, 1, FileType.TEXT);
                MessageBox.Show(ib1.ReadOneTime().ToString());
                

            }

            
                /*// Read the header file
                using (TextReader reader = File.OpenText(headerFile))
                {
                    string[] bits = reader.ReadLine().Split(' ');
                    signalsCount = Int32.Parse(bits[1]);
                    F = Int32.Parse(bits[2]);
                    samples = Int32.Parse(bits[3]);

                    MainWindow.Instance.FrequencyTextBlock.Text = F.ToString();
                }

                // Read the data file
                using (TextReader reader = File.OpenText(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] bits = line.Split(' ');
                        foreach (string bit in bits)
                        {
                            decimal value;
                            if (decimal.TryParse(bit, NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                            {
                                data.Add(value);
                            }
                        }
                    }
                }
            }


            // Init timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)TimeSpan.FromSeconds(1.0/F).TotalMilliseconds);


            // Init plot
            PlotModel model = new PlotModel { Title = "EKG Signal" };
                model.Series.Add(lineSeries);
            MainWindow.Instance.EKG_Plot.Model = model;

            // Add default data to plot
            for (int i = -Duration; i < 0; i++)
                lineSeries.Points.Add(new DataPoint(i * 1.0 / F, 950));

            //lineSeries.Smooth = true;

            */
        }

        



        public void Play()
        {
            if (dispatcherTimer != null || data.Count != 0)
            {
                dispatcherTimer.Start();
            }
        }


        public void Stop()
        {
            if (dispatcherTimer != null || data.Count != 0)
            {
                dispatcherTimer.Stop();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            lineSeries.Points.RemoveAt(0);
            lineSeries.Points.Add(new DataPoint(index * 1.0/F, (double)data[index]));



            index = (index+1)%(samples-Duration);
            MainWindow.Instance.EKG_Plot.InvalidatePlot();      // This updates the plot
        }
    }
}
