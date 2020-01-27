using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class ReversePlan : Plan
    {
        public ReversePlan(string description, List<Object> expectedResultCollection, Network network, TransactionConfig transactionConfig) : base(description, expectedResultCollection, network, transactionConfig)
        {
        }

        public override void CreateTransactions()
        {
            //Console.WriteLine("===Start Plan.ReversePlan()===");
            Transactions.Add(new Transfer(TransactionConfig));
        }
    }
}
