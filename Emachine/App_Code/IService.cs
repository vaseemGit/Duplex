using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.

   [ServiceContract(CallbackContract = typeof(IMyContractCallBack))]
    public interface IService
    {
        [OperationContract(IsOneWay = true)]
        void NormalFunction();
        
        [OperationContract(IsOneWay = true)]
        void InsertData(string macineKey);

        [OperationContract(IsOneWay = true)]
        void NotifyServer(string clientAddress);

       [OperationContract(IsOneWay = true)]
        void UpdateData(string functionList, string machineKey);
    }
    public interface IMyContractCallBack
    {
        [OperationContract(IsOneWay = true)]
        void CallBackFunction(string str);
    }

 