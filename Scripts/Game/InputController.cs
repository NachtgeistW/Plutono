using Godot;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

public partial class InputController : Node
{
    [Export] private Game Game { get; set; }
    [Export] private TimeController TimeControl { get; set; }


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex == MouseButton.Left)
            {
                if (eventMouseButton.IsPressed())
                {
                    Debug.Log("Pressed");
                }
                else
                {
                    Debug.Log("Released");
                    var pos = ScreenToWorldPoint(Game.OrthographicCamera, eventMouseButton.Position);
                    EventCenter.Broadcast(new FingerDownEvent { WorldPos = pos, Time = TimeControl.CurTime });
                }
                Debug.Log("Mouse Click/Unclick at: ", eventMouseButton.Position);
                Debug.Log(ScreenToWorldPoint(Game.OrthographicCamera, eventMouseButton.Position), " ", TimeControl.CurTime);
            }
        }

        if (@event is InputEventKey eventKey && eventKey.Keycode == Key.Space)
        {
            if (eventKey.IsPressed())
            {
                EventCenter.Broadcast(new FingerDownEvent { Finger = new Finger(), WorldPos = new Vector3(3, 0, 0), Time = TimeControl.CurTime });
            }
            else
            {
                EventCenter.Broadcast(new FingerUpEvent { Finger = new Finger(), WorldPos = new Vector3(3, 0, 0), Time = TimeControl.CurTime });
            }
        }
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