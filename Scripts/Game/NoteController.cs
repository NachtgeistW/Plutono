﻿using Godot;
using Plutono.Core.Note;
using Plutono.Scripts.Game;
using Plutono.Util;
using System.Collections.Generic;
using BlankNote = Plutono.Scripts.Notes.BlankNote;

public partial class NoteController : Node3D
{
    [Export] private Game Game { get; set; }
    [Export] private TimeController TimeControl { get; set; }

    public List<BlankNote> BlankNotes { get; } = new();
    public List<HoldNote> HoldNotes { get; } = new();

    [Export] BlankNote blankNote;
    [Export] BlankNote blankNote2;
    [Export] HoldNote holdNote;

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

        blankNote.data = new BlankNoteData(1, 0, 1.2, 1.5);
        blankNote2.data = new BlankNoteData(1, -10f, 1.2, 3);

        BlankNotes.Add(blankNote);
        BlankNotes.Add(blankNote2);

        HoldNotes.Add(holdNote);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        UpdateNotes(TimeControl.CurTime, Game.ChartPlaySpeed);
    }

    public void UpdateNotes(double curTime, float chartPlaySpeed)
    {
        foreach (var note in BlankNotes)
        {
            note.Move(curTime, chartPlaySpeed);
        }

        foreach (var note in HoldNotes)
        {
            note.Move(curTime, chartPlaySpeed);
        }
    }

    private void OnNoteClear(NoteClearEvent<BlankNote> evt) => BlankNotes.Remove(evt.Note);
    private void OnNoteClear(NoteClearEvent<HoldNote> evt) => HoldNotes.Remove(evt.Note);
}