using Godot;
using Plutono.Core.Note.Render;
using System;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;

namespace Plutono.Core.Note
{
	public partial class PianoNote : Note, IMovable, ITappable, IPianoSoundPlayable
	{
		public PianoNoteData Data;
		[Export] public TapNoteRenderer NoteRenderer { get; set; }

		public override void Initialize()
		{
			throw new NotImplementedException();
		}

		public void Initialize(float chartPlaySpeed)
		{
			throw new NotImplementedException();
		}

		public void Move(double delta, float chartPlaySpeed)
		{
			throw new NotImplementedException();
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

		public bool IsTouch(float xPos, double touchTime, out float deltaXPos, out double deltaTime)
		{
			var noteJudgingSize = Data.size < 1.2 ? 0.6 * Parameters.noteSizeScale : Data.size * Parameters.noteSizeScale / 2;
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
			throw new NotImplementedException();
		}

		public bool OnTap(float xPos, double hitTime, out float deltaXPos, out double deltaTime)
		{
			throw new NotImplementedException();
		}

		public void OnPlayPianoSounds()
		{
			throw new NotImplementedException();
		}
	}
}