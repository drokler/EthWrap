using System.Collections.Generic;
using System.Linq;

namespace EthWrapGenerator.Solidity.Visitors
{
    public class SolidityInterfaceDeclarationVisitor : SolidityBaseDeclarationVisitor<SolidityInterfaceModel>
    {
        public override List<SolidityInterfaceModel> VisitSourceUnit(SolidityParser.SourceUnitContext context)
        {
            return context.contractDefinition().SelectMany(Visit).ToList();
        }

        public override List<SolidityInterfaceModel> VisitContractDefinition(
            SolidityParser.ContractDefinitionContext context)
        {
            var type = context.contractType().GetText();
            if (type != "interface") return new List<SolidityInterfaceModel>();

            return new List<SolidityInterfaceModel>
            {
                new SolidityInterfaceModel
                {
                    Name = GetValidPropertyName(context.identifier().GetText())
                }
            };
        }
    }
}