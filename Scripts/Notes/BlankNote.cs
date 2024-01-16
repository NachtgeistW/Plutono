using System;
using Godot;
using Plutono.Core.Note;
using Plutono.Core.Note.Render;

namespace Plutono.Scripts.Notes
{
    public partial class BlankNote : Note, IMovable, ITapable
    {
        public BlankNoteData data;
        [Export] public TapNoteRenderer NoteRenderer { get; set; }

        public BlankNote()
        {
            data = new BlankNoteData(1, -3, 1.2, 10);
        }

        public BlankNote(BlankNoteData data)
        {
            this.data = data;
        }

        public BlankNote(BlankNoteData data, TapNoteRenderer noteRenderer)
        {
            this.data = data;
            NoteRenderer = noteRenderer;
        }

        public void Initialize()
        {
            //data = new BlankNoteData(1, 1, 1.2, 10);
        }

        public bool IsTouch(float xPos, out float deltaXPos, double touchTime, out double deltaTime)
        {
            var noteJudgingSize = data.size < 1.2 ? 0.6 : data.size / 2;
            var noteDeltaXPos = Mathf.Abs(xPos - data.pos);
            if (noteDeltaXPos <= noteJudgingSize)
            {
                deltaXPos = noteDeltaXPos;
                deltaTime = Math.Abs(touchTime - data.time);
                return true;
            }
            else
            {
                deltaXPos = float.MaxValue;
                deltaTime = double.MaxValue;
                return false;
            }
        }

        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos)
        {
            throw new NotImplementedException();
        }

        public bool ShouldMiss()
        {
            throw new NotImplementedException();
        }

    }
}