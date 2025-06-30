using Avalonia;
using Avalonia.Controls;

namespace GuardianUI.Components;

public partial class GhostPopup : UserControl
{
    public static readonly StyledProperty<string> GhostMessageProperty =
        AvaloniaProperty.Register<GhostPopup, string>(nameof(GhostMessage), "Thinking...");

    public string GhostMessage
    {
        get => GetValue(GhostMessageProperty);
        set => SetValue(GhostMessageProperty, value);
    }

    public GhostPopup()
    {
        InitializeComponent();
        // DataContext = this; // If GhostMessage is intended to be bound from this component itself.
    }
}
