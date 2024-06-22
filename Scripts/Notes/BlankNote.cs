using System;
using System.Diagnostics;
using Godot;
using Plutono.Core.Note;
using Plutono.Core.Note.Render;
using Plutono.Scripts.Game;
using Plutono.Util;

namespace Plutono.Scripts.Notes
{
    public partial class BlankNote : Note, IMovable, ITapable
    {
        public BlankNoteData data;
        [Export] private TapNoteRenderer NoteRenderer { get; set; }

        public BlankNote()
        {
            data = new BlankNoteData(1, -1, 1.2, 10);
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

        public override void _Ready()
        {
            base._Ready();

            NoteRenderer.OnNoteLoaded();
        }

        public void Move(double curTime, float chartPlaySpeed)
        {
            const float maximumNoteRange = 10f;
            var transform = Transform;

            var zPos = (float)(maximumNoteRange / NoteFallTime(chartPlaySpeed) * (data.time - curTime));
            transform.Origin.Z = -zPos;

            Transform = transform;

            static float NoteFallTime(float chartPlaySpeed)
            {
                var falldownSpeedRevision = 3f;
                return maximumNoteRange / (chartPlaySpeed * falldownSpeedRevision);
            }
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

        public void OnClear(NoteGrade grade)
        {
            NoteRenderer.OnClear(grade);
            EventCenter.Broadcast(new NoteClearEvent<BlankNote>
            {
                Note = this,
                Grade = grade,
                //DeltaXPos = deltaXPos
            });

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