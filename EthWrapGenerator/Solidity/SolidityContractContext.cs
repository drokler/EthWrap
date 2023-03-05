using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using EthWrapGenerator.Solidity.Visitors;

namespace EthWrapGenerator.Solidity
{
    public class SolidityContractContext
    {
        public string Namespace { get; set; }
        private List<SolidityContractModel> _contracts { get; }
        private List<SolidityInterfaceModel> _interfaces { get; set; }
        private List<SolidityContractStructModel> _structures { get; set; }
        public SolidityContractModel Contract { get; set; }

        public SolidityContractContext()
        {
            _contracts = new List<SolidityContractModel>();
            _interfaces = new List<SolidityInterfaceModel>();
            _structures = new List<SolidityContractStructModel>();
        }

        public void AddFiles(List<string> contents)
        {
            var interfaceVisitor = new SolidityInterfaceDeclarationVisitor();
            var structureVisitor = new SolidityStructuresDeclarationVisitor();
            var contractVisitor = new SolidityContractDeclarationVisitor();
            
            foreach (var content in contents)
            {
                var inputStream = new AntlrInputStream(content);
                var lexer = new SolidityLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new SolidityParser(tokenStream);
                
                var interfaceVisitResult = interfaceVisitor.VisitSourceUnit(parser.sourceUnit());
                _interfaces.AddRange(interfaceVisitResult);
                parser.Reset();
                
                var structVisitResult = structureVisitor.VisitSourceUnit(parser.sourceUnit());
                _structures.AddRange(structVisitResult);
                parser.Reset();
                
                var contractVisitResult = contractVisitor.VisitSourceUnit(parser.sourceUnit());
                _contracts.AddRange(contractVisitResult);
            }

            var contract = _contracts.Single(t => !t.IsAbstract);
            Contract = contract;

        }
        
        protected string GetSystemTypeName(SolidityParser.TypeNameContext typeNameContext, List<SolidityInterfaceModel> interfaces = null)
        {
            if (typeNameContext.elementaryTypeName() != null)
            {
                var elementaryType = typeNameContext.elementaryTypeName();
                var elementaryTypeText = elementaryType.GetText();
                // : 'address' | 'bool' | 'string' | 'var' | Int | Uint | 'byte' | Byte | Fixed | Ufixed ;
                switch (elementaryTypeText)
                {
                    case "address": return "string";
                    case "string": return "string";
                    case "bool": return "bool";
                    case "byte": return "byte";
                }

                if (elementaryType.Int() != null || elementaryType.Uint() != null)
                {
                    return "BigInteger";
                }

                if (elementaryType.Fixed() != null || elementaryType.Ufixed() != null)
                {
                    return "decimal";
                }

                if (elementaryType.Byte() != null)
                {
                    return "byte[]";
                }

            } else if (typeNameContext.userDefinedTypeName() != null )
            {
                var userTypeName = typeNameContext.userDefinedTypeName();
                var userTypeNameText = userTypeName.GetText();
                if (interfaces != null && interfaces.Any(e => e.Name == userTypeNameText))
                {
                    return "string";
                }

                return userTypeNameText;
            } else if (typeNameContext.mapping() != null)
            {
                var mappingTypeName = typeNameContext.mapping();
                
            }

            return "String";
        }
    }
}