using System.Collections.Generic;
using System.Linq;

namespace EthWrapGenerator.Solidity
{
    public class SolidityInterfaceDeclarationVisitor : SolidityBaseDeclarationVisitor<SolidityInterfaceModel>
    {
        public override List<SolidityInterfaceModel> VisitSourceUnit(SolidityParser.SourceUnitContext context)
        {
            return context.contractDefinition().SelectMany(Visit).ToList();
        }

        public override List<SolidityInterfaceModel> VisitContractDefinition(SolidityParser.ContractDefinitionContext context)
        {
            var ret = new SolidityInterfaceModel();
            var type = context.contractType().GetText();
            if (type == "interface" || type == "library")
            {
                ret.IsLibrary = type == "library";
                ret.Name = GetValidPropertyName(context.identifier().GetText());
                
                var structDefinition = context.contractPart()
                    .Select(t => t.structDefinition())
                    .Where(t => t != null )
                    .ToList();
            
                foreach (var structure in structDefinition)
                {
                    ret.Structures.Add(new SolidityContractStructModel()
                    {
                        Name = GetValidPropertyName(structure.identifier().GetText()),
                        Properties = structure.variableDeclaration().Select(t => new SolidityContractPropertyModel()
                        {
                            Name = GetValidPropertyName(t.identifier().GetText()),
                            Type = GetSystemTypeName(t.typeName())
                        } ).ToList(),
                    
                    });
                }

                if (!ret.Structures.Any() && ret.IsLibrary)
                {
                    return new List<SolidityInterfaceModel>();
                }
                
                return new List<SolidityInterfaceModel>{ ret };
            }

            return new List<SolidityInterfaceModel>();

        }

   
    }
}