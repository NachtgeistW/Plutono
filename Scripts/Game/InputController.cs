using Godot;
using Plutono.Core.Note;
using System.Collections.Generic;
using Plutono.Scripts.Utils;
using Plutono.Util;
using BlankNote = Plutono.Scripts.Notes.BlankNote;

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

    [Export] GpuParticles3D explosion;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex == MouseButton.Left)
            {
                if (eventMouseButton.IsPressed())
                {
                    Debug.Log("Pressed");
                }
                else
                {
                    Debug.Log("Released");
                    EventCenter.Broadcast(new FingerUpEvent {WorldPos = new Vector3(1, 0, 0), Time = 10});
                }
                Debug.Log("Mouse Click/Unclick at: ", eventMouseButton.Position);

                // var transform = explosion.Transform;
                // transform.Origin = new Vector3(blankNote.Transform.Origin.X, transform.Origin.Y, transform.Origin.Z);
                // explosion.Transform = transform;
                // explosion.Emitting = true;

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

public struct FingerDownEvent : IEvent
{
    //public Finger Finger;
    public Vector3 WorldPos;
    public double Time;
}

public struct FingerMoveEvent : IEvent
{
    //public Finger Finger;
    public Vector3 WorldPos;
    public double Time;
}

public struct FingerUpEvent : IEvent
{
    //public Finger Finger;
    public Vector3 WorldPos;
    public double Time;
}
