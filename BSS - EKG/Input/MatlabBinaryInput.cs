using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
 * TO BE COMPLETED
 * Currently we don't know binary format
 * in which MATLAB stores it's data.
 * That's left for the further development.
 */

namespace BSS___EKG
{
    class MatlabBinaryInput : Input
    {
        protected FileStream file;
        protected BinaryReader reader;
        public MatlabBinaryInput(String file, int sleepInterval)
        {
            filename = file;
            this.sleepInterval = sleepInterval;
        }

        public override void read(InputBuffer ib, int channel)
        {
            try
            {
                file = new FileStream(filename, FileMode.Open);
                reader = new BinaryReader(file);
            }
            catch (Exception e1)
            {
                // Greska pri otvaranju fajla
            }
            sbyte[] buffer = new sbyte[1024];
            for (int i = 0; i < 1024; i++)
            {
                buffer[i] = reader.ReadSByte();
            }       

        }
        public override void stop()
        {
            file.Close();
            reader.Close();
        }
    }
}
