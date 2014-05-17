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
        public static int MAX_BUFFER_SIZE = 100;
        public static List<decimal> dataSignal = new List<decimal>();
        public static List<decimal> dataTime = new List<decimal>();
        static private int currentPoint = 0;
        static Input input;

        public static void Open(String filename, short channel, FileType filetype){
            try
            {
                if (filetype == FileType.TEXT)
                {
                    try
                    {
                        TextInput txtInpt = new TextInput(filename, 1000);
                        txtInpt.read(channel);
                    }
                    catch (Exception) { }

                }
                else if (filetype == FileType.BINARY)
                {
                    MessageBox.Show("BIN");
                }
                else
                {
                    MessageBox.Show("nesto trece");
                }
            }
            catch(Exception){}
        }
        public static decimal ReadOne(){
            return dataSignal[currentPoint];
            currentPoint++;
        }
        public static decimal ReadOneTime()
        {
            return dataTime[currentPoint];
            currentPoint++;
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
