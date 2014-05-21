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
        private Int32 sampleNum;
        private Decimal samplingFrequency;
        private decimal channelGain;
        private Int32 zeroADC;


        public BinaryInput(String file, int sleepInterval = 30, decimal sf = 360, decimal cg = 200, Int32 zero = 1024)
        {
            filename = file;
            this.sleepInterval = sleepInterval;
            sampleNum = 0;
            samplingFrequency = sf;
            channelGain = cg;
            zeroADC = zero;
        }

        public override void read(InputBuffer ib, int channel)
        {
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
            decimal timeStep = 1/samplingFrequency;
            
		    
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
                                    while (!ib.Write(sampleNum * timeStep, convertValue(buf[0] + (low << 8) - 4096, zeroADC, channelGain))) ;
							    else
                                    while (!ib.Write(sampleNum * timeStep, convertValue((buf[0] + (low << 8)), zeroADC, channelGain))) ;
						    flag = 1;
						    break;
					    case 1:
						    if (channel == j)
							    if (high > 127)
                                    while (!ib.Write(sampleNum * timeStep, convertValue(buf[2] + (high << 4) - 4096,zeroADC,channelGain))) ;
							    else
                                    while (!ib.Write(sampleNum * timeStep, convertValue((buf[2] + (high << 4)),zeroADC,channelGain))) ;
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
