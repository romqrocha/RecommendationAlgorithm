using System;

namespace AlgorithmTest;

using System.Runtime.CompilerServices;

///<summary>
///Provides the full path to the source directory of the current project.<br/>
///(Only meaningful on the machine where this code was compiled.)<br/>
///From <a href="https://stackoverflow.com/a/66285728/773113"/>
///</summary>
internal static class ProjectSourcePath
{
    private const string myRelativePath = nameof(ProjectSourcePath) + ".cs";
    private static string? lazyValue;

    ///<summary>
    ///The full path to the source directory of the current project.
    ///</summary>
    public static string Value => lazyValue ??= Calculate();

    private static string Calculate([CallerFilePath] string? path = null)
    {
        // Assert(path!.EndsWith(myRelativePath, StringComparison.Ordinal));
        return path!.Substring(0, path.Length - myRelativePath.Length);
    }
}
