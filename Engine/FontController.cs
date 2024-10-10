using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Editor.Engine;

internal class FontController
{
	internal enum Size { Small = 16, Normal = 18, Large = 20 };

	private readonly SpriteFont smallFont = null, normalFont = null, largeFont = null;

	public FontController(ContentManager contentManager)
	{
		smallFont = contentManager.Load<SpriteFont>("Arial16");
		normalFont = contentManager.Load<SpriteFont>("Arial18");
		largeFont = contentManager.Load<SpriteFont>("Arial20");
	}

	public void Draw(SpriteBatch spriteBatch, Size size, string text, Vector2 position, Color color)
	{
		var font = size switch
		{
			Size.Small => smallFont,
			Size.Normal => normalFont,
			Size.Large => largeFont,
			_ => throw new System.NotImplementedException(),
		};
		spriteBatch.DrawString(font, text, position, color);
	}
}
