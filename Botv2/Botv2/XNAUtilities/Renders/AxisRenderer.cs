using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Botv2.XNAUtilities.Renderers
{
	public class AxisRenderer : Renderer
	{
		private VertexPositionColor[] Axes;
		private BasicEffect AxisEffect;


		public override void Load()
		{
			this.AxisEffect = new BasicEffect(this.GraphicsDevice);
			this.AxisEffect.World = Matrix.Identity;
			this.AxisEffect.TextureEnabled = false;
			this.AxisEffect.VertexColorEnabled = true;

			this.Axes = new VertexPositionColor[6];
			this.Axes[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Yellow);
			this.Axes[1] = new VertexPositionColor(new Vector3(1000, 0, 0), Color.Yellow);
			this.Axes[2] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
			this.Axes[3] = new VertexPositionColor(new Vector3(0, 0, 1000), Color.Red);
			this.Axes[4] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue);
			this.Axes[5] = new VertexPositionColor(new Vector3(0, 1000, 0), Color.Blue);
		}

		public override void Draw()
		{
			this.AxisEffect.View = this.Camera.ViewMatrix;
			this.AxisEffect.Projection = this.Camera.ProjectionMatrix;

			// Draw the axis with a LineList
			foreach (EffectPass pass in this.AxisEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, this.Axes, 0, 3);
			}
		}
	}
}