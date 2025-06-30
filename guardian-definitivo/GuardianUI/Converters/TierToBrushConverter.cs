// guardian-definitivo/GuardianUI/Converters/TierToBrushConverter.cs
using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace GuardianUI.Converters
{
    public class TierToBorderBrushConverter : IValueConverter
    {
        // Colores de tu guía (o similares para tiers)
        private static readonly SolidColorBrush ExoticBrush = SolidColorBrush.Parse("#FFC857"); // Solar (Dorado)
        private static readonly SolidColorBrush LegendaryBrush = SolidColorBrush.Parse("#A170F0"); // Void (Púrpura)
        private static readonly SolidColorBrush RareBrush = SolidColorBrush.Parse("#3790FF");     // Arcane (Azul)
        private static readonly SolidColorBrush UncommonBrush = new SolidColorBrush(Colors.Green); // Verde (Común en Destiny)
        private static readonly SolidColorBrush CommonBrush = new SolidColorBrush(Colors.White);   // Blanco
        private static readonly SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.Gray);

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string tierTypeName)
            {
                return tierTypeName.ToLowerInvariant() switch
                {
                    "exotic" => ExoticBrush,
                    "legendary" => LegendaryBrush,
                    "rare" => RareBrush,
                    "uncommon" => UncommonBrush,
                    "common" => CommonBrush,
                    _ => DefaultBrush,
                };
            }
            return DefaultBrush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TierToBackgroundBrushConverter : IValueConverter
    {
        // Colores más oscuros o adaptados para fondos de la "etiqueta" del tier
        private static readonly SolidColorBrush ExoticBgBrush = SolidColorBrush.Parse("#B8860B"); // DarkGoldenrod
        private static readonly SolidColorBrush LegendaryBgBrush = SolidColorBrush.Parse("#6A0DAD"); // DarkViolet
        private static readonly SolidColorBrush RareBgBrush = SolidColorBrush.Parse("#0073E6");     // Darker Blue
        private static readonly SolidColorBrush UncommonBgBrush = SolidColorBrush.Parse("#006400"); // DarkGreen
        private static readonly SolidColorBrush CommonBgBrush = SolidColorBrush.Parse("#606060");   // DarkGray
        private static readonly SolidColorBrush DefaultBgBrush = SolidColorBrush.Parse("#444444");

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string tierTypeName)
            {
                 return tierTypeName.ToLowerInvariant() switch
                {
                    "exotic" => ExoticBgBrush,
                    "legendary" => LegendaryBgBrush,
                    "rare" => RareBgBrush,
                    "uncommon" => UncommonBgBrush,
                    "common" => CommonBgBrush,
                    _ => DefaultBgBrush,
                };
            }
            return DefaultBgBrush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
