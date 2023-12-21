using Godot;
using Plutono.Core.Note;
using System;

public partial class Gameplay : Node3D
{
    [Export] BlankNote blankNote;
    [Export] HoldNote holdNote;

    protected float chartPlaySpeed = 5f;

    public override void _Ready()
    {
        base._Ready();
        blankNote.Initialize();
        holdNote.Initialize();
    }

    public override void _Process(double delta)
    {
        blankNote.NoteRenderer.Move(delta, chartPlaySpeed);
        holdNote.NoteRenderer.Move(delta, chartPlaySpeed);
    }

}
