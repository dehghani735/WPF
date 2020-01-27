using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class Transfer : NdcTransactionRequestMessage
    {
        public static bool isCorrect_PIN = false;
        public static bool isAmount = true;
        public static bool isEnough_Cash = true;
        public static bool isOther = false;
        public static bool isTest1 = true;

        public Transfer()
        {
            // due to error 'No parameterless constructor defined for this object.'
        }

        public Transfer(TransactionConfig transactionConfig) : base(transactionConfig)
        {
            //TODO: is not correct
            AmountEntryField = "000000000000";
            OperationCodeData = "AAAC   A";
        }

        public void SetTransactionConfig(TransactionConfig tc)
        {
            this.TransactionConfig = tc;
        }

        public override void Ali()
        {
            Console.WriteLine("ali transfer");
        }

        public override string Process(Network network)
        {
            OnStatusStarting(TransactionConfig);
            OnStatusProcessing(TransactionConfig); // for example

            PrepareMessage();
            // before here i should change the message based on transactionConfig (98/07/08)
            //TODO: a list of Msgs that form a Plan to send
            var msg = this.GetNdcTransactionRequestMessage();

            var received = ""; // network.SendAndReceive(msg); //"" //TODO 
            Console.WriteLine("Received: " + received);

            var replyCommand = new ReplyTransfer(received);

            if (replyCommand.parseReplyCommand())
            {
                //TODO
                if (replyCommand.Equals(this.TransactionConfig.ExpectedResult))
                {
                    OnStatusPassed(TransactionConfig);
                    return this.GetType().Name + " (Passed)";
                }
                OnStatusFailed(TransactionConfig, replyCommand.ErrorMessage);
                return this.GetType().Name + " (Failed)";
            }
            else
            {
                OnStatusFailed(TransactionConfig, replyCommand.ErrorMessage);
                return this.GetType().Name + "Exception Occurred in Parsing NDCTransactionReplyCommand(Failed)";
            }
        }

        private void PrepareMessage()
        {
            //---------------------------------------------------------------------------------Amount
            if (TransactionConfig.ConditionSet.Contains("~Amount"))
            {
                AmountEntryField = "73".PadLeft(12, '0');
                Console.WriteLine("transfer contains: ~Amount");
            }
            else
            {
                AmountEntryField = TransactionConfig.Amount.PadLeft(12, '0');
            }
            //---------------------------------------------------------------------------------Enough_Cash
            if (TransactionConfig.ConditionSet.Contains("~Enough_Cash"))
            {
                AmountEntryField = "83".PadLeft(12, '0');
                Console.WriteLine("transfer contains: ~Enough_Cash");
            }
            //else
            //{
            //    AmountEntryField = TransactionConfig.Amount.PadLeft(12, '0');
            //}
            //---------------------------------------------------------------------------------Correct_PIN
            if (TransactionConfig.ConditionSet.Contains("~Correct_PIN"))
            {
                AmountEntryField = "93".PadLeft(12, '0');
                Console.WriteLine("transfer contains: ~Correct_PIN");
            }
            //else
            //{
            //    AmountEntryField = TransactionConfig.Amount.PadLeft(12, '0');
            //}
        }
    }
}