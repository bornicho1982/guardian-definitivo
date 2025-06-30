using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GuardianUI.Converters;

public class ImageSourceConverter : IValueConverter
{
    private static readonly HttpClient _httpClient = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url && Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            // Asynchronously load image from URL
            return new TaskCompletionNotifier<Bitmap?>(LoadImageAsync(url));
        }
        if (value is string path && !string.IsNullOrEmpty(path)) // Could be a local asset path
        {
             try
            {
                // Attempt to load from assets if it's a relative path
                // This needs a proper URI like "avares://AppName/Assets/Images/image.png"
                // For now, let's assume it might be an absolute path or needs more context
                if (File.Exists(path)) // Basic check for local file system path
                {
                    return new Bitmap(path);
                }
                // If it's an Avalonia resource path (e.g., /Assets/icons/my_icon.png)
                // it needs to be prefixed correctly.
                // Example: new Bitmap(AssetLoader.Open(new Uri($"avares://GuardianUI{path}")))
                // This part is tricky without knowing the exact app name for avares at this stage.
                // The View using this converter will need to ensure the path is correct.
                // For now, we'll keep it simple.
                 if (path.StartsWith("avares://"))
                 {
                    return new Bitmap(AssetLoader.Open(new Uri(path)));
                 }

            }
            catch (Exception)
            {
                // Log error or return a default placeholder image
                return GetPlaceholderBitmap();
            }
        }
        return GetPlaceholderBitmap(); // Default or placeholder image
    }

    private async Task<Bitmap?> LoadImageAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            return new Bitmap(stream);
        }
        catch (Exception) // Log error
        {
            return GetPlaceholderBitmap(); // Fallback to placeholder
        }
    }

    private Bitmap? GetPlaceholderBitmap()
    {
        // Consider creating a small placeholder bitmap programmatically or loading one from assets
        // For simplicity, returning null, which might clear the image or show nothing.
        // Example: return new Bitmap(AssetLoader.Open(new Uri("avares://GuardianUI/Assets/images/placeholder.png")));
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// Helper class for handling async Task<T> in bindings
public class TaskCompletionNotifier<TResult> : System.ComponentModel.INotifyPropertyChanged
{
    public Task<TResult> Task { get; private set; }
    public TResult? Result => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default;
    public TaskStatus Status => Task.Status;
    public bool IsCompleted => Task.IsCompleted;
    public bool IsNotCompleted => !Task.IsCompleted;
    public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
    public bool IsCanceled => Task.IsCanceled;
    public bool IsFaulted => Task.IsFaulted;
    public AggregateException? Exception => Task.Exception;
    public string? ErrorMessage => Exception?.InnerExceptions.FirstOrDefault()?.Message;

    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

    public TaskCompletionNotifier(Task<TResult> task)
    {
        Task = task;
        if (!task.IsCompleted)
        {
            var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
            task.ContinueWith(t =>
            {
                var propertyChanged = PropertyChanged;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Status)));
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsCompleted)));
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsNotCompleted)));
                    if (t.IsCanceled)
                    {
                        propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsCanceled)));
                    }
                    else if (t.IsFaulted)
                    {
                        propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsFaulted)));
                        propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Exception)));
                        propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(ErrorMessage)));
                    }
                    else
                    {
                        propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
                        propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Result)));
                    }
                }
            },
            System.Threading.CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            scheduler);
        }
    }
}
