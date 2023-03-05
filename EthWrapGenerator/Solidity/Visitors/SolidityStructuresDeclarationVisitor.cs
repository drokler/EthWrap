using System.Collections.Generic;
using System.Linq;

namespace EthWrapGenerator.Solidity.Visitors
{
    public class SolidityStructuresDeclarationVisitor : SolidityBaseDeclarationVisitor<SolidityContractStructModel>
    {
        public override List<SolidityContractStructModel> VisitSourceUnit(SolidityParser.SourceUnitContext context)
        {
            return context.contractDefinition().SelectMany(Visit).ToList();
        }

        public override List<SolidityContractStructModel> VisitContractDefinition(SolidityParser.ContractDefinitionContext context)
        {
            var ret = new List<SolidityContractStructModel>();
            var contractName = GetValidPropertyName(context.identifier().GetText());
            
            var structDefinition = context.contractPart()
                .Select(t => t.structDefinition())
                .Where(t => t != null )
                .ToList();
            
            foreach (var structure in structDefinition)
            {
                ret.Add(new SolidityContractStructModel()
                {
                    ContractName = contractName,
                    Name = GetValidPropertyName(structure.identifier().GetText()),
                    Properties = structure.variableDeclaration().Select(t => new SolidityContractPropertyModel()
                    {
                        Name = GetValidPropertyName(t.identifier().GetText()),
                        Type = t.typeName()
                    } ).ToList(),
                    
                });
            }

            return ret;
        }
    }
}