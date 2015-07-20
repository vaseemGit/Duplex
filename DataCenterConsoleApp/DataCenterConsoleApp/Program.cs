using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCenterConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Listener obj = new Listener();
            obj.callService();
            Console.WriteLine("processing completed");
            Console.Read();
            obj.Dispose();
        }
    }
}
