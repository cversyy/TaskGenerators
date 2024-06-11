namespace Program.Structures
{
    /// <summary>
    /// Класс результата работы генератора
    /// </summary>
    internal class GeneratorResult
    {
        #region Свойства ответа
        /// <summary>
        /// Результат работы генератора
        /// </summary>
        internal List<string> Result { get; set; }
        /// <summary>
        /// Дополнения которые отправил генератор, например ответы
        /// </summary>
        internal string Annotations { get; set; }
        /// <summary>
        /// Свойство для дополнительного ответа, напривер картинки
        /// </summary>
        internal List<string> ResultType { get; set; }
        #endregion
        #region Конструкторы
        internal GeneratorResult(List<string> result, string annotations, List<string> resultType)
        {
            Result = result;
            Annotations = annotations;
            ResultType = resultType;
        }
        #endregion
    }
}
