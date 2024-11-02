using Editor.Engine;
using GUI.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Editor.Editor;

public class GameEditor : Game
{
	public event Action OnAssetsUpdated;

	internal Project Project
	{
		get => project;
		set
		{
			if (project == value) return;

			project = value;
			project.OnAssetsUpdated += OnAssetsUpdated;
		}
	}

    private GraphicsDeviceManager graphics;
	private FormEditor parent;
	private SpriteBatch spriteBatch;
	private FontController fontController;

	private Project project;

	public GameEditor()
	{
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	public GameEditor(FormEditor parent) : this()
	{
		this.parent = parent;
		var gameForm = Control.FromHandle(Window.Handle) as Form;
		gameForm.TopLevel = false;
		gameForm.Dock = DockStyle.Fill;
		gameForm.FormBorderStyle = FormBorderStyle.None;
		this.parent.splitContainer.Panel1.Controls.Add(gameForm);
	}

	protected override void Initialize()
	{
		base.Initialize();

		Project = new(Content, GraphicsDevice);

        RasterizerState rasterizerState = new() { CullMode = CullMode.None };
		GraphicsDevice.RasterizerState = rasterizerState;
		DepthStencilState depthStencilState = new() { DepthBufferEnable = true };
		GraphicsDevice.DepthStencilState = depthStencilState;
	}

	protected override void LoadContent()
	{
		spriteBatch = new(GraphicsDevice);
		fontController = new(Content);
	}

	protected override void Update(GameTime gameTime)
	{
        Content.RootDirectory = Project.AssetFolder;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		Project.Update(deltaTime);
		InputController.Clear();

		var selectedObjects = Project.CurrentLevel.SelectedObjects;
		if (!selectedObjects.SequenceEqual(parent.propertyGrid.SelectedObjects))
		{
			parent.propertyGrid.SelectedObjects = selectedObjects;
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		Project.Render();

		spriteBatch.Begin();

		string text = InputController.ToString();
		Vector2 position = new(20f, 20f);
		fontController.Draw(spriteBatch, FontController.Size.Large, text, position, Color.White);
		text = Project.CurrentLevel.ToString();
		position = new(20f, 80f);
		fontController.Draw(spriteBatch, FontController.Size.Small, text, position, Color.Yellow);

		spriteBatch.End();

		base.Draw(gameTime);
	}

	public void AdjustAspectRatio()
	{
		Camera camera = Project.CurrentLevel.Camera;
		camera.Viewport = graphics.GraphicsDevice.Viewport;
		camera.Update(camera.Position, graphics.GraphicsDevice.Viewport.AspectRatio);
	}
}