using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XNAUtilities;

namespace Botv2.XNAUtilities.Renderers
{
	public abstract class Renderer
	{
		protected GraphicsDevice GraphicsDevice
		{
			get { return GameView.instance.GraphicsDevice; }
		}

		protected Camera Camera
		{
			get { return GameView.instance.camera; }
		}

		public abstract void Load();

		public abstract void Draw();
	}
}
