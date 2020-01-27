using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class BadDataPlan : Plan
    {
        public BadDataPlan(string description, List<Object> expectedResultCollection, Network network, TransactionConfig transactionConfig) : base(description, expectedResultCollection, network, transactionConfig)
        {
        }

        public override void CreateTransactions()
        {
            // Console.WriteLine("===Start Plan.BadDataPlan()===");
            Transactions.Add(new Balance(TransactionConfig));
            Transactions.Add(new Withdrawal(TransactionConfig));
            Transactions.Add(new Balance(TransactionConfig));
        }


    }
}
