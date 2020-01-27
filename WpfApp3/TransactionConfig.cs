using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WpfApp3
{
    [Serializable]
    public class TransactionConfig
    {
        private Card _card;
        private string _amount;
        private ExpectedResult _expectedResult;
        private HashSet<string> _conditionSet;
        private List<string> _badData;
        private string _description; // added 981024 for UI and Events
        //private List<Card> card;
        //private List<Network> networks;

        public ExpectedResult ExpectedResult
        {
            get { return _expectedResult; }
            set { _expectedResult = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public TransactionConfig()
        {
            _amount = "200000";
            _conditionSet = new HashSet<string>();
        }

        public TransactionConfig(Card card) : this()
        {
            _card = card;
        }

        public TransactionConfig(Card card, string pinBufferA) : this(card)
        {
            _card.PinBufferA = pinBufferA; // set the incorrect pin
        }

        public TransactionConfig(string amount, List<string> badData, Card card)
        {
            _conditionSet = new HashSet<string>();
            _amount = amount;
            _badData = badData;
            _card = card;
        }

        public void SetConditionSet(string conditionString)
        {
            _conditionSet.Clear();

            if (conditionString.Equals(""))
                return;
            string[] conditions = conditionString.Split(' ');
            foreach (var cdn in conditions)
            {
                this._conditionSet.Add(cdn);
            }
        }

        public Card Card
        {
            get { return _card; }
        }

        public string Amount
        {
            get { return _amount; }
        }

        public HashSet<string> ConditionSet
        {
            get { return this._conditionSet; }
        }

        public static T DeepCopy<T>(T obj)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new Exception("The source object must be serializable");
            }
            if (Object.ReferenceEquals(obj, null))
            {
                throw new Exception("The source object must not be null");
            }
            T result = default(T);
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                result = (T)formatter.Deserialize(memoryStream);
                memoryStream.Close();
            }
            return result;
        }

        static void Main()
        {
            Console.WriteLine("ali");
            TransactionConfig tc = new TransactionConfig();
            ExpectedResult eee = new ExpectedResult();
            //  eee.Condition = "ddd";
            eee.Category = "ddd";
            eee.Screen = "333";
            eee.State = "990";
            tc.ExpectedResult = eee;

            Console.WriteLine(tc.Amount + " " + tc.ExpectedResult.State);


            TransactionConfig cloned = DeepCopy<TransactionConfig>(tc);

            if (Object.ReferenceEquals(tc, cloned))
            {
                Console.WriteLine("References are the same.");
            }
            else
            {
                Console.WriteLine("References are different.");
            }


            //TransactionConfig tc2 = new TransactionConfig("7890");
            //Console.WriteLine(tc2.V + " " + tc2.X + " " + tc2.Amount);
        }
    }
}