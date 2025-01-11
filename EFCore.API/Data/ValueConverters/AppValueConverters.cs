using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.API.Data.ValueConverters;

public static class AppValueConverters
{
    public static ValueConverter<DateTime, string> GetDateTimeToChar8Converter()
    {
        return new ValueConverter<DateTime, string>(
            dateTime => dateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
            char8 => DateTime.ParseExact(char8, "yyyyMMdd", CultureInfo.InvariantCulture)
        );
    }
}
