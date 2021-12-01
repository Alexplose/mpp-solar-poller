using System;
using System.IO.Ports;

namespace MonoSerial
{
  class Program
  {
    public static void Main()
    {
      SerialPort Serial_tty = new SerialPort(); 

      Serial_tty.PortName = "/dev/hidraw0"; //Assign the port name,
      //here ttyUSB0 is the name of the USB to Serial Converter used. 

      try
      {
	Serial_tty.BaudRate = 2400;
	Serial_tty.Parity = Parity.None;
	Serial_tty.DataBits = 8;
Serial_tty.StopBits = StopBits.One;
       Serial_tty.Open();//Open the Port
       Console.WriteLine("Serial Port {0} Opened",Serial_tty.PortName);
      }    
      catch (Exception e)
      {
Console.WriteLine(e.ToString());
        Console.WriteLine("ERROR in Opening Serial Port");
      }  
    }
  }
}
