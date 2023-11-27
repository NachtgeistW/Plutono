using Godot;
using System;

namespace Plutono.Core.Note
{
    public abstract partial class Note : Node3D
    {
        public bool IsHit()
        {
            return false;
        }
    }
}