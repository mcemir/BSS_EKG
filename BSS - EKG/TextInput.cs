using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

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
        public override void read(int channel)
        {
            String line = "";
		    decimal signal;
            MessageBox.Show("init");
		    try
		    {
			    try
			    {
				    reader = new StreamReader(filename);
                    MessageBox.Show("reader");
			    }
			    catch (Exception e1)
			    {
				    // Izuzetak pri vec otvorenom ili
                    // neuspjesno pokrenutim StreamReader objektom
			    }

			    line = reader.ReadLine();

			    while (!(line.Contains("0.000")))       //Dio koda koji pronalazi prvu liniju sa korisnim
			    {                                       //inforamcijama o vrijednosti signala tj. tacne
				    if (line == null)                   //registrovane vrijednosti signala.
					    break;
				    try
				    {
					    line = reader.ReadLine();
				    }
				    catch (Exception e2)
				    {
					    return;
				    }
			    }

			    do
			    {
				    signal = (decimal.Parse(line.Split('\t')[channel]));
				    signal = signal / 1000;                                 // signal je cjelobrojni na 11 bita - dijeli se sa 1000 - mV
                    InputBuffer.Write(signal);                              // simulacija cekanja na prekid od ulaznog uredjaja -  
                } while ((line = reader.ReadLine()) != null);               // kad istekne vrijeme ucita sesljedeca vrijednost EKG signala    
            }
		    catch(Exception e)
		    {
                
                // Catch blok glavnog try scopa
		    }
		    stop();
            
        }

        public override void stop()
        {
            reader.Close();
        }
        

    }
}
