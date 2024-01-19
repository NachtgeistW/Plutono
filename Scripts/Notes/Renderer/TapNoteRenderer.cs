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

        public void Move(double elapsedTime, float chartPlaySpeed)
        {
            var transform = Transform;

            var zPos = Transform.Origin.Z + chartPlaySpeed * (float)elapsedTime;
            transform.Origin.Z = zPos;

            Transform = transform;
        }

        public void OnClear(NoteGrade grade)
        {
            noteSprite.Visible = false;

            explosion.Visible = true;
            explosion.Play();
        }

        private void OnExplosionAnimateFinish() => explosion.Visible = false;

        public void OnDispose()
        {
            throw new System.NotImplementedException();
        }


        public void Render(Note note)
        {
            throw new System.NotImplementedException();
        }
    }
}