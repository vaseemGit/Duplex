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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
  
        public void NormalFunction()
        {   try
            {
            IMyContractCallBack callback = OperationContext.Current.GetCallbackChannel<IMyContractCallBack>();
            callback.CallBackFunction("Identity",null,null);
             }
            catch (Exception ex)
            {
                log.Info(ex.Message.ToString());
            }
        }
        public void InsertData(string macineKey)
        {
            try
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
            catch (Exception ex)
            {
                log.Info(ex.Message.ToString());
            }
          
        }
        public void UpdateData(string FunctionList,string ClientAddress)
        {
            try
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
            catch (Exception ex)
            {
                log.Info(ex.Message.ToString());
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
                            log.Info(ex.Message.ToString());
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
