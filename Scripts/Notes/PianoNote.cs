using Godot;
using System;

namespace Plutono.Core.Note
{
    public partial class PianoNote : Note, IMovable, ITapable, IPianoSoundPlayable
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public bool ShouldMiss()
        {
            throw new NotImplementedException();
        }

        public bool IsTouch()
        {
            throw new NotImplementedException();
        }

        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos)
        {
            throw new NotImplementedException();
        }

        public void OnPlayPianoSounds()
        {
            throw new NotImplementedException();
        }
    }
}