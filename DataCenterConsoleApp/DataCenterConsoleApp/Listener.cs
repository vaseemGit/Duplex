using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCenterConsoleApp.DuplexEMachineService;
using System.ServiceModel;
using System.ComponentModel;
using System.Management;
using log4net;
namespace DataCenterConsoleApp
{
    class Listener : IServiceCallback,IDisposable
    {
        ServiceClient proxy;
        Program p = new Program();
        public string msg=string.Empty;
        private ServiceClient _client;
       
        public void CallBackFunction(string functionType, string clientAddress, string functonName)
        {
            try
            {

            string s=string.Empty;
            if (functionType == "Identity")
            {
               MachineIdentity();
            }
            else  if (functionType == "GetFunction")
            {
                List<string> FunctionList= getFunctions();
                foreach (string fcn in FunctionList) // Loop through List with foreach.
                {
                    s=s+","+fcn;
                }
                InstanceContext context = new InstanceContext(this);
                proxy = new ServiceClient(context);
                proxy.UpdateData(s.TrimStart(','), clientAddress);
            }
            else if (functionType == "ExecuteFunction")
            {
                Console.WriteLine(functonName+" Command executed.");
            }

            }
            catch (Exception ex)
            {

                Program.logger.Error(ex.Message.ToString());
            }
            
        }
    
        public void callService()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                proxy = new ServiceClient(context);
                proxy.NormalFunction();
            }
            catch (EndpointNotFoundException ex)
            {
                Program.logger.Error(ex.Message.ToString());
                Console.WriteLine("Error:" + ex.Message.ToString());
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error:" + ex.Message.ToString());
                Program.logger.Error(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message.ToString());
                Program.logger.Error(ex.Message.ToString());
            }

        }

        public void Dispose()
        {
            proxy.Close();
        }
        public void MachineIdentity()
        {
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            mbsList = mbs.Get();
            string processorId = "";
            foreach (ManagementObject mo in mbsList)
            {
                processorId = mo["ProcessorID"].ToString();
            }

            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = mos.Get();
            string motherBoard = "";
            foreach (ManagementObject mo in moc)
            {
                motherBoard = (string)mo["SerialNumber"];
            }

              string s=  Environment.ProcessorCount + "/" +
                Environment.MachineName + "/" +
                Environment.UserDomainName + "\\" +
                Environment.UserName + "/" +
                Environment.GetLogicalDrives().Length +"/"+ processorId.Trim()+"/"+motherBoard.Trim();
            Guid clientGuid = Guid.NewGuid();
            InstanceContext context = new InstanceContext(this);
            proxy = new ServiceClient(context);
            proxy.InsertData(s);
           
        }
        public string MachineIdentityName()
        {
            string s = Environment.ProcessorCount + "/" +
                Environment.MachineName + "/" +
                Environment.UserDomainName + "\\" +
                Environment.UserName + "/" +
                Environment.GetLogicalDrives().Length;
            return s;

        }
        public List<string> getFunctions()
        {
            List<string> FunctName = new List<string>();
            FunctName.Add("Print");
            FunctName.Add("Open File");
            FunctName.Add("New");
            FunctName.Add("Save");
            FunctName.Add("Add");
            FunctName.Add("Identity");
            return FunctName;
        }

    }
    
}
