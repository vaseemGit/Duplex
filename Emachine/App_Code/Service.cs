using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Service : IService
    {
         private static Dictionary<string, IMyContractCallBack> clients =
             new Dictionary<string, IMyContractCallBack>();
        private static object locker = new object();
        public string s = string.Empty;
      
        public void NormalFunction()
        {
            IMyContractCallBack callback = OperationContext.Current.GetCallbackChannel<IMyContractCallBack>();
            callback.CallBackFunction("Identity");
        }
        public void InsertData(string macineKey)
        {
            IMyContractCallBack callback = OperationContext.Current.GetCallbackChannel<IMyContractCallBack>();
               using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
               {
                    EndpointAddress clientAddress = OperationContext.Current.Channel.RemoteAddress;
                    lock (locker)
                    {
                        //remove the old client
                        if (clients.Keys.Contains(clientAddress.Uri.ToString()))
                            clients.Remove(clientAddress.Uri.ToString());
                        clients.Add(clientAddress.Uri.ToString(), callback);
                    }
                   
                    MachineDetail tbl = new MachineDetail();
                    tbl.MachineKey = macineKey;
                    tbl.CreatedDate=System.DateTime.Now;
                    tbl.ClientAddress = clientAddress.Uri.ToString();
                    entities.MachineDetails.Add(tbl);
                    entities.SaveChanges();
                }
          
        }
        public void UpdateData(string FunctionList,string machineKey)
        {
             using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
            {
                var data = entities.MachineDetails.Where(m => m.MachineKey ==machineKey).FirstOrDefault();
                if (data != null)
                {
                    data.AvailableFunction = FunctionList;
                    entities.SaveChanges();
                }
            }

        }
    public  void NotifyServer(string clientAddress)
        {
            lock (locker)
            {
                var inactiveClients = new List<string>();
                foreach (var client in clients)
                {
                    if (client.Key == clientAddress)
                    {
                        try
                        {
                            client.Value.CallBackFunction("GetFunction");
                            System.Threading.Thread.Sleep(5000);
                        
                        }
                        catch (Exception ex)
                        {
                            inactiveClients.Add(client.Key);
                        }
                    }
                }

               if (inactiveClients.Count > 0)
                {
                    foreach (var client in inactiveClients)
                    {
                        clients.Remove(client);
                    }
                }
            }
         }

   }
