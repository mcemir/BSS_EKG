using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Input.cs
// Sadrzi abstraktne metode za kontrolu citanja
// izvorisnih fajlova. Dvije medote, jedna za iniciranje
// citanja, a druga za prekid citanja podataka.

namespace BSS___EKG
{
    abstract class Input
    {
        protected String filename;
        protected int channel;
        protected int sleepInterval;
        
        public abstract void read(int channel);
        public abstract void stop();
    }
}
