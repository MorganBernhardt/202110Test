using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ConsoleApp1
{
    class Program
    {
        private static int loopcounter = 0;
        static void Main(string[] args)
        {
            Console.Title = "Joao.Leong@gmail.com Petroineos Power Volume Test";
            Logging.WriteLine("Process Start");
            Logging.WriteLine("Ease of excel header Date | Loop | Trade number | Period | Volume ");
            pretend_error_never_happened:         
            looptrystartline:
            try
            {
                
                //Console.WriteLine("hello nerd");
                
                loopcounter = loopcounter + 1;
                LoopTry();
                Thread.Sleep(1000 * Properties.Settings.Default.extractionintervalsecond);
                goto looptrystartline;
                
            }
            catch (Exception ex)
            {
                Logging.WriteLine(string.Format("{0} loop {2} Program.Main : Error Message {1}", DateTime.Now, ex.Message, loopcounter));
                goto pretend_error_never_happened;
            }
        }
        static void LoopTry()
        {
            DateTime RunDate = DateTime.Today;
            Services.PowerService ps = new Services.PowerService();
            System.Collections.IEnumerable tt = ps.GetTrades(RunDate);
            Logging.WriteLine("loop " + loopcounter.ToString() + " started for run date : " + RunDate.ToString() );
            
            double [] ptSum = new double[24];
            int tradecount = 0;
            foreach (Services.PowerTrade powerTrade  in tt )
            {   
                for (int ni = 0; ni < 24; ni++)
                {
                    Logging.WriteLine("|"+ loopcounter.ToString() +"| Trade " + tradecount.ToString() +"|"+ powerTrade.Periods[ni].Period.ToString() + "|" + powerTrade.Periods[ni].Volume.ToString());
                    ptSum[ni] = ptSum[ni]+ powerTrade.Periods[ni].Volume;
                    
                }
                tradecount = tradecount + 1;
            }
            Logging.WriteLine("number of trades processed: " + tradecount.ToString());
            Logging.WriteLine("Aggregated volume: " + tradecount.ToString());
            //<<actual output setting>>
            string OutPutFullPath = Properties.Settings.Default.outputpath + "PowerPosition_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
            Logging.WriteToFile("LocalTime,Volume", OutPutFullPath);
            //<<//actual output setting>>
            string sLocalTime;
            for (int ni = 0; ni < 24; ni++)
            {
                Logging.WriteLine("|" + loopcounter.ToString()  + " | Total| " + (ni+1).ToString() + "|" + ptSum[ni]);
                //<<actual output>>
                if (ni == 0)
                {
                    sLocalTime = "23:00";
                }
                else
                {
                    sLocalTime = "0" + (ni - 1).ToString();                    
                    sLocalTime = sLocalTime.Substring(sLocalTime.Length - 2,2) + ":00";
                }
                Logging.WriteToFile(sLocalTime + "," + ptSum[ni] , OutPutFullPath);
                //<<//actual output>>    
            }

            Logging.WriteLine("loop "  + loopcounter.ToString() + " completed");

            
        }
    }
}
