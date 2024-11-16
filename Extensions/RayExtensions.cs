using Editor.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Editor.Extensions;

internal static class RayExtensions
{
    public static float? IntersectsModelRenderer(this Ray ray, in ModelRenderer modelRenderer)
    {
        Matrix transform = ((ISelectable)modelRenderer).Transform;

        if (!modelRenderer.Model.Meshes.Any(modelMesh => ray.IntersectsBoundingSphere(modelMesh, transform))) return null;

        foreach (var modelMesh in modelRenderer.Model.Meshes)
        {
            foreach (var meshPart in modelMesh.MeshParts)
            {
                int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride / 4;
                var vertices = new float[meshPart.VertexBuffer.VertexCount * stride];
                meshPart.VertexBuffer.GetData(vertices);
                var indices = new short[meshPart.IndexBuffer.IndexCount];
                meshPart.IndexBuffer.GetData(indices);

                Vector3 GetVertex(int i)
                {
                    int index = (meshPart.VertexOffset + indices[i]) * stride;
                    return Vector3.Transform(new(vertices[index], vertices[index + 1], vertices[index + 2]), transform);
                }

                for (int i = meshPart.StartIndex; i < meshPart.StartIndex + meshPart.PrimitiveCount * 3;)
                {
                    float? intersectionDistance = ray.IntersectsTriangle(GetVertex(i++), GetVertex(i++), GetVertex(i++));
                    if (intersectionDistance.HasValue) return intersectionDistance;
                }
            }
        }

        return null;
    }

    public static bool IntersectsBoundingSphere(this Ray ray, in ModelMesh modelMesh, in Matrix transform)
    {
        BoundingSphere boundingSphere = modelMesh.BoundingSphere;
        boundingSphere = boundingSphere.Transform(transform);
        float? intersectionDistance = ray.Intersects(boundingSphere);

        return intersectionDistance.HasValue;
    }

    public static float? IntersectsTerrain(this Ray ray, in Terrain terrain)
    {
        Matrix transform = ((ISelectable)terrain).Transform;

        Vector3 GetVertex(in Terrain t, int i)
        {
            int index = t.Indices[i];
            return Vector3.Transform(t.Vertices[index].Position, transform);
        }

        for (int i = 0; i < terrain.Indices.Length;)
        {
            float? intersectionDistance = ray.IntersectsTriangle(GetVertex(terrain, i++), GetVertex(terrain, i++), GetVertex(terrain, i++));
            if (intersectionDistance.HasValue) return intersectionDistance;
        }

        return null;
    }

    /// <summary>
    /// Checks if this <see cref="Ray"/> intersects with triangle formed by <paramref name="vertex1"/>, <paramref name="vertex2"/>, <paramref name="vertex3"/>
    /// </summary>
    /// <returns>The distance the <see cref="Ray"/> travels to the triangle if it intersects</returns>
    public static float? IntersectsTriangle(this Ray ray, in Vector3 vertex1, in Vector3 vertex2, in Vector3 vertex3)
    {
        Vector3 edge1 = vertex2 - vertex1;
        Vector3 edge2 = vertex3 - vertex1;
        Vector3 directionCrossEdge2 = Vector3.Cross(ray.Direction, edge2);
        float determinant = Vector3.Dot(edge1, directionCrossEdge2);
        if (determinant > -float.Epsilon && determinant < float.Epsilon) return null;

        Vector3 distanceVector = ray.Position - vertex1;
        float inverseDeterminant = 1f / determinant;
        float triangleU = Vector3.Dot(directionCrossEdge2, distanceVector) * inverseDeterminant;
        if (triangleU < 0f || triangleU > 1f) return null;

        Vector3 distanceCrossEdge1 = Vector3.Cross(distanceVector, edge1);
        float triangleV = Vector3.Dot(ray.Direction, distanceCrossEdge1) * inverseDeterminant;
        if (triangleV < 0f || triangleU + triangleV > 1f) return null;

        float rayDistance = Vector3.Dot(edge2, distanceCrossEdge1) * inverseDeterminant;
        if (rayDistance < 0f) return null;

        return rayDistance;
    }
}
