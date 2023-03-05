using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;

namespace EthWrapGenerator.Solidity.Visitors
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

        
    }
}