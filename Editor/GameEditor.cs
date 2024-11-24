using Editor.Engine;
using Editor.Engine.Scripting;
using Editor.GUI;
using GUI.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
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
		this.parent.GamePanel.Controls.Add(gameForm);
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

		ScriptManager.Instance.Execute(Path.GetFileNameWithoutExtension(Project.BeforeUpdateScriptFileName) + "Main");

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		Project.Update(deltaTime);
		UpdateSelected();
		InputController.Clear();
		
        ScriptManager.Instance.Execute(Path.GetFileNameWithoutExtension(Project.AfterUpdateScriptFileName) + "Main");

        base.Update(gameTime);
	}

	private void UpdateSelected()
	{
        var selectedObjects = Project.CurrentLevel.SelectedObjects.OfType<ModelRenderer>().ToArray();
        if (!selectedObjects.SequenceEqual(parent.Inspector.SelectedObjects))
        {
            parent.Inspector.SelectedObjects = selectedObjects;

            if (selectedObjects.Length == 1)
            {
                var selectedListItem = parent.Hierarchy.Items.Cast<LevelListItem>().First(listItem => listItem.Model == selectedObjects[0]);
                parent.Hierarchy.SetSelected(parent.Hierarchy.Items.IndexOf(selectedListItem), true);
            }
            else parent.Hierarchy.SelectedIndex = -1;
        }
    }

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		ScriptManager.Instance.Execute(Path.GetFileNameWithoutExtension(Project.BeforeRenderScriptFileName) + "Main");

		Project.Render();

		spriteBatch.Begin();
		string text = InputController.ToString();
		fontController.Draw(spriteBatch, FontController.Size.Large, text, new(20f, 20f), Color.White);
		text = Project.CurrentLevel.ToString();
		fontController.Draw(spriteBatch, FontController.Size.Small, text, new(20f, 80f), Color.Yellow);
		spriteBatch.End();

		ScriptManager.Instance.Execute(Path.GetFileNameWithoutExtension(Project.AfterRenderScriptFileName) + "Main");

		base.Draw(gameTime);
	}

	public void AdjustAspectRatio()
	{
		Camera camera = Project.CurrentLevel.Camera;
		camera.Viewport = graphics.GraphicsDevice.Viewport;
		camera.Update(camera.Position, graphics.GraphicsDevice.Viewport.AspectRatio);
	}
}