using Godot;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

public partial class InputController : Node
{
    [Export] private Game Game { get; set; }
    [Export] private TimeController TimeControl { get; set; }

    private Vector2 lastPosition = Vector2.Inf;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex == MouseButton.Left)
            {
                if (eventMouseButton.IsPressed())
                {
	                Debug.Log("InputEventMouseButton Pressed");
	                OnFingerDown(eventMouseButton.Position);
                }
                else
                {
                    Debug.Log("InputEventMouseButton Released");
					OnFingerUp(eventMouseButton.Position);
                }
				Debug.Log("Mouse Click/Unclick at: ", eventMouseButton.Position);
                Debug.Log(ScreenToWorldPoint(Game.OrthographicCamera, eventMouseButton.Position), " ", TimeControl.CurTime);
            }
        }

        if (@event is InputEventMouseMotion inputEventMouseMotion && inputEventMouseMotion.ButtonMask == MouseButtonMask.Left)
        {
	        if (lastPosition == Vector2.Inf)
	        {
		        lastPosition = inputEventMouseMotion.Position;
	        }

	        if ((inputEventMouseMotion.Position - lastPosition).Length() > 10f)
	        {
				Debug.Log($"InputEventMouseMotion Moved {(inputEventMouseMotion.Position - lastPosition).Length()}");
				OnFingerMove(inputEventMouseMotion.Position);
	        }
        }
    }

    private void OnFingerDown(Vector2 screenPos)
    {
	    var pos = ScreenToWorldPoint(Game.OrthographicCamera, screenPos);
	    EventCenter.Broadcast(new FingerDownEvent { WorldPos = pos, Time = TimeControl.CurTime });
    }

    private void OnFingerMove(Vector2 screenPos)
    {
	    var pos = ScreenToWorldPoint(Game.OrthographicCamera, screenPos);
	    EventCenter.Broadcast(new FingerMoveEvent { Finger = new Finger(), WorldPos = pos, Time = TimeControl.CurTime });
    }

    private void OnFingerUp(Vector2 screenPos)
    {
	    var pos = ScreenToWorldPoint(Game.OrthographicCamera, screenPos);
	    EventCenter.Broadcast(new FingerUpEvent { WorldPos = pos, Time = TimeControl.CurTime });
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="screenPos"></param>
	/// <returns>WorldPoint, or Vector3.Inf if worldPoint is null</returns>
	private static Vector3 ScreenToWorldPoint(Camera3D camera, Vector2 screenPos)
    {
        return camera.ProjectPosition(screenPos, camera.GlobalTransform.Origin.Z);
    }
}

public struct FingerDownEvent : IEvent
{
    public Finger Finger;
    public Vector3 WorldPos;
    public double Time;
}

public struct FingerMoveEvent : IEvent
{
    public Finger Finger;
    public Vector3 WorldPos;
    public double Time;
}

public struct FingerUpEvent : IEvent
{
    public Finger Finger;
    public Vector3 WorldPos;
    public double Time;
}

public struct Finger
{
    public Vector2 Position;
    public int Index;
}