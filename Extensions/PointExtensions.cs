using Microsoft.Xna.Framework;

namespace Editor.Extensions;

public static class PointExtensions
{
    public static Vector2 ToVector(this System.Drawing.Point point) => new(point.X, point.Y);
}
