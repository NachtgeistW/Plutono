using System;
using Godot;

namespace Plutono.Core.Note.Render
{
    public partial class HoldNoteRenderer : MovableNoteRenderer
    {
        [Export] public Sprite3D head;
        [Export] public Sprite3D body;
        [Export] public Sprite3D end;

        private float width = 128;
        private float height = 128;
        protected float HoldingLength;

        public HoldNoteRenderer()
        {
        }

        public override void OnNoteLoaded()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="length"></param>
        /// <param name="endTime"></param>
        public void Init(float beginTime, float length, float endTime)
        {
            var headTransform = head.Transform;
            var endTransform = end.Transform;
            headTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime);
            endTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, endTime);

            var bodyTransform = body.Transform;
            bodyTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime + length / 2);
                        
            head.Transform = headTransform;
            body.Transform = bodyTransform;
            end.Transform = endTransform;

            body.Scale = new Vector3(2, 1, length * Parameters.pixel_per_unit / height / 2);
        }

        /// <summary>
        /// 控制 hold 的整体渲染
        /// </summary>
        protected override void Render()
        {
            //IsHolding = HoldingFingers.Count > 0;
            UpdateComponentStates();
            UpdateComponentOpacity();
            UpdateTransformScale();
        }

        private void UpdateComponentStates()
        {

        }

        private void UpdateComponentOpacity()
        {

        }

        private void UpdateTransformScale()
        {

        }

        /// <summary>
        /// note 被玩家 clear 的行为
        /// </summary>
        /// <param name="grade"></param>
        public override void OnClear(NoteGrade grade)
        {
            //Call the effect controller to play hit effect
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// note 被销毁时的行为
        /// </summary>
        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
