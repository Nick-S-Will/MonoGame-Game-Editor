using Microsoft.Xna.Framework;
using System.IO;

namespace Editor.Engine;

internal static class Vector3Extensions
{
	public static void Serialize(this Vector3 vector, BinaryWriter binaryWriter)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static Vector3 Deserialize(BinaryReader binaryReader)
	{
		return new Vector3
		{
			X = binaryReader.ReadSingle(),
			Y = binaryReader.ReadSingle(),
			Z = binaryReader.ReadSingle()
		};
	}
}
