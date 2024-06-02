using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Program.Extensions
{
    internal static class PathExtensions
    {
        internal static string GetDllName(this string Dllpath)
        {
            string DllName = Path.GetFileName(Dllpath);
            DllName = DllName.Replace(".dll", ".Dll");
            return DllName;
        }
        internal static string GetPythonModule(this string Dllpath)
        {
            string PythonModuleName = Path.GetFileName(Dllpath);
            PythonModuleName = PythonModuleName.Remove(PythonModuleName.Length - 3);
            return PythonModuleName;
        }

    }
}
