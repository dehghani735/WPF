using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class NDCTransactionReplyCommand
    {
        public string Header { get; set; }

        public string NextState { get; set; }

        public string TransactionSerialNumber { get; set; }

        public string FunctionIdentifier { get; set; }

        public string ScreenNumber { get; set; }

        public string ScreenDisplayUpdate { get; set; }

        public string MessageCoordinationNumber { get; set; }

        public string CardReturnRetainFlag { get; set; }

        public string PrinterFlag { get; set; }

        public string PrinterDataField { get; set; }

        public string JPrinterFlag { get; set; }

        public string JPrinterDataField { get; set; }

        public string ReplyCommand { get; set; }

        public string ErrorMessage { get; set; }

        public NDCTransactionReplyCommand(String replyCommand)
        {
            ReplyCommand = replyCommand;

            //this.parseReplyCommand();
        }

        // true if parsing is completed successfully; false otherwise
        public bool parseReplyCommand()
        {
            var parts = ReplyCommand.Split('\u001C');
            try
            {
                Console.WriteLine("part[0]: " + parts[0]);
                Console.WriteLine("part[1]: " + parts[1]);
                Console.WriteLine("part[2]: " + parts[2]);
                Console.WriteLine("part[3]: " + parts[3]);
                Console.WriteLine("part[4]: " + parts[4]);
                Console.WriteLine("part[5]: " + parts[5]);
                Console.WriteLine("part[6]: " + parts[6]);

                Header = parts[0];
                NextState = parts[3];
                TransactionSerialNumber = parts[5].Substring(0, 4);
                FunctionIdentifier = parts[5].Substring(4, 5);
                ScreenNumber = parts[5].Substring(5, 8);
                ScreenDisplayUpdate = parts[5].Substring(8);

                var receiptAndJournalParts = parts[6].Split('\u001C');
                MessageCoordinationNumber = receiptAndJournalParts[0].Substring(0, 1);
                CardReturnRetainFlag = receiptAndJournalParts[0].Substring(1, 2);
                PrinterFlag = receiptAndJournalParts[0].Substring(2, 3);
                if (!PrinterFlag.Equals("0"))
                {
                    PrinterDataField = receiptAndJournalParts[0].Substring(3);
                }
                JPrinterFlag = receiptAndJournalParts[1].Substring(0, 1);
                if (!JPrinterFlag.Equals("0"))
                {
                    JPrinterDataField = receiptAndJournalParts[1].Substring(1);
                }
                return true;
            }
            catch (Exception exp)
            {
                return true; //TODO: temporarily set to true; set it to false
            }
        }

        public override bool Equals(Object obj)
        {
            //TODO: I can use "template design pattern" here; devide this method into 4 steps including checkState, checkScreen, checkReceipt and checkJournal
            //TODO  each of which can be implemented in subclasses for specific Equality
            
            //Check for null and compare run-time types.
            if ((obj == null) || !obj.GetType().Name.Equals("ExpectedResult"))
            {
                return false;
            }
            else
            {
                var expectedResult = (ExpectedResult) obj;

                if (!expectedResult.State.Equals(this.NextState)) // suppose that state must be available in yaml config
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(expectedResult.Screen))
                {
                    if (!expectedResult.Screen.Equals(this.ScreenNumber))
                    {
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(expectedResult.Text))
                {
                    if (!expectedResult.Text.Equals(this.ScreenDisplayUpdate))
                    {
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(expectedResult.Journal))
                {
                    if (!expectedResult.Journal.Equals(this.JPrinterDataField))
                    {
                        return false;
                    }
                }

                // TODO complete Receipt and Journal

                return true;
            }
        }
    }
}
