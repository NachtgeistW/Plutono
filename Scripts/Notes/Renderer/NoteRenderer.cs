using Godot;

namespace Plutono.Core.Note.Render
{
    public abstract partial class NoteRenderer : Node3D
    {
        public bool DisplayNoteId { get; protected set; }

        protected abstract void Render();
        public abstract void OnNoteLoaded();
        public abstract void OnDispose();
    }
}