using System.Collections.Generic;
using System.Linq;

namespace EthWrapGenerator.Solidity.Visitors
{
    public class SolidityContractDeclarationVisitor : SolidityBaseDeclarationVisitor<SolidityContractModel>
    {
        public override List<SolidityContractModel> VisitSourceUnit(SolidityParser.SourceUnitContext context)
        {
            return context.contractDefinition().SelectMany(Visit).ToList();
        }

        public override List<SolidityContractModel> VisitContractDefinition(
            SolidityParser.ContractDefinitionContext context)
        {
            var ret = new SolidityContractModel();
            var type = context.contractType().GetText();

            if (type != "contract")
            {
                return new List<SolidityContractModel>();
            }

            ret.IsAbstract = context.AbstractKeyword()?.GetText() == "abstract";
            ret.Name = GetValidPropertyName(context.identifier().GetText());
            ret.IsImplement = context.inheritanceSpecifier()?.Select(t => t.userDefinedTypeName().GetText()).ToList() ??
                              new List<string>();
            var stateVariableDeclaration = context.contractPart()
                .Select(t => t.stateVariableDeclaration())
                .Where(t => t != null && !t.ConstantKeyword().Any())
                .ToList();

            foreach (var property in stateVariableDeclaration)
            {
                ret.ContractProperties.Add(new SolidityContractPropertyModel()
                {
                    Name = GetValidPropertyName(property.identifier().GetText()),
                    Type = property.typeName()
                });
            }

            return new List<SolidityContractModel> { ret };
        }
    }
}