using System.IO;

namespace Editor.Editor.Scripting;

public delegate void ScriptUpdate(string scriptPath);

internal class ScriptMonitor
{
    public event ScriptUpdate OnScriptUpdated;

    private readonly FileSystemWatcher fileSystemWatcher;

    public ScriptMonitor(string scriptFolder)
    {
        fileSystemWatcher = new(scriptFolder);
        fileSystemWatcher.Changed += OnChanged;
        fileSystemWatcher.Created += OnCreated;
        fileSystemWatcher.Filter = "*.lua";
        fileSystemWatcher.IncludeSubdirectories = false;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        OnScriptUpdated?.Invoke(e.FullPath);
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        OnScriptUpdated?.Invoke(e.FullPath);
    }
}
