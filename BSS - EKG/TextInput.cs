using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BSS___EKG
{
    class TextInput : Input
    {
        protected StreamReader reader;

        public TextInput(String file, int sleepInterval)
        {
            filename = file;
            this.sleepInterval = sleepInterval;
        }
        public override void read()
        {
            throw new NotImplementedException();
        }

        public override void stop()
        {
            throw new NotImplementedException();
        }
        

    }
}
