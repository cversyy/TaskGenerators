namespace Program.Structures;
using System.IO;

/// <summary>
/// Класс констант
/// </summary>
internal class Definitions
{
    /// <summary>
    /// Путь к модулям Python
    /// </summary>
    static internal string PythonModulesPath { get { return @".\libs\py\"; } }
    /// <summary>
    /// Путь к Python311
    /// </summary>
    static internal string PythonProviderPath { get { return @".\libs\python_provider\python311.dll"; } }
    /// <summary>
    /// Путь к модулям C#
    /// </summary>
    static internal string CsModulesPath { get { return @".\libs\cs"; } }
}
