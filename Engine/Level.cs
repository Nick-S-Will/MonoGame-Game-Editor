using Microsoft.Xna.Framework;
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
	private readonly List<ModelRenderer> modelRenderers = new();
	private readonly Camera camera = new(new Vector3(0f, 2f, 2f), 16 / 9);
	private readonly HashSet<ModelRenderer> selectedModelRenderers = new();

	public Camera Camera => camera;
	public ModelRenderer[] SelectedModelRenderers => selectedModelRenderers.ToArray();

	public void LoadContent(ContentManager contentManager)
	{
		ModelRenderer teapot = new(contentManager, "Teapot", "Metal", "MyShader", Vector3.Zero, 1f);
		AddModel(teapot);
		teapot = new(contentManager, "Teapot", "Metal", "MyShader", Vector3.Right, 1f);
		AddModel(teapot);
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
			translation.X = deltaMousePosition.X;
			translation.Y = -deltaMousePosition.Y;
		}

		if (InputController.MouseWheelDelta != 0) translation.Z = InputController.MouseWheelDelta;

		if (translation == Vector3.Zero) return;

		if (selectedModelRenderers.Count == 0) camera.Translate(translation);
		else
		{
			foreach(var renderer in selectedModelRenderers)
			{
				renderer.Translate(translation, camera);
			}
		}
	}

	private void HandleRotation(float delta)
	{
		if (!(InputController.IsMouseButtonDown(MouseButtons.Right) && !InputController.IsKeyDown(Keys.Menu))) return;

		Vector2 deltaMousePosition = InputController.DeltaMousePosition;
		if (deltaMousePosition == Vector2.Zero) return;

		Vector3 eulerAngles = delta * new Vector3(deltaMousePosition.Y, deltaMousePosition.X, 0);
		
		if (selectedModelRenderers.Count == 0) camera.Rotate(eulerAngles);
		else
		{
			foreach (var renderer in selectedModelRenderers)
			{
				renderer.EulerAngles += eulerAngles;
			}
		}
	}

	private void HandleScale(float delta)
	{
		if (!(InputController.IsMouseButtonDown(MouseButtons.Right) && InputController.IsKeyDown(Keys.Menu))) return;

		float scaler = InputController.DeltaMousePosition.X * delta;
		if (scaler == 0f) return;

		foreach (var renderer in selectedModelRenderers)
		{
			renderer.Scale += scaler;
		}
	}

	private void HandleSelection()
	{
		if (!InputController.IsMouseButtonDown(MouseButtons.Left)) return;

		Ray mousePositionRay = InputController.GetMousePositionRay(camera);
		Color selectionColor = Color.Red;
		selectedModelRenderers.Clear();
		foreach (ModelRenderer modelRenderer in modelRenderers)
		{
			modelRenderer.Tint = Color.White;

			foreach (ModelMesh mesh in modelRenderer.Model.Meshes)
			{
				BoundingSphere boundingSphere = mesh.BoundingSphere;
				boundingSphere = boundingSphere.Transform(modelRenderer.Transform);
				float? intersectionDistance = mousePositionRay.Intersects(boundingSphere);
				if (intersectionDistance.HasValue)
				{
					modelRenderer.Tint = selectionColor;
					selectedModelRenderers.Add(modelRenderer);
				}
			}
		}
	}

	public void Renderer()
	{
		foreach (var renderer in modelRenderers)
		{
			renderer.Render(camera.View, camera.Projection);
		}
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
		foreach (var renderer in selectedModelRenderers)
		{
			levelText.AppendLine("Model: Pos: " + renderer.Position.ToString() + " Rot: " + renderer.EulerAngles.ToString());
		}

		return levelText.ToString();
	}
}