using Godot;

namespace Plutono.Core.Note.Render
{
    public partial class TapNoteRenderer : NoteRenderer, IRendererTouchable
    {
        [Export] Sprite3D noteSprite;
        
        public bool DisplayNoteId { get; set; }

        public TapNoteRenderer()
        {
        }

        public void Move(double elapsedTime, float chartPlaySpeed)
        {
            var transform = Transform;

            var zPos = Transform.Origin.Z - chartPlaySpeed * (float)elapsedTime;
            transform.Origin = new Vector3(Transform.Origin.X, Transform.Origin.Y, zPos);

            Transform = transform;
        }

        public void OnClear(NoteGrade grade)
        {
            throw new System.NotImplementedException();
        }

        public void OnDispose()
        {
            throw new System.NotImplementedException();
        }


        public void Render(Note note)
        {
            throw new System.NotImplementedException();
        }
    }
}