// guardian-definitivo/GuardianUI/Converters/IntToBoolConverter.cs
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace GuardianUI.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        // Convierte un int a bool. Útil para IsVisible si PowerLevel es > 0.
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                // Considerar 0 como "no mostrar" (false), cualquier otro valor como "mostrar" (true)
                // O puedes pasar un parámetro al conversor para definir el umbral.
                return intValue > 0;
            }
            return false; // Por defecto, no mostrar si no es un int o es 0
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
