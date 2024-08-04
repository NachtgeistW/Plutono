using System;
using Godot;

namespace Plutono.Core.Note.Render
{
    public partial class TapNoteRenderer : NoteRenderer, IRendererTouchable
    {
        [Export] private Sprite3D noteSprite;
        [Export] private AnimatedSprite3D explosion;

        public bool DisplayNoteId { get; set; }

        public TapNoteRenderer()
        {
        }

        public override void _EnterTree()
        {
            base._EnterTree();

            explosion.AnimationFinished += OnExplosionAnimateFinish;
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            explosion.AnimationFinished -= OnExplosionAnimateFinish;
        }

        public void OnNoteLoaded()
        {
            explosion.Visible = false;
        }

        public void Move(double elapsedTime, float chartPlaySpeed)
        {
            var transform = Transform;

            var zPos = Transform.Origin.Z + chartPlaySpeed * (float)elapsedTime;
            transform.Origin.Z = zPos;

            Transform = transform;
        }

        public void OnTouch(NoteGrade grade)
        {
            throw new NotImplementedException();
        }

        public void OnClear(NoteGrade grade)
        {
            noteSprite.Visible = false;

            explosion.Visible = true;


            switch (grade)
            {
                case NoteGrade.Perfect:
                    var transform = Transform;
                    transform.Origin.Z = 0;
                    Transform = transform;
                    explosion.Modulate = new Color("ffd000");
                    explosion.Play("perfect");
                    break;
                case NoteGrade.Good:
                    explosion.Modulate = new Color("00b300");
                    explosion.Play("good");
                    break;
                case NoteGrade.Bad:
                    explosion.Modulate = new Color("0079ff");
                    explosion.Play("good");
                    break;
                case NoteGrade.Miss:
                case NoteGrade.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(grade), grade, null);
            }
        }

        private void OnExplosionAnimateFinish() => explosion.Visible = false;

        public void OnDispose()
        {
            throw new System.NotImplementedException();
        }


        public void Render(double curTime)
        {
            throw new System.NotImplementedException();
        }
    }
}