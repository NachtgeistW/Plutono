using Godot;

namespace Plutono.Core.Note.Render
{
    public partial class TouchNoteRenderer : MovableNoteRenderer
    {
        [Export] Sprite3D noteSprite;
        
        public TouchNoteRenderer()
        {
        }

        public override bool IsTouch()
        {
            throw new System.NotImplementedException();
        }

        public override void OnClear(NoteGrade grade)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        public override void OnHit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnNoteLoaded()
        {
            throw new System.NotImplementedException();
        }

        protected override void Render()
        {
            throw new System.NotImplementedException();
        }
    }
}