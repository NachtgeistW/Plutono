using Godot;
using Plutono.Scripts.Game;
using Plutono.Scripts.Utils;
using Plutono.Util;

public partial class InputController : Node
{
    [Export] private Game Game { get; set; }
    [Export] private TimeController TimeControl { get; set; }
    [Export] private Camera3D perspectiveCamera;
    [Export] private Camera3D orthographicCamera;


	private Vector2 lastPosition = Vector2.Inf;

    public override void _Input(InputEvent @event)
    {
        if (OsDetector.Platform == Platform.PC)
        {
            if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left } eventMouseButton)
            {
	            if (eventMouseButton.IsPressed())
	            {
		            Debug.Log("InputEventMouseButton Pressed");
		            OnFingerDown(eventMouseButton.Position, 0);

		            Vector3 worldPos = ConvertPerspectiveToOrthographic(eventMouseButton.Position);
		            Debug.Log("Converted world position: ", worldPos);
	            }
	            else
	            {
		            Debug.Log("InputEventMouseButton Released");
		            OnFingerUp(eventMouseButton.Position, 0);
	            }
	            
	            Debug.Log("---\nMouse Click/Unclick at: ", eventMouseButton.Position);
	            Debug.Log(ConvertPerspectiveToOrthographic(eventMouseButton.Position), " ", TimeControl.CurTime);
			}

			if (@event is InputEventMouseMotion { ButtonMask: MouseButtonMask.Left } inputEventMouseMotion)
            {
	            if (lastPosition == Vector2.Inf)
	            {
		            lastPosition = inputEventMouseMotion.Position;
	            }

	            if ((inputEventMouseMotion.Position - lastPosition).Length() > 10f)
	            {
				    Debug.Log($"InputEventMouseMotion Moved {(inputEventMouseMotion.Position - lastPosition).Length()}" +
						$"{ConvertPerspectiveToOrthographic(inputEventMouseMotion.Position).X}");
				    OnFingerMove(inputEventMouseMotion.Position, 0);
	            }
            }
        }
        else if (OsDetector.Platform is Platform.Android or Platform.iOS)
        {
			if (@event is InputEventScreenDrag eventScreenDrag)
			{
				if (lastPosition == Vector2.Inf)
				{
					lastPosition = eventScreenDrag.Position;
				}

				if ((eventScreenDrag.Position - lastPosition).Length() > 10f)
				{
					Debug.Log($"InputEventMouseMotion Moved {(eventScreenDrag.Position - lastPosition).Length()}");
					OnFingerMove(eventScreenDrag.Position, eventScreenDrag.Index);
				}
			}
            if (@event is InputEventScreenTouch eventScreenTouch)
            {
                if (eventScreenTouch.IsPressed())
                {
	                Debug.Log("InputEventScreenTouch Pressed");
	                OnFingerDown(eventScreenTouch.Position, eventScreenTouch.Index);
				}
				else
				{
					Debug.Log("InputEventScreenTouch Released");
					OnFingerUp(eventScreenTouch.Position, eventScreenTouch.Index);
				}
				Debug.Log($"Mouse Click/Unclick at: {eventScreenTouch.Position}, Finger index: {eventScreenTouch.Index}");
				Debug.Log(ConvertPerspectiveToOrthographic(eventScreenTouch.Position), " ", TimeControl.CurTime);

			}
		}
    }

    private void OnFingerDown(Vector2 screenPos, int fingerIndex)
    {
	    var pos = ConvertPerspectiveToOrthographic(screenPos);
	    EventCenter.Broadcast(new FingerDownEvent
	    {
            Finger = new Finger { Position = screenPos, Index = fingerIndex },
		    WorldPos = pos, 
		    Time = TimeControl.CurTime
	    });
    }

    private void OnFingerMove(Vector2 screenPos, int fingerIndex)
    {
	    var pos = ConvertPerspectiveToOrthographic(screenPos);
	    EventCenter.Broadcast(new FingerMoveEvent {
		    Finger = new Finger { Position = screenPos, Index = fingerIndex },
            WorldPos = pos, 
            Time = TimeControl.CurTime 
        });
    }

    private void OnFingerUp(Vector2 screenPos, int fingerIndex)
    {
	    var pos = ConvertPerspectiveToOrthographic(screenPos);
	    EventCenter.Broadcast(new FingerUpEvent {
			Finger = new Finger { Position = screenPos, Index = fingerIndex },
            WorldPos = pos, 
            Time = TimeControl.CurTime 
        });
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="screenPos"></param>
	/// <returns>WorldPoint, or Vector3.Inf if worldPoint is null</returns>
	private static Vector3 ScreenToWorldPoint(Camera3D camera, Vector2 screenPos)
    {
        return camera.ProjectPosition(screenPos, camera.Position.Z);
    }

	private Vector3 ConvertPerspectiveToOrthographic(Vector2 screenPosition)
	{
		Vector3 rayOrigin = perspectiveCamera.ProjectRayOrigin(screenPosition);
		Vector3 rayDirection = perspectiveCamera.ProjectRayNormal(screenPosition);

		float t = -rayOrigin.Z / rayDirection.Z;
		Vector3 intersectionPoint = rayOrigin + rayDirection * t;

		Vector2 orthoScreenPosition = orthographicCamera.UnprojectPosition(intersectionPoint);
		
		Vector3 finalWorldPosition = orthographicCamera.ProjectRayOrigin(orthoScreenPosition);

		return finalWorldPosition;
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