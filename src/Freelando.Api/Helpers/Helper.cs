using System.Runtime.CompilerServices;

namespace Freelando.Api.Helpers;

public static class Helper
{
    /// <summary>
    /// Valida se o newValue é nulo, se for retorna o valor antigo, se não retorna o novo valor.
    /// Garante que não atuliza o valor para vazio ou nulo
    /// </summary>
    /// <param name="value"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string? GetValue(string? value, string? newValue)
    {
        return newValue is null ? value : newValue;
    }

}
