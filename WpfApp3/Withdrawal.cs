using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class Withdrawal : NdcTransactionRequestMessage
    {
        public static bool isCorrect_PIN = true;
        public static bool isAmount = true;
        public static bool isEnough_Cash = true;
        public static bool isOther = false;
        public static bool isTest1 = true;

        public Withdrawal()
        {
        }

        public Withdrawal(TransactionConfig transactionConfig) : base(transactionConfig)
        {
            // AmountEntryField = config.Amount.PadLeft(12, '0');
            AmountEntryField = transactionConfig.Amount.PadLeft(12, '0');
            OperationCodeData = "ADFI   A";
        }

        public void SetTransactionConfig(TransactionConfig tc)
        {
            this.TransactionConfig = tc;
        }

        public override void Ali()
        {
            Console.WriteLine("ali withdrawal");
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

            var replyCommand = new ReplyWithdrawal(received);

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
                AmountEntryField = "72".PadLeft(12, '0');
                Console.WriteLine("withdrawal contains: ~Amount");
            }
            else
            {
                AmountEntryField = TransactionConfig.Amount.PadLeft(12, '0');
            }
            //---------------------------------------------------------------------------------Enough_Cash
            if (TransactionConfig.ConditionSet.Contains("~Enough_Cash"))
            {
                AmountEntryField = "82".PadLeft(12, '0');
                Console.WriteLine("withdrawal contains: ~Enough_Cash");
            }
            //else
            //{
            //    AmountEntryField = TransactionConfig.Amount.PadLeft(12, '0');
            //}
            //---------------------------------------------------------------------------------Correct_PIN
            if (TransactionConfig.ConditionSet.Contains("~Correct_PIN"))
            {
                AmountEntryField = "92".PadLeft(12, '0');
                Console.WriteLine("withdrawal contains: ~Correct_PIN");
            }
            //else
            //{
            //    AmountEntryField = TransactionConfig.Amount.PadLeft(12, '0');
            //}
        }

        //000000000000
        //000000500000
        //        "11&#28;000&#28;&#28;&#28;1?&#28;;5894631511409724=99105061710399300020?&#28;&#28;ADFI   A&#28;000000500000&#28;&gt;106&lt;?1&gt;82&lt;7&gt;9=2&#28;&#28;&#28;&#28;20000100000000000000000000";
    }
}