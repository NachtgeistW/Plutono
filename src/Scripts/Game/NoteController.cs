using Godot;
using Plutono.Core.Note;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;
using System.Collections.Generic;
using System.Linq;

public partial class NoteController : Node3D
{
    [Export] private Game Game { get; set; }
    [Export] private TimeController TimeControl { get; set; }

    public List<BlankNote> BlankNotes { get; } = new();
    public List<HoldNote> HoldNotes { get; } = new();
    public List<SlideNote> SlideNotes { get; } = new();

    [Export] private BlankNote blankNote;
    [Export] private BlankNote blankNote2;
    [Export] private HoldNote holdNote;
    [Export] private SlideNote slideNote;
    [Export] private SlideNote slideNote2;
    [Export] private SlideNote slideNote3;

    #region Event

    public override void _EnterTree()
    {
        base._EnterTree();

        //TODO: 能用逆变解决这个吗
        EventCenter.AddListener<NoteClearEvent<BlankNote>>(OnNoteClear);
        EventCenter.AddListener<NoteClearEvent<HoldNote>>(OnNoteClear);
        EventCenter.AddListener<NoteClearEvent<SlideNote>>(OnNoteClear);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        EventCenter.RemoveListener<NoteClearEvent<BlankNote>>(OnNoteClear);
        EventCenter.RemoveListener<NoteClearEvent<HoldNote>>(OnNoteClear);
        EventCenter.RemoveListener<NoteClearEvent<SlideNote>>(OnNoteClear);
    }

    #endregion

    public override void _Ready()
    {
        base._Ready();

        blankNote.Data = new BlankNoteData(1, 0, 1.2, 3.5);
        blankNote2.Data = new BlankNoteData(2, -10f, 1.2, 4.5);
        holdNote.Data = new HoldNoteData(3, 6, 1.2, 4.5, 6);
        slideNote.Data = new SlideNoteData(4, -2f, 1.2, 1.5);
        slideNote2.Data = new SlideNoteData(5, 0, 1.2, 2);
        slideNote3.Data = new SlideNoteData(6, 2f, 1.2, 2.5);

        BlankNotes.Add(blankNote);
        BlankNotes.Add(blankNote2);
        HoldNotes.Add(holdNote);
        SlideNotes.Add(slideNote);
        SlideNotes.Add(slideNote2);
        SlideNotes.Add(slideNote3);
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

        foreach (var note in SlideNotes)
        {
            note.Move(curTime, chartPlaySpeed);
        }

        BlankNotes.RemoveAll(x => x.ShouldMiss(curTime, Game.Mode));
        SlideNotes.RemoveAll(x => x.ShouldMiss(curTime, Game.Mode));
        HoldNotes.RemoveAll(x => x.ShouldMiss(curTime, Game.Mode));
    }

	private void OnNoteClear(NoteClearEvent<BlankNote> evt) => BlankNotes.Remove(evt.Note);
    private void OnNoteClear(NoteClearEvent<HoldNote> evt) => HoldNotes.Remove(evt.Note);
    private void OnNoteClear(NoteClearEvent<SlideNote> evt) => SlideNotes.Remove(evt.Note);
}