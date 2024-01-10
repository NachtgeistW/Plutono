using Godot;
using System;

namespace Plutono.Core.Note
{
    public partial class HoldNote : Note, IMovable, IHoldable
    {
        public HoldNoteData data;

        private float chartPlaySpeed = 5.0f;

        public float beginTime { get; private set; } = 0f;
        public float endTime { get; private set; } = 10f;
        protected float HoldingLength;

        [Export] public Render.HoldNoteRenderer NoteRenderer { get; set; }
        public float HoldingStartingTime { get; protected set; } = float.MaxValue;
        public float HeldDuration { get; protected set; }
        //public List<int> HoldingFingers { get; } = new List<int>(2);
        public bool IsHolding { get; protected set; }

        private double nowTime;
        private float offset;

        public void Initialize()
        {
            HoldingLength = endTime - beginTime;
            NoteRenderer.OnNoteLoaded(this);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            nowTime += delta;
        }

        public bool IsTouch(float xPos, out float deltaXPos, double touchTime, out double deltaTime)
        {
            var noteJudgingSize = data.size < 1.2 ? 0.6 : data.size / 2;
            var noteDeltaXPos = Mathf.Abs(xPos - data.pos);
            if (noteDeltaXPos <= noteJudgingSize)
            {
                deltaXPos = noteDeltaXPos;
                deltaTime = Math.Abs(touchTime - data.time);
                return true;
            }
            else
            {
                deltaXPos = float.MaxValue;
                deltaTime = double.MaxValue;
                return false;
            }
        }

        public void OnHoldStart(Vector2 worldPos, float curTime)
        {
            //计算手势是否点到自己
            // if 点到自己
            //{
            //  判定离开判定区间 = false
            //    isHolding = true;
            //  统计分数和生成特效
            //  移出头判判定序列
            //  移入按住判定的判定序列
            //}
            if (!IsHolding)
            {
                IsHolding = true;
                HoldingStartingTime = curTime;
                NoteRenderer.head.Hide();
                GD.Print($"OnHoldStart HoldingStartingTime {HoldingStartingTime} curTime: {curTime}");

                nowTime = curTime;
            }
        }

        public void UpdateHold(Vector2 worldPos, float curTime)
        {
            if (IsHolding)
            {
                HeldDuration = (curTime - HoldingStartingTime) * chartPlaySpeed;
                GD.Print($"curTime {curTime} HeldDuration {HeldDuration}");
                if (HeldDuration >= HoldingLength)
                {
                    OnHoldEnd();
                }
            }
            else
            {
                GD.Print("!IsHolding");
                OnHoldMiss();
            }

            /*
            if isHolding
            {
                计时
                isHolding = false
                修改音符长度和位置
                if 计时器 > 按住的时间
                {
                    OnHoldEnd()
                }
                else
                {
                    OnHoldMiss
                }
            }

            isHolding一直为true的方法：
            public bool holding值调整(Vector2 worldPos)
            {
                计算手势是否点到自己
                if 点到自己
                {
                    isHolding = true
                }
            }
            */
        }

        public void OnHoldEnd()
        {
            /*
                统计分数和生成特效
                将自己移出判定序列
                删除自己
            */
            GD.Print($"OnHoldEnd");
            IsHolding = false;
            QueueFree();
        }

        public void OnHoldMiss()
        {
            GD.Print("OnHoldMiss");
        }

        public bool ShouldMiss()
        {
            return !IsHolding;
        }
    }
}
