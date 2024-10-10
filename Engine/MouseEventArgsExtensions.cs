using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace Editor.Engine;

internal static class MouseEventArgsExtensions
{
	public static Vector2 GetPosition(this MouseEventArgs mouseEvent) => new(mouseEvent.X, mouseEvent.Y);

}
