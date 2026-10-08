using System.Reflection;
using System.Runtime.Versioning;
using System.Security;
using Microsoft.Win32;

namespace OpenCodeLauncher.Core.Services;

/// <summary>
/// Реализация автозапуска через ключ реестра HKCU
/// <c>Software\Microsoft\Windows\CurrentVersion\Run</c> (значение <c>OpenCodeLauncher</c>).
/// Приложение целевое — Windows.
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class WindowsStartupService : IWindowsStartupService
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "OpenCodeLauncher";

    /// <inheritdoc/>
    public bool IsEnabled()
    {
        var current = CurrentExePath();
        if (string.IsNullOrEmpty(current))
        {
            return false;
        }

        using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath);
        var value = key?.GetValue(ValueName) as string;
        return !string.IsNullOrWhiteSpace(value)
            && string.Equals(NormalizeExePath(value), current, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public void Enable()
    {
        var current = CurrentExePath();
        if (string.IsNullOrEmpty(current))
        {
            throw new InvalidOperationException("Не удалось определить путь к исполняемому файлу приложения.");
        }

        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
            key?.SetValue(ValueName, BuildCommandLine(current), RegistryValueKind.String);
        }
        catch (Exception ex) when (ex is SecurityException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Не удалось включить автозапуск: нет доступа к реестру.", ex);
        }
    }

    /// <inheritdoc/>
    public void Disable()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true);
            if (key?.GetValue(ValueName) is not null)
            {
                key.DeleteValue(ValueName, throwOnMissingValue: false);
            }
        }
        catch (Exception ex) when (ex is SecurityException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Не удалось отключить автозапуск: нет доступа к реестру.", ex);
        }
    }

    /// <summary>Формирует командную строку запуска с кавычками: <c>"&lt;путь&gt;"</c>.</summary>
    public static string BuildCommandLine(string exePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(exePath);
        return $"\"{exePath}\"";
    }

    /// <summary>Снимает кавычки и окружающие пробелы со значения из реестра; null → пустая строка.</summary>
    public static string NormalizeExePath(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return value.Trim().Trim('"').Trim();
    }

    /// <summary>Текущий путь к исполняемому файлу приложения (может быть null, если не определён).</summary>
    private static string? CurrentExePath()
    {
        if (!string.IsNullOrEmpty(Environment.ProcessPath))
        {
            return Environment.ProcessPath;
        }

        // Single-file приложения: Assembly.Location пуст, а exe лежит в AppContext.BaseDirectory.
        var exeName = Assembly.GetEntryAssembly()?.GetName().Name + ".exe";
        if (string.IsNullOrEmpty(exeName))
        {
            return null;
        }

        var candidate = Path.Combine(AppContext.BaseDirectory, exeName);
        return File.Exists(candidate) ? candidate : null;
    }
}