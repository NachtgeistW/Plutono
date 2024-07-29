using Godot;
using Plutono.Core.Note.Render;
using Plutono.Scripts.Game;
using Plutono.Util;
using System;
using System.Drawing;
using Plutono.Scripts.Utils;

namespace Plutono.Core.Note;

public partial class SlideNote : Note, IMovable, ITapable, ISlidable
{
	public SlideNoteData Data { get; set; }
	[Export] private TapNoteRenderer NoteRenderer { get; set; }

	public bool IsClear { get; private set; }
	public bool IsSliding { get; private set; }
	public double SlideStartTime { get; private set; }
	public float slideStartXPos;
	private float moved;

	private double noteJudgingSize;

	public override void _Ready()
	{
		base._Ready();

		noteJudgingSize = Data.size < 1.2 ? 0.6 * Parameters.noteSizeScale : Data.size * Parameters.noteSizeScale / 2;

		NoteRenderer.OnNoteLoaded();
	}

	public void Move(double curTime, float chartPlaySpeed)
	{
		var transform = Transform;

		var zPos = (float)(IMovable.maximumNoteRange / IMovable.NoteFallTime(chartPlaySpeed) * (Data.time - curTime));
		transform.Origin.Z = -zPos;

		Transform = transform;
	}

	public bool ShouldMiss()
	{
		throw new System.NotImplementedException();
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
		moved = xPos - slideStartXPos;
	}

	public bool CanBeClear(float xPos)
	{
		//TODO: 这个Data.time作为判断条件好像是有问题的
		if (!IsTouch(xPos, Data.time, out _, out _))
			return true;
		var noteJudgingSize = Data.size < 1.2 ? 0.6 * Parameters.noteSizeScale : Data.size * Parameters.noteSizeScale / 2;

		return Mathf.Abs(moved) >= noteJudgingSize / 2;
	} 

	public void OnSlideEnd(NoteGrade grade)
	{
		Debug.Log("OnSlideEnd");
		IsSliding = false;
		IsClear = true;
		OnClear(grade);
	}

	public void OnSlideEnd(NoteGrade grade, double curTime)
	{
		OnSlideEnd(grade);
		Debug.Log("NoteJudgeControl Broadcast NoteClearEvent\n" +
		          $"Note: {Data.id} Time: {Data.time} CurTime: {curTime} Pos: {Data.pos} JudgeSize: {noteJudgingSize}");
	}

	public void OnClear(NoteGrade grade)
	{
		NoteRenderer.OnClear(grade);
		EventCenter.Broadcast(new NoteClearEvent<SlideNote>
		{
			Note = this,
			Grade = grade,
		});
	}

}