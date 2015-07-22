using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCenterConsoleApp
{
    class Program
    {

        static System.Timers.Timer timer1 = new System.Timers.Timer();

        static void Main(string[] args)
        {
            Listener obj = new Listener();
            obj.callService();
            Console.WriteLine("processing completed");
            timer1.Interval = 3000;//one second
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Start();
            Console.Read();
            obj.Dispose();
        }
        static private void timer1_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {

            Listener obj = new Listener();
            obj.callService();
            obj.Dispose();
            //Console.WriteLine("processing completed");
        }
       
    }
}
