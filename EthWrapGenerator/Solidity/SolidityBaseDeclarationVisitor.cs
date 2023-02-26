using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;

namespace EthWrapGenerator.Solidity
{
    public abstract class SolidityBaseDeclarationVisitor<T> : SolidityBaseVisitor<List<T>>
    {
        private static readonly CSharpCodeProvider _codeProvider = new CSharpCodeProvider();

        protected string GetValidPropertyName(string name)
        {
            var upperName = string.Concat(char.ToUpper(name[0]), name.Substring(1));
            return !_codeProvider.IsValidIdentifier(upperName)
                ? _codeProvider.CreateValidIdentifier(upperName)
                : upperName;
        }

        protected string GetSystemTypeName(SolidityParser.TypeNameContext typeNameContext)
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

            }

            return "String";
        }
    }
}