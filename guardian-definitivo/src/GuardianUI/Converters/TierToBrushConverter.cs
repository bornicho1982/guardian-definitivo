using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace GuardianUI.Converters;

public class TierToBrushConverter : IValueConverter
{
    // Define Brushes for different tiers, potentially load from resources
    public IBrush ExoticBrush { get; set; } = new SolidColorBrush(Colors.Gold); // Example: #FFC857 (Solar)
    public IBrush LegendaryBrush { get; set; } = new SolidColorBrush(Colors.Purple); // Example: #A170F0 (Vacio)
    public IBrush RareBrush { get; set; } = new SolidColorBrush(Colors.Blue); // Example: #3790FF (Arcano)
    public IBrush UncommonBrush { get; set; } = new SolidColorBrush(Colors.Green);
    public IBrush CommonBrush { get; set; } = new SolidColorBrush(Colors.White);
    public IBrush DefaultBrush { get; set; } = new SolidColorBrush(Colors.Gray);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string tierName)
        {
            // This logic will need to be adjusted based on the actual tier names from Bungie API
            // e.g., "Exotic", "Legendary", "Rare", "Uncommon", "Common"
            return tierName.ToLowerInvariant() switch
            {
                "exotic" => ExoticBrush,
                "legendary" => LegendaryBrush,
                "rare" => RareBrush,
                "uncommon" => UncommonBrush,
                "common" => CommonBrush,
                _ => DefaultBrush,
            };
        }
        // Could also convert based on an enum or int if tier is represented differently
        return DefaultBrush;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
