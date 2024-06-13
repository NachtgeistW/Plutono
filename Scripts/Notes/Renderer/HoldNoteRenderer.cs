using System;
using Godot;

namespace Plutono.Core.Note.Render
{
    public partial class HoldNoteRenderer : MovableNoteRenderer
    {
        [Export] public Sprite3D head;
        [Export] public Sprite3D body;
        [Export] public Sprite3D end;
        protected float HoldingLength;

        public HoldNoteRenderer()
        {
        }

        public void Init(float beginTime, float length, float endTime)
        {
            var headTransform = head.Transform;
            var endTransform = end.Transform;
            headTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime);
            endTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, endTime);

            head.Transform = headTransform;
            end.Transform = endTransform;
        }

        protected override void Render()
        {
            //IsHolding = HoldingFingers.Count > 0;
        }

        public override bool IsTouch()
        {
            throw new System.NotImplementedException();
        }

        public override void OnHit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnClear(NoteGrade grade)
        {
            throw new System.NotImplementedException();
        }

        public override void OnNoteLoaded()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
