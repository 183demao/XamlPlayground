using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Reflection;

Console.WriteLine();

public partial class MyClass
{
    [JSExport]
    internal async static Task<string> Greeting(string source)
    {
        var baseUrl = GetHRef().Replace("index.html", "");
        var httpClient = new HttpClient() { BaseAddress = new(baseUrl) };

        var syntaxTree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest));

        var dllNames = new string[]
        {
            "System.dll", "System.Core.dll", "System.Runtime.dll", "mscorlib.dll", "System.Private.CoreLib.dll",
            "System.Console.dll"
        };

        var referemces = new List<MetadataReference>();
        foreach (var dllName in dllNames)
        {
            using var stream = await httpClient.GetStreamAsync(dllName);
            referemces.Add(MetadataReference.CreateFromStream(stream));
        }

        var compilation = CSharpCompilation.Create("GeneratedAssembly")
                            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, concurrentBuild: false))
                            .AddReferences(referemces)
                            .AddSyntaxTrees(syntaxTree);

        var outputStream = new MemoryStream();
        var compilationResult = compilation.Emit(outputStream);
        outputStream.Position = 0;

        var msg = string.Join('\n', compilationResult.Diagnostics.Select(x => x.GetMessage()));

        Console.WriteLine("csc finish");
        Console.WriteLine(msg);
        
        var assContext = new AssemblyLoadContext("sandbox", true);
        var assembly = assContext.LoadFromStream(outputStream);

        var entryPoint = assembly.GetTypes().Select(x => x.GetMethod("Main", BindingFlags.Static | BindingFlags.Public)).Single();
        entryPoint.Invoke(null, null);

        assContext.Unload();

        return msg;
    }

    [JSImport("window.location.href", "main.js")]
    internal static partial string GetHRef();
}
