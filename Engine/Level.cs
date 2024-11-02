using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor.Engine;

internal class Level : ISerializable
{
    public Camera Camera => camera;
    public ISelectable[] SelectedObjects => selectedObjects.ToArray();

    private readonly HashSet<ISelectable> selectedObjects = new();
    private readonly List<ModelRenderer> modelRenderers = new();
    private readonly Camera camera = new(new Vector3(20f, 290f, 600f), 16f / 9f);

    private Effect terrainEffect;
    private Terrain terrain;

    public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        terrainEffect = contentManager.Load<Effect>("TerrainEffect");
        terrain = new(graphicsDevice, contentManager.Load<Texture2D>("Grass"), contentManager.Load<Texture2D>("HeightMap"), 200f);
    }

    public void AddModel(ModelRenderer teapot)
    {
        modelRenderers.Add(teapot);
    }

    public void Update(float delta)
    {
        HandleTranslate(delta);
        HandleRotation(delta);
        HandleScale(delta);

        HandleSelection();
    }

    private void HandleTranslate(float delta)
    {
        Vector3 translation = Vector3.Zero;
        if (InputController.IsKeyDown(Keys.Left)) translation.X -= delta;
        if (InputController.IsKeyDown(Keys.Right)) translation.X += delta;
        if (InputController.IsKeyDown(Keys.Up))
        {
            if (InputController.IsKeyDown(Keys.Menu)) translation.Z += delta;
            else translation.Y += delta;
        }
        if (InputController.IsKeyDown(Keys.Down))
        {
            if (InputController.IsKeyDown(Keys.Menu)) translation.Z -= delta;
            else translation.Y -= delta;
        }

        if (InputController.IsMouseButtonDown(MouseButtons.Middle))
        {
            Vector2 deltaMousePosition = InputController.DeltaMousePosition;
            translation.X = delta * deltaMousePosition.X;
            translation.Y = delta * -deltaMousePosition.Y;
        }

        if (InputController.MouseWheelDelta != 0) translation.Z = delta * InputController.MouseWheelDelta;

        if (translation == Vector3.Zero) return;

        if (selectedObjects.Count > 0)
        {
            foreach (var obj in selectedObjects) obj.Translate(translation, camera);
        }
        else camera.Translate(translation);
    }

    private void HandleRotation(float delta)
    {
        if (!(InputController.IsMouseButtonDown(MouseButtons.Right) && !InputController.IsKeyDown(Keys.Menu))) return;

        Vector2 deltaMousePosition = InputController.DeltaMousePosition;
        if (deltaMousePosition == Vector2.Zero) return;

        Vector3 eulerAngles = delta * new Vector3(deltaMousePosition.Y, deltaMousePosition.X, 0);

        if (selectedObjects.Count == 0) camera.Rotate(eulerAngles);
        else
        {
            foreach (var obj in selectedObjects)
            {
                obj.EulerAngles += eulerAngles;
            }
        }
    }

    private void HandleScale(float delta)
    {
        if (!(InputController.IsMouseButtonDown(MouseButtons.Right) && InputController.IsKeyDown(Keys.Menu))) return;

        float scaler = InputController.DeltaMousePosition.X * delta;
        if (scaler == 0f) return;

        foreach (var obj in selectedObjects)
        {
            obj.Scale += scaler;
        }
    }

    private void HandleSelection()
    {
        if (!InputController.IsMouseButtonDown(MouseButtons.Left)) return;

        selectedObjects.Clear();

        Ray mousePositionRay = camera.GetRayFromScreenPosition(InputController.MousePosition);
        Color selectionColor = Color.Red;
        foreach (var modelRenderer in modelRenderers)
        {
            bool intersectingModel = mousePositionRay.IntersectsModelRenderer(modelRenderer).HasValue;
            modelRenderer.Tint = intersectingModel ? selectionColor : Color.Black;
            if (intersectingModel) selectedObjects.Add(modelRenderer);
        }

        bool intersectingTerrain = mousePositionRay.IntersectsTerrain(terrain).HasValue;
        terrain.Tint = intersectingTerrain ? selectionColor : Color.Black;
        if (intersectingTerrain) selectedObjects.Add(terrain);
    }

    public void Render()
    {
        foreach (var renderer in modelRenderers)
        {
            renderer.Render(camera.View, camera.Projection);
        }

        terrain.Draw(terrainEffect, camera.View, camera.Projection, new Vector3(0f, -1f, 0f));
    }

    public void Serialize(BinaryWriter binaryWriter)
    {
        binaryWriter.Write(modelRenderers.Count);
        foreach (var renderer in modelRenderers) renderer.Serialize(binaryWriter);
        camera.Serialize(binaryWriter);
    }

    public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
    {
        int modelCount = binaryReader.ReadInt32();
        for (int i = 0; i < modelCount; i++)
        {
            ModelRenderer renderer = new();
            renderer.Deserialize(binaryReader, contentManager);
            modelRenderers.Add(renderer);
        }
        camera.Deserialize(binaryReader, contentManager);
    }

    public override string ToString()
    {
        StringBuilder levelText = new(camera.ToString() + "\n");
        foreach (var renderer in selectedObjects)
        {
            levelText.AppendLine("Model: Pos: " + renderer.Position.ToString() + " Rot: " + renderer.EulerAngles.ToString());
        }

        return levelText.ToString();
    }
}