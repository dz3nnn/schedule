using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MainLibrary
{
    public class Log
    {
        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void WriteLog(string message)
        {
            if (!Directory.Exists(@"C:\ScheduleLog"))
                Directory.CreateDirectory(@"C:\ScheduleLog");
            if (!File.Exists(@"C:\ScheduleLog\log.txt"))
                using (File.Create(@"C:\ScheduleLog\log.txt")) { }
            StreamWriter sw = new StreamWriter(@"C:\ScheduleLog\log.txt",true,System.Text.Encoding.UTF8);
            sw.WriteLine(DateTime.Now.ToString("dd.MM.yy HH:mm:ss")+" "+message);
            sw.Dispose();
            sw.Close();
            //check
        }
    }
}
