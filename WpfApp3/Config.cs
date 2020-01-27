using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;

namespace WpfApp3
{
    public class Config
    {
        // TODO: I think it should be singleton

        private YamlStream _file;

        private List<string> _financial;
        private List<string> _nonFinancial;
        private List<string> _complete;
        private List<string> _condition;
        private List<string> _beforeChecking;
        private List<string> _afterChecking;
        private List<string> _reverse;
        private List<string> _badData;
        private List<Card> _card;
        private string _amount;
        private List<Network> _networks;
        public List<string> _expectedResultNames;
        public Form4 _form4;

        //public Dictionary<HashSet<string>, ExpectedResult> _expectedResults;

        public List<Object> _expectedResultList;

        //private List<Network> _G;
        // private List<Network> _networks;
        //private 

        public List<Object> ExpectedResultList
        {
            get { return _expectedResultList; }
        }

        public List<string> Financial
        {
            get { return _financial; }
        }

        public List<string> NonFinancial
        {
            get { return _nonFinancial; }
        }

        public List<string> Complete
        {
            get { return _complete; }
        }

        public List<string> Condition
        {
            get { return _condition; }
        }

        public List<string> BeforeChecking
        {
            get { return _beforeChecking; }
        }

        public List<string> AfterChecking
        {
            get { return _afterChecking; }
        }

        public List<string> Reverse
        {
            get { return _reverse; }
        }

        public List<string> BadData
        {
            get { return _badData; }
        }

        public List<Card> Card
        {
            get { return _card; }
        }

        public string Amount
        {
            get { return _amount; }
        }

        public List<Network> Networks
        {
            get { return _networks; }
        }

        public List<string> ExpectedResultNames
        {
            get { return _expectedResultNames; }
        }

        public Network GetNetwork(string networkName)
        {
            foreach (var network in _networks)
            {
                if (network.ServerName.Equals(networkName))
                {
                    return network;
                }
            }
            return null;
        }

        /*internal List<string> GetNetworkConfig()
        {
            return new List<string>() { "10.15.1.61", "9009" };
        }*/

        public Config(Form4 form4)
        {
            _financial = new List<string>();
            _nonFinancial = new List<string>();
            _complete = new List<string>();
            _condition = new List<string>();
            _beforeChecking = new List<string>();
            _afterChecking = new List<string>();
            _reverse = new List<string>();
            _badData = new List<string>();
            _card = new List<Card>();
            _amount = "0";
            _networks = new List<Network>();
            _expectedResultNames = new List<string>();
            _form4 = form4;

            // _expectedResults = new Dictionary<HashSet<string>, ExpectedResult>(HashSet<string>.CreateSetComparer());

            _expectedResultList = new List<Object>();
        }

        public void ReadFile()
        {
            using (
                var input =
                    new StreamReader(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\..\..\Config.yaml")
            )
            {
                // Load the stream
                _file = new YamlStream();
                _file.Load(input);
            }
        }

        public void Parse()
        {
            // Examine the stream
            var mapping =
                (YamlMappingNode) _file.Documents[0].RootNode;

            foreach (var entry in mapping.Children)
            {
                Console.Write(((YamlScalarNode) entry.Key).Value + " : ");

                string value = Regex.Replace((entry.Value).ToString(), @"\s+", string.Empty);
                Console.Write(value.Trim('[').Trim(']'));

                Console.WriteLine();
                //output.WriteLine(((YamlScalarNode)entry.Key).Value);
            }
            Console.WriteLine("====================");

            // List all the items
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Financial")])
                _financial.Add(item.ToString());
            Console.WriteLine("Financial");
            foreach (string i in _financial)
            {
                Console.WriteLine(i + " ");
            }
            Console.WriteLine("====================");
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Non-Financial")])
                _nonFinancial.Add(item.ToString());
            Console.WriteLine("nonFinancial");
            foreach (string i in _nonFinancial)
                Console.WriteLine(i + " ");
            Console.WriteLine("====================");
            //====
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Complete")])
                _complete.Add(item.ToString());
            Console.WriteLine("Complete");
            foreach (string i in _complete)
                Console.Write(i + " ");
            Console.WriteLine("====================");
            //====
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Condition")])
                _condition.Add(item.ToString());
            Console.WriteLine("Condition");
            foreach (string i in _condition)
                Console.Write(i + " ");
            Console.WriteLine("====================");
            //====
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Before-Checking")])
                _beforeChecking.Add(item.ToString());
            Console.WriteLine("Before-Checking");
            foreach (string i in _beforeChecking)
                Console.Write(i + " ");
            Console.WriteLine("====================");
            //====
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("After-Checking")])
                _afterChecking.Add(item.ToString());
            Console.WriteLine("After-Checking");
            foreach (string i in _afterChecking)
                Console.Write(i + " ");
            Console.WriteLine("====================");
            //====
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Reverse")])
                _reverse.Add(item.ToString());
            Console.WriteLine("Reverse");
            foreach (string i in _reverse)
                Console.Write(i + " ");
            Console.WriteLine("====================");
            //====
            foreach (YamlScalarNode item in (YamlSequenceNode) mapping.Children[new YamlScalarNode("Bad-Data")])
                _badData.Add(item.ToString());
            Console.WriteLine("Bad-Data");
            foreach (string i in _badData)
                Console.Write(i + " ");

            var dataParameters = (YamlMappingNode) mapping.Children[new YamlScalarNode("Data-Parameters")];
            var cards = (YamlSequenceNode) dataParameters.Children[new YamlScalarNode("Cards")];
            foreach (YamlMappingNode data in cards)
            {
                Card card = new Card();
                card.CardNumber = data.Children[new YamlScalarNode("Card_Number")].ToString();
                card.CardType = data.Children[new YamlScalarNode("Card_Type")].ToString();
                card.Track = data.Children[new YamlScalarNode("Track")].ToString();
                card.PinBufferA = data.Children[new YamlScalarNode("PinA")].ToString();
                _card.Add(card);
            }

            Console.WriteLine("====================");
            foreach (Card card in _card)
            {
                Console.WriteLine(card.CardNumber + ", " + card.CardType + ", " + card.Track);
            }
            Console.WriteLine("====================");

            var amount = (YamlScalarNode) dataParameters.Children[new YamlScalarNode("Amount")];
            _amount = amount.ToString();
            Console.WriteLine(amount);

            Console.WriteLine("====================");

            var networks = (YamlSequenceNode) dataParameters.Children[new YamlScalarNode("Networks")];
            foreach (YamlMappingNode data in networks)
            {
                var network = new Network();
                network.ServerName = data.Children[new YamlScalarNode("Name")].ToString();
                network.ServerIp = data.Children[new YamlScalarNode("ServerIP")].ToString();
                network.PortNo = int.Parse(data.Children[new YamlScalarNode("Port")].ToString());
                _networks.Add(network);
            }

            Console.WriteLine("====================");

            _expectedResultNames.Add("General");
            foreach (var tran in _financial)
            {
                _expectedResultNames.Add(tran);
            }
            foreach (var tran in _complete)
            {
                _expectedResultNames.Add(tran);
            }
            foreach (var tran in _nonFinancial)
            {
                _expectedResultNames.Add(tran);
            }

            var expectedResult = (YamlSequenceNode) mapping.Children[new YamlScalarNode("Expected-Results")];
            foreach (var tran in _expectedResultNames)
            {
                Dictionary<HashSet<string>, ExpectedResult> _expectedResults =
                    new Dictionary<HashSet<string>, ExpectedResult>(HashSet<string>.CreateSetComparer());

                foreach (YamlMappingNode data in expectedResult)
                {
                    if (data.Children[new YamlScalarNode("Category")].ToString().Equals(tran))
                    {
                        string[] conditionList =
                            CreateConditionList(data.Children[new YamlScalarNode("Condition")].ToString());

                        foreach (var condition in conditionList)
                        {
                            ExpectedResult expected = new ExpectedResult();
                            expected.Category = data.Children[new YamlScalarNode("Category")].ToString();
                            //Console.WriteLine("mdtmdt" + data.Children[new Yaml("Condition")].ToString());
                            //expected.Condition = data.Children[new YamlScalarNode("Condition")].ToString();
                            expected.State = data.Children[new YamlScalarNode("State")].ToString();
                            expected.Screen = data.Children[new YamlScalarNode("Screen")].ToString();
                            expected.Text = data.Children[new YamlScalarNode("Text")].ToString();
                            expected.Journal = data.Children[new YamlScalarNode("Journal")].ToString();
                            try
                            {
                                _expectedResults.Add(CreateConditionSet(condition), expected);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Duplicate Expected Result found!!!");
                            }
                        }
                    }
                }
                _expectedResultList.Add(_expectedResults);
            }
        }

        private HashSet<string> CreateConditionSet(string condition)
        {
            condition = condition.TrimStart(' ');

            HashSet<string> result = new HashSet<string>();

            string[] conditions = condition.Split(' ');
            foreach (var cdn in conditions)
            {
                result.Add(cdn);
            }
            return result;
        }

        private string[] CreateConditionList(string conditionString)
        {
            string str = conditionString.TrimEnd(']').TrimStart('[').TrimStart(' ').TrimEnd(' ');
            return str.Split(',');
        }

        static void Main()
        {
            //Config cf = new Config();
            //cf.ReadFile();
            //cf.Parse();
            // Console.WriteLine(cf.Card.Count);
        }
    }
}