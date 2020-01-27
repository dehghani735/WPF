using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    [Serializable]
    public class Card
    {
        /*private string _cardNumber;
        private string _cardType;
        private string _track;
        private string _pinBufferA; //TODO: should be here or not? 
        */

        public Card()
        {
            CardNumber = "5894631899999999";
            CardType = "ddd";
            Track = "ffffff";
        }


        public string CardNumber { get; set; }

        public string CardType { get; set; }

        public string Track { get; set; }

        public string PinBufferA { get; set; }

        public string Sub()
        {
            return CardNumber;
        }

        static void Main()
        {
            // copy from java
            // requestMessage.toString().replace("&#28;", String.valueOf((char)28)).replace("&gt;", String.valueOf((char)62)).replace("&lt;", String.valueOf((char)60)); 
            Card cd = new Card();
            cd.Track = "44444444";
            cd.CardNumber = "5";
            cd.CardType =
                "11&#28;000&#28;&#28;&#28;18&#28;;5894631511409724=99105061710399300020?&#28;&#28;AAAC   A&#28;000000000000&#28;&gt;106&lt;?1&gt;82&lt;7&gt;9=2&#28;&#28;&#28;&#28;20000100000000000000000000";

            cd.CardType =
                cd.CardType.Replace("&#28;", ((char) 0x28).ToString())
                    .Replace("&gt;", ((char) 0x62).ToString())
                    .Replace("&lt;", ((char) 0x60).ToString());

            //Console.WriteLine( Path.DirectorySeparatorChar + "ali");
            //Console.Write((char) 0x60);
            Console.WriteLine(cd.CardType);
        }
    }
}