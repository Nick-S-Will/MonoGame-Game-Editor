using Editor.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Editor.Editor;

public class GameEditor : Game
{
	private GraphicsDeviceManager graphics;
	private FormEditor parent;
	private SpriteBatch spriteBatch;
	private FontController fontController;
	internal Project Project { get; set; }

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

		Project = new(Content);

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
		float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		Project.Update(deltaTime);
		InputController.Clear();

		var selectedModels = Project.CurrentLevel.SelectedModelRenderers;
		if (selectedModels.Length == 0) parent.propertyGrid.SelectedObject = null;
		else if (selectedModels.Length == 1) parent.propertyGrid.SelectedObject = selectedModels[0];
		else parent.propertyGrid.SelectedObjects = selectedModels;

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