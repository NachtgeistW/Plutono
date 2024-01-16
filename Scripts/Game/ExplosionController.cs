using Godot;
using Plutono.Util;

namespace Plutono.Scripts.Game;

public partial class ExplosionController : Node
{
    [Export] private GpuParticles3D explosion;

    public override void _EnterTree()
    {
        base._EnterTree();

        EventCenter.AddListener<FingerUpEvent>(Explode);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        EventCenter.RemoveListener<FingerUpEvent>(Explode);
    }

    private void Explode(FingerUpEvent evt)
    {
        var transform = explosion.Transform;
        transform.Origin = new Vector3(evt.WorldPos.X, transform.Origin.Y, transform.Origin.Z);
        explosion.Transform = transform;
        explosion.Emitting = true;
    }
}