using Godot;

namespace Plutono.Scripts.Utils;

public static class Debug
{
    public static void Log(params object[] message)
    {
        GD.Print(message);
    }
}