using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//using C1.Win.C1Input;

//using NUnit.Framework.Interfaces;

namespace WpfApp3
{
	public class PlanEventArgs : EventArgs
	{
		public enum eStatus
		{
			Started,
			Processing,
			Passed,
			Failed
		}

		public eStatus EventType { get; set; }
		public string EventMessage { get; set; }

		public PlanEventArgs(eStatus eventType, string eventMessage)
		{
			this.EventType = eventType;
			this.EventMessage = eventMessage;
		}
	}

	public class PlanFactory // TODO: as invoker
	{
		private readonly Config _config;
		private List<string> _bools = new List<string>();
    public List<Plan> _plans { get; set; }
		//private List<Plan> _plans;

		// -----

		private List<BackgroundWorker> _workers = new List<BackgroundWorker>();
		private List<List<Plan>> _plansList = new List<List<Plan>>();

		private int total_workers = 4;
		private int completed_workers = 0;

		private List<Plan> _plan1 = new List<Plan>();
		private List<Plan> _plan2 = new List<Plan>();
		private List<Plan> _plan3 = new List<Plan>();
		private List<Plan> _plan4 = new List<Plan>();

		public event EventHandler<PlanEventArgs> PlanAdded;

		public PlanFactory(Config config)
		{
			_config = config;
      _plans = new List<Plan>();
    }

		protected virtual void OnPlanAdded(string condition)
		{
			if (PlanAdded != null)
			{
				PlanAdded(this,
						new PlanEventArgs(PlanEventArgs.eStatus.Started, condition));
				// for example
			}
		}

		public void CreateTruthTable(int n, string s)
		{
			if (n == 0)
			{
				Console.WriteLine(s);
				_bools.Add(s);
				//counter++;
				return;
			}
			CreateTruthTable(n - 1, s + "T");
			CreateTruthTable(n - 1, s + "F");
		}

		public List<string> GenerateConditionStrings(List<string> conditionList)
		{
			_bools.Clear(); // TODO is it OK?

			//CreateTruthTable(_config.Condition.Count, ""); //TODO delete
			CreateTruthTable(conditionList.Count, "");
			//Console.WriteLine(_bools.Count); // TODO delete
			var result = new List<string>();
			foreach (var bul in _bools)
			{
				var newFlow = new StringBuilder();
				for (int i = 0; i < bul.Length; i++)
				{
					if (bul[i] == 'T')
					{
						//newFlow.Append(_config.Condition[i] + " "); //TODO delete
						//Console.WriteLine(_config.Condition[i]);    //TODO delete
						newFlow.Append(conditionList[i] + " ");
						Console.WriteLine(conditionList[i]);
					}
					else
					{
						//newFlow.Append("~" + _config.Condition[i] + " "); //TODO delete
						//Console.WriteLine("~" + _config.Condition[i]);    //TODO delete
						newFlow.Append("~" + conditionList[i] + " ");
						Console.WriteLine("~" + conditionList[i]);
					}
				}
				result.Add(newFlow.ToString());
				Console.WriteLine("+++++++++++++++++++++");
			}
			return result;
		}

		public void GeneratePlans()
		{
			///Application.EnableVisualStyles();
			//Form4 form4 = new Form4();
			//form4.ShowDialog();

			//Application.Run(form4);

			Console.WriteLine("===Start PlanFactory.GeneratePlans()===");

			//_plans = new List<Plan>();

			//TODO: generate the list of plans base on the config file

			/**
			 * expectedResultCollection: the first element would be the list of the names of the transactions for expected result
			 *                           the second element would be the list of condition as key of dictionary and expected result as value
			 */
			var expectedResultCollection = new List<Object>();
			expectedResultCollection.Add(_config.ExpectedResultNames);
			expectedResultCollection.Add(_config.ExpectedResultList);

			var formSubscriber = new FormSubscriber(); // subscriber

			// Condition-based plans
			foreach (var financial in _config.Financial)
			{
				// return the confluence(eshterak) of active conditions of the specific transaction and condition part of the config yaml file
				var activeConditions = GetActiveConditions(financial);
				var conditionsStrings = GenerateConditionStrings(activeConditions);

				// now conditionsStrings is the strings of different real conditions
				foreach (var condition in conditionsStrings)
				{
					var transactionConfig = new TransactionConfig(_config.Amount, _config.BadData, _config.Card[0]);

					_plans.Add(new ConditionBasedPlan(financial + "," + condition, expectedResultCollection,
							_config.GetNetwork("ATM"), transactionConfig));
					OnPlanAdded(financial + ": " + condition);

					//_config._form4.
					foreach (var transaction in _plans[_plans.Count - 1].Transactions)
					{
						transaction.StatusChanged += formSubscriber.OnTransactionChangeStatus; // subscribe to the event
						transaction.StatusChanged += _config._form4.OnTransactionChangeStatus; // subscribe to the event
					}
					//_plans[_plans.Count - 1].Transactions[0].StatusChanged += formSubscriber.OnTransactionChangeStatus; // subscribe to the event
				}
			}
			//_config._form4.Show();
			//form4.ShowDialog();
			Console.WriteLine("Condition-based plans Completed");

			/*
			// Reverse plans

			foreach (var financial in _config.Financial)
			{
					if (financial.Equals("Withdrawal", StringComparison.InvariantCultureIgnoreCase))
					{
							if (_config.Reverse.Contains("Not_Withdrawal", StringComparer.InvariantCultureIgnoreCase))
									_plans.Add(new ReversePlan("Not_Withdrawal; don't take card", _config.GetNetwork("ATM"), new TransactionConfig(_config.Card[0])));
					}
			}

			// BadData plans

			foreach (var financial in _config.Financial)
			{
					if (financial.Equals("Withdrawal", StringComparison.InvariantCultureIgnoreCase))
					{
							if (_config.BadData.Contains("Amount", StringComparer.InvariantCultureIgnoreCase))
									_plans.Add(new BadDataPlan("Bad Amount Data", _config.GetNetwork("ATM"), new TransactionConfig(_config.Card[0])));  // TODO: transaction config should be corrected

							if (_config.BadData.Contains("PIN", StringComparer.InvariantCultureIgnoreCase))
									_plans.Add(new BadDataPlan("Bad PIN Data", _config.GetNetwork("ATM"), new TransactionConfig(_config.Card[0])));     // TODO: transaction config should be corrected

							if (_config.BadData.Contains("Track", StringComparer.InvariantCultureIgnoreCase))
									_plans.Add(new BadDataPlan("Bad Track Data", _config.GetNetwork("ATM"), new TransactionConfig(_config.Card[0])));   // TODO: transaction config should be corrected
					}
			}

			// Complete plans

			foreach (var complete in _config.Complete)
			{
					if (complete.Equals("Balance", StringComparison.InvariantCultureIgnoreCase))
					{
							if (_config.Condition.Contains("Incorrect_PIN", StringComparer.InvariantCultureIgnoreCase))
									_plans.Add(new CompletePlan("Incorrect Pin", _config.GetNetwork("ATM"), new TransactionConfig(_config.Card[0])));  // TODO: transaction config should be corrected

							_plans.Add(new CompletePlan("All inputs are Correct", _config.GetNetwork("ATM"), new TransactionConfig(_config.Card[0])));
					}
			}
			*/

			for (int i = 0; i < _plans.Count; i++)
			{
				if (i % 4 == 0)
				{
					_plan1.Add(_plans[i]);
				}
				else if (i % 4 == 1)
				{
					_plan2.Add(_plans[i]);
				}
				else if (i % 4 == 2)
				{
					_plan3.Add(_plans[i]);
				}
				else if (i % 4 == 3)
				{
					_plan4.Add(_plans[i]);
				}
			}

			Console.WriteLine("plans: " + _plans.Count);
			Console.WriteLine("plan1: " + _plan1.Count);
			Console.WriteLine("plan2: " + _plan2.Count);
			Console.WriteLine("plan3: " + _plan3.Count);
			Console.WriteLine("plan4: " + _plan4.Count);

			_plansList.Add(_plan1);
			_plansList.Add(_plan2);
			_plansList.Add(_plan3);
			_plansList.Add(_plan4);
		}

		private List<string> GetActiveConditions(string financial)
		{
			var result = new List<string>();

			string transactionToCheckStatics = "TransactionTest." + financial;

			var objectType = Type.GetType(transactionToCheckStatics);
			//Console.WriteLine("==>: " + objectType.GetField("CardNumber"));
			var instantiatedObject = Activator.CreateInstance(objectType);

			var Correct_PIN =
					(bool)objectType.InvokeMember("isCorrect_PIN", BindingFlags.GetField, null, instantiatedObject, null);
			var Enough_Cash =
					(bool)objectType.InvokeMember("isEnough_Cash", BindingFlags.GetField, null, instantiatedObject, null);
			var Amount =
					(bool)objectType.InvokeMember("isAmount", BindingFlags.GetField, null, instantiatedObject, null);
			var Test1 =
					(bool)objectType.InvokeMember("isTest1", BindingFlags.GetField, null, instantiatedObject, null);
			// TODO: add conditions continuously during system change

			Dictionary<string, bool> staticConditions = new Dictionary<string, bool>();

			staticConditions.Add(nameof(Correct_PIN), Correct_PIN);
			staticConditions.Add(nameof(Enough_Cash), Enough_Cash);
			staticConditions.Add(nameof(Amount), Amount);
			staticConditions.Add(nameof(Test1), Test1);
			// TODO: add conditions continuously during system change


			foreach (var cnd in _config.Condition)
			{
				if (staticConditions[cnd])
				{
					result.Add(cnd);
				}
			}
			return result;
		}

		private void bg_DoWork(object sender, DoWorkEventArgs e)
		{
			Console.WriteLine("thread started!");

			List<Plan> _plns = (List<Plan>)e.Argument;
			foreach (var pln in _plns)
			{
				Reporter.Log(pln.Process());
			}
		}

		private void bg_runWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			completed_workers++;
		}

		public void ProcessPendingPlans()
		{
			Console.WriteLine("start pending!");

			//-------------------------------------------------------------980708 TODO should be uncomment later(for multi - threading)
			//    for (int i = 0; i < 4; i++)
			//    {
			//        BackgroundWorker bw = new BackgroundWorker();
			//        _workers.Add(bw);
			//        _workers[i].DoWork += bg_DoWork;
			//        _workers[i].RunWorkerCompleted += bg_runWorker_Completed;
			//        _workers[i].RunWorkerAsync(_plansList[i]);
			//    }

			//while (total_workers != completed_workers)
			//    Thread.Sleep(1000);
			//-------------------------------------------------------------
			// 980708

			foreach (var pln in _plans)
			{
				Reporter.Log(pln.Process());
			}

			//-------------------------------------------------------------
			/*Parallel.ForEach(_plans, plan =>
			{
					Reporter.Log(plan.Process());
			});
			*/

			// Thread th = new Thread(new ThreadStart());
			// th.Start();

			/*foreach (var plan in _plans)
			{
					// ThreadStart childref = new ThreadStart();
					Reporter.Log(plan.Process());

					//Thread.Sleep(5000);
					//System.Threading.Thread.Sleep(4000);
			}*/
		}


		static void Main()
		{
			/*
			Console.WriteLine("salam");

			HashSet<string> a = new HashSet<string> {"correctpi", "enoughcash", "amount"};
			HashSet<string> b = new HashSet<string> {"enoughcash", "correctpin", "amount"};

			HashSet<string> c = new HashSet<string> {"enoughcash", "amount", "correctpin"};


			var yyy = new Dictionary<HashSet<string>, string>(HashSet<string>.CreateSetComparer());

			yyy.Add(a, "ali");
			yyy.Add(b, "mammad");

			if (a.Contains("!correctpi"))
			{
					Console.WriteLine("exist");
			}

			Console.WriteLine(yyy[c]);

			var arr = new ArrayList();
			arr.Add(yyy);
			arr.Add(yyy);
			arr.Add(yyy);
			*/


			//string dog = "Balance";
			//var dogObj = Activator.CreateInstance(Type.GetType(dog)) as NdcTransactionRequestMessage;
			//string cat = "Cat";
			//var catObj = Activator.CreateInstance(Type.GetType(cat)) as Animal;
			//Console.WriteLine(dogObj);
			//Console.WriteLine(catObj);

			// object obj = ((ObjectHandle)Activator.CreateInstance(null, "Balance"));
			//Type type = obj.GetType();
			//type.GetProperty("Name").SetValue(obj, "Hello World", null);
			//string personName = type.GetProperty("Name").GetValue(obj, null).ToString();


			const string objectToInstantiate = "TransactionTest.Balance";

			var objectType = Type.GetType(objectToInstantiate);
			//Console.WriteLine("==>: " + objectType.GetField("CardNumber"));
			var instantiatedObject = Activator.CreateInstance(objectType);

			TransactionConfig tf = new TransactionConfig(new Card());
			//tf.Amount = "555";

			//classType.InvokeMember("Sub", BindingFlags.InvokeMethod, null, instance, new object[] { 23, 42 });
			Console.WriteLine(objectType.InvokeMember("fff", BindingFlags.InvokeMethod, null, instantiatedObject,
					new object[] { tf }));
			//Console.WriteLine(objectType.InvokeMember());

			Console.WriteLine(instantiatedObject.ToString());
		}
	}
}
