using System.Collections.Generic;

namespace EthWrapGenerator.Solidity
{
    
    public class SolidityInterfaceModel
    { 
        public string Name { get; set; }
    }

    public class SolidityContractStructModel
    {
        public string ContractName { get; set; }
        public string Name { get; set; }
        public List<SolidityContractPropertyModel> Properties { get; set; }

        public SolidityContractStructModel()
        {
            Properties = new List<SolidityContractPropertyModel>();
        }
    }

    public class SolidityContractPropertyModel
    {
        public string Name { get; set; }
        public SolidityParser.TypeNameContext Type { get; set; }
        public string EvaluatedType => "string";

    }
    
    public class SolidityContractModel
    {
        public string Name { get; set; }
        public bool IsAbstract { get; set; }
        public List<string> IsImplement { get; set; } 
        public List<SolidityContractPropertyModel> ContractProperties { get; set; }

        public SolidityContractModel()
        {
            IsImplement = new List<string>();
            ContractProperties = new List<SolidityContractPropertyModel>();
        }
    }

}