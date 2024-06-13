using Godot;

namespace Plutono.Core.Note.Render
{
    public abstract partial class MovableNoteRenderer : NoteRenderer
    {
        public abstract bool IsTouch();
        public abstract void OnHit();
        public abstract void OnClear(NoteGrade grade);
        
        public void Move(double elapsedTime, float chartPlaySpeed, float curTime)
        {
            var transform = Transform;

            var zPos = Transform.Origin.Z + chartPlaySpeed * (float)elapsedTime;
            transform.Origin = new Vector3(Transform.Origin.X, Transform.Origin.Y, zPos);

            Transform = transform;
        }
    }
}