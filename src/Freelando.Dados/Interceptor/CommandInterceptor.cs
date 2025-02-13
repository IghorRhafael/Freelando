using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Freelando.Dados.Interceptor;

public class CommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine($"Data/Hora: {DateTime.Now} \n\rQuery: {command.CommandText}");

        Console.BackgroundColor = ConsoleColor.White;

        return base.ReaderExecuting(command, eventData, result);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Data/Hora: {DateTime.Now} \n\rQuery: {command.CommandText}");
        Console.BackgroundColor = ConsoleColor.White;
        return base.NonQueryExecuting(command, eventData, result);

    }
}
