using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class DB : Transaction
    {
        public DB(TransactionConfig transactionConfig) : base(transactionConfig)
        {
            
        }

        public override void Ali()
        {
            throw new NotImplementedException();
        }

        public override string Process(Network network)
        {
            throw new NotImplementedException();
        }
    }
}
