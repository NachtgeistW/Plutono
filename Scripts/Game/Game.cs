using Godot;
using Plutono.Scripts.Utils;

namespace Plutono.Scripts.Game;

public partial class Game : Node3D
{
	[Export] public Camera3D OrthographicCamera { get; set; }
	[Export] public RichTextLabel curTimeText { get; set; }

	protected float chartPlaySpeed = 10f;
	public GameMode Mode { get; private set; } = GameMode.Floro;
	public double CurTime { get; private set; }

	public override void _Process(double delta)
	{
		CurTime += delta;

		curTimeText.Text = CurTime.ToString();
	}
}
