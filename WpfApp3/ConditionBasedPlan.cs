using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class ConditionBasedPlan : Plan // TODO: as ConcreteCommand
    {
        public ConditionBasedPlan(string description, List<Object> expectedResultCollection, Network network,
            TransactionConfig transactionConfig)
            : base(description, expectedResultCollection, network, transactionConfig)
        {
        }

        public override void CreateTransactions()
        {
            // Console.WriteLine("===Start Plan.ConditionBasedPlan()===");
            TransactionConfig.Description = Description;
            TransactionConfig.ExpectedResult = FindExpectedResult("Balance", this.Description.Split(',')[1]);
            TransactionConfig.SetConditionSet(""); // parameter is nothing ( because all of them are true)
            Transactions.Add(new Balance(TransactionConfig));

            // ===============================================================================
            var transactionName = this.Description.Split(',')[0];
            string objectToInstantiate = "TransactionTest." + transactionName;
            var objectType = Type.GetType(objectToInstantiate);
            var instantiatedObject = Activator.CreateInstance(objectType);

            // object Cloning
            var withdrawalTransactionConfig = TransactionConfig.DeepCopy<TransactionConfig>(TransactionConfig);
            withdrawalTransactionConfig.ExpectedResult = FindExpectedResult(transactionName,
                this.Description.Split(',')[1]);
            withdrawalTransactionConfig.SetConditionSet(this.Description.Split(',')[1]);

            objectType.InvokeMember("SetTransactionConfig", BindingFlags.InvokeMethod, null,
                instantiatedObject,
                new object[] {withdrawalTransactionConfig});
            Transactions.Add((Transaction) instantiatedObject);

            // ===============================================================================

            Transactions.Add(new Balance(TransactionConfig));

            // ===============================================================================
        }

        public ExpectedResult FindExpectedResult(string transactionName, string condition)
        {
            var transactionNames = (List<string>) ExpectedResultCollection[0];
            var expectedResultList = (List<Object>) ExpectedResultCollection[1];
            // Descrition string change to HashSet()
            string[] conditionList = condition.TrimStart(' ').TrimEnd(' ').Split(' ');
            HashSet<string> conditionSet = new HashSet<string>();

            foreach (var lst in conditionList)
            {
                conditionSet.Add(lst);
            }

            // bool exist = true;
            int index = transactionNames.FindIndex(item => item.Equals(transactionName));
            //Global changed to first parameter
            if (index == -1)
            {
                // search in Global 
                var selectedTransactionDict = (Dictionary<HashSet<string>, ExpectedResult>) expectedResultList[0];
                if (selectedTransactionDict.ContainsKey(conditionSet))
                {
                    return selectedTransactionDict[conditionSet];
                }
                else
                {
                    Console.WriteLine("Exception Occurred, key not found in the list");
                    return null;
                }
            }

            var selectedTransactionDict1 = (Dictionary<HashSet<string>, ExpectedResult>) expectedResultList[index];

            try
            {
                if (selectedTransactionDict1.ContainsKey(conditionSet))
                {
                    return selectedTransactionDict1[conditionSet];
                }
                else
                {
                    // search in Global 
                    selectedTransactionDict1 = (Dictionary<HashSet<string>, ExpectedResult>) expectedResultList[0];
                    if (selectedTransactionDict1.ContainsKey(conditionSet))
                    {
                        return selectedTransactionDict1[conditionSet];
                    }
                    else
                    {
                        Console.WriteLine("Exception Occurred, key not found in the list");
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Exception Occurred, 2");
            }
            return null;
        }
    }
}
