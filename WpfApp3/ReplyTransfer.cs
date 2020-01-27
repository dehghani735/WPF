using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{   
    // created 980708
    class ReplyTransfer : NDCTransactionReplyCommand
    {
        public ReplyTransfer(string replyCommand) : base(replyCommand)
        {
        }

        public override bool Equals(Object obj)
        {
            // Specifically Equal method for Transfer transaction
            ErrorMessage = "Journal is not the same";
            Console.WriteLine("ReplyTransfer started");
            return false;
        }
    }
}