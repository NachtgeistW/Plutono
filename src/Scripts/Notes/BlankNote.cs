using System;
using Godot;
using Plutono.Core.Note.Render;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

namespace Plutono.Core.Note;

public partial class BlankNote : Note, IMovableNote, ITappable
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

		Initialize();
		NoteRenderer.OnNoteLoaded();
	}

	public void Initialize()
	{
		SetNoteJudgingSize();
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
		throw new NotImplementedException();
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

	public void OnMiss()
	{
		EventCenter.Broadcast(new NoteMissEvent<BlankNote>
		{
			Note = this,
		});
		QueueFree();
	}

	public bool ShouldMiss(double curTime, GameMode mode)
	{
		return mode switch
		{
			GameMode.Stelo => curTime - Data.time > SteloMode.badDeltaTime,
			GameMode.Arbo => curTime - Data.time > ArboMode.badDeltaTime,
			GameMode.Floro => curTime - Data.time > ArboMode.badDeltaTime,
			GameMode.Autoplay => false,
			_ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
		};
	}
}