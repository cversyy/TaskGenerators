namespace Program.Structures
{
    /// <summary>
    /// Класс для создания запроса генератору
    /// </summary>
    internal class GeneratorRequest
    {
        #region Свойсва запроса
        /// <summary>
        /// Кол-во задач которые надо сделать
        /// </summary>
        internal string TasksNumber { get; set; }
        /// <summary>
        /// Нужно ли показывать дополнения, например ответы
        /// </summary>
        internal bool IsShowAnnotations { get; set; }
        /// <summary>
        /// Тип генератора который надо использовать
        /// </summary>
        internal string GeneratorType { get; set; }
        internal string Loader { get; set; }
        #endregion
        #region Конструктор
        internal GeneratorRequest(string tasksNumber,string generatorType, string loader, bool isShowAnnotations=false)
        {
            TasksNumber = tasksNumber;
            IsShowAnnotations = isShowAnnotations;
            GeneratorType = generatorType;
            Loader = loader;
        }
        #endregion
    }
}
