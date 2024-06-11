using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedTimeDistance
{
    internal class DefaultVariables
    {
        private protected virtual int Speed1 {get;set;}
        private protected virtual int Distance {get;set;}
        private protected virtual int Time { get;set;}
        private protected virtual int Speed2 { get; set; }

        private protected Random random = new Random();
        

    }
    internal class HumanTasks : DefaultVariables
    {
       
        protected internal void SetVariables()
        {
            Speed1 = random.Next(5, 12);
            Speed2 = random.Next(4, 14);
            Time = random.Next(2, 5);
            Distance = (Speed1 + Speed2) * Time;
        }
        private List<string> ANNOTATIONS = new List<string>();
        protected internal string StringGenerator()
        {
            int TypeOfTask = random.Next(1, 3);
            StringBuilder TaskString = new StringBuilder();
            switch (TypeOfTask)
            {
                case 1:
                    TaskString.Append($"Два бегуна одновременно выбежали из одного города в разные стороны," +
                        $"\n Скорость первого бегуна {Speed1}км/ч. За {Time} часов они пробежали {Distance} км. С какой скоростью бежал второй бегун?");
                    ANNOTATIONS.Add(Speed2.ToString());
                    break;
                case 2:
                    TaskString.Append($"Два бегуна одновременно выбежали навстречу друг другу. " +
                        $"\n Первый бежал со скоростью {Speed1}км/ч, второй со скоростью {Speed2}км/ч. Какое расстояние было между ними, "+ 
                        $"\n если они встретились через {Time} часа?");
                    ANNOTATIONS.Add(Distance.ToString());
                    break;
                case 3:
                    TaskString.Append($"Из двух поселков, находящихся на расстоянии {Distance}км," +
                        $"\n вышли одновременно навстречу друг другу два лыжника. Скорость первого {Speed2} км/ч. С какой скоростью шел " + 
                        $"\n второй лыжник, если они встретились через {Time} часа?");
                    ANNOTATIONS.Add(Speed1.ToString());
                    break;
            }
            string RESULT = TaskString.ToString();
            return RESULT;
        }
        protected internal List<string> GetAnnotations() { return ANNOTATIONS; }
    }
    internal class ShipTasks : DefaultVariables
    {
        
    }
    internal class CarTasks : DefaultVariables 
    {
        
    }
    public class ResultDescription
    {
        public List<string> ResultType { get; set; }
        public List<string> Result { get; set; }
        public string Annotations { get; set; }

    }
    public class Dll
    {
        
        //Функция для самой задачи
        public string TaskGenerator(string ARGUMENT, bool ShowAnnotations)
        {
            List<string> TaskString = new List<string>();
            StringBuilder ANNOTATIONS = new StringBuilder();
            for (int i = 0; i < int.Parse(ARGUMENT); i++)
            {
                HumanTasks obj = new HumanTasks();
                obj.SetVariables();
                TaskString.Add($"\n Задача №{i+1} \n "+obj.StringGenerator());
                if (ShowAnnotations == true) 
                {
                    foreach (var info in obj.GetAnnotations())
                    {
                        ANNOTATIONS.Append($"\nОтвет к задаче {i + 1}: " + info);
                    }
                }
            }
            if (ShowAnnotations == false) { ANNOTATIONS.Append(""); }
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
            List<string> tags = new List<string>() {"Начальная школа"};
            var jsonDesc = new
            {
                ProjectName = "TasksGenerator",
                Name = "Задачи на движение",
                Subject = "Математика",
                APIversion = 1.0,
                MethodName = "GetValue",
                Description = "Задачи на Скорость/Время/Расстояние",
                Tags = tags
            };
            string DLL_DESC = JsonConvert.SerializeObject(jsonDesc);
            return DLL_DESC;
        }
    }
}
