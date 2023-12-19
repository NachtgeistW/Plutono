using Godot;

namespace Plutono.Core.Note.Render
{
    /// <summary>
    /// 用于渲染可以移动的note
    /// </summary>
    public abstract partial class MovableNoteRenderer : NoteRenderer
    {
        public abstract void OnClear(NoteGrade grade);
        
        public void Move(double elapsedTime, float chartPlaySpeed, float curTime)
        {
            var transform = Transform;

            var zPos = Transform.Origin.Z - chartPlaySpeed * (float)elapsedTime;
            transform.Origin = new Vector3(Transform.Origin.X, Transform.Origin.Y, zPos);

            Transform = transform;
        }
    }
}