using Microsoft.Xna.Framework;

namespace Editor.Engine;

internal interface ISelectable
{
    public Matrix Transform => Matrix.CreateTranslation(Position) * Matrix.CreateFromYawPitchRoll(EulerAngles.Y, EulerAngles.X, EulerAngles.Z) * Matrix.CreateScale(Scale);
    public Vector3 Position { get; set; }
    public Vector3 EulerAngles { get; set; }
    public float Scale { get; set; }
    public Color Tint { get; set; }

    public void Translate(Vector3 translation, Camera camera)
    {
        if (translation == Vector3.Zero) return;

        float distance = Vector3.Distance(Position, camera.Target);
        Vector3 forward = Vector3.Normalize(camera.Target - Position);
        Vector3 left = Vector3.Normalize(Vector3.Cross(forward, Vector3.Up));
        Vector3 up = Vector3.Normalize(Vector3.Cross(left, forward));
        Position += left * translation.X * distance;
        Position += up * translation.Y * distance;
        Position += forward * translation.Z * 100f;
    }
}
