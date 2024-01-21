﻿using Godot;
using Plutono.Core.Note;
using Plutono.Scripts.Game;
using Plutono.Util;
using System.Collections.Generic;
using BlankNote = Plutono.Scripts.Notes.BlankNote;

public partial class NoteController : Node3D
{
    public List<BlankNote> BlankNotes { get; } = new();
    public List<HoldNote> HoldNotes { get; } = new();

    [Export] BlankNote blankNote;
    [Export] HoldNote holdNote;

    protected float chartPlaySpeed = 10f;

    #region Event

    public override void _EnterTree()
    {
        base._EnterTree();

        EventCenter.AddListener<NoteClearEvent<BlankNote>>(OnNoteClear);
        EventCenter.AddListener<NoteClearEvent<HoldNote>>(OnNoteClear);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        EventCenter.RemoveListener<NoteClearEvent<BlankNote>>(OnNoteClear);
        EventCenter.RemoveListener<NoteClearEvent<HoldNote>>(OnNoteClear);
    }

    #endregion

    public override void _Ready()
    {
        base._Ready();

        BlankNotes.Add(blankNote);

        HoldNotes.Add(holdNote);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

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

    private void OnNoteClear(NoteClearEvent<BlankNote> evt) => BlankNotes.Remove(evt.Note);
    private void OnNoteClear(NoteClearEvent<HoldNote> evt) => HoldNotes.Remove(evt.Note);
}