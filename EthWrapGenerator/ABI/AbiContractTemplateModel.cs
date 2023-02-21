using System.Collections.Generic;
using System.Linq;
using Nethereum.ABI.Model;

namespace EthWrapGenerator.ABI
{

    public class ContractModel
    {
        public string Namespace { get; set; }
        public string ContractName { get; set; }
    }

    public class AbiContractStructModel
    {
        public string Name { get; set; }
        public AbiContractParameterModel[] Parameters { get; set; }
    }

    public class AbiContractParameterModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public bool Indexed { get; set; }
        public string SystemType { get; set; }
    }
    
    public class AbiContractFunctionModel
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        
        public bool NeedRequest { get; set; }
        
        public AbiContractParameterModel[] RequestModel { get; set; }
        public AbiContractParameterModel[] ResponseModel { get; set; }
        
    }
    

}