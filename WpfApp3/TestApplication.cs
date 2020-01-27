using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using NUnit.Framework.Internal;

namespace WpfApp3
{
		class TestApplication
		{
				static void Main()
				{
						//Form4 form4 = new Form4();
						//form4.Show();
						//form4.Close();

						Application.EnableVisualStyles();
						Form4 form4 = new Form4();
						//form4.ShowDialog();
						Application.Run(form4);

						// now go to form4.cs for more information 981020


			//==============================
						//Run(form4);

						//var config = new Config(form4);
						//config.ReadFile();
						//config.Parse();

						//var planFactory = new PlanFactory(config);

						//// TODO: generate plans is incomplete
						//planFactory.GeneratePlans();

						//Reporter.Log(System.DateTime.Now.ToString());

						//planFactory.ProcessPendingPlans();

				}

				public static void Run(Form4 form4)
				{

						var config = new Config(form4);
						config.ReadFile();
						config.Parse();

						var planFactory = new PlanFactory(config);

						// TODO: generate plans is incomplete
						planFactory.GeneratePlans();

						Reporter.Log(System.DateTime.Now.ToString());

						planFactory.ProcessPendingPlans();
				}
		}
}
