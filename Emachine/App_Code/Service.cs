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
        public string errorMsg = string.Empty;
      
        public void NormalFunction()
        {
            IMyContractCallBack callback = OperationContext.Current.GetCallbackChannel<IMyContractCallBack>();
            callback.CallBackFunction("Identity",null,null);
        }
        public void InsertData(string macineKey)
        {
            IMyContractCallBack callback = OperationContext.Current.GetCallbackChannel<IMyContractCallBack>();
               using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
               {
                    var data = entities.MachineDetails.Where(m => m.MachineKey == macineKey).FirstOrDefault();
                    if (data != null)
                    {
                        string clientAddress = data.ClientAddress;
                        lock (locker)
                        {
                            //remove the old client
                            if (clients.Keys.Contains(clientAddress.ToString()))
                                clients.Remove(clientAddress.ToString());
                            clients.Add(clientAddress.ToString(), callback);
                        }
 
                    }
                    else
                    {

                        string clientAddress = Guid.NewGuid().ToString();
                        //EndpointAddress clientAddress = OperationContext.Current.Channel.RemoteAddress;
                        lock (locker)
                        {
                            //remove the old client
                            if (clients.Keys.Contains(clientAddress.ToString()))
                                clients.Remove(clientAddress.ToString());
                            clients.Add(clientAddress.ToString(), callback);
                        }

                        MachineDetail tbl = new MachineDetail();
                        tbl.MachineKey = macineKey;
                        tbl.CreatedDate = System.DateTime.Now;
                        tbl.ClientAddress = clientAddress.ToString();
                        entities.MachineDetails.Add(tbl);
                        entities.SaveChanges();
                    }
                }
          
        }
        public void UpdateData(string FunctionList,string ClientAddress)
        {
             using (DuplexApp_dbEntities entities = new DuplexApp_dbEntities())
            {
                var data = entities.MachineDetails.Where(m => m.ClientAddress == ClientAddress).FirstOrDefault();
                if (data != null)
                {
                    data.AvailableFunction = FunctionList;
                    entities.SaveChanges();
                }
            }

        }
        public void NotifyServer(string clientAddress, string functionType, string functionName)
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
                            if (functionType == "FindFunction")
                            {
                                client.Value.CallBackFunction("GetFunction", clientAddress,null);

                                System.Threading.Thread.Sleep(5000);
                            }
                            else if (functionType == "ExecuteFunction")
                            {
                                client.Value.CallBackFunction("ExecuteFunction", clientAddress,functionName);

                                System.Threading.Thread.Sleep(5000);
                            }
                        
                        }
                        catch (Exception ex)
                        {
                            errorMsg = ex.Message.ToString();
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
