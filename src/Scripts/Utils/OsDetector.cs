using Godot;

namespace Plutono.Scripts.Utils;

public partial class OsDetector : Node3D
{
	public static Platform Platform { get; private set; }

	public override void _Ready()
	{
		base._Ready();

		Platform = OS.GetName() switch
		{
			"Windows" or "macOS" or "Linux" or "FreeBSD" or "NetBSD" or "OpenBSD" or "BSD" => Platform.PC,
			"Android" => Platform.Android,
			"iOS" => Platform.iOS,
			"Web" => Platform.Web,
			_ => Platform.GenericDevice
		};
	}
}

public enum Platform
{
	// ReSharper disable once InconsistentNaming
	PC,
	Android,
	// ReSharper disable once InconsistentNaming
	iOS,
	Web,
	GenericDevice
}