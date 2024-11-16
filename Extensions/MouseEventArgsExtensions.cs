using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace Editor.Extensions;

internal static class MouseEventArgsExtensions
{
    public static Vector2 GetPosition(this MouseEventArgs mouseEvent) => new(mouseEvent.X, mouseEvent.Y);
    public static Vector2 GetMousePosition(this DragEventArgs dragEvent) => new(dragEvent.X, dragEvent.Y);
}
