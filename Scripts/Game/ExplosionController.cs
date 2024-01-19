﻿using Godot;
using Plutono.Scripts.Notes;
using Plutono.Util;

namespace Plutono.Scripts.Game;

public partial class ExplosionController : Node
{
    [Export] private AnimatedSprite3D animate;

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

    private static void Explode(NoteClearEvent<BlankNote> evt) => evt.Note.NoteRenderer.OnClear(evt.Grade);
}