using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Editor.Engine;

internal class Terrain : ISelectable
{
    public GraphicsDevice GraphicsDevice { get; set; }
    public Texture2D Texture { get; set; }
    public Texture2D HeightMap
    {
        get => heightMap;
        set
        {
            if (heightMap == value) return;

            heightMap = value;
            UpdateVertexData();
        }
    }
    public float Height
    {
        get => height;
        set
        {
            if (height == value) return;

            height = value;
            UpdateVertexData();
        }
    }
    public int[] Indices { get; private set; }
    public IndexBuffer IndexBuffer { get; private set; }
    public VertexPositionNormalTexture[] Vertices { get; private set; }
    public VertexBuffer VertexBuffer { get; private set; }
    public int Width => HeightMap.Width;
    public int Length => HeightMap.Height;
    public Vector3 Position { get; set; }
    public Vector3 EulerAngles { get; set; }
    public float Scale { get; set; } = 1f;
    public Color Tint { get; set; } = Color.Black;

    private Texture2D heightMap;
    private float height;

    public Terrain(GraphicsDevice graphicsDevice, Texture2D texture, Texture2D heightMap, float height)
    {
        GraphicsDevice = graphicsDevice;
        Texture = texture;
        this.heightMap = heightMap;
        this.height = height;

        UpdateVertexData();
    }

    #region Vertex Data Calculation
    private void UpdateVertexData()
    {
        UpdateIndices();
        UpdateVertices();
        UpdateNormals();

        IndexBuffer.SetData(Indices);
        VertexBuffer.SetData(Vertices);
    }

    private void UpdateIndices()
    {
        int indexCount = (Width - 1) * (Length - 1) * 6;
        Indices = new int[indexCount];
        IndexBuffer = new(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indexCount, BufferUsage.WriteOnly);
        int indicesIndex = 0;
        for (int y = 0; y < Length - 1; y++)
        {
            for (int x = 0; x < Width - 1; x++)
            {
                int index1 = y * Width + x;
                int index2 = index1 + 1;
                int index3 = index1 + Width;
                int index4 = index3 + 1;

                Indices[indicesIndex++] = index1;
                Indices[indicesIndex++] = index2;
                Indices[indicesIndex++] = index3;

                Indices[indicesIndex++] = index3;
                Indices[indicesIndex++] = index2;
                Indices[indicesIndex++] = index4;
            }
        }
    }

    private void UpdateVertices()
    {
        int vertexCount = Width * Length;
        var heightMapData = new Color[vertexCount];
        HeightMap.GetData(heightMapData);
        Vertices = new VertexPositionNormalTexture[vertexCount];
        VertexBuffer = new(GraphicsDevice, Vertices.GetType().GetElementType(), vertexCount, BufferUsage.WriteOnly);
        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var height = heightMapData[y * Width + x].R / 255f * Height;
                Vertices[y * Width + x] = new()
                {
                    Position = new Vector3(x, height, y),
                    TextureCoordinate = new Vector2((float)x / Width, (float)y / Length)
                };
            }
        }
    }

    private void UpdateNormals()
    {
        for (int indicesIndex = 0; indicesIndex < Indices.Length;)
        {
            Vector3 vertex1 = Vertices[Indices[indicesIndex]].Position;
            Vector3 vertex2 = Vertices[Indices[indicesIndex + 1]].Position;
            Vector3 vertex3 = Vertices[Indices[indicesIndex + 2]].Position;
            Vector3 normal = Vector3.Normalize(Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1));

            for (int i = 0; i < 3; i++) Vertices[Indices[indicesIndex++]].Normal += normal;
        }

        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertices[i].Normal.Normalize();
        }
    } 
    #endregion

    public void Draw(Effect effect, Matrix view, Matrix projection, Vector3 lightDirection)
    {
        effect.Parameters["WorldViewProjection"].SetValue(((ISelectable)this).Transform * view * projection);
        effect.Parameters["BaseTexture"].SetValue(Texture);
        effect.Parameters["TextureTiling"].SetValue(15f);
        effect.Parameters["LightDirection"].SetValue(lightDirection);
        effect.Parameters["Tint"].SetValue(new Vector3(Tint.R / 255f, Tint.G / 255f, Tint.B / 255f));

        GraphicsDevice.SetVertexBuffer(VertexBuffer);
        GraphicsDevice.Indices = IndexBuffer;

        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.Length / 3);
        }
    }
}
