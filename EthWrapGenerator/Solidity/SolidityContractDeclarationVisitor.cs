using System.Collections.Generic;
using System.Linq;

namespace EthWrapGenerator.Solidity
{
    public class SolidityContractDeclarationVisitor : SolidityBaseDeclarationVisitor<SolidityContractModel>
    {
        public override List<SolidityContractModel> VisitSourceUnit(SolidityParser.SourceUnitContext context)
        {
            return context.contractDefinition().SelectMany(Visit).ToList();
        }

        public override List<SolidityContractModel> VisitContractDefinition(SolidityParser.ContractDefinitionContext context)
        {
            var ret = new SolidityContractModel();
            var type = context.contractType().GetText();

            if (type != "contract")
            {
                return new List<SolidityContractModel>();
            }
            
            ret.IsAbstract = context.AbstractKeyword()?.GetText() == "abstract";
            ret.Name = GetValidPropertyName(context.identifier().GetText());

               
            var structDefinition = context.contractPart()
                .Select(t => t.structDefinition())
                .Where(t => t != null)
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

            var stateVariableDeclaration = context.contractPart()
                .Select(t => t.stateVariableDeclaration())
                .Where(t => t != null && !t.ConstantKeyword().Any())
                .ToList();

            foreach (var property in stateVariableDeclaration)
            {
                ret.ContractProperties.Add(new SolidityContractPropertyModel()
                {
                    Name = GetValidPropertyName(property.identifier().GetText()),
                    Type = GetSystemTypeName(property.typeName())
                });
            }


            if (ret.IsAbstract && !ret.ContractProperties.Any() && !ret.Structures.Any())
            {
                return new List<SolidityContractModel>();
            }
            return new List<SolidityContractModel>{ ret };

        }

    }
}