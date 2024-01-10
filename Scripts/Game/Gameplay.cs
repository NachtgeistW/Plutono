using Godot;
using Plutono.Scripts.Utils;

namespace Plutono.Scripts.Game;

public partial class Gameplay : Node3D
{
    public GameMode Mode { get; private set; } = GameMode.Floro;
}