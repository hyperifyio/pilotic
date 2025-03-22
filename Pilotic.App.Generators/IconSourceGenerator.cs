using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Pilotic.App.Generators;

[Generator]
public class IconSourceGenerator : IIncrementalGenerator
{
    private static readonly string[] Sizes = { "16", "20", "24" };
    private static readonly string[] Styles = { "outline", "solid" };

    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        // Find all SVG files in the project
        var svgFiles = initContext.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase));

        // Transform each SVG file into a tuple of (name, content, size, style)
        var svgContents = svgFiles.Select((file, cancellationToken) => 
        {
            var path = file.Path;
            var fileName = Path.GetFileNameWithoutExtension(path);
            var directory = Path.GetDirectoryName(path);
            var style = Path.GetFileName(directory);
            var size = Path.GetFileName(Path.GetDirectoryName(directory));
            var content = file.GetText(cancellationToken)?.ToString() ?? string.Empty;
            return (fileName, content, size, style);
        }).Collect();

        // Generate a single file with all icons
        initContext.RegisterSourceOutput(svgContents, (spc, allSvgFiles) =>
        {
            var source = new StringBuilder();
            source.AppendLine("#nullable enable");
            source.AppendLine();
            source.AppendLine("namespace Pilotic.App.Icons");
            source.AppendLine("{");
            source.AppendLine("    public static class IconPaths");
            source.AppendLine("    {");
            
            // Generate constants for each icon name
            var uniqueNames = allSvgFiles.Select(f => f.fileName).Distinct();
            foreach (var name in uniqueNames)
            {
                var pascalName = ToPascalCase(name);
                source.AppendLine($"        public const string {pascalName} = \"{name}\";");
            }
            
            source.AppendLine("    }");
            source.AppendLine();
            source.AppendLine("    public static class Icons");
            source.AppendLine("    {");
            source.AppendLine("        private static readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> _svgCache = new();");
            source.AppendLine();
            source.AppendLine("        public static string GetSvg(string name, string size = \"24\", string style = \"outline\")");
            source.AppendLine("        {");
            source.AppendLine("            if (!_svgCache.TryGetValue(name, out var sizeDict))");
            source.AppendLine("            {");
            source.AppendLine("                sizeDict = new Dictionary<string, Dictionary<string, string>>();");
            source.AppendLine("                _svgCache[name] = sizeDict;");
            source.AppendLine("            }");
            source.AppendLine();
            source.AppendLine("            if (!sizeDict.TryGetValue(size, out var styleDict))");
            source.AppendLine("            {");
            source.AppendLine("                styleDict = new Dictionary<string, string>();");
            source.AppendLine("                sizeDict[size] = styleDict;");
            source.AppendLine("            }");
            source.AppendLine();
            source.AppendLine("            if (styleDict.TryGetValue(style, out var cachedSvg))");
            source.AppendLine("            {");
            source.AppendLine("                return cachedSvg;");
            source.AppendLine("            }");
            source.AppendLine();
            source.AppendLine("            string svg = (name, size, style) switch");
            source.AppendLine("            {");
            
            foreach (var svgFile in allSvgFiles)
            {
                source.AppendLine($"                (\"{svgFile.fileName}\", \"{svgFile.size}\", \"{svgFile.style}\") => @\"{svgFile.content.Replace("\"", "\"\"")}\",");
            }
            
            source.AppendLine("                _ => throw new ArgumentException($\"Icon '{name}' with size '{size}' and style '{style}' not found.\", nameof(name))");
            source.AppendLine("            };");
            source.AppendLine();
            source.AppendLine("            styleDict[style] = svg;");
            source.AppendLine("            return svg;");
            source.AppendLine("        }");
            source.AppendLine("    }");
            source.AppendLine("}");

            spc.AddSource("Icons.g.cs", source.ToString());
        });
    }

    private static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var words = input.Split(new[] { '-', '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", words.Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }
} 