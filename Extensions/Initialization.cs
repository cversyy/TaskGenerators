namespace Program.Extensions
{
    internal static class Initialization
    {
        /// <summary>
        /// Получение текущей директории
        /// </summary>
        /// <returns></returns>
        public static string GetCurrenWorkDir()
        {
            return Environment.CommandLine.Remove(Environment.CommandLine.Length - Environment.CommandLine.GetFileName().Length);
        }
        /// <summary>
        /// Установка текущей директории
        /// </summary>
        /// <param name="dir"></param>
        public static void SetWorkDirectory(string dir)
        {
            Environment.CurrentDirectory = dir;
        }
    }
}
