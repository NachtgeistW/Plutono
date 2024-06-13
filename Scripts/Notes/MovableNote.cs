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


        public void Move(double elapsedTime, float chartPlaySpeed, float curTime)
        {
        }

        public virtual bool ShouldMiss()
        {
            throw new NotImplementedException();
        }
    }
}
