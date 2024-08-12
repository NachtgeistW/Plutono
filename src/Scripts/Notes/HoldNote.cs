using System;
using Godot;
using Plutono.Scripts.Utils;
using Plutono.Util;
using Plutono.Scripts.Game;
using Plutono.Core.Note.Render;

namespace Plutono.Core.Note;

public partial class HoldNote : Note, IMovableNote, IHoldable
{
	public HoldNoteData Data;
	[Export] private HoldNoteRenderer NoteRenderer { get; set; }

	public float chartPlaySpeed;

	public float HoldingLength;

	private double noteJudgingSize;
	public double HoldingStartingTime { get; protected set; } = float.MaxValue;
	public double HeldDuration { get; protected set; }
	//public List<int> HoldingFingers { get; } = new List<int>(2);
	public bool IsHolding { get; protected set; }
	public bool IsClear { get; private set; }

	private double nowTime;

	public HoldNote()
	{
		chartPlaySpeed = 5f;
	}

	public HoldNote(float playSpeed)
	{
		chartPlaySpeed = playSpeed;
	}

	public override void _Ready()
	{
		base._Ready();

		Initialize();

		NoteRenderer.OnNoteLoaded(chartPlaySpeed);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		nowTime += delta;
	}

	public void Initialize()
	{
		SetNoteJudgingSize();
			
		SetHoldData();
		return;

		void SetNoteJudgingSize() => noteJudgingSize = Data.size < 1.2 ? 0.6 * Parameters.noteSizeScale : Data.size * Parameters.noteSizeScale / 2;

		void SetHoldData()
		{
			var beginZPosInScene = IMovableNote.maximumNoteRange / IMovableNote.NoteFallTime(chartPlaySpeed) * (float)Data.BeginTime;
			var endZPosInScene = IMovableNote.maximumNoteRange / IMovableNote.NoteFallTime(chartPlaySpeed) * (float)Data.EndTime;
			HoldingLength = endZPosInScene - beginZPosInScene;
		}
	}

	public void Move(double curTime, float chartPlaySpeed)
	{
		if (!IsHolding)
		{
			var transform = Transform;

			var zPos = (float)(IMovableNote.maximumNoteRange / IMovableNote.NoteFallTime(chartPlaySpeed) * (Data.BeginTime - curTime));
			transform.Origin.Z = -zPos;

			Transform = transform;
		}
	}

	public bool IsTouch(float xPos, double touchTime, out float deltaXPos, out double deltaTime)
	{
		var noteDeltaXPos = Mathf.Abs(xPos - Data.pos);
		if (noteDeltaXPos <= noteJudgingSize)
		{
			deltaXPos = noteDeltaXPos;
			deltaTime = Math.Abs(touchTime - Data.BeginTime);
			return true;
		}
		else
		{
			deltaXPos = float.MaxValue;
			deltaTime = double.MaxValue;
			return false;
		}
	}

	public void OnHoldStart(Vector3 worldPos, double curTime, NoteGrade grade)
	{
		/*计算手势是否点到自己
         if 点到自己
        {
            判定离开判定区间 = false
            isHolding = true;
            统计分数和生成特效
            移出头判判定序列
          移入按住判定的判定序列
        }*/
		if (!IsHolding)
		{
			IsHolding = true;
			HoldingStartingTime = curTime;
			Debug.Log($"OnHoldStart HoldingStartingTime {HoldingStartingTime} HoldingLength {HoldingLength}");

			nowTime = curTime;

			//NoteRenderer.head.Hide();
			NoteRenderer.SetExplosionColour(grade);
		}
	}

	public void UpdateHold(Vector3 worldPos, double curTime)
	{
		if (IsHolding)
		{
			HeldDuration = (IMovableNote.maximumNoteRange / IMovableNote.NoteFallTime(chartPlaySpeed) * (curTime - HoldingStartingTime));
			//Debug.Log($"curTime {curTime} HeldDuration {HeldDuration}");

			//TODO:Verify 0.001
			if (HoldingLength - HeldDuration < 0.0001)
			{
				OnHoldEnd(NoteGrade.Perfect);
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

	public void OnHoldEnd(NoteGrade grade)
	{
		/*
            统计分数和生成特效
            将自己移出判定序列
            删除自己
        */
		Debug.Log($"OnHoldEnd");
		IsHolding = false;
		IsClear = true;
		OnClear(grade);
		//QueueFree();
	}

	public void OnHoldEnd(NoteGrade grade, double curTime)
	{
		OnHoldEnd(grade);
		Debug.Log("NoteJudgeControl Broadcast NoteClearEvent\n" +
		          $"Note: {Data.id} Time: {Data.BeginTime} CurTime: {curTime} Pos: {Data.pos} JudgeSize: {noteJudgingSize}");
	}

	public void OnHoldMiss()
	{
		Debug.Log("OnHoldMiss");
	}

	public void OnClear(NoteGrade grade)
	{
		NoteRenderer.OnClear(grade);
		EventCenter.Broadcast(new NoteClearEvent<HoldNote>
		{
			Note = this,
			Grade = grade,
			//DeltaXPos = deltaXPos
		});
	}

	public bool ShouldMiss(double curTime, GameMode mode)
	{
		return !IsHolding;
	}

	public void OnMiss()
	{
		EventCenter.Broadcast(new NoteMissEvent<HoldNote>
		{
			Note = this,
		});
		QueueFree();
	}
}