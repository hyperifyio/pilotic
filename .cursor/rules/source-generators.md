# Source Generators

## Rule: Use Incremental Source Generators

Source generators should implement `IIncrementalGenerator` instead of the obsolete `ISourceGenerator` interface.

### Why?

Incremental source generators (`IIncrementalGenerator`) provide several advantages over the older `ISourceGenerator`:

1. **Better Performance**: Incremental generators can cache results between compilations, only regenerating when inputs actually change.
2. **Finer-grained Control**: They allow for more precise control over when and what gets regenerated.
3. **Better IDE Integration**: They work more efficiently in interactive environments like Visual Studio.
4. **Future-proof**: `ISourceGenerator` is considered obsolete, and new features will be added to `IIncrementalGenerator`.

### How to Fix

Instead of:
```csharp
[Generator]
public class MyGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }
    public void Execute(GeneratorExecutionContext context) { }
}
```

Use:
```csharp
[Generator]
public class MyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Define your pipeline here
        var source = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".txt"))
            .Select((file, cancellationToken) => file.GetText(cancellationToken)?.ToString());

        context.RegisterSourceOutput(source, (spc, content) =>
        {
            // Generate your source here
            spc.AddSource("MyGeneratedFile.cs", content);
        });
    }
}
```

### References

- [Incremental Generators Documentation](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)
- [Source Generators Best Practices](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) 