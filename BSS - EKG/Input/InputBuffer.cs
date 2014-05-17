using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
<<<<<<< HEAD
using System.Windows;

namespace BSS___EKG
{
    public enum FileType{
        TEXT,
        BINARY
    }
    class InputBuffer
    {
        public static int MAX_BUFFER_SIZE = 100;
        public static List<decimal> dataSignal = new List<decimal>();
        public static List<decimal> dataTime = new List<decimal>();
        static private int currentPoint = 0;
        static Input input;

        public static void Open(String filename, short channel, FileType filetype){
=======
using System.Windows;

namespace BSS___EKG
{
    public enum FileType{
        TEXT,
        BINARY
    }
    class InputBuffer
    {
        public int MAX_BUFFER_SIZE = 100;
        public List<decimal> dataSignal = new List<decimal>();
        public List<decimal> dataTime = new List<decimal>();
        private int currentPoint = 0;
        Input input;

        public InputBuffer()
        {

        }
        public void Open(String filename, short channel, FileType filetype){
>>>>>>> 0424a7a7bbf32c2b7eabb0a8359d5a5923f78161
            try
            {
                if (filetype == FileType.TEXT)
                {
                    try
                    {
                        TextInput txtInpt = new TextInput(filename, 1000);
                        txtInpt.read(this, channel);
                    }
                    catch (Exception) { }

                }
                else if (filetype == FileType.BINARY)
                {
                    BinaryInput binInput = new BinaryInput(filename, 1000);
                    binInput.read(this,channel);
                }
                else
                {
                    MessageBox.Show("nesto trece");
                }
<<<<<<< HEAD
            }
            catch(Exception){}
        }
        public static decimal ReadOne(){
=======
            }
            catch(Exception){}
        }
        public decimal ReadOne(){
>>>>>>> 0424a7a7bbf32c2b7eabb0a8359d5a5923f78161
            return dataSignal[currentPoint];
            currentPoint++;
        }
        public decimal ReadOneTime()
        {
            return dataTime[currentPoint];
            currentPoint++;
<<<<<<< HEAD
        }
        public static List<decimal> ReadMany(){
            return new List<decimal>();
        }
        public static bool Write(decimal timeValue, decimal signalValue){
            dataSignal.Add(signalValue);
            dataTime.Add(timeValue);
            return true;
        }
        public static void Clear(){
        }
            
    }
}
=======
        }
        public List<decimal> ReadMany(){
            return new List<decimal>();
        }
        public bool Write(decimal timeValue, decimal signalValue){
            dataSignal.Add(signalValue);
            dataTime.Add(timeValue);
            return true;
        }
        public void Clear(){
        }
            
    }
}
>>>>>>> 0424a7a7bbf32c2b7eabb0a8359d5a5923f78161
