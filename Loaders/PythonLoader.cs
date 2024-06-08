using Newtonsoft.Json.Linq;
using Python.Runtime;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection.Emit;
using System.Text;

namespace PythonGeneratorsLoader
{
    public class Loader
    {   
        string PythonModulesPath { get { return @".\libs\py\"; } }
        string PythonProviderPath { get { return @".\libs\python_provider\python311.dll"; } }
        StringBuilder Log = null;
        const string DLL_MASK = "*.py";
        private string GetModuleName(string Dllpath)
        {
            string PythonModuleName = Path.GetFileName(Dllpath);
            return PythonModuleName.Remove(PythonModuleName.Length - 3);
        }
            
        /// <summary>
        /// Получение списка информации генераторов
        /// </summary>
        /// <returns></returns>
        public void GetGeneratorsInfo(ref Dictionary<JObject, string> GeneratorsAndPaths)
        {

            foreach (string dllPath in Directory.GetFiles(PythonModulesPath, DLL_MASK))
            {   
                dynamic generator = GetGeneratorInstance(dllPath);
                if (generator == null)
                {
                    continue;
                }
                JObject info = JObject.Parse(generator.GetInfo().ToString());
                GeneratorsAndPaths.Add(info, dllPath);

            }
            
        }

        /// <summary>
        /// Получение образца библиотеки
        /// </summary>
        /// <param name="dllPath">Путь к библиотеке</param>
        /// <param name="request">Запрос библиотеке</param>
        /// <returns></returns>
        public dynamic GetGeneratorInstance(string dllPath)
        {
            InitializePythonEngine();
            using (Py.GIL())
            {
                dynamic script = Py.Import(GetModuleName(dllPath));
                if (script.Contains(GetModuleName(dllPath)))
                {
                    return script;
                }
                else
                {
                    ChangeLog($"Cannot load file {dllPath}");
                    return null;
                }
                    
            }
          
        }

       private void InitializePythonEngine()
        {
            PythonEngine.Shutdown();
            Runtime.PythonDLL = PythonProviderPath;
            if (Runtime.PythonDLL == null)
            {
                ChangeLog($"Missing Python provider!");
            }
            PythonEngine.Initialize();
            dynamic sys = Py.Import("sys");
            sys.path.append(PythonModulesPath);
        }
        private void ChangeLog(string error)
        {
            if (Log == null) Log = new StringBuilder();
            Log.Append($"\n{error}" + " ! ");
        }
        public string GetLog()
        {
            if (Log == null) return null;
            else return Log.ToString();
        }

    }
}
