using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GuardianUI.ViewModels;
using System;
using System.Collections.Generic;

namespace GuardianUI;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, Func<Control?>> _locator = new();

    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var type = data.GetType();
        if (_locator.TryGetValue(type, out var factory))
        {
            return factory();
        }
        else
        {
            var name = type.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            if (name == type.FullName) // Avoid infinite loop if "ViewModel" not in name
            {
                return new TextBlock { Text = "View Not Found for " + name };
            }

            var viewType = Type.GetType(name);
            if (viewType != null)
            {
                return (Control)Activator.CreateInstance(viewType)!;
            }
            else
            {
                return new TextBlock { Text = "View Not Found: " + name };
            }
        }
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }

    // Optional: Register specific ViewModel to View mappings if the convention isn't enough
    public void Register<TViewModel, TView>() where TViewModel : ViewModelBase where TView : Control, new()
    {
        _locator[typeof(TViewModel)] = () => new TView();
    }
}
