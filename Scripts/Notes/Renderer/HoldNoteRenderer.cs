using System;
using Godot;
using Plutono.Scripts.Utils;
using static Godot.CameraFeed;

namespace Plutono.Core.Note.Render
{
	public partial class HoldNoteRenderer : NoteRenderer, IRendererHoldable
	{
		[Export] public Sprite3D head;
		[Export] public Sprite3D body;
		[Export] public Sprite3D end;
		[Export] public Node3D node;
		[Export] private AnimatedSprite3D explosion;

		[Export] private HoldNote note;

		private float width = 128;
		private float height = 128;

		bool INoteRenderer.DisplayNoteId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public HoldNoteRenderer()
		{
		}

		#region GodotEvent

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

		public override void _Process(double delta)
		{
			base._Process(delta);

            Render(delta);
            //Debug.Log($"head pos:{head.Position.Z} body pos: {body.Position.Z} end pos:{end.Position.Z} length:{head.Position.Z - end.Position.Z}");
        }

        #endregion


        public void OnNoteLoaded(float chartPlaySpeed)
		{
			var beginTime = -(float)(IRendererHoldable.maximumNoteRange / IRendererHoldable.NoteFallTime(chartPlaySpeed) * note.BeginTime);

            var endTime = -(float)(IRendererHoldable.maximumNoteRange / IRendererHoldable.NoteFallTime(chartPlaySpeed) * note.EndTime);
			var length = beginTime - endTime;
			Debug.Log(length);

			var headTransform = head.Transform;
			var endTransform = end.Transform;
			headTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime);
			endTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, endTime);

			var bodyTransform = body.Transform;
			bodyTransform.Origin = new Vector3(headTransform.Origin.X, headTransform.Origin.Y, beginTime - length / 2);

			head.Transform = headTransform;
			body.Transform = bodyTransform;
			end.Transform = endTransform;

			body.Scale = new Vector3(2, 1, length * Parameters.pixel_per_unit / height / 2);

			explosion.Visible = false;
			explosion.Position = head.Position;
		}

		public void Render(double delta)
		{
			UpdateComponentStates();
			UpdateComponentOpacity();
			UpdateTransformScale(delta);
        }

        public void UpdateComponentStates()
		{
			if (!note.IsClear && note.IsHolding && !explosion.IsPlaying())
			{
				explosion.Visible = true;
                explosion.Modulate = new Color("ffd000");
				explosion.Play("good_start");
			}
		}

		public void UpdateComponentOpacity()
		{
            //TODO: UpdateComponentOpacity;
        }

		public void UpdateTransformScale(double delta)
		{
			if (!note.IsClear && note.IsHolding)
			{
				// Calculate the current end position
				var moveDelta = -(float)(IRendererHoldable.maximumNoteRange / IRendererHoldable.NoteFallTime(note.chartPlaySpeed) * delta);

				// Update end position
				var endTransform = end.Transform;
				endTransform.Origin.Z -= moveDelta;
                endTransform.Origin = new Vector3(endTransform.Origin.X, endTransform.Origin.Y, endTransform.Origin.Z);
				end.Transform = endTransform;

				// Calculate the new length
				var headPosition = head.Position.Z;
				var length = Math.Abs(endTransform.Origin.Z - headPosition);

				// Update body position and scale
				var bodyTransform = body.Transform;
				bodyTransform.Origin = new Vector3(bodyTransform.Origin.X, bodyTransform.Origin.Y, headPosition - length / 2);
				body.Transform = bodyTransform;
				body.Scale = new Vector3(2, 1, length * Parameters.pixel_per_unit / height / 2);
			}
		}

		private void OnExplosionAnimateFinish()
		{
			explosion.Play("good_onhold");
		}

        public void OnClear(NoteGrade grade)
		{
            switch (grade)
            {
				case NoteGrade.Perfect:
                    explosion.Modulate = new Color("ffd000");
                    explosion.Play("perfect_end");
                    break;
                case NoteGrade.Good:
                    explosion.Modulate = new Color("00b300");
                    explosion.Play("good_end");
                    break;
                case NoteGrade.Bad:
                    explosion.Modulate = new Color("0079ff");
                    explosion.Play("good_end");
                    break;
                case NoteGrade.Miss:
                    explosion.Modulate = new Color("494949");
                    explosion.Play("good_end");
					break;
                case NoteGrade.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(grade), grade, null);
            }
		}

		public void OnDispose()
		{
			throw new NotImplementedException();
		}
	}
}
