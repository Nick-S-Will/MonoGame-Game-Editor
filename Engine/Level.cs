using Editor.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor.Engine;

internal class Level : ISerializable
{
    public event Action<ISelectable> OnSelect;

    public Camera Camera => camera;
    public virtual IEnumerable<ModelRenderer> ModelRenderers => modelRenderers;
    public IEnumerable<ISelectable> SelectedObjects => selectedObjects;
    public Color SelectionColor { get; set; } = Color.Red;

    private readonly Light light = new() { Position = new(0, 400, -500), Color = new(.9f, .9f, .9f) };
    private readonly Camera camera = new(new Vector3(20f, 290f, 600f), 16f / 9f);
    private readonly HashSet<ISelectable> selectedObjects = new();
    private readonly List<ModelRenderer> modelRenderers = new();
    private Terrain terrain;

    public Level()
    {
        OnSelect += PlaySelectSound;
    }

    public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        Renderer.Instance.Camera = camera;
        Renderer.Instance.Light = light;

        //terrain = new(graphicsDevice, contentManager.Load<Texture2D>("Grass"), contentManager.Load<Effect>("TerrainEffect"), contentManager.Load<Texture2D>("HeightMap"), 200f);
    }

    public void AddModel(ModelRenderer modelRenderer)
    {
        modelRenderers.Add(modelRenderer);
    }

    public void Update(float delta)
    {
        HandleTranslate(delta);
        HandleRotation(delta);
        HandleScale(delta);

        HandleSelection();
    }

    #region Object Translation
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

        if (selectedObjects.Count == 0) camera.Rotate(-eulerAngles);
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
    #endregion

    #region Selection
    private void HandleSelection()
    {
        if (!InputController.IsMouseButtonDown(MouseButtons.Left)) return;

        ClearSelectedObjects();

        foreach (var selectable in GetObjects(InputController.MousePosition)) SelectObject(selectable);
    }

    public void ClearSelectedObjects()
    {
        selectedObjects.Clear();
        foreach (var renderer in modelRenderers) renderer.Tint = Color.Black;
        if (terrain != null) terrain.Tint = Color.Black;
    }

    public void SelectObject(ISelectable selectable)
    {
        selectable.Tint = SelectionColor;
        selectedObjects.Add(selectable);

        OnSelect.Invoke(selectable);
    }

    public ISelectable[] GetObjects(Vector2 screenPosition)
    {
        List<ISelectable> selectables = new();

        Ray mousePositionRay = camera.GetRayFromScreenPosition(screenPosition);
        foreach (var modelRenderer in modelRenderers)
        {
            bool intersectingModel = mousePositionRay.IntersectsModelRenderer(modelRenderer).HasValue;
            if (intersectingModel) selectables.Add(modelRenderer);
        }

        if (terrain != null)
        {
            bool intersectingTerrain = mousePositionRay.IntersectsTerrain(terrain).HasValue;
            if (intersectingTerrain) selectables.Add(terrain);
        }

        return selectables.ToArray();
    }
    #endregion

    #region Audio
    private void PlaySelectSound(ISelectable selectable)
    {
        if (selectable is not ISoundEmitter soundEmitter) return;
        if (!soundEmitter.SoundEffects.TryGetValue(ISoundEmitter.Sound.Select, out var sound)) return;

        if (sound.Value.State == SoundState.Stopped) sound.Value.Play();
    }
    #endregion

    public void Render()
    {
        foreach (var renderer in modelRenderers)
        {
            Renderer.Instance.Render(renderer);
        }

        if (terrain != null) Renderer.Instance.Render(terrain);
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