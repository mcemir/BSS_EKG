using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BSS___EKG
{
    class BinaryInput : Input
    {
        protected FileStream file;
        protected StreamReader reader;

        public BinaryInput(String filename, int sleepInterval = 30)
        {

        }

        public override void read(InputBuffer ib, int channel)
        {
            BinaryInput binInput = new BinaryInput(filename, 1000);
            binInput.read(ib, channel);
        }
        public override void stop()
        {
            throw new NotImplementedException();
        }

    }
}
