using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class Network
    {
        private string _serverName;
        private string _serverIp;
        private int _portNo;

        public string ServerName { get; set; }

        public string ServerIp { get; set; }

        public int PortNo
        {
            get { return _portNo; }
            set { _portNo = value; }
        }

        /*public Network(List<string> list)
        {
          //  _serverIp = list[0];
          //  _portNo = int.Parse(list[1]);
        }*/

        public Network()
        {

        }

        public string SendAndReceive(string sendMessage)
        {
            try
            {
                int len = sendMessage.Length;
                byte[] lenBytes = { (byte)(len / 256), (byte)(len % 256) };
                //---create a TCPClient object at the IP and port no.---
                TcpClient client = new TcpClient(ServerIp, PortNo);
                NetworkStream nwStream = client.GetStream();

                nwStream.Write(lenBytes, 0, 2);
                byte[] bytesToSend = Encoding.UTF8.GetBytes(sendMessage); // ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending: " + sendMessage);
                //Console.WriteLine("Sending: " + bytesToSend.ToString());
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                //nwStream.Write(textToSend, 0, textToSend.Length);

                //---read back the text---
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                // Console.WriteLine("Received : " + Encoding.UTF8.GetString(bytesToRead, 0, bytesRead));
                string received = Encoding.UTF8.GetString(bytesToRead, 0, bytesRead);

                // previous: Encoding.ASCII.GetString(bytesToRead, 0, bytesRead)            Console.ReadLine();
                client.Close();

                //previous: return Encoding.UTF8.GetString(bytesToRead, 0, bytesRead);
                return received;
            }
            catch (Exception e)
            {
                return e.ToString();
                //throw;
            }
            
        }

        static void Main(string[] args)
        {
            //---data to send to the server---
            //string textToSend = DateTime.Now.ToString();
            string textToSend =
                "11&#28;000&#28;&#28;&#28;18&#28;;5894631511409724=99105061710399300020?&#28;&#28;AAAC   A&#28;000000000000&#28;&gt;106&lt;?1&gt;82&lt;7&gt;9=2&#28;&#28;&#28;&#28;20000100000000000000000000";

            string rrr =
                "11&#28;000&#28;&#28;&#28;1?&#28;;5894631511409724=99105061710399300020?&#28;&#28;ADFI   A&#28;000000500000&#28;&gt;106&lt;?1&gt;82&lt;7&gt;9=2&#28;&#28;&#28;&#28;20000100000000000000000000";

            /*public static String toNDCString(String s)
                        {
                            s = s.replace(String.valueOf(RS), NonPrintableAscii.toPrint(RS));
                            s = s.replace(String.valueOf(GS), NonPrintableAscii.toPrint(GS));
                            s = s.replace(String.valueOf(SO), NonPrintableAscii.toPrint(SO));
                            s = s.replace(String.valueOf(LF), NonPrintableAscii.toPrint(LF));
                            s = s.replace(String.valueOf(FF), NonPrintableAscii.toPrint(FF));
                            s = s.replace(String.valueOf(ESC), NonPrintableAscii.toPrint(ESC));
                            return s.replace(String.valueOf(FS), NonPrintableAscii.toPrint(FS));
            }*/
            textToSend = textToSend.Replace("&#28;", "\u001C").Replace("&gt;", "\u003E").Replace("&lt;", "\u003C");
            int len = textToSend.Length;
            byte[] lenBytes = {(byte) (len/256), (byte) (len%256)};
            //---create a TCPClient object at the IP and port no.---
            TcpClient client = new TcpClient("", 0);
            NetworkStream nwStream = client.GetStream();

            nwStream.Write(lenBytes, 0, 2);
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend); // ASCIIEncoding.ASCII.GetBytes(textToSend);

            //---send the text---
            Console.WriteLine("Sending: " + textToSend);
            //Console.WriteLine("Sending: " + bytesToSend.ToString());
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            //nwStream.Write(textToSend, 0, textToSend.Length);

            //---read back the text---
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.UTF8.GetString(bytesToRead, 0, bytesRead));
            // previous: Encoding.ASCII.GetString(bytesToRead, 0, bytesRead)            Console.ReadLine();
            client.Close();
        }
    }
}