using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Freelando.Dados.Mapeamentos;
public class DateTimeToDateConverter : ValueConverter<DateTime, DateOnly>
{
    public DateTimeToDateConverter() : base(
        d => DateOnly.FromDateTime(d),
        d => d.ToDateTime(TimeOnly.MinValue)
        )
    {

    }
}
