# Experimental Roslyn Source Generator for Kentico Kontent
This project tries to demonstrate the power of [Roslyn's C# Source Generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) for generating POCO models for Kentico Kontent apps writtein in C#.
It's based on an implementation of the `Microsoft.CodeAnalysis.ISourceGenerator` interface decorated by the `Microsoft.CodeAnalysis.GeneratorAttribute`.

# Motivation
The main advantage of this approach is that one doesn't have to run any external app to keep their models in sync with Kentico Kontent's content model. The models are added dynamically to the `Compilation` object without the user even noticing. One can start using the models as soon as the `[assembly: GenerateKontentModelsFor(ProjectId)]` attribute is added to the project and the models will always be up to date.

Read more here: https://github.com/Kentico/kontent-generators-net/issues/103

# Prerequisites
- [VS Preview](https://visualstudio.microsoft.com/vs/preview/) 

# Usage
- Reference the `Kentico.Kontent.SourceGenerator` project
`<ProjectReference Include="..\Kentico.Kontent.SourceGenerator\Kentico.Kontent.SourceGenerator.csproj"  OutputItemType="Analyzer" ReferenceOutputAssembly="false" />`
- Add an assembly-level attribute `[assembly: GenerateKontentModelsFor("<your-project-id>")]`
- You can start using your models (e.g. `await client.GetItemsAsync<Article>()`)

# Complete example
![Solution](https://i.imgur.com/PodnPq9.png)

As you can see, there are no additional files in the project. All models are being added dynamically during the compile time.

**Program.cs**
```csharp
using System;
using System.Threading.Tasks;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Builders.DeliveryClient;
using KenticoKontentModels;
using static Kentico.Kontent.SourceGenerator.Demo.Program;

[assembly: GenerateKontentModelsFor(ProjectId)]

namespace Kentico.Kontent.SourceGenerator.Demo
{
    class Program
    {
        public const string ProjectId = "975bf280-fd91-488c-994c-2f04416e5ee3";

        public static async Task Main(string[] args)
        {
            IDeliveryClient client = DeliveryClientBuilder.WithProjectId(ProjectId).WithTypeProvider(new CustomTypeProvider()).Build();
            var articles = await client.GetItemsAsync<Article>();
            foreach (var article in articles.Items)
            {
                Console.WriteLine($"The article '{article.Title}' was posted on {article.PostDate.Value.ToShortDateString()}.");
            }
            Console.ReadLine();
        }
    }
}
```
**Output:**
![Console App](https://i.stack.imgur.com/Dr1aR.png)

# Known Issues
- Publishing / referencing `Kentico.Kontent.SourceGenerator` as a NuGet package doesn't work ATM
- [Tests](https://github.com/petrsvihlik/kontent-source-generator/tree/master/src/Kentico.Kontent.SourceGenerator.Tests) don't work
