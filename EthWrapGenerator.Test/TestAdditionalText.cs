using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace EthWrapGenerator.Test;

public class TestAdditionalText: AdditionalText
{

    public static TestAdditionalText From(string folder, string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var path = assembly.GetManifestResourceNames()
            .FirstOrDefault(t => t.EndsWith($"{folder}.{fileName}", StringComparison.InvariantCultureIgnoreCase));
        var directoryString = System.IO.Path.GetDirectoryName(assembly.Location)!;
        using var stream  = assembly.GetManifestResourceStream(path!);
        using var reader  = new StreamReader(stream!);
        return new TestAdditionalText(System.IO.Path.Combine(directoryString, folder, fileName), reader.ReadToEnd());
    }
    
    private readonly string _content;

    public TestAdditionalText(string path, string content)
    {
        Path = path;
        _content = content;
    }
    
    public override SourceText? GetText(CancellationToken cancellationToken = new CancellationToken())
    {
        return SourceText.From(_content);
    }

    public override string Path { get; }
}