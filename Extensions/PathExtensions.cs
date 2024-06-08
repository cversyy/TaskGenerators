using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.CodeDom;

namespace Program.Extensions
{
    internal static class PathExtensions
    {
        internal static string GetLoaderName(this string dllPath)
        {
            return dllPath.GetFileName().Replace(".dll", ".Loader");
        }
        internal static string GetDLLName(this string dllPath)
        {
            return dllPath.GetFileName().Replace(".dll", ".Dll");
        }
        internal static string GetFileName(this string dllPath)
        {
            return Path.GetFileName(dllPath);
        }
        internal static string GetPythonModule(this string Dllpath)
        {
            string PythonModuleName = Path.GetFileName(Dllpath);
            PythonModuleName = PythonModuleName.Remove(PythonModuleName.Length - 3);
            return PythonModuleName;
        }

    }
}
