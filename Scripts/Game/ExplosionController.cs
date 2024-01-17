using Godot;
using Plutono.Util;

namespace Plutono.Scripts.Game;

public partial class ExplosionController : Node
{
    [Export] private AnimatedSprite3D animate;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        animate.AnimationFinished += OnAnimateFinish;
        EventCenter.AddListener<FingerUpEvent>(Explode);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        animate.AnimationFinished -= OnAnimateFinish;
        EventCenter.RemoveListener<FingerUpEvent>(Explode);
    }

    private void Explode(FingerUpEvent evt)
    {
        {
            animate.Visible = true;

            var transform = animate.Transform;
            transform.Origin.X = evt.WorldPos.X;
            animate.Transform = transform;
            animate.Play();
        }
    }

    private void OnAnimateFinish() => animate.Visible = false;
}