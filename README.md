<p align="center">
    <a href="https://github.com/csmir/CSF.NET/wiki">
        <img src="https://user-images.githubusercontent.com/68127614/199816747-eadf3197-8be7-460a-879a-ae5ad2a903af.png" alt="logo">
    </a>
</p>

<p align="center">
    <img alt="buildstatus" src="https://img.shields.io/github/actions/workflow/status/csmir/CSF.NET/dotnet.yml?branch=master&style=for-the-badge">
    <a href="https://nuget.org/packages/CSF.NET"><img alt="download" src="https://img.shields.io/static/v1?style=for-the-badge&message=download%20on%20nuget&color=004880&logo=NuGet&logoColor=FFFFFF&label="></a>
    <a href="https://discord.gg/T7hCvShAx5"><img alt="help" src="https://img.shields.io/discord/1092510256384450652?style=for-the-badge"></a>
</p>

# ⚒️ CSF.NET - The Command Standardization Framework for .NET

Welcome to the new era of commands! CSF aims to reduce your boilerplate while keeping customizability high. It is intuitive, developer friendly and scalable, all with performance in mind. For more about writing code with this framework, visit the [documentation](https://github.com/csmir/CSF.NET/wiki)!

### 🔍 Ease of Use

CSF's main focus is for its users to enjoy writing code with the library and master it with relative ease. For this claim to have any value, a great deal is done to help a developer intuitively write code, alongside keeping documentation simple and complete.

### ⚒️ Customizability

While already fully functional out of the box, this command framework does not shy away from covering extensive applications with more specific needs, which in turn need more than the base features to function according to its developer's expectations. Within CSF, the `CommandManager` can be inherited, opening up a range of protected virtual methods that are applicable during pipeline execution. These can be overridden to modify and customize the pipeline to accompany various needs. 

Alongside this, `CommandContext`'s, `TypeReader`'s, `PreconditionAttribute`'s and `Parser`'s can all be inherited and custom ones created for environmental specifics, custom type interpretation and more.

### ⚡ Performance

While CSF's primary focus is to be the most versatile and easy to use command framework, it also aims to carry high performance throughout the entire execution flow, while keeping source code readable and comprehensible for library contributors and users alike. 
Benchmarks are ran for every version of CSF, with a near complete guarantee that it will not reach higher values upon newly released versions.

> The only cases where optimization will be cast aside is when the developer experience outweighs the need for further optimization. 
> This can be by reason of introducing certain features in the execution pipeline, or by newly introduced steps within said pipeline. In any other cases, be it minor updates or revisions, there will be no change in execution performance.

Benchmarks of the most recent stable release (`CSF 2.0`) show the following results:

|                Method |     Mean |    Error |  StdDev |   Gen0 | Allocated |
|---------------------- |---------:|---------:|--------:|-------:|----------:|
|         Parameterless | 701.7 ns |  8.99 ns | 8.41 ns | 0.1745 |   1.07 KB |
|           Parametered | 884.9 ns | 10.56 ns | 9.36 ns | 0.2060 |   1.27 KB |
| ParameteredUnprovided | 898.5 ns | 10.40 ns | 9.22 ns | 0.2069 |   1.27 KB |
|   NestedParameterless | 693.6 ns |  7.41 ns | 6.94 ns | 0.1745 |   1.07 KB |
|     NestedParametered | 895.2 ns |  5.68 ns | 5.03 ns | 0.2060 |   1.27 KB |

*These benchmarks represent the command execution pipeline from the moment when a parsed `CommandContext` is provided to the framework to be executed, to post execution handling.*

#### Legend:
- Mean      : Arithmetic mean of all measurements.
- Error     : Half of 99.9% confidence interval.
- StdDev    : Standard deviation of all measurements.
- Gen0      : GC Generation 0 collects per 1000 operations.
- Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B).
- 1 ns      : 1 Nanosecond (0.000000001 sec).
