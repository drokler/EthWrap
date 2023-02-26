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
        
        var generator = new WrapperSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(ImmutableArray.Create(generator))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("era", "era7Card.abi")));
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
        var runResult = driver.GetRunResult();
    }
    
    [Fact]
    public void AbiGenerateSol()
    {
        var compilation = CreateCompilation("");
        
        var generator = new WrapperSourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(ImmutableArray.Create(generator))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "biswap.abi")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "AuctionNFT.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "Address.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "Context.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "IERC20.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "IERC165.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "IERC721.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "IERC721Receiver.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "Ownable.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "Pausable.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "ReentrancyGuard.sol")))
            .AddAdditionalTexts(ImmutableArray.Create<AdditionalText>(TestAdditionalText.From("biswapAuction", "SafeERC20.sol")));
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
        var runResult = driver.GetRunResult();
    }

    

    private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}