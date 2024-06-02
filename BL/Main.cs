using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Program.Extensions;
using Program.Structures;
using Python.Runtime;
using System.IO;
using System.Reflection;
using static Program.Structures.Definitions;

namespace Program.BL
{
    internal class Main
    {   
        /// <summary>
        /// </summary>
        /// <param name="path">Путь к С# dll</param>
        /// <returns></returns>
        private static dynamic GetCsGeneratorInstance(string path)
        {
            Assembly DLL = Assembly.LoadFrom(path);
            Type? type = DLL.GetType(path.GetDllName());
            dynamic? obj = Activator.CreateInstance(type);
            return obj;
        }
        
        /// <summary>
        /// Получение C# генераторов
        /// </summary>
        /// <param name="Generators"></param>
        /// <returns></returns>
        private static List<TaskGenerator> AddCsGenerator(List<TaskGenerator> Generators)
        {
            foreach (var dllpath in Directory.GetFiles(CsModulesPath, "*.dll"))
            {
                dynamic generator = GetCsGeneratorInstance(dllpath);
                JObject info = JObject.Parse(generator.GetInfo());
                Generators.Add(CreateTaskGenerator(info, dllpath));
            }
            return Generators;
        }

        /// <summary>
        /// Получение json файла с информацией из dll
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dllpath"></param>
        /// <returns></returns>
        private static TaskGenerator CreateTaskGenerator(JObject info,string dllpath)
        {   
            List<string> ListTags = JsonConvert.DeserializeObject<List<string>>(info["Tags"].ToString());
            TaskGenerator Generator = new TaskGenerator((string)info[nameof(TaskGenerator.Name)], 
                                                        (string)info[nameof(TaskGenerator.Description)], 
                                                        (string)info[nameof(TaskGenerator.Subject)], 
                                                        (string)info[nameof(TaskGenerator.APIversion)], 
                                                        (string)info[nameof(TaskGenerator.ProjectName)], 
                                                        ListTags, 
                                                        dllpath);
            return Generator;
        }

        /// <summary>
        /// Получение модуля из Python файла
        /// </summary>
        /// <param name="request">Параметр который получает модуль</param>
        /// <param name="dllpath">Путь к Pyhton файлу</param>
        /// <param name="module">Названия функции которую мы хотим получить</param>
        /// <returns></returns>
        private static dynamic GetPyGeneratorInstance(string dllPath, string module, string? request)
        {
            PythonEngine.Shutdown();
            Runtime.PythonDLL = PythonProviderPath;
            PythonEngine.Initialize();
            dynamic sys = Py.Import("sys");
            sys.path.append(PythonModulesPath);
            dynamic result;
            using (Py.GIL())
            {
                dynamic script = Py.Import(dllPath.GetPythonModule());
                if (request != null)
                {
                    result = script.GetValue(request);
                }
                else { result = script.GetInfo(); }
            }
            return result;
        }

        /// <summary>
        /// Получение Python генераторов
        /// </summary>
        /// <param name="Generators"></param>
        /// <returns></returns>
        private static List<TaskGenerator> AddPyGenerator(List<TaskGenerator> Generators)
        {
            foreach (var dllpath in Directory.GetFiles(PythonModulesPath, "*.py"))
            {
                //dynamic GetInfo = GetPyGeneratorInstance(Definitions.PythonSideModulesPath, dllpath,"GetInfo");
                dynamic GetInfo = GetPyGeneratorInstance(dllpath, "GetInfo", null);
                string infa = GetInfo.ToString();
                JObject info = JObject.Parse(infa);
                Generators.Add(CreateTaskGenerator(info, dllpath));
            }
            return Generators;
        }

        /// <summary>
        /// Получение значения из C# генератора
        /// </summary>
        /// <param name="dllpath">Путь к dll</param>
        /// <param name="value">Кол-во задач</param>
        /// <param name="showAnnotations">Отвечает за показ дополнений</param>
        /// <returns></returns>
        protected static string GetCsGeneratedTasks(string dllpath, string value, bool showAnnotations)
        {
            var funcRequest = new
            {
                Arguments = value.ToString(),
                ShowAnnotations = showAnnotations.ToString().ToLower()
            };
            dynamic Generator = GetCsGeneratorInstance(dllpath);
            string RequestDATA = JsonConvert.SerializeObject(funcRequest);
            return Generator.GetValue(RequestDATA);

        }

        /// <summary>
        /// Получение значения из Python генератора
        /// </summary>
        /// <param name="dllpath">Путь к dll</param>
        /// <param name="value">Кол-во задач</param>
        /// <param name="showAnnotations">Отвечает за показ дополнений</param>
        /// <returns></returns>
        protected static string GetPyGeneratedTasks(string dllpath, string value, bool showAnnotations)
        {
            var funcRequest = new
            {
                Arguments = value.ToString(),
                ShowAnnotations = showAnnotations.ToString().ToLower()
            };
            string RequestDATA = JsonConvert.SerializeObject(funcRequest);
            //dynamic Generator = GetPyGeneratorInstance(Definitions.PythonSideModulesPath, dllpath, "GetValue");
            dynamic Generator = GetPyGeneratorInstance(dllpath, "GetValue", RequestDATA);
            return Generator.ToString();
        }

        /// <summary>
        /// Получение списка генераторов задач
        /// </summary>
        /// <returns></returns>
        /// 
        static protected internal List<TaskGenerator> GetGenerators()
        {
            List<TaskGenerator> Generators = new List<TaskGenerator>();
            Generators = AddCsGenerator(Generators);
            Generators = AddPyGenerator(Generators);
            return Generators;
        }

        /// <summary>
        /// Генерация задачи
        /// </summary>
        /// <param name="generatorRequest">Параметры генерации</param>
        /// <returns></returns>
        static protected internal GeneratorResult GenerateTask(GeneratorRequest generatorRequest)
        {

            string stringResult;

            if (Path.GetExtension(generatorRequest.GeneratorType) == ".dll")
            {
                stringResult = GetCsGeneratedTasks(generatorRequest.GeneratorType, generatorRequest.TasksNumber, generatorRequest.IsShowAnnotations);
            }
            else
            {
                stringResult = GetPyGeneratedTasks(generatorRequest.GeneratorType, generatorRequest.TasksNumber, generatorRequest.IsShowAnnotations);
            }
            JObject jsonResult = JObject.Parse(stringResult);
            GeneratorResult RESULT = new GeneratorResult((string)jsonResult[nameof(GeneratorResult.Result)],
                                                         (string)jsonResult[nameof(GeneratorResult.Annotations)],
                                                         (string)jsonResult[nameof(GeneratorResult.ResultType)]);
            return RESULT;
        }

    }
}


