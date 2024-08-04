using Godot;
using System.Diagnostics;

namespace Plutono.Scripts.Utils;

public static class Debug
{
    [Conditional("DEBUG")]
    public static void Log(string message) { GD.Print(message); }
    [Conditional("DEBUG")]
    public static void Log(params object[] message) { GD.Print(message); }
    [Conditional("DEBUG")]
    public static void LogWarning(string message) { GD.PushWarning(message); }
    [Conditional("DEBUG")]
    public static void LogWarning(params object[] message) { GD.PushWarning(message); }
    [Conditional("DEBUG")]
    public static void LogError(string message) { GD.PushError(message); }
    [Conditional("DEBUG")]
    public static void LogError(params object[] message) { GD.PushError(message); }
}