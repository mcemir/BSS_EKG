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

    public struct RecordDescription
    {
        public String signalID;
        public short numberOfChannels;
        public Int64 numberOfSamples;
        public decimal samplingFrequency;
        public List<List<int>> channels;
    }
    class InputBuffer
    {
        public List<decimal> dataSignal = new List<decimal>();
        public List<decimal> dataTime = new List<decimal>();
        private int currentPoint = 0;
        private Input input;
        public RecordDescription recDescription;
  

        public InputBuffer(){


        }

        public bool prepareBinaryInfo(String filename)
        {
            String headerFile = filename.Remove(filename.Length - 3) + "hea";
            try
            {
                using (TextReader header = File.OpenText(headerFile))
                {
                    string tempLine = header.ReadLine();
                    string[] tempInfo = tempLine.Split(' ');

                    recDescription = new RecordDescription();
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
                        for (int i = 1; i < tempInfo.Length - 1; i++)
                            recDescription.channels[recDescription.channels.Count - 1].Add(Convert.ToInt32(tempInfo[i])); ;
                    }
                }
            }catch(Exception e1){
                return false;
            }
            return true;
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
                        prepareBinaryInfo(filename);
                        if(channel<1 || channel>recDescription.channels.Count){
                            MessageBox.Show("Greska pri citanju binarne datoteke.");
                        }
                        else{
                            BinaryInput binInput = new BinaryInput(filename, 1000);
                            binInput.read(this, channel); 
                        }
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

        private bool checkBoundaries()
        {
            if (recDescription.numberOfSamples == 0) return false;
            else if (currentPoint == recDescription.numberOfSamples && currentPoint > 0)
            {
                currentPoint--;
            }
            return true;
        }

        public decimal ReadOne(){
                return dataSignal[currentPoint++];
        }
        public decimal ReadOneTime()
        {
                return dataTime[currentPoint++];
  
        }

        public decimal ReadOneCurrent()
        {
            return dataSignal[currentPoint];
        }
        public decimal ReadOneTimeCurrent()
        {
            return dataTime[currentPoint];
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
