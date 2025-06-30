// guardian-definitivo/GuardianUI/ViewLocator.cs
using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GuardianUI.ViewModels; // Asumiendo que los ViewModels estarán aquí

namespace GuardianUI
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object? data)
        {
            if (data is null)
                return new TextBlock { Text = "data is null" };

            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase; // ViewModelBase es una clase base común para ViewModels
        }
    }
}
