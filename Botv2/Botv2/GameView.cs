using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XNAUtilities;
using Botv2.XNAUtilities.Renderers;
using System.Threading;

namespace Botv2
{
	public class GameView : Microsoft.Xna.Framework.Game
	{
		public static GameView instance;

		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public Camera camera;

		public Renderer BSPRenderer { get; set; }
		public Renderer PlayerRenderer { get; set; }
		public Renderer AxisRenderer { get; set; }

		public Boolean ready = false;

		public GameView()
		{
			instance = this;

			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			this.Window.Title = "Bot - Game View";

			this.camera = new Camera();
			
			this.camera.ViewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);
			this.camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.3f, 15000);
			this.camera.UpdateViewMatrix();

			this.BSPRenderer = new BSPRenderer();
			this.PlayerRenderer = new PlayerRenderer();
			this.AxisRenderer = new AxisRenderer();

			base.Initialize();
			graphics.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

			ready = true;
		}

		protected override void LoadContent()
		{
			this.BSPRenderer.Load();
			this.PlayerRenderer.Load();
			this.AxisRenderer.Load();

			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void UnloadContent() { }

		protected override void Update(GameTime gameTime)
		{
			if (((BSPRenderer)BSPRenderer).mapName != Botv2.Utilities.GameHelper.getMapName() && Botv2.Utilities.GameHelper.getMapName().Length > 0) BSPRenderer.Load();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gray);

			BSPRenderer.Draw();
			PlayerRenderer.Draw();

			base.Draw(gameTime);
		}
	}
}
