using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace CsGeneratorsLoader
{
    public class Loader
    {

        private string GetDllName(string dllPath)
        {
            return GetFileName(dllPath).Replace(".dll", ".Dll");
        }
        private string GetFileName(string dllPath)
        {
            return Path.GetFileName(dllPath);
        }
        private string CsModulesPath { get { return @".\libs\cs"; } }

        private StringBuilder Log = null;
        /// <summary>
        ///  Получение образца библиотеки
        /// </summary>
        /// <param name="path"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public dynamic GetGeneratorInstance(string path)
        {
            Assembly DLL = Assembly.LoadFrom(path);
            Type? type = DLL.GetType(GetDllName(path));
            if (type == null)
            {
                ChangeLog($"Cannot load file {path}");
                //throw new Exception( $"Cannot load file {path}");
                return null;
            }
            dynamic? obj = Activator.CreateInstance(type);
            if (obj == null)
            {
                ChangeLog($"Cannot create instance of {path}");
                return null; 
            }
                //throw new Exception($"Cannot create instance of {GetDllName(path)}");
            
            return obj;
        }

        /// <summary>
        /// Получение списка генераторов
        /// </summary>
        /// <param name="Generators"></param>
        public void GetGeneratorsInfo(ref Dictionary<JObject, string> GeneratorsAndPaths)
        {

            foreach (var dllPath in Directory.GetFiles(CsModulesPath, "*.dll"))
            {
                
                dynamic generator = GetGeneratorInstance(dllPath);
                if (generator == null)
                {
                    continue;
                }
                JObject info = JObject.Parse(generator.GetInfo());
                GeneratorsAndPaths.Add(info, dllPath);
                
            }

        }
        private void ChangeLog(string error)
        {
            if (Log == null) Log = new StringBuilder();
            Log.Append($"\n{error}"+" ! ");
        }
        public string GetLog() 
        {
            if (Log == null) return null;
            else return Log.ToString();
        }

    }
}
