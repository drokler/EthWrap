using System.Collections.Generic;
using System.Linq;

namespace EthWrapGenerator.Solidity
{
    public class SolidityImportVisitor : SolidityBaseVisitor<List<string>>
    {
        public override List<string> VisitSourceUnit(SolidityParser.SourceUnitContext context)
        {
            return context.importDirective().SelectMany(Visit).ToList();
        }
        
        public override List<string> VisitImportDirective(SolidityParser.ImportDirectiveContext context)
        {
            return new List<string>() { context.GetText() };
        }
    }
}