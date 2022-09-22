using System;
using UnityEngine;

/// <summary>
/// [Project Setting] - [Other Setting] - [Scripting Define Symbols] - [Add] - [ENABLE_LOG]
/// </summary>
public static class Logger
{
    public const string ENABLE_LOG = "ENABLE_LOG";

    public static bool IsDebugBuild
    {
        get { return Debug.isDebugBuild; }
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void Log(object message, UnityEngine.Object context)
    {
       Debug.Log(message, context);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void LogFormat(string format, params object[] args)
    {
        Debug.LogFormat(format, args);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void LogError(object message, UnityEngine.Object context)
    {
        Debug.LogError(message, context);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message.ToString());
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        Debug.LogWarning(message.ToString(), context);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        Debug.DrawLine(start, end, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional(ENABLE_LOG)]
    public static void Assert(bool condition)
    {
        if (!condition) throw new Exception();
    }
}