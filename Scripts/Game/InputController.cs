using Godot;
using Plutono.Core.Note;
using System;
using System.Collections.Generic;

public partial class InputController : Node
{
    public double curTime;

    //public readonly Dictionary<int, FlickNote> FlickingNotes = new Dictionary<int, FlickNote>(); // Finger index to note
    public readonly Dictionary<int, HoldNote> HoldingNotes = new(); // Finger index to note
    public readonly List<HoldNote> TouchableHoldNotes = new(); // Hold
    public readonly List<Note> TouchableNormalNotes = new(); // Piano, Blank, Hold, Flick

    public void OnNoteCollected(Note note)
    {
        if (note is HoldNote)
        {
            HoldingNotes.Clear();
        }
    }

    [Export] BlankNote blankNote;
    [Export] HoldNote holdNote;

    [Export] Godot.GpuParticles3D explosion;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex == MouseButton.Left)
            {
                if (eventMouseButton.IsPressed())
                {
                    GD.Print("Pressed");
                }
                else
                    GD.Print("Released");
                GD.Print("Mouse Click/Unclick at: ", eventMouseButton.Position);

                // var transform = explosion.Transform;
                // transform.Origin = new Vector3(blankNote.Transform.Origin.X, transform.Origin.Y, transform.Origin.Z);
                // explosion.Transform = transform;
                // explosion.Emitting = true;

                blankNote.QueueFree();
            }
        }

        if (@event is InputEventKey eventKey && eventKey.Keycode == Key.Space)
        {
            if (eventKey.IsPressed())
            {
                holdNote.OnHoldStart(Vector2.Zero, (float)curTime);
            }
            else
            {
                holdNote.OnHoldEnd();
            }
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        curTime += delta;
        if (holdNote.IsHolding)
            holdNote.UpdateHold(Vector2.Zero, (float)curTime);
    }

}
