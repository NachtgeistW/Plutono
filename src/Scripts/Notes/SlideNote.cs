using Godot;
using Plutono.Core.Note.Render;
using Plutono.Scripts.Game;
using Plutono.Util;
using System;
using System.Drawing;
using Plutono.Scripts.Utils;

namespace Plutono.Core.Note;

public partial class SlideNote : Note, IMovableNote, ITappable, ISlidable
{
	public SlideNoteData Data { get; set; }
	[Export] private TapNoteRenderer NoteRenderer { get; set; }

	public bool IsClear { get; private set; }
	public bool IsSliding { get; private set; }
	public double SlideStartTime { get; private set; }
	public float slideStartXPos { get; private set; }
	private float moved;

	private double noteJudgingSize;

	public SlideNote()
	{
		Data = new SlideNoteData(1, -1, 2.4, 10);
	}

	public SlideNote(SlideNoteData data)
	{
		this.Data = data;
	}

	public override void _Ready()
	{
		base._Ready();

		Initialize();
		NoteRenderer.OnNoteLoaded(Data);
	}

	public void Initialize()
	{
		SetNoteJudgingSize();
		noteJudgingSize = 1.2;
		Debug.Log(noteJudgingSize);
		return;

		void SetNoteJudgingSize() => noteJudgingSize = Data.size < 1.2 ? 0.6 * Parameters.noteSizeScale : Data.size * Parameters.noteSizeScale / 2;
	}

	public void Move(double curTime, float chartPlaySpeed)
	{
		var transform = Transform;

		var zPos = (float)(IMovableNote.maximumNoteRange / IMovableNote.NoteFallTime(chartPlaySpeed) * (Data.time - curTime));
		transform.Origin.Z = -zPos;

		Transform = transform;
	}

	public bool ShouldMiss(double curTime, GameMode mode)
	{
		return mode switch
		{
			GameMode.Stelo => curTime - Data.time > SteloMode.goodDeltaTime,
			GameMode.Arbo => curTime - Data.time > ArboMode.goodDeltaTime,
			GameMode.Floro => curTime - Data.time > ArboMode.goodDeltaTime,
			GameMode.Autoplay => false,
			_ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
		};
	}

	public bool IsTouch(float xPos, double touchTime, out float deltaXPos, out double deltaTime)
	{
		var noteDeltaXPos = Mathf.Abs(xPos - Data.pos);
		if (noteDeltaXPos <= noteJudgingSize)
		{
			deltaXPos = noteDeltaXPos;
			deltaTime = Math.Abs(touchTime - Data.time);
			return true;
		}
		else
		{
			deltaXPos = float.MaxValue;
			deltaTime = double.MaxValue;
			return false;
		}
	}

	public bool OnTap(float xPos, double hitTime, out float deltaXPos, out double deltaTime)
	{
		deltaTime = double.MaxValue;
		deltaXPos = float.MaxValue;
		
		if (IsClear) return true;
		return IsTouch(xPos, hitTime, out deltaXPos, out deltaTime);
	}

	public void OnSlideStart(float xPos, double curTime)
	{
		if (IsSliding || IsClear)
			return;

		if (!IsTouch(xPos, curTime, out _, out _)) return;
		IsSliding = true;
		SlideStartTime = curTime;
		slideStartXPos = xPos;
	}

	public void UpdateSlide(float xPos)
	{
		moved = (float)Math.Round(xPos - slideStartXPos, 3);
	}

	public bool CanBeClear(float xPos, GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Arbo:
			case GameMode.Floro:
				return IsReachRequirementPlutono();
			case GameMode.Stelo:
			case GameMode.Autoplay:
			default:
				return IsReachRequirementPlutono();
		}

		bool IsReachRequirementDeemo()
		{
			if (!IsSliding) return false;
			return IsTouch(xPos, Data.time, out _, out _);
		}

		bool IsReachRequirementPlutono()
		{
			if (!IsSliding) return false;
			if (!IsTouch(xPos, Data.time, out _, out _))
				return true;
			return Mathf.Abs(moved) >= noteJudgingSize;
		}
	} 

	public void OnSlideEnd(NoteGrade grade)
	{
		IsSliding = false;

		if (grade == NoteGrade.Miss)
		{
			OnMiss();
		}
		else
		{
			OnClear(grade);
		}
	}

	public void OnSlideEnd(NoteGrade grade, double curTime)
	{
		OnSlideEnd(grade);

		Debug.Log("---\nOnSlideEnd" +
			$"Note: {Data.id} Time: {Data.time} CurTime: {curTime} Pos: {Data.pos} JudgeSize: {noteJudgingSize}\n" +
		    $"SlideStartXPos: {slideStartXPos} SlideStartTime: {SlideStartTime}");
	}

	public void OnClear(NoteGrade grade)
	{
		IsClear = true;

		NoteRenderer.OnClear(grade);
		EventCenter.Broadcast(new NoteClearEvent<SlideNote>
		{
			Note = this,
			Grade = grade,
		});
	}

	public void OnMiss(double curTime)
	{
		Debug.Log($"---\nOnMiss\nNote: {Data.id} Time: {Data.time} CurTime: {curTime} Pos: {Data.pos}");
		OnMiss();
	}

	public void OnMiss()
	{
		IsClear = true;

		EventCenter.Broadcast(new NoteMissEvent<SlideNote>
		{
			Note = this,
		});
		QueueFree();
	}
}