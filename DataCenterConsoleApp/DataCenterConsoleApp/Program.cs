using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DataCenterConsoleApp
{
    class Program
    {
        public static ILog logger;
        static System.Timers.Timer timer1 = new System.Timers.Timer();

        static void Main(string[] args)
        {
            try
            {
                InitializeLogger();
                Listener obj = new Listener();
                obj.callService();
                Console.WriteLine("processing completed");
                timer1.Interval = 3000;//one second
                timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
                timer1.Start();
                Console.Read();
                obj.Dispose();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                logger.Error(ex.Message.ToString());
            }
        }
        static private void timer1_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {

            Listener obj = new Listener();
            obj.callService();
            //obj.Dispose();
            //Console.WriteLine("processing completed");
        }
        #region InitializeLogger
        private static void InitializeLogger()
        {
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
                string ActualPath = path.Remove(path.IndexOf("bin\\Debug"));
                string configFile = ActualPath + "App.config";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
            }
            logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
        #endregion
       
    }
}
