using Godot;
using Plutono.Core.Note;
using System.Collections.Generic;
using Plutono.Util;
using BlankNote = Plutono.Scripts.Notes.BlankNote;

public partial class NoteController : Node3D
{
    public List<BlankNote> BlankNotes = new();
    public List<HoldNote> HoldNotes = new();

    [Export] BlankNote blankNote;
    [Export] HoldNote holdNote;

    protected float chartPlaySpeed = 5f;

    public override void _Ready()
    {
        base._Ready();

        blankNote.Initialize();
        BlankNotes.Add(blankNote);

        holdNote.Initialize();
        HoldNotes.Add(holdNote);
    }

    public override void _Process(double delta)
    {
        UpdateNotes(delta);
    }

    public void UpdateNotes(double delta)
    {
        foreach (var note in BlankNotes)
        {
            note.NoteRenderer.Move(delta, chartPlaySpeed);
        }

        foreach (var note in HoldNotes)
        {
            note.NoteRenderer.Move(delta, chartPlaySpeed);
        }
    }

}