using Godot;
using System;

namespace Plutono.Core.Note
{
    public abstract partial class MovableNote : Note, IMovable
    {
        protected float chartPlaySpeed = 5f;

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual bool ShouldMiss()
        {
            throw new NotImplementedException();
        }

        public bool IsTouch()
        {
            throw new NotImplementedException();
        }
    }
}
