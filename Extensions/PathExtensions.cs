using System.IO;

namespace Program.Extensions
{
    internal static class PathExtensions
    {
        internal static string GetLoaderName(this string dllPath)
        {
            return dllPath.GetFileName().Replace(".dll", ".Loader");
        }
        internal static string GetFileName(this string dllPath)
        {
            return Path.GetFileName(dllPath);
        }
        

    }
}
