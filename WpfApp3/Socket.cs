using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public abstract class Socket : Transaction
    {
        public Socket(TransactionConfig transactionConfig) : base(transactionConfig) { }

        public Socket() { }

        public abstract string GetNdcTransactionRequestMessage();
    }
}