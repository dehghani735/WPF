using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class CompletePlan : Plan
    {
        public CompletePlan(string description, List<Object> expectedResultCollection, Network network, TransactionConfig transactionConfig) : base(description, expectedResultCollection, network, transactionConfig)
        {
        }

        public override void CreateTransactions()
        {
            //Console.WriteLine("===Start Plan.CompletePlan()===");
            Transactions.Add(new Balance(TransactionConfig));
        }
    }
}
