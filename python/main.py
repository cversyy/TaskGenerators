import os
import sys
import importlib.util
import json
import clr
import importlib
# списки путей
PY_DLL_PATHS = []
CS_DLL_PATHS = []

# загрузка python файлов
def Find_Py_Files(paths):
	directory = "./libs/py"
	#в paths список названий файлов
	paths += os.listdir(directory)
	for i in range(len(paths)):
		paths[i] = "./libs/py/" + paths[i]

Find_Py_Files(PY_DLL_PATHS)

# загрузка python модуля
def load_module(file_name, module_name):
    spec = importlib.util.spec_from_file_location(module_name, file_name)
    module = importlib.util.module_from_spec(spec)
    sys.modules[module_name] = module
    spec.loader.exec_module(module)
    return module

#Печатаем название модулей
for i in range(len(PY_DLL_PATHS)-1):
	#второй аргумент может быть любым названием
	my_module = load_module(PY_DLL_PATHS[i], "module")
	import module
	jsonInfo = module.GetInfo()
	DLL_DESC = json.loads(jsonInfo)
	print(DLL_DESC["Name"])

#загрузка C# файла
def Find_Cs_Files(paths):
	directory = "./libs/cs"
	paths += os.listdir(directory)
	for i in range(len(paths)):
		if(paths[i]!= "Newtonsoft.Json.dll"):
			paths[i] = paths[i][:-4]

Find_Cs_Files(CS_DLL_PATHS)

# загрузка C# модуля
def loadCSmodule(file_name):
	sys.path.append("./libs/cs")
	clr.AddReference(file_name)
	Dll = __import__(file_name)
	module = Dll.Dll()
	jsonInfo = module.GetInfo()
	DLL_DESC = json.loads(jsonInfo)
	return (DLL_DESC["Name"])

#Вывод названий модулей
for i in range(len(CS_DLL_PATHS)-1):
	DllName = loadCSmodule(CS_DLL_PATHS[i])
	print(DllName)

#class Loader:
#	def __init__(self):
#		self.info = ""
#	def getinfo(self,file_name):
#		sys.path.append("./libs/cs")
#		clr.AddReference(file_name)
#		Dll = __import__(file_name);
#		module = Dll.Dll();
#		jsonInfo = module.GetInfo()
#		print("jsoninfo: "+jsonInfo)
#		DLL_DESC = json.loads(jsonInfo)
#		print(DLL_DESC["Name"])
#		self.info = DLL_DESC["Name"]
#	def __del__(self):
#		print("del")