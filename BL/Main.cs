namespace Program.BL
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Program.Extensions;
    using Program.Structures;
    using System;
    using System.IO;
    using System.Reflection;
    using static Program.Structures.Definitions;

    internal class Main
    {   
        /// <summary>
        /// Создание списка генераторов
        /// </summary>
        /// <returns></returns>
        static protected internal List<TaskGenerator> GetGeneratorsList()
        {
            List<TaskGenerator> Generators = new List<TaskGenerator>();
            Dictionary<JObject,string> GeneratorsAndPaths = new Dictionary<JObject,string>();
            GetGeneratorsInfo(ref Generators, ref GeneratorsAndPaths);
            return Generators;
        }

        /// <summary>
        /// Получение информации о генераторах
        /// </summary>
        /// <param name="Generators">Список для храниния информации о генераторах</param>
        /// <param name="GeneratorsAndPaths">Словарь с путями задачи<JObject>Информация о генераторах</JObject>,<string>Пути к генератору</string></param>
        private static void GetGeneratorsInfo(ref List<TaskGenerator> Generators, ref Dictionary<JObject, string> GeneratorsAndPaths)
        {
            
                foreach (string loaderPath in Directory.GetFiles(LoadersPath, "*.dll"))
                {
                    List<JObject> jsonInfo = new List<JObject>();
                    dynamic loaders = GetGeneratorsInstance(loaderPath, loaderPath.GetLoaderName());
                try
                {
                    if (loaders != null)
                    {
                        loaders.GetGeneratorsInfo(ref GeneratorsAndPaths);
                        string DllLog = loaders.GetLog();
                        if (DllLog != null) { 
                            throw new Exception(DllLog);                        
                        }
                    }
                    else throw new Exception($"Using wrong loader! {loaderPath.GetLoaderName()}");
                }
                catch (Exception e)
                {
                    Log(e);
                }
                CreateTaskGenerators(GeneratorsAndPaths, ref Generators, loaderPath);
                GeneratorsAndPaths.Clear();
            }
            
        }

        /// <summary>
        /// Получение стороннего модуля
        /// </summary>
        /// <param name="loaderPath">Путь к загрузчику модуля</param>
        /// <param name="FileType">Тип загружаемого файла(загрузчик или библиотека)</param>
        /// <returns></returns>
        private static dynamic GetGeneratorsInstance(string loaderPath, string FileType)
        {
                
                Assembly DLL = Assembly.LoadFrom(loaderPath);
                Type? type = DLL.GetType(FileType);
            if (type == null)
            {
                return null;
            }
                dynamic? obj = Activator.CreateInstance(type);
                return obj;
            
            
        }
        
        /// <summary>
        /// Получение значения после работы генератора
        /// </summary>
        /// <param name="loaderPath">Путь к загрузчику</param>
        /// <param name="value">Кол-во задач</param>
        /// <param name="taskPath">Путь к библиотеке</param>
        /// <param name="showAnnotations">Нужно ли показывать дополнения(например ответы)</param>
        /// <returns></returns>
        internal static string GetGeneratedTasks(string loaderPath, string value, string taskPath, bool showAnnotations)
        {
            var funcRequest = new
            {
                Arguments = value.ToString(),
                ShowAnnotations = showAnnotations.ToString().ToLower()
            };
            dynamic Generator = GetGeneratorsInstance(loaderPath,loaderPath.GetLoaderName());
            string RequestDATA = JsonConvert.SerializeObject(funcRequest);
            Generator = Generator.GetGeneratorInstance(taskPath);
            return Generator.GetValue(RequestDATA);
        }

        /// <summary>
        /// Парсер генератора из JObject в TaskGenerator
        /// </summary>
        /// <param name="GeneratorsAndPaths">Пути к задачам</param>
        /// <param name="Generators">Словарь с путями задачи и информацией</param>
        /// <param name="loader">Путь к загрузчику генератора</param>
        private static void CreateTaskGenerators(Dictionary<JObject, string> GeneratorsAndPaths,ref List<TaskGenerator> Generators, string loader)
        {
            foreach (var info in GeneratorsAndPaths)
            {
                List<string>? ListTags = JsonConvert.DeserializeObject<List<string>>(info.Key["Tags"].ToString());
                TaskGenerator task = new TaskGenerator((string)info.Key[nameof(TaskGenerator.Name)],
                                                       (string)info.Key[nameof(TaskGenerator.Description)],
                                                       (string)info.Key[nameof(TaskGenerator.Subject)],
                                                       (string)info.Key[nameof(TaskGenerator.APIversion)],
                                                       (string)info.Key[nameof(TaskGenerator.ProjectName)],
                                                       ListTags,
                                                       info.Value,
                                                       loader);
                Generators.Add(task);
            }
            
        }
        /// <summary>
        /// Парсер полученного значения из генератора
        /// </summary>
        /// <param name="generatorRequest">Запрос генератору</param>
        /// <returns></returns>
        static protected internal GeneratorResult GenerateTasks(GeneratorRequest generatorRequest)
        {
            string stringResult;
            stringResult = GetGeneratedTasks(generatorRequest.Loader, generatorRequest.TasksNumber, generatorRequest.GeneratorType, generatorRequest.IsShowAnnotations);
            JObject jsonResult = JObject.Parse(stringResult);
            List<string>? ListResultType = JsonConvert.DeserializeObject<List<string>>(jsonResult["ResultType"].ToString());
            List<string>? ListResult = JsonConvert.DeserializeObject<List<string>>(jsonResult["Result"].ToString());
            GeneratorResult RESULT = new GeneratorResult(ListResult,
                                                         (string)jsonResult[nameof(GeneratorResult.Annotations)],
                                                         ListResultType);
            return RESULT;
        }
        static internal void Log(Exception ex)
        {
            MainWindow.LogError(ex.Message);
        }
    }
}


