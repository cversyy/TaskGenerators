import json
import random
from io import BytesIO
from PIL import ImageDraw
from PIL import Image
from PIL import ImageFont
import base64
"""
    Функция для получения информации о файле
"""
def GetInfo():
    tags = ["8 Класс", "Треугольники"]
    jsonInfo = {
        "ProjectName":"TasksGenerator",
        "Name" : "Теорема Пифагора",
        "Subject" : "Геометрия",
        "APIversion" : 1.0,
        "Description": "Задачи на теорему Пифагора",
        "Tags" : tags,
        "Extension" : None
    }
    DLL_DESC = json.dumps(jsonInfo)
    return DLL_DESC

"""
    Класс для определения сторон и углов треугольника
"""

class Triangle:
        def GetABC(self):
            while(self.A**2 + self.B**2 != self.C**2):
                self.A = random.randint(5,10)
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
        self.Sides = []
    def GetTask(self):
        self.TRIANGLE.GetABC()
        taskType = random.randint(1,2)
        if taskType == 1:
            self.taskString += f"В прямоугольном треугольнике один катет равен {self.TRIANGLE.A}," + f"\nа второй {self.TRIANGLE.B}. Найдите гипотенузу этого треугольника."
            self.ANNOTATIONS.append(str(self.TRIANGLE.C))
            self.Sides.append(str(self.TRIANGLE.A))
            self.Sides.append(str(self.TRIANGLE.B))
            self.Sides.append("?")
        elif taskType == 2:
            self.taskString += f"В прямоугольном треугольнике гипотенуза равна {self.TRIANGLE.C},"+ f"\nа один катет {self.TRIANGLE.A}. Найдите второй катет этого треугольника"
            self.ANNOTATIONS.append(str(self.TRIANGLE.B))
            self.Sides.append(str(self.TRIANGLE.A))
            self.Sides.append("?")
            self.Sides.append(str(self.TRIANGLE.C))
        return self.taskString
    def GetAnnotations(self):
        return self.ANNOTATIONS
    def GetSides(self):
        return self.Sides

"""
    Генерация картинок
"""

def FontSmall():
    return ImageFont.truetype('Roboto-Black.ttf', 10)

def FontBig():
    return ImageFont.truetype('Roboto-Black.ttf',20)

def CreateClearImage():
    return Image.new('RGB',(800,300), color="white")


def DrawOnImage(image,sideA,sideB,sideC):
    ImageDrawing = ImageDraw.Draw(image)
    ImageDrawing.polygon([(50,30), (50,200), (350, 200)], outline="black")
    TextTuple = (50,230)
    DrawTextOnImage(ImageDrawing,TextTuple,"","black", ImageFont.truetype('Roboto-Black.ttf', 15))
    FirstPointTuple = (30,100)
    DrawTextOnImage(ImageDrawing,FirstPointTuple,sideA,"black",FontBig())
    SecondPointTuple = (170,200)
    DrawTextOnImage(ImageDrawing,SecondPointTuple,sideB,"black",FontBig())
    ThirdPointTuple = (210,100)
    DrawTextOnImage(ImageDrawing,ThirdPointTuple,sideC,"black",FontBig())

def DrawTextOnImage(Draw,Points,Text,Color,Font):
    return Draw.text(Points,Text,Color,Font)

def Buffering(image):
    Buffer = BytesIO()
    image.save(Buffer,format='bmp')
    image = base64.b64encode(Buffer.getvalue())
    return image

def CreateImage(sideA, sideB, sideC):
    taskImage = CreateClearImage()
    DrawOnImage(taskImage,sideA,sideB,sideC)
    taskImageBase64 = Buffering(taskImage)
    return taskImageBase64

"""
    Функция для упаковки задачи перед отправкой
"""

def TasksGenerator(taskArg, showAnnotations):
    TaskPictures = []
    ANNOTATIONS = ""
    taskString = []
    resultType = []
    for i in range(int(taskArg)):
    #taskType = random.randint(1,2)
        taskType = 1
        if taskType == 1:
            TASK = PythagoreanTheorem()
            sides = TASK.GetSides()
            taskInfo = f"\nЗадча №{i+1} \n" + TASK.GetTask()
            resultType.append(CreateImage(sides[0],sides[1],sides[2]).decode("utf-8"));
            taskString.append(taskInfo)
            if showAnnotations == "true":
                #ANNOTATIONS = TASK.GetAnnotations()
                for info in TASK.GetAnnotations():
                    annotationInfo = f"\nОтвет к задаче номер {i+1}: "+ info
                    ANNOTATIONS += annotationInfo
 
    jsonInfo = {
        #Используется для картинок и тп
        "ResultType" : resultType,
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
