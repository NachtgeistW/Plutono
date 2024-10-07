using System;
using Godot;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

namespace Plutono.Core.Note;

public interface INote
{
	public void Initialize();

}

public interface IMovableNote : INote
{
	public void Move(double curTime, float chartPlaySpeed);

	/// <summary>
	/// note 是否被触摸（点击、按着或滑动）
	/// </summary>
	/// <returns>只要有一只手指按住就返回 true</returns>
	public bool IsTouch(float xPos, double touchTime, out float deltaXPos, out double deltaTime);

	public void OnClear(NoteGrade grade);
	public bool ShouldMiss(double curTime, GameMode mode);

	public void OnMiss(double curTime);

	protected const float maximumNoteRange = 10f;
	protected static float NoteFallTime(float chartPlaySpeed)
	{
		const float maximumNoteRange = 10f;
		var falldownSpeedRevision = 3f;
		return maximumNoteRange / (chartPlaySpeed * falldownSpeedRevision);
	}
}

public interface IPianoSoundPlayable : INote
{
	public void OnPlayPianoSounds();
}

/// <summary>
/// control the tap action
/// </summary>
// ReSharper disable once IdentifierTypo
public interface ITappable : INote
{
	/// <summary>
	/// 
	/// </summary>
	/// <returns>Is </returns>
	public bool OnTap(float xPos, double hitTime, out float deltaXPos, out double deltaTime);
}

/// <summary>
/// control the slide action
/// </summary>
// ReSharper disable once IdentifierTypo
public interface ISlidable : INote
{
	public void OnSlideStart(float xPos, double curTime);
	public void UpdateSlide(float xPos);
	public void OnSlideEnd(NoteGrade grade);
}

public interface IHoldable : INote
{
	public void OnHoldStart(Vector3 worldPos, double curTime, NoteGrade startGrade);
	public void UpdateHold(Vector3 worldPos, double curTime);
	public void OnHoldEnd(NoteGrade endGrade);
	public void OnHoldMiss();
}

// ReSharper disable once IdentifierTypo
public interface IFlickable : INote
{
	public void OnFlickStart(Vector2 worldPos, double curTime);
	public bool UpdateFlick(Vector2 worldPos);
	public void OnFlickEnd();
}