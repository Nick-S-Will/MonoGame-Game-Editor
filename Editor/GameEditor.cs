using Editor.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Editor.Editor;

public class GameEditor : Game
{
	private GraphicsDeviceManager graphics;
	private FormEditor parent;
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

		RasterizerState state = new() { CullMode = CullMode.None };
		GraphicsDevice.RasterizerState = state;
	}

	protected override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		Project?.Render();

		base.Draw(gameTime);
	}

	public void AdjustAspectRatio()
	{
		if (Project == null) return;

		Camera camera = Project.CurrentLevel.Camera;
		camera.Update(camera.Position, graphics.GraphicsDevice.Viewport.AspectRatio);
	}
}