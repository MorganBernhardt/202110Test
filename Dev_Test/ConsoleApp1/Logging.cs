using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ConsoleApp1
{
    class Logging
    {
        public static void WriteLine(string Log_Content)
        {
            //string Local_Log_Path = "C:\\temp\\" + DateTime.Now.ToString("yyyyMMdd_") + "PowerTradeVolume.txt";
            string Local_Log_Path = Properties.Settings.Default.logpath  + DateTime.Now.ToString("yyyyMMdd_") + "PowerTradeVolume.txt";
            string Enhanced_Log_Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + Log_Content;
            //<<calling to log locally>>
            WriteToFile(Enhanced_Log_Content, Local_Log_Path);
            //<<//calling to log locally>>
            Console.WriteLine(Enhanced_Log_Content);
        }
        public static void WriteToFile(string Enhanced_Log_Content, string full_log_path)
        {
            try
            {
                if (File.Exists(full_log_path))
                {
                    TextWriter tw = new StreamWriter(full_log_path, true);
                    tw.WriteLine(Enhanced_Log_Content);
                    tw.Close();
                }
                else
                {
                    File.Create(full_log_path).Close();
                    TextWriter tw = new StreamWriter(full_log_path, true);
                    tw.WriteLine(Enhanced_Log_Content);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                //<<do not raise further alarm/>>
            }
        }
    }

}
