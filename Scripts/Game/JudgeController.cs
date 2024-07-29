using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Plutono.Core.Note;
using Plutono.Scripts.Utils;
using Plutono.Util;
using Debug = Plutono.Scripts.Utils.Debug;

namespace Plutono.Scripts.Game;

public partial class JudgeController : Node3D
{
	[Export] private Game Game { get; set; }

	[Export] private NoteController NoteControl { get; set; }
	[Export] private TimeController TimeControl { get; set; }

	private readonly Dictionary<int, SlideNote> notesOnSliding = new(); //Finger index and sliding note on it
	private readonly Dictionary<int, HoldNote> notesOnHolding = new(); //Finger index and holding note on it


	#region Event

	public override void _EnterTree()
	{
		base._EnterTree();

		EventCenter.AddListener<FingerDownEvent>(OnFingerDown);
		EventCenter.AddListener<FingerMoveEvent>(OnFingerMove);
		EventCenter.AddListener<FingerUpEvent>(OnFingerUp);
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		EventCenter.RemoveListener<FingerDownEvent>(OnFingerDown);
		EventCenter.RemoveListener<FingerMoveEvent>(OnFingerMove);
		EventCenter.RemoveListener<FingerUpEvent>(OnFingerUp);
	}

	#endregion

	public override void _Process(double delta)
	{
		base._Process(delta);

		foreach (var note in notesOnHolding)
		{
			if (note.Value.IsHolding)
				note.Value.UpdateHold(Vector3.Zero, TimeControl.CurTime);
		}
	}

	private void OnFingerDown(FingerDownEvent evt)
	{
		var worldPos = evt.WorldPos;
		var curTime = evt.Time;

		{
			//Query slide note
			foreach (var curDetectingNote in NoteControl.SlideNotes
						 .Where(note => note.OnTap(worldPos.X, curTime, out _, out _)))
			{
				if (notesOnSliding.ContainsKey(evt.Finger.Index) || notesOnSliding.ContainsValue(curDetectingNote))
				{
					// Player is sliding on another note, pass
					continue;
				}
				notesOnSliding.Add(evt.Finger.Index, curDetectingNote);
				curDetectingNote.OnSlideStart(worldPos.X, curTime);
				return;
			}
		}
		{
			// Query hold note
			foreach (var curDetectingNote in NoteControl.HoldNotes
						 .Where(note => note.IsTouch(worldPos.X, curTime, out _, out _)))
			{
				if (notesOnHolding.ContainsKey(evt.Finger.Index) || notesOnHolding.ContainsValue(curDetectingNote))
				{
					// Player is holding on another note, pass
					continue;
				}
				notesOnHolding.Add(evt.Finger.Index, curDetectingNote);
				var startGrade = GetNoteGrade(Math.Abs(TimeControl.CurTime - curDetectingNote.BeginTime), Game.Mode);
				curDetectingNote.OnHoldStart(worldPos, curTime, startGrade);
				return;
			}
		}
		{
			// Blank note
			var note = TryGetClosestHitNote(NoteControl.BlankNotes, worldPos, curTime,
				out var deltaXPos, out var grade);
			if (note is not null)
			{
				note.OnClear(grade, curTime);
				return;
			}
		}
	}

	private void OnFingerMove(FingerMoveEvent evt)
	{
		var worldPos = evt.WorldPos;
		var curTime = evt.Time;

		QuerySlideNote();

		void QuerySlideNote()
		{
			if (notesOnSliding.TryGetValue(evt.Finger.Index, out var note))
			{
				if (note.IsClear) return;

				note.UpdateSlide(worldPos.X);
				if (note.CanBeClear(worldPos.X))
				{
					var grade = GetNoteGrade(Math.Abs(TimeControl.CurTime - note.SlideStartTime), Game.Mode);
					note.OnSlideEnd(grade, curTime);
					notesOnSliding.Remove(evt.Finger.Index);
				}
			}
			else
			{
				foreach (var curDetectingNote in NoteControl.SlideNotes
					         .Where(newNote => newNote.OnTap(worldPos.X, curTime, out _, out _)))
				{
					notesOnSliding.Add(evt.Finger.Index, curDetectingNote);
					curDetectingNote.OnSlideStart(worldPos.X, curTime);
					return;
				}
			}
		}
	}

	private void OnFingerUp(FingerUpEvent evt)
	{
		var curTime = evt.Time;

		// Force clear this note
		{
			if (notesOnHolding.TryGetValue(evt.Finger.Index, out var note))
			{
				if (note.IsClear) return;

				var endGrade = GetNoteGrade(Math.Abs(TimeControl.CurTime - note.EndTime), Game.Mode);
				note.OnHoldEnd(endGrade, curTime);
				notesOnHolding.Remove(evt.Finger.Index);
			}
		}
		{
			if (notesOnSliding.TryGetValue(evt.Finger.Index, out var note))
			{
				if (note.IsClear) return;

				var endGrade = GetNoteGrade(Math.Abs(TimeControl.CurTime - note.SlideStartTime), Game.Mode);
				note.OnSlideEnd(endGrade, curTime);
				notesOnSliding.Remove(evt.Finger.Index);
			}
		}
	}

	/// <summary>
	/// Check if player hit a note
	/// </summary>
	/// <param name="notes"></param>
	/// <param name="pos"></param>
	/// <param name="touchTime">the time when player touches the screen</param>
	/// <param name="deltaXPos">MaxValue if none</param>
	/// <param name="grade">grade the note get</param>
	/// <returns>The hit note. null if none</returns>
	private TNote TryGetClosestHitNote<TNote>(List<TNote> notes, Vector3 pos, double touchTime,
		out float deltaXPos, out NoteGrade grade)
		where TNote : Note, IMovable
	{
		TNote note = null;
		deltaXPos = float.MaxValue;
		grade = NoteGrade.None;

		//if (finger.OnGui)
		//    return false;

		//if (pos.y < 0.6) return false;

		var lastDeltaXPos = float.MaxValue;
		var lastDeltaTime = double.MaxValue;
		foreach (var curDetectingNote in notes)
		{
			if (!curDetectingNote.IsTouch(pos.X, touchTime, out deltaXPos, out var deltaTime))
				continue;
			var curNoteGrade = GetNoteGrade(deltaTime, Game.Mode);
			if (curNoteGrade > grade)
			{
				note = curDetectingNote;
				lastDeltaXPos = deltaXPos;
				grade = curNoteGrade;
			}
			else if (curNoteGrade == grade && deltaXPos < lastDeltaXPos)
			{
				note = curDetectingNote;
				lastDeltaXPos = deltaXPos;
				grade = curNoteGrade;
			}
			//if (deltaXPos < lastDeltaXPos)
			//{
			//    note = curDetectingNote;
			//    lastDeltaXPos = deltaXPos;
			//    lastDeltaTime = deltaTime;
			//}
			//// Touch point is closer to the centre of this note, despite having a larger interval.
			//else if (deltaXPos >= lastDeltaXPos && deltaTime < lastDeltaTime)
			//{
			//    note = curDetectingNote;
			//    lastDeltaXPos = deltaXPos;
			//    lastDeltaTime = deltaTime;
			//}

			//if (deltaTime < lastDeltaTime)
			//{
			//    note = curDetectingNote;
			//    lastDeltaXPos = deltaXPos;
			//    lastDeltaTime = deltaTime;
			//}
			//// Touch point is closer to the centre of this note, despite having a larger interval.
			//else if (deltaTime >= lastDeltaTime && deltaXPos < lastDeltaXPos)
			//{
			//    note = curDetectingNote;
			//    lastDeltaXPos = deltaXPos;
			//    lastDeltaTime = deltaTime;
			//}
		}

		return note;
	}

	/// <summary>
	/// 找到离手指触碰点最近的note
	/// </summary>
	/// <typeparam name="TNote"></typeparam>
	/// <param name="notes"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	private TNote FindClosestNoteToFingerPos<TNote>(TNote notes)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// 确定某个note在当前游戏设定下的Grade
	/// </summary>
	private static NoteGrade GetNoteGrade(double interval, GameMode mode)
	{
		return mode switch
		{
			GameMode.Stelo => interval switch
			{
				<= SteloMode.perfectDeltaTime => NoteGrade.Perfect,
				<= SteloMode.goodDeltaTime => NoteGrade.Good,
				<= SteloMode.badDeltaTime => NoteGrade.Bad,
				_ => NoteGrade.Miss
			},
			GameMode.Arbo or GameMode.Floro => interval switch
			{
				<= ArboMode.perfectDeltaTime => NoteGrade.Perfect,
				<= ArboMode.goodDeltaTime => NoteGrade.Good,
				<= ArboMode.badDeltaTime => NoteGrade.Bad,
				_ => NoteGrade.Miss
			},
			_ => throw new NotImplementedException(),
		};
	}
}

/// <summary>
/// Settings related to Arbo(Deemo1) Mode
/// </summary>
public static class ArboMode
{
	public const float perfectDeltaTime = 0.05f;
	public const float goodDeltaTime = 0.12f;
	public const float badDeltaTime = 0.3f;
}

/// <summary>
/// Settings related to Stelo(Plutono) Mode
/// </summary>
public static class SteloMode
{
	public const float perfectDeltaTime = 0.035f;
	public const float goodDeltaTime = 0.07f;
	public const float badDeltaTime = 0.1f;
}
