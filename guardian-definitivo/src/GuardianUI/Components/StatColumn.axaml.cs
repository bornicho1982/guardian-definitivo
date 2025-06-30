using Avalonia;
using Avalonia.Controls;

namespace GuardianUI.Components;

public partial class StatColumn : UserControl
{
    // Example AvaloniaProperties for binding
    public static readonly StyledProperty<string> StatNameProperty =
        AvaloniaProperty.Register<StatColumn, string>(nameof(StatName), "STAT");

    public string StatName
    {
        get => GetValue(StatNameProperty);
        set => SetValue(StatNameProperty, value);
    }

    public static readonly StyledProperty<int> StatValueProperty =
        AvaloniaProperty.Register<StatColumn, int>(nameof(StatValue), 0);

    public int StatValue
    {
        get => GetValue(StatValueProperty);
        set => SetValue(StatValueProperty, value);
    }

    public StatColumn()
    {
        InitializeComponent();
        // DataContext = this; // Set DataContext to itself if properties are defined here
    }
}
