using System.Collections.Generic;

namespace EthWrapGenerator.Solidity
{
    
    public class SolidityInterfaceModel
    {
        public string Name { get; set; }
        public bool IsLibrary { get; set; }
        public List<SolidityContractStructModel> Structures { get; set; }
        public SolidityInterfaceModel()
        {
            Structures = new List<SolidityContractStructModel>();
        }
    }

    
    public class SolidityContractModel
    {
        public string Name { get; set; }
        public bool IsAbstract { get; set; }
        public List<SolidityContractModel> IsImplement { get; set; } 
        public List<SolidityContractPropertyModel> ContractProperties { get; set; }
        public List<SolidityContractStructModel> Structures { get; set; }

        public SolidityContractModel()
        {
            IsImplement = new List<SolidityContractModel>();
            ContractProperties = new List<SolidityContractPropertyModel>();
            Structures = new List<SolidityContractStructModel>();
        }
    }

    public class SolidityContractStructModel
    {
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
        public string Type { get; set; }
        
    }

    public class SolidityContractArrayPropertyModel
    {
        public int Size { get; set; } // -1 dynamic;
    }
}