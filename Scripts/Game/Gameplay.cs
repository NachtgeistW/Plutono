using Godot;
using Plutono.Core.Note;
using System;

public partial class Gameplay : Node3D
{
    [Export] BlankNote blankNote;
    [Export] HoldNote holdNote;

    protected float chartPlaySpeed = 5f;

    public override void _Process(double delta)
    {
        blankNote.NoteRenderer.Move(delta, chartPlaySpeed, 0);
        holdNote.NoteRenderer.Move(delta, chartPlaySpeed, 0);
    }

}
