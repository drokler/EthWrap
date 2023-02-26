using System.Collections.Generic;
using Antlr4.Runtime;

namespace EthWrapGenerator.Solidity
{
    public class SolidityContractContext
    {
        public string Namespace { get; set; }
        public List<SolidityContractModel> Contracts { get; }
        public SolidityContractContext()
        {
            Contracts = new List<SolidityContractModel>();
        }

        public void AddFile(string content)
        {
            var inputStream = new AntlrInputStream(content);
            var lexer = new SolidityLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SolidityParser(tokenStream);
 
            var visitor = new SolidityContractDeclarationVisitor();
            
            var result = visitor.Visit(parser.sourceUnit());
            Contracts.AddRange(result);
            ;
        }
    }
}