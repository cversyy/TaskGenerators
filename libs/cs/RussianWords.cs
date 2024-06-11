using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RussianWords
{
    internal class GenerateListOfWords
    {
        private protected Random random = new Random();
        private protected List<string> WrongWords = new List<string>() { "вероисповедание", "диспансер", "жалюзи", "еретик", "банты", "бухгалтеров", "краны", "иксы",
                                                                         "ногтя", "отрочество", "средства", "торты", "шарфы", "красивее", "сливовый", "прозорливый",
                                                                         "брала", "вручит", "закупорить", "заперла", "звала", "лгала", "облегчить", "полила", "донельзя"};
        private protected List<string> CorrectWords = new List<string>() { "вероисповЕдание", "диспансЕр", "жалюзИ", "еретИк", "бАнты", "бухгАлтеров", "крАны", "Иксы",
                                                                         "нОгтя", "Отрочество", "срЕдства", "тОрты", "шАрфы", "красИвее", "слИвовый", "прозорлИвый",
                                                                         "бралА", "вручИт", "закУпорить", "заперлА", "звалА", "лгалА", "облегчИть", "полилА", "донЕльзя"};
        private protected List<string> ListOfWords = new List<string>(); 
        private protected List<string> AnswersList = new List<string>();

        protected internal void SetVariables()
        {
            while (ListOfWords.Count != 8)
            {
                {
                    int i = random.Next(0, WrongWords.Count);
                    if (!ListOfWords.Contains(WrongWords[i]))
                    {
                        ListOfWords.Add(WrongWords[i]);
                        AnswersList.Add(CorrectWords[i]);
                    }
                    else { continue; }

                }
            }
        }
        private List<string> ANNOTATIONS = new List<string>();
        protected internal string StringGenerator()
        {
            int TypeOfTask = 1;
            StringBuilder TaskString = new StringBuilder();
            switch (TypeOfTask)
            {
                case 1:
                    StringBuilder WORDS = new StringBuilder();
                    foreach (var word in ListOfWords) WORDS.Append(word+" ");
                    TaskString.Append(WORDS.ToString());
                    StringBuilder ANNOT = new StringBuilder();
                    foreach (var word in AnswersList) ANNOT.Append(word + " ");
                    ANNOTATIONS.Add(ANNOT.ToString());
                    break;
                
            }
            string RESULT = TaskString.ToString();
            return RESULT;
        }
        protected internal List<string> GetAnnotations() { return ANNOTATIONS; }
    }
    public class ResultDescription
    {
        public List<string> ResultType { get; set; }
        public List<string> Result { get; set; }
        public string Annotations { get; set; }

    }
    public class Dll
    {   

        public string TaskGenerator(string ARGUMENT, bool ShowAnnotations)
        {
            List<string> TaskString = new List<string>();
            StringBuilder ANNOTATIONS = new StringBuilder();
            for (int i = 0; i < int.Parse(ARGUMENT); i++)
            {
                GenerateListOfWords obj = new GenerateListOfWords();
                obj.SetVariables();
                TaskString.Add($"\n Задача №{i + 1}\n" + $"\n " + obj.StringGenerator());
                if (ShowAnnotations == true)
                {
                    foreach (var info in obj.GetAnnotations())
                    {
                        ANNOTATIONS.Append($"\nОтвет к задаче {i + 1}: " + info);
                    }
                }
            }
            if (ShowAnnotations == false) { ANNOTATIONS.Append(""); }
            //string textResult = TaskString.ToString();
            string textAnnotations = ANNOTATIONS.ToString();
            var jsonDesc = new ResultDescription()
            {
                //Используется для картинок и тп
                ResultType = null,
                //Численный результат
                Result = TaskString,
                //Дополнения в виде ответов и тп
                Annotations = textAnnotations
            };
            string RESULT = JsonConvert.SerializeObject(jsonDesc);
            return RESULT;
        }
        //Функция для возвращения результата
        public string GetValue(string TaskInfo)
        {
            JObject taskInfo = JObject.Parse(TaskInfo);
            var TaskArgs = (string)taskInfo["Arguments"];
            bool ShowAnnotations = (bool)taskInfo["ShowAnnotations"];
            string RESULT = TaskGenerator(TaskArgs, ShowAnnotations);
            return RESULT;
        }
        //Функция получения информации о DLL
        public string GetInfo()
        {
            List<string> tags = new List<string>() {"К ЕГЭ"};
            var jsonDesc = new
            {
                ProjectName = "TasksGenerator",
                Name = "Ударения",
                Subject = "Русский язык",
                APIversion = 1.0,
                MethodName = "GetValue",
                Tags = tags,
                Description = "Программа для проверки постановки ударения"
            };
            string DLL_DESC = JsonConvert.SerializeObject(jsonDesc);
            return DLL_DESC;
        }
    }
}
