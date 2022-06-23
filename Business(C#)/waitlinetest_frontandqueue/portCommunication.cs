using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace waitlinetest_frontandqueue
{
    class portCommunication
    {
        public SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        public void arduino_refresh(int code, string trans_data_people)
        {
            byte address = Convert.ToByte(code);
            byte[] information = Encoding.Default.GetBytes(trans_data_people);
            byte[] arduino_head = { 0xA9, address };
            byte[] arduino_data = { arduino_head[0], arduino_head[1], information[0], information[1], information[2], information[3], information[4], information[5], information[6] };
            try
            {
                port.Close();
                Thread.Sleep(10);
                port.Open();
                port.Write(arduino_data, 0, arduino_data.Length);
                Thread.Sleep(50);
                port.Close();
            }
            finally
            {
                Thread.Sleep(10);
                port.Open();
            }
        }
        public void LED_display(string display_data)
        {
            byte[] Head = { 0x55, 0xAA };
            byte[] temp = big5_to_gb2312(display_data);

            byte command = 0xC0;
            byte stopbit = 0;
            byte method = 1;
            byte color = 3;
            byte data_len = Convert.ToByte(temp.Length + 8);

            byte[] msg_head = { Head[0], Head[1], 255, data_len, command, 0, 0, method, 0, 0, color };
            byte[] trans = new byte[msg_head.Length + temp.Length + 1];
            Buffer.BlockCopy(msg_head, 0, trans, 0, msg_head.Length);
            Buffer.BlockCopy(temp, 0, trans, msg_head.Length, temp.Length);
            foreach (byte j in temp)
            {
                stopbit += j;
            }
            stopbit += command;
            stopbit += color;
            stopbit += method;
            trans[msg_head.Length + temp.Length] = stopbit;
            try
            {
                port.Close();
                Thread.Sleep(10);
                port.Open();
                port.Write(trans, 0, trans.Length);
                Thread.Sleep(50);
                port.Close();
            }
            finally
            {
                Thread.Sleep(10);
                port.Open();
            }
        }
        public static byte[] big5_to_gb2312(string str)
        {
            byte[] transfer = Encoding.Default.GetBytes(str);
            transfer = Encoding.Convert(Encoding.GetEncoding("BIG5"), Encoding.GetEncoding("GB2312"), transfer);
            return transfer;
        }
    }
}
