using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WpfApp3
{
    public static class Reporter
    {
        private static ReportBase reporter = new FileReporter();

        public static void Log(string log)
        {
            reporter.Log(log);
        }
    }
}
