using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    class ReplyWithdrawal : NDCTransactionReplyCommand
    {
        public ReplyWithdrawal(string replyCommand) : base(replyCommand)
        {
        }

        public override bool Equals(Object obj)
        {
            // Specifically Equal method for Withdrawal transaction

            ErrorMessage = "Receipt is not the same";
            Console.WriteLine("ReplyWithdrawal started");
            return false;
        }
    }
}
