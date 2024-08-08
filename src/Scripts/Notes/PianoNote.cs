using Godot;
using Plutono.Core.Note.Render;
using System;

namespace Plutono.Core.Note
{
	public partial class PianoNote : Note, IMovable, ITappable, IPianoSoundPlayable
	{
		public PianoNoteData Data;
		[Export] public TapNoteRenderer NoteRenderer { get; set; }

		public void Initialize()
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

		public bool ShouldMiss()
		{
			throw new NotImplementedException();
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