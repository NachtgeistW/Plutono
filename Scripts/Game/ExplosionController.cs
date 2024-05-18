using Godot;
using Plutono.Scripts.Notes;
using Plutono.Util;

namespace Plutono.Scripts.Game;

public partial class ExplosionController : Node
{
    public override void _EnterTree()
    {
        base._EnterTree();
        
        EventCenter.AddListener<NoteClearEvent<BlankNote>>(Explode);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        EventCenter.RemoveListener<NoteClearEvent<BlankNote>>(Explode);
    }

    private static void Explode(NoteClearEvent<BlankNote> evt) => System.Linq.Expressions.Expression.Empty();
}