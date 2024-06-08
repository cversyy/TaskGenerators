import json
import random

"""
    Функция для получения информации о файле
"""
def GetInfo():
    tags = ["7 Класс"]
    jsonInfo = {
        "ProjectName":"TasksGenerator",
        "Name" : "Выделение теплоты",
        "Subject" : "Физика",
        "APIversion" : "0.1",
        "Description": "Задачи на выделение теплоты",
        "Tags" : tags
    }
    DLL_DESC = json.dumps(jsonInfo)
    return DLL_DESC

"""
    Класс для определения сторон и углов треугольника
"""

class ValuesGenerator:
        def GetValues(self):
            values = ["140","460","500","920","400"]
            self.m = random.randint(2,30)
            self.t1 = random.randint(5,30)
            self.t2 = random.randint(30,100)
            self.c = random.choice(values)
            self.Q = int(self.c) * self.m * (self.t2 - self.t1)
        def __init__(self):
            self.Q = 0 
            self.t1 = 0
            self.t2 = 0
            self.c = 0
            self.m = 0





"""
    Класс для генерации задачи на теорему пифагора
"""

class HeatCapacity:
    def __init__(self):
        self.ANNOTATIONS = []
        self.taskString = ""
        self.VALUES = ValuesGenerator()
    def GetTask(self):
        self.VALUES.GetValues()
        taskType = random.randint(1,2)
        if taskType == 1:
            self.taskString += f"Деталь механизма массой {self.VALUES.m}кг нагревают от {self.VALUES.t1}," + f"\nдо {self.VALUES.t2} градусов. Какова теплоемкость детали, если выделилось {self.VALUES.Q}Дж тепла?"
            self.ANNOTATIONS.append(str(self.VALUES.c))
        elif taskType == 2:
            self.taskString += f"При нагреве детали выделилось {self.VALUES.Q}Дж тепла," + f"\nмасса детали {self.VALUES.m}кг, а ее теплоемкость {self.VALUES.c}. На сколько изменились температура детали?"
            T = self.VALUES.t2 - self.VALUES.t1
            self.ANNOTATIONS.append(str(T))
        return self.taskString
    def GetAnnotations(self):
        return self.ANNOTATIONS

def TasksGenerator(taskArg, showAnnotations):
    ANNOTATIONS = ""
    taskString = []
    for i in range(int(taskArg)):
    #taskType = random.randint(1,2)
        taskType = 1
        if taskType == 1:
            TASK = HeatCapacity()
            taskInfo = f"\nЗадча №{i+1} \n" + TASK.GetTask()
            taskString.append(taskInfo)
            if showAnnotations == "true":
                for info in TASK.GetAnnotations():
                    annotationInfo = f"\nОтвет к задаче номер {i+1}: " + info
                    ANNOTATIONS += annotationInfo
 
    jsonInfo = {
        #Используется для картинок и тп
        "ResultType" : None,
        #Численный результат
        "Result" : taskString,
        #Дополнения в виде ответов и тп
        "Annotations" : ANNOTATIONS
    }
    DLL_DESC = json.dumps(jsonInfo)
    return DLL_DESC


"""
    Функция для отправки задачи назад в Main
"""

def GetValue(Task_Json_Description):
    taskDescription = json.loads(Task_Json_Description)
    taskArg = str(taskDescription["Arguments"])
    showAnnotations = str(taskDescription["ShowAnnotations"])
    # Упаковка задач
    RESULT = TasksGenerator(taskArg,showAnnotations)
    return RESULT