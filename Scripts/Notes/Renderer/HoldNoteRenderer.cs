using System;
using Godot;
using Plutono.Scripts.Utils;

namespace Plutono.Core.Note.Render
{
    public partial class HoldNoteRenderer : NoteRenderer, IRendererHold
    {
        [Export] public Sprite3D head;
        [Export] public Sprite3D body;
        [Export] public Sprite3D end;
        [Export] private AnimatedSprite3D explosion;

        private float width = 128;
        private float height = 128;
        protected float HoldingLength;

        bool INoteRenderer.DisplayNoteId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public HoldNoteRenderer()
        {
        }

        public void OnNoteLoaded(HoldNote note)
        {
            var beginTime = -note.beginTime;
            var endTime = -note.endTime;
            var length = beginTime - endTime;
            Debug.Log(length);

            var headTransform = head.Transform;
            var endTransform = end.Transform;
            headTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime);
            endTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, endTime);

            var bodyTransform = body.Transform;
            bodyTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime - length / 2);

            head.Transform = headTransform;
            body.Transform = bodyTransform;
            end.Transform = endTransform;

            body.Scale = new Vector3(2, 1, length * Parameters.pixel_per_unit / height / 2);

            explosion.Visible = false;
        }

        public void Move(double elapsedTime, float chartPlaySpeed)
        {
            var transform = Transform;

            var zPos = Transform.Origin.Z + chartPlaySpeed * (float)elapsedTime;
            transform.Origin.Z = zPos;

            Transform = transform;
        }

        public void Render(Note note)
        {
            UpdateComponentStates();
            UpdateComponentOpacity();
            UpdateTransformScale();
        }

        public void UpdateComponentStates()
        {
            throw new NotImplementedException();
        }

        public void UpdateComponentOpacity()
        {
            throw new NotImplementedException();
        }

        public void UpdateTransformScale()
        {
            throw new NotImplementedException();
        }

        public void OnClear(NoteGrade grade)
        {
            //Call the effect controller to play hit effect
            explosion.Play("good_end");
        }

        public void OnDispose()
        {
            throw new NotImplementedException();
        }
    }
}
