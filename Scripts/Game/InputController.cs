using Godot;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

public partial class InputController : Node
{
    [Export] private Game Game { get; set; }

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
                    EventCenter.Broadcast(new FingerDownEvent {WorldPos = new Vector3(-1, 0, 0), Time = 10});
                }
                Debug.Log("Mouse Click/Unclick at: ", eventMouseButton.Position);
                Debug.Log(ScreenToWorldPoint(Game.Camera, eventMouseButton.Position));
            }
        }

        if (@event is InputEventKey eventKey && eventKey.Keycode == Key.Space)
        {
            if (eventKey.IsPressed())
            {
                EventCenter.Broadcast(new FingerDownEvent { Finger = new Finger(), WorldPos = new Vector3(3, 0, 0), Time = Game.CurTime });
            }
            else
            {
                EventCenter.Broadcast(new FingerUpEvent { Finger = new Finger(), WorldPos = new Vector3(3, 0, 0), Time = Game.CurTime });
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="localPos"></param>
    /// <returns>WorldPoint, or Vector3.Inf if worldPoint is null</returns>
    private static Vector3 ScreenToWorldPoint(Camera3D camera, Vector2 localPos)
    {
        var dropPlane = new Plane(new Vector3(0, 0, 1), 0);
        return dropPlane.IntersectsRay(
            camera.ProjectRayOrigin(localPos),
            camera.ProjectRayNormal(localPos)) ?? Vector3.Inf;
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