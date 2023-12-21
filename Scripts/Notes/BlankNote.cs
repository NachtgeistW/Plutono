using Godot;
using Plutono.Core.Note.Render;
using System;

namespace Plutono.Core.Note
{
    public partial class BlankNote : Note, IMovable, ITapable
    {
        [Export] public TapNoteRenderer NoteRenderer { get; set; }

        public BlankNote()
        {
            
        }

        public void Initialize()
        {
            
        }

        public bool IsTouch()
        {
            throw new NotImplementedException();
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