using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for mazrab.xaml
    /// </summary>
    public partial class mazrab : Page
    {
        private int total_workers = 1;
        private int completed_workers = 0;
        private int currentProgress;
        Double _PRG;
        private BackgroundWorker _workers;
        private BlockingCollection<mzr> myCollection;

        public Double PRG
        {
            get { return _PRG; }
            set
            {
                if (value <= 100)
                {
                    if (value >= 0) { _PRG = value; }
                    else { _PRG = 0; }
                }
                else { _PRG = 100; }
                //NotifyPropertyChanged("Value");
            }
        }


        public int CurrentProgress
        {
            get { return currentProgress; }
            private set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    //OnPropertyChanged("CurrentProgress");
                }
            }
        }

        public mazrab()
        {
            InitializeComponent();
            num.Focus();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void num_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            // your event handler here
            //if (mainGrid.I)
                myFunc();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myFunc();
        }

        private void myFunc()
        {
            CurrentProgress = 0;
            //progressBar.Value = 0;
            completed_workers = 0;

            MenList.ItemsSource = null;

            int counter = 0;
            int number = 0;
            try
            {
                number = Int32.Parse(num.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("عدد وارد شده صحیح نمی باشد");
                num.Background = Brushes.Red;
                return;
            }

            _workers = new BackgroundWorker();
            _workers.DoWork += myAsyncLoop;
            _workers.WorkerReportsProgress = true;
            _workers.RunWorkerCompleted += bg_runWorker_Completed;
            _workers.ProgressChanged += bw_ProgressChanged;
            _workers.RunWorkerAsync(number);

            /*while (total_workers != completed_workers)
                Thread.Sleep(1000);
                */

            num.Background = Brushes.White;
            num.Clear();
            num.Focus();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Text = (e.ProgressPercentage).ToString();

            pbStatus.Value = e.ProgressPercentage;

            Console.WriteLine(e.ProgressPercentage);

            //MenList.ItemsSource = myCollection;

            /*
            Duration dur = new Duration(TimeSpan.FromSeconds(30));
            DoubleAnimation dblani = new DoubleAnimation(100, dur);
            pbStatus.BeginAnimation(ProgressBar.ValueProperty, dblani);
            */
            //PRG = e.ProgressPercentage;
            //UpdateProgressBarDelegate
        }

        private void bg_runWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            completed_workers++;

            MenList.ItemsSource = myCollection;
        }

        private void myAsyncLoop(object sender, DoWorkEventArgs e)
        {
            int number = (int) e.Argument;
            myCollection = new BlockingCollection<mzr>();
            double temp = 0.1;
            for (int i = 1; i <= number; i++)
            {
                int progressPercentage = Convert.ToInt32(((double) i/number)*100);
                if (i%15 == 0)
                {
                    myCollection.Add(new mzr() {nbr = i.ToString(), dsc = "مضرب 15"});
                }
                else
                {
                    if (i%3 == 0)
                    {
                        myCollection.Add(new mzr() {nbr = i.ToString(), dsc = "مضرب 3"});
                    }
                    else if (i%5 == 0)
                    {
                        myCollection.Add(new mzr() {nbr = i.ToString(), dsc = "مضرب 5"});
                    }
                    else
                    {
                        myCollection.Add(new mzr() {nbr = i.ToString(), dsc = "هیچکدام"});
                    }
                }

                double prg = (double)i/number;
                if (prg > temp)
                {
                    temp += 0.1;
                    _workers.ReportProgress(progressPercentage);
                }
            }
            //_workers.ReportProgress(100);

            completed_workers++;
        }

        private void mySimpleLoop(int number)
        {
            for (int i = 1; i <= number; i++)
            {
                int progressPercentage = Convert.ToInt32(((double)i / number) * 100);
                if (i % 15 == 0)
                {
                    //counter++;
                    MenList.Items.Add(new mzr() { nbr = i.ToString(), dsc = "مضرب 15" });

                }
                else
                {
                    if (i % 3 == 0)
                    {
                        MenList.Items.Add(new mzr() { nbr = i.ToString(), dsc = "مضرب 3" });
                        //progressBar.Value = progressPercentage;
                    }
                    else if (i % 5 == 0)
                    {
                        MenList.Items.Add(new mzr() { nbr = i.ToString(), dsc = "مضرب 5" });
                    }
                    else
                    {
                        MenList.Items.Add(new mzr() { nbr = i.ToString(), dsc = "هیچکدام" });
                    }
                }
            }

            //progressBar.Value = 100;
            num.Background = Brushes.White;
            num.Clear();
            num.Focus();
        }
    }

    class mzr
    {
        public string nbr { get; set; }
        public string dsc { get; set; }
    }
}
