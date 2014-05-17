using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BSS___EKG
{
    public enum FileType{
        TEXT,
        BINARY
    }
    class InputBuffer
    {
        public static int MAX_BUFFER_SIZE = 100;
        public static List<decimal> data = new List<decimal>();
        static Input input;
        public static void Open(String filename, short channel, FileType filetype){

        }
        public static bool ReadOne(){
            return true;
        }
        public static bool ReadMany(){
            return true;
        }
        public static bool Write(decimal value){
            data.Add(value);
            return true;
        }
        public static void Clear(){
        }
            
    }
}
