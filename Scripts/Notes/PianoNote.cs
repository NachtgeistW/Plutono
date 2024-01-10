using Godot;
using Plutono.Core.Note.Render;
using System;

namespace Plutono.Core.Note
{
    public partial class PianoNote : Note, IMovable, ITapable, IPianoSoundPlayable
    {
        public PianoNoteData data;
        [Export] public TapNoteRenderer NoteRenderer { get; set; }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public bool ShouldMiss()
        {
            throw new NotImplementedException();
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

        public void OnPlayPianoSounds()
        {
            throw new NotImplementedException();
        }
    }
}