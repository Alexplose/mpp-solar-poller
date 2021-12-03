using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MppSolarPoller.Common
{


    public abstract class Command : INotifyPropertyChanged, ICommand
    {
        public string CommandName { get; set; }
        public int ResponseSize { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void Parse(string rawData);


        public virtual string ReadCommand(Stream hidStream)
        {
            var cmd = System.Text.Encoding.ASCII.GetBytes(CommandName);
            var crc = BitConverter.GetBytes(GenCrc16(cmd));
            var toWrite = new byte[cmd.Length + crc.Length + 1];
            System.Buffer.BlockCopy(cmd, 0, toWrite, 0, cmd.Length);
            System.Buffer.BlockCopy(crc, 0, toWrite, cmd.Length, crc.Length);
            toWrite[toWrite.Length - 1] = 0x0d;
            //Console.WriteLine(System.Text.Encoding.ASCII.GetString(toWrite));
            hidStream.Write(toWrite, 0, toWrite.Length);
            var buf = new byte[Math.Min(9, ResponseSize)];
            var resBuffer = new byte[ResponseSize];
            int read = 0;
            int totalRead = 0;
            while (totalRead < ResponseSize)
            {
                read = hidStream.Read(buf, 0, buf.Length);
                //  Console.WriteLine($"Loop TotalRead={totalRead}, Count={read}");
                if (totalRead + read > ResponseSize)
                    System.Buffer.BlockCopy(buf, 0, resBuffer, totalRead, ResponseSize - totalRead);
                else
                    System.Buffer.BlockCopy(buf, 0, resBuffer, totalRead, read);
                totalRead += read;
            }
            //Console.WriteLine($"TotalRead={totalRead}, Count={read}");
            //System.Buffer.BlockCopy(buf, 0, resBuffer, totalRead, read);

            return System.Text.Encoding.ASCII.GetString(resBuffer).Replace("\0", "");
        }

        public virtual void ProcessCommand(Stream hidStream)
        {
            Parse(ReadCommand(hidStream));
        }

        /// <summary>
        /// Gens the CRC16.
        /// CRC-1021 = X(16)+x(12)+x(5)+1
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="nByte">The n byte.</param>
        /// <returns>System.Byte[][].</returns>
        public ushort GenCrc16(byte[] c)
        {
            ushort Polynominal = 0x1021;
            ushort InitValue = 0x0;

            ushort i, j, index = 0;
            ushort CRC = InitValue;
            ushort Remainder, tmp, short_c;
            for (i = 0; i < c.Length; i++)
            {
                short_c = (ushort)(0x00ff & (ushort)c[index]);
                tmp = (ushort)((CRC >> 8) ^ short_c);
                Remainder = (ushort)(tmp << 8);
                for (j = 0; j < 8; j++)
                {

                    if ((Remainder & 0x8000) != 0)
                    {
                        Remainder = (ushort)((Remainder << 1) ^ Polynominal);
                    }
                    else
                    {
                        Remainder = (ushort)(Remainder << 1);
                    }
                }
                CRC = (ushort)((CRC << 8) ^ Remainder);
                index++;
            }
            return CRC;
        }

    }


}
