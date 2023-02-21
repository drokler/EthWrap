using System.Collections.Immutable;
using System.Reflection;
using EthWrapGenerator.ABI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


namespace EthWrapGenerator.Test;

public class AbiTest
{

    [Fact]
    public void AbiGenerate()
    {
        var compilation = CreateCompilation("");
        
        var generator = new ABIWrapperSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(ImmutableArray.Create(generator))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("era", "era7Card.abi")));
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
        var runResult = driver.GetRunResult();
    }
    

    private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}