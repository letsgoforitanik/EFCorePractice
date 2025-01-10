using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.API.Data.ValueConverters;

// Approach One - Seperate class for each converter
public class DateTimeToChar8Converter() : ValueConverter<DateTime, string>(dt => DateTimeToChar8(dt), char8 => Char8ToDateTime(char8))
{
    private static string DateTimeToChar8(DateTime dateTime)
    {
        return dateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
    }

    private static DateTime Char8ToDateTime(string char8String)
    {
        return DateTime.ParseExact(char8String, "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}
