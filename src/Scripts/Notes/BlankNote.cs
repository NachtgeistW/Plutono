using System;
using Godot;
using Plutono.Core.Note.Render;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

namespace Plutono.Core.Note;

public partial class BlankNote : Note, IMovable, ITappable
{
	public BlankNoteData Data { get; set; }
	[Export] private TapNoteRenderer NoteRenderer { get; set; }

	private double noteJudgingSize;

	public BlankNote()
	{
		Data = new BlankNoteData(1, -1, 1.2, 10);
	}

	public BlankNote(BlankNoteData data)
	{
		this.Data = data;
	}

	public BlankNote(BlankNoteData data, TapNoteRenderer noteRenderer)
	{
		this.Data = data;
		NoteRenderer = noteRenderer;
	}

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

	public void OnClear(NoteGrade grade)
	{
		NoteRenderer.OnClear(grade);
		EventCenter.Broadcast(new NoteClearEvent<BlankNote>
		{
			Note = this,
			Grade = grade,
			//DeltaXPos = deltaXPos
		});
	}

	public void OnClear(NoteGrade grade, double curTime)
	{
		OnClear(grade);
		Debug.Log("NoteJudgeControl Broadcast NoteClearEvent\n" +
		          $"Note: {Data.id} Time: {Data.time} CurTime: {curTime} Pos: {Data.pos} JudgeSize: {noteJudgingSize}");
	}

	public bool OnTap(float xPos, double hitTime, out float deltaXPos, out double deltaTime)
	{
		throw new NotImplementedException();
	}

	public bool ShouldMiss()
	{
		throw new NotImplementedException();
	}

}