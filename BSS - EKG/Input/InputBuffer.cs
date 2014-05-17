using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
                    try
                    {
                        BinaryInput binInput = new BinaryInput(filename, 1000);
                        binInput.read(this, channel);
                    }
                    catch { }
                    
                }
                else
                {
                    MessageBox.Show("Sta si dirao!");
                }
            }
            catch(Exception){}
        }
        public decimal ReadOne(){
            return dataSignal[currentPoint];
            currentPoint++;
        }
        public decimal ReadOneTime()
        {
            return dataTime[currentPoint];
            currentPoint++;
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
