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
        [Export] public Node3D node;
        [Export] private AnimatedSprite3D explosion;

        [Export] private HoldNote note;

        private float width = 128;
        private float height = 128;
        protected float HoldingLength;

        bool INoteRenderer.DisplayNoteId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public HoldNoteRenderer()
        {
        }

        #region GodotEvent

        public override void _EnterTree()
        {
            base._EnterTree();
            explosion.AnimationFinished += OnExplosionAnimateFinish;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            explosion.AnimationFinished -= OnExplosionAnimateFinish;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            UpdateComponentStates();
        }

        #endregion


        public void OnNoteLoaded()
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
            var transform = node.Transform;
            transform.Origin.Z += chartPlaySpeed * (float)elapsedTime;
            node.Transform = transform;
        }

        public void Render()
        {
            UpdateComponentStates();
            UpdateComponentOpacity();
            UpdateTransformScale();
        }

        public void UpdateComponentStates()
        {
            if (!note.IsClear && note.IsHolding && !explosion.IsPlaying())
            {
                explosion.Visible = true;
                explosion.Play("good_start");
            }
        }

        private void OnExplosionAnimateFinish()
        {
            explosion.Play("good_onhold");
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
