using Editor.Editor;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System;
using System.IO;
using System.Reflection;

namespace Editor.Engine.Scripting;

internal class ScriptManager
{
    public static ScriptManager Instance => instance.Value;

    private static readonly Lazy<ScriptManager> instance = new(() => new ScriptManager());

    private readonly Script luaScript = new();

    private ScriptManager() { }

    public void LoadSharedObjects(Project project)
    {
        UserData.RegisterType<Light>();
        UserData.RegisterType<Terrain>();
        UserData.RegisterType<Level>();
        UserData.RegisterType<Camera>();
        UserData.RegisterType<Project>();
        DynValue dynamicProject = UserData.Create(project);
        luaScript.Globals.Set("project", dynamicProject);
    }

    public void RegisterCSharpMethods()
    {

    }

    public DynValue LoadScript(string script)
    {
        return luaScript.DoString(script);
    }

    public void LoadScriptFile(string filePath)
    {
        using FileStream inStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader streamReader = new(inStream);
        string script = streamReader.ReadToEnd();

        _ = LoadScript(script);
    }

    public void LoadEmbeddedScript(string filePath)
    {
        luaScript.Options.ScriptLoader = new EmbeddedResourcesScriptLoader(Assembly.GetCallingAssembly());
        luaScript.DoFile(filePath);
    }

    public DynValue Execute(string functionName, params object[] parameters)
    {
        DynValue function = luaScript.Globals.Get(functionName);
        if (function.IsNil()) return function;

        return luaScript.Call(function, parameters);
    }
}
