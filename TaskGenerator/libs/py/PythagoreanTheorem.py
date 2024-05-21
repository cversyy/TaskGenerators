import json
import random

"""
    Функция для получения информации о файле
"""
def GetInfo():
    Tags = ["математика"]
    jsonInfo = {
        "ProjectName":"TasksGenerator",
        "Name" : "ТеоремаПифагора",
        "Subject" : "Math",
        "APIversion" : "0.1",
        "Description": "Задачи на теорему Пифагора",
        "Tags" : "math"
    }
    DLL_DESC = json.dumps(jsonInfo)

    return DLL_DESC

"""
    Класс для определения сторон и углов треугольника
"""

class Triangle:
        def GetABC(self):
            while(self.A**2 + self.B**2 != self.C**2):
                self.A = random.randint(5,20)
                self.B = random.randint(5,20)
                self.C = random.randint(5,20)
        def GetAngles(self):
            while(self.angleA + self.angleC + self.angleB != 180):
                self.angleA = random.randint(10,80)
                self.angleB = random.randint(10,80)
        def __init__(self):
            self.A = 1
            self.B = 1
            self.C = 3
            self.angleA = 0
            self.angleB = 0
            self.angleC = 90
         

"""
    Класс для генерации задачи на теорему пифагора
"""

class PythagoreanTheorem:
    def __init__(self):
        self.ANNOTATIONS = []
        self.taskString = ""
        self.TRIANGLE = Triangle()
    def GetTask(self):
        self.TRIANGLE.GetABC()
        taskType = random.randint(1,2)
        if taskType == 1:
            self.taskString += f"В прямоугольном тереугольнике один катет равен {self.TRIANGLE.A}" + f"\n , а второй {self.TRIANGLE.B}. Найдите гипотенузу этого треугольника"
            self. ANNOTATIONS.append(str(self.TRIANGLE.C))
        elif taskType == 2:
            self.taskString += f"В прямоугольном тереугольнике гипотенуза равна {self.TRIANGLE.C}"+ f"\n , а один катет {self.TRIANGLE.A}. Найдите второй катет этого треугольника"
            self.ANNOTATIONS.append(str(self.TRIANGLE.B))
        return self.taskString
    def GetAnnotations(self):
        return self.ANNOTATIONS

"""
    Функция для упаковки задачи перед отправкой
"""

def TasksGenerator(taskArg, showAnnotations):

    ANNOTATIONS = ""
    taskString = ""
    for i in range(int(taskArg)):
    #taskType = random.randint(1,2)
        taskType = 1
        if taskType == 1:
            TASK = PythagoreanTheorem()
            taskInfo = f"\nЗадча номер {i+1}: " + TASK.GetTask()
            taskString += taskInfo
            if showAnnotations == "true":
                #ANNOTATIONS = TASK.GetAnnotations()
                for info in TASK.GetAnnotations():
                    annotationInfo = f"\nОтвет к задаче номер {i+1}: " + info
                    ANNOTATIONS += annotationInfo
 
    jsonInfo = {
        #Используется для картинок и тп
        "ResultType" : " ",
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
