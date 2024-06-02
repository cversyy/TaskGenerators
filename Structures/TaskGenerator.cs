namespace Program.Structures
{
    internal class TaskGenerator
    {
        #region Поля генератора
        //Поля нужны для того, чтобы ничего не сломать при создании интерфейса
        //Эти поля не должны быть изменяемы в ходе выполнения программы
        /// <summary>
        /// Название генератора
        /// </summary>
        private string name;
        /// <summary>
        /// Описание возможностей генератора
        /// </summary>
        private string description;
        /// <summary>
        /// Предмет к которому относится генератор
        /// </summary>
        private string subject;
        /// <summary>
        /// Версия API может быть нужно для проверки 
        /// возможности использования функций
        /// </summary>
        private string apiversion;
        /// <summary>
        /// Название текущего проекта
        /// </summary>
        // TODO: Добавить провекру версий и проекта
        private string projectName;
        /// <summary>
        /// Путь к генератору
        /// </summary>
        private string path;
        /// <summary>
        /// Список тегов для генератора
        /// </summary>
        private List<string> tags;
        #endregion

        #region Свойства генератора
        /// <summary>
        /// Название генератора
        /// </summary>
        internal string Name { get { return name; } }
        /// <summary>
        /// Описание возможностей генератора
        /// </summary>
        internal string Description { get { return description; } }
        /// <summary>
        /// Список тегов для генератора
        /// </summary>
        internal List<string> Tags { get { return tags; } }
        /// <summary>
        /// Предмет к которому относится генератор
        /// </summary>
        internal string Subject { get { return subject; } }
        /// <summary>
        /// Версия API может быть нужно для проверки 
        /// возможности использования функций
        /// </summary>
        internal string APIversion { get { return apiversion; } }
        /// <summary>
        /// Название текущего проекта
        /// </summary>
        internal string ProjectName { get { return projectName; } }
        /// <summary>
        /// Путь к генератору
        /// </summary>
        internal string Path { get { return path; } }
        #endregion
        #region Конструктор генератора
        internal TaskGenerator(string name, string desc, string subject, string apiversion, string projectname, List<string> tags, string path)
        {
            this.name = name;
            this.description = desc;
            this.subject = subject;
            this.apiversion = apiversion;
            this.projectName = projectname;
            this.tags = tags;
            this.path = path;
        }
        #endregion
    }
}
