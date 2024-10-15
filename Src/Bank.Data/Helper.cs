using System.Reflection;

namespace Bank.Data;

internal class Helper
{
    internal static string LoadSqlStatement(string statementName)
    {
        var sqlStatement = string.Empty;

        var resourceName = $"{Assembly.GetExecutingAssembly().GetName().Name}.{statementName}";

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream != null)
        {
            sqlStatement = new StreamReader(stream).ReadToEnd();
        }

        return sqlStatement;
    }
}