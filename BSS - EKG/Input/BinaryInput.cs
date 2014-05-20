using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace BSS___EKG
{
    class BinaryInput : Input
    {
        protected FileStream file;
        protected BinaryReader reader;
        protected TextReader header;
        protected String headerFile;
        protected struct recordDescription
        {
            public String signalID;
            public short numberOfChannels;
            public Int64 numberOfSamples;
            public decimal samplingFrequency;
            public List<List<int>> channels;
        };

        protected recordDescription recDescription;
        private Int32 sampleNum;
        

        public BinaryInput(String file, int sleepInterval = 30)
        {
            filename = file;
            headerFile = filename.Remove(filename.Length - 3) + "hea";
            this.sleepInterval = sleepInterval;
            sampleNum = 0;
           
            try
            {
                using (header = File.OpenText(headerFile))
                {
                    string tempLine = header.ReadLine();
                    string[] tempInfo = tempLine.Split(' ');

                    recDescription = new recordDescription();
                    recDescription.channels = new List<List<int>>();
                    recDescription.signalID = tempInfo[0];
                    recDescription.numberOfChannels = Convert.ToInt16(tempInfo[1]);
                    recDescription.samplingFrequency = Convert.ToDecimal(tempInfo[2]);
                    recDescription.numberOfSamples = Convert.ToInt64(tempInfo[3]);
                    while (!tempLine.Contains("#"))
                    {
                        tempLine = header.ReadLine();
                        if (tempLine.Contains("#"))
                        {
                            break;
                        }
                        tempInfo = tempLine.Split(' ');
                        recDescription.channels.Add(new List<int>());
                        for(int i=1; i<tempInfo.Length-1; i++)
                            recDescription.channels[recDescription.channels.Count - 1].Add(Convert.ToInt32(tempInfo[i])); ;
                    }
                    // Gain parametar pojacanja se nalazi u drugoj koloni liste. list[][1]
                    
                }
                
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        public override void read(InputBuffer ib, int channel)
        {
            if (channel < 0 || channel > recDescription.numberOfChannels) { return; }
            long fileLength = 0;
		    try
		    {
			    file = new FileStream(filename, FileMode.Open);
			    reader = new BinaryReader(file);
		    }
		    catch (Exception e1)
		    {
                MessageBox.Show(e1.ToString());
		    }
            decimal channelGain = recDescription.channels[channel-1][1];
            decimal timeStep = 1/recDescription.samplingFrequency;
            Int32 zero = recDescription.channels[channel-1][3];
		    
            fileLength = file.Length;
		    short flag = 0;
		    Int64 low = 0, high = 0;
		    byte[]  buf = new byte[]{ 0, 0, 0 };
		    for (int i = 0; i < fileLength / 3; i++)
		    {
			    for (short j = 1; j <= 2; j++)
			    {
				    switch (flag)
				    {
					    case 0:
						    try
						    {
							    buf = reader.ReadBytes(3);
						    }
						    catch (Exception e2)
						    {
							    return;
						    }
						    low = buf[1] & 0x0F;
						    high = buf[1] & 0xF0;
						    if (channel == j)
							    if (low > 7)
                                    while (!ib.Write(sampleNum * timeStep, convertValue(buf[0] + (low << 8) - 4096, zero, channelGain))) ;
							    else
                                    while (!ib.Write(sampleNum * timeStep, convertValue((buf[0] + (low << 8)), zero, channelGain))) ;
						    flag = 1;
						    break;
					    case 1:
						    if (channel == j)
							    if (high > 127)
                                    while (!ib.Write(sampleNum * timeStep, convertValue(buf[2] + (high << 4) - 4096,zero,channelGain))) ;
							    else
                                    while (!ib.Write(sampleNum * timeStep, convertValue((buf[2] + (high << 4)),zero,channelGain))) ;
						    flag = 0;
						    break;
				    }
			    }
		    }
		stop();
        }
        public override void stop()
        {
            file.Close();
            reader.Close();
            header.Close();
            sampleNum = 0;
        }

        private decimal convertValue(Int64 val, int zeroADC, decimal gain){
            sampleNum++;
            return (Convert.ToDecimal(val)-zeroADC)/gain;
        }

    }
}
