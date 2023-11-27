using Godot;
using Plutono.Core.Note.Render;
using System;

namespace Plutono.Core.Note
{
    public partial class BlankNote : MovableNote, ITapable
    {
        [Export] public TouchNoteRenderer NoteRenderer { get; set; }

        public BlankNote()
        {
            
        }

        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos)
        {
            throw new NotImplementedException();
        }

    }
}