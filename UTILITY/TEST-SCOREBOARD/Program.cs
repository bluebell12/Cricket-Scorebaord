using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace TestScoreboard
{
    class Program
    {
        static string [] controlStrings = new string [] {
            "4,---,---,---,-,--,---#",
            "4,111,111,111,1,11,111#",
            "4,222,222,222,2,22,222#",
            "4,333,333,333,3,33,333#",
            "4,444,444,444,4,44,444#",
            "4,555,555,555,5,55,555#",
            "4,666,666,666,6,66,666#",
            "4,777,777,777,7,77,777#",
            "4,888,888,888,8,88,888#",
            "4,999,999,999,9,99,999#"
        };

        static SerialPort serialPort;
        
        static void Main(string[] args)
        {
            int portNumber = 0;
            foreach (string portName in SerialPort.GetPortNames())
                Console.WriteLine($"{portNumber}  {portName}");
        
            Console.WriteLine("Enter number of port that scoreboard is connected to:");
            
            int chosenPortNumber = -1;
            if( int.TryParse(Console.ReadKey().KeyChar.ToString(), out chosenPortNumber) &&
                chosenPortNumber >= 0 &&
                chosenPortNumber < SerialPort.GetPortNames().Length )
            {
                serialPort = new SerialPort(
                    SerialPort.GetPortNames()[chosenPortNumber], 
                    57600, Parity.None, 8, StopBits.One);  
                serialPort.Handshake = Handshake.None;   

                serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived); 
                serialPort.Open();
                
                int i = 0;
                while(!Console.KeyAvailable)
                {
                    serialPort.Write(controlStrings[i++ % controlStrings.Length]);
                    Thread.Sleep(2000);
                }

                serialPort.Close();
            }
            else
                Console.WriteLine("Please enter a valid port number.");
        }

        static void DataReceived(object sender, SerialDataReceivedEventArgs e)  
        {  
            Console.Write(serialPort.ReadExisting());
        }  
    }
}