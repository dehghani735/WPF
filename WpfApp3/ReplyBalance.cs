using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    class ReplyBalance : NDCTransactionReplyCommand
    {
        public ReplyBalance(string replyCommand) : base(replyCommand)
        {
        }

        public override bool Equals(Object obj)
        {
            Console.WriteLine("ReplyBalance started");
            return false;
        }
    }
}
