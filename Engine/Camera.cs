using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Editor.Engine;

internal class Camera : ISerializable
{
    public Viewport Viewport { get; set; }
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Target { get; set; } = new(250f, 0f, 250f);
    public Vector3 Right => Vector3.Cross(Vector3.Up, Forward);
    public Vector3 Up => Vector3.Cross(Forward, Right);
    public Vector3 Forward => Vector3.Normalize(Target - Position);
    public Matrix View { get; set; } = Matrix.Identity;
    public Matrix Projection { get; set; } = Matrix.Identity;
    public float NearPlaneDistance { get; set; } = .1f;
    public float FarPlaneDistance { get; set; } = 10000f;
    public float AspectRatio { get; set; } = 16f / 9f;

    public Camera() { }

    public Camera(Vector3 position, float aspectRatio)
    {
        Update(position, aspectRatio);
    }

    public void Update(Vector3 position, float aspectRatio)
    {
        Position = position;
        View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 4f, aspectRatio, NearPlaneDistance, FarPlaneDistance);
        AspectRatio = aspectRatio;
    }

    public void Translate(Vector3 translation)
    {
        if (translation == Vector3.Zero) return;

        float distance = Vector3.Distance(Position, Target);
        Vector3 forward = Vector3.Normalize(Target - Position);
        Vector3 left = Vector3.Normalize(Vector3.Cross(forward, Vector3.Up));
        Vector3 up = Vector3.Normalize(Vector3.Cross(left, forward));
        Position += left * translation.X * distance;
        Position += up * translation.Y * distance;
        Position += forward * translation.Z * 100f;
        Target += left * translation.X * distance;
        Target += up * translation.Y * distance;

        Update(Position, AspectRatio);
    }

    public void Rotate(Vector3 rotation)
    {
        Position = Vector3.Transform(Position - Target, Matrix.CreateRotationY(rotation.Y));
        Position += Target;

        Update(Position, AspectRatio);
    }

    public Ray GetRayFromScreenPosition(Vector2 mousePosition)
    {
        Vector3 nearPoint = new(mousePosition, 0f);
        Vector3 farPoint = new(mousePosition, 1f);

        nearPoint = Viewport.Unproject(nearPoint, Projection, View, Matrix.Identity);
        farPoint = Viewport.Unproject(farPoint, Projection, View, Matrix.Identity);

        Vector3 direction = Vector3.Normalize(farPoint - nearPoint);

        return new Ray(nearPoint, direction);
    }

    public void Serialize(BinaryWriter binaryWriter)
    {
        Position.Serialize(binaryWriter);
        binaryWriter.Write(NearPlaneDistance);
        binaryWriter.Write(FarPlaneDistance);
        binaryWriter.Write(AspectRatio);
    }

    public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
    {
        Position = Vector3Extensions.Deserialize(binaryReader);
        NearPlaneDistance = binaryReader.ReadSingle();
        FarPlaneDistance = binaryReader.ReadSingle();
        AspectRatio = binaryReader.ReadSingle();
        Update(Position, AspectRatio);
    }

	public override string ToString()
	{
        return "Camera Position: " + Position.ToString();
	}
}