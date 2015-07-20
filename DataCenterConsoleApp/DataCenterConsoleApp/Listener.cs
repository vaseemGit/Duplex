using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCenterConsoleApp.DuplexEMachineService;
using System.ServiceModel;
using System.ComponentModel;
namespace DataCenterConsoleApp
{
    class Listener : IServiceCallback,IDisposable
    {
        ServiceClient proxy;
        public string msg=string.Empty;
        private ServiceClient _client;
        
        public void CallBackFunction(string str)
        {
            
            string s=string.Empty;
            if (str == "Identity")
            {
               MachineIdentity();
            }
            if (str == "GetFunction")
            {
                List<string> FunctionList= getFunctions();
                foreach (string fcn in FunctionList) // Loop through List with foreach.
                {
                    s=s+","+fcn;
                }
                InstanceContext context = new InstanceContext(this);
                proxy = new ServiceClient(context);
                proxy.UpdateData(s.TrimStart(','), MachineIdentityName());
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

                Console.WriteLine("Error:" + ex.Message.ToString());
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error:" + ex.Message.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message.ToString());
            }

        }

        public void Dispose()
        {
            proxy.Close();
        }
        public void MachineIdentity()
        {
            string s=  Environment.ProcessorCount + "/" +
                Environment.MachineName + "/" +
                Environment.UserDomainName + "\\" +
                Environment.UserName + "/" +
                Environment.GetLogicalDrives().Length;
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
