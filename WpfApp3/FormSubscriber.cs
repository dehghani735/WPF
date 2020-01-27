using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp3
{
    public class FormSubscriber
    {
        public string filePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\..\..\Events.txt";
        //public string filePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "";
        public void OnTransactionChangeStatus(object source, TransactionEventArgs args)
        {
            // Console.WriteLine(source + " Transaction % is in state % " + args.EventType.ToString() + " " +
            //                   args.EventMessage.ConditionSet.Count());

            //string x = 
            using (StreamWriter streamWriter = File.AppendText(filePath)) // File(filePath))
            {
                streamWriter.WriteLine(System.DateTime.Now.ToString() + " | " + source + " Transaction % is in state % " +
                                       args.EventType.ToString() + " " +
                                       args.EventMessage.ConditionSet.Count());
                //Console.WriteLine(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
                switch (args.EventType)
                {
                    case TransactionEventArgs.eStatus.Passed:
                    case TransactionEventArgs.eStatus.Failed:
                        streamWriter.WriteLine("====================================================================");
                        break;
                    default:
                        break;
                }
                streamWriter.Close(); // TODO: to the end of logging
            }
        }
    }
}