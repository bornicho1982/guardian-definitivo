using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace GuardianUI.Converters;

/// <summary>
/// Converts an integer to a boolean.
/// Parameter can be used to specify the value that should result in true. Defaults to 1.
/// If parameter is "NotZero", any non-zero integer results in true.
/// If parameter is "Zero", zero results in true.
/// </summary>
public class IntToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            string? paramString = parameter as string;

            if (paramString == "NotZero")
            {
                return intValue != 0;
            }
            if (paramString == "Zero")
            {
                return intValue == 0;
            }

            if (int.TryParse(paramString, out int checkValue))
            {
                return intValue == checkValue;
            }

            // Default behavior: true if intValue is 1 or greater (positive)
            return intValue > 0;
        }
        return false; // Default to false if not an int or if logic doesn't apply
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            string? paramString = parameter as string;
            if (paramString == "NotZero")
            {
                return boolValue ? 1 : 0; // Or throw, as "NotZero" is ambiguous for true
            }
            if (paramString == "Zero")
            {
                return boolValue ? 0 : 1; // Or throw
            }
            if (int.TryParse(paramString, out int checkValue))
            {
                return boolValue ? checkValue : (checkValue == 0 ? 1: 0) ; // Return checkValue if true, else something else
            }
            return boolValue ? 1 : 0; // Default
        }
        return 0; // Default
    }
}
