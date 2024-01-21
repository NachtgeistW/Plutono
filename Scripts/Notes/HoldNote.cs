using System;
using Godot;
using Plutono.Scripts.Utils;
using Plutono.Util;
using Plutono.Scripts.Game;

namespace Plutono.Core.Note
{
    public partial class HoldNote : Note, IMovable, IHoldable
    {
        public HoldNoteData data;

        private float chartPlaySpeed;

        public float beginTime { get; private set; } = 0f;
        public float endTime { get; private set; } = 10f;
        protected float HoldingLength;

        [Export] public Render.HoldNoteRenderer NoteRenderer { get; set; }
        public double HoldingStartingTime { get; protected set; } = float.MaxValue;
        public double HeldDuration { get; protected set; }
        //public List<int> HoldingFingers { get; } = new List<int>(2);
        public bool IsHolding { get; protected set; }
        public bool IsClear { get; protected set; }

        private double nowTime;
        private float offset;

        public HoldNote()
        {
            data = new HoldNoteData(1, 3, 1.2, 10);
            chartPlaySpeed = 10f;
        }

        public HoldNote(float playSpeed)
        {
            data = new HoldNoteData(1, 3, 1.2, 10);
            chartPlaySpeed = playSpeed;
        }

        public override void _Ready()
        {
            base._Ready();

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

        public void OnHoldStart(Vector3 worldPos, double curTime)
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
                Debug.Log($"OnHoldStart HoldingStartingTime {HoldingStartingTime} curTime: {curTime}");

                nowTime = curTime;
            }
        }

        public void UpdateHold(Vector3 worldPos, double curTime)
        {
            if (IsHolding)
            {
                HeldDuration = (curTime - HoldingStartingTime) * chartPlaySpeed;
                Debug.Log($"curTime {curTime} HeldDuration {HeldDuration}");
                //TODO:Verify 0.001
                if (HoldingLength - HeldDuration < 0.0001)
                {
                    OnHoldEnd();
                }
            }
            else
            {
                Debug.Log("!IsHolding");
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
            Debug.Log($"OnHoldEnd");
            IsHolding = false;
            IsClear = true;
            EventCenter.Broadcast(new NoteClearEvent<HoldNote>
            {
                Note = this,
                //Grade = NoteGradeJudgment.Judge(deltaTime, mode),
                //DeltaXPos = deltaXPos
            });
            QueueFree();
        }

        public void OnHoldMiss()
        {
            Debug.Log("OnHoldMiss");
        }

        public bool ShouldMiss()
        {
            return !IsHolding;
        }
    }
}
