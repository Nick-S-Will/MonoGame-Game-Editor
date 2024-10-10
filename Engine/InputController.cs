using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Editor.Engine;

internal class InputController
{
	private static readonly Lazy<InputController> instance = new(() => new());
	private static InputController Instance => instance.Value;

	private readonly Dictionary<Keys, bool> keyStates = new();
	private readonly Dictionary<MouseButtons, bool> mouseStates = new();

    public static Vector2 LastMousePosition { get; private set; }
    public static Vector2 MousePosition { get; private set; }
	public static Vector2 DeltaMousePosition => MousePosition - LastMousePosition;
    public static Vector2 MouseDragStart { get; private set; }
    public static Vector2 MouseDragEnd { get; private set; }
    public static int MouseWheelDelta { get; private set; }

    private InputController()
	{
		foreach (Keys key in Enum.GetValues(typeof(Keys)))
		{
			keyStates[key] = false;
		}

		foreach (MouseButtons button in Enum.GetValues(typeof(MouseButtons)))
		{
			mouseStates[button] = false;
		}
	}

	#region Event Handlers
	public static void HandlePressEvent(object sender, KeyEventArgs keyEvent)
	{
		Instance.keyStates[keyEvent.KeyCode] = true;
		keyEvent.Handled = true;
	}

	public static void HandleReleaseEvent(object sender, KeyEventArgs keyEvent)
	{
		Instance.keyStates[keyEvent.KeyCode] = false;
		keyEvent.Handled = true;
	}

	public static void HandlePressEvent(object sender, MouseEventArgs mouseEvent)
	{
		Instance.mouseStates[mouseEvent.Button] = true;
		MouseDragStart = mouseEvent.GetPosition();

		HandleMouseEvent(sender, mouseEvent);
	}

	public static void HandleReleaseEvent(object sender, MouseEventArgs mouseEvent)
	{
		Instance.mouseStates[mouseEvent.Button] = false;
		MouseDragEnd = mouseEvent.GetPosition();

		HandleMouseEvent(sender, mouseEvent);
	}

	public static void HandleMouseEvent(object sender, MouseEventArgs mouseEvent)
	{
		MousePosition = mouseEvent.GetPosition();
		MouseWheelDelta += mouseEvent.Delta;
	}
	#endregion

	public static Ray GetMousePositionRay(Camera camera)
	{
		Vector3 nearPoint = new(MousePosition, 0f);
		Vector3 farPoint = new(MousePosition, 1f);

		nearPoint = camera.Viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
		farPoint = camera.Viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

		Vector3 direction = Vector3.Normalize(farPoint - nearPoint);

		return new Ray(nearPoint, direction);
	}

	public static void Clear()
	{
		LastMousePosition = MousePosition;
		MouseWheelDelta = 0;
	}

	#region State Accessors
	public static bool IsKeyDown(Keys key) => Instance.keyStates[key];

	public static bool IsMouseButtonDown(MouseButtons button) => Instance.mouseStates[button];
	#endregion

	public static new string ToString()
	{
		StringBuilder inputText = new("Keys Down:");
		foreach (var key in Instance.keyStates) if (key.Value) inputText.Append(" " + key.Key);

		inputText.Append("\nButtons Down:");
		foreach (var button in Instance.mouseStates) if (button.Value) inputText.Append(" " + button.Key);

		return inputText.ToString();
	}
}
