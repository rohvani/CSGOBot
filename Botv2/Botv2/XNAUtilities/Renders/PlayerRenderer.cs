using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Botv2.Utilities.IO;
using Botv2.Utilities.IO.FileFormats;
using Botv2.Utilities;

namespace Botv2.XNAUtilities.Renderers
{
	public class PlayerRenderer : Renderer
	{
		private BasicEffect Effect;

		public override void Load()
		{
			this.Effect = new BasicEffect(this.GraphicsDevice);
			this.Effect.World = Matrix.Identity;
			this.Effect.TextureEnabled = false;
			this.Effect.VertexColorEnabled = true;
		}

		public override void Draw()
		{
			this.Effect.View = this.Camera.ViewMatrix;
			this.Effect.Projection = this.Camera.ProjectionMatrix;

			foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
			{
				pass.Apply();

				for (int i = 0; i < 64; i++)
				{
					if (i == Botv2.Bot.localEntityIndex || GameHelper.getPlayerHealth(i) <= 0) continue;

					var playerpos = Botv2.Utilities.GameHelper.getPlayerPositon(i);
					if (playerpos.isZero()) continue;
					
					Color color = Botv2.Utilities.GameHelper.getPlayerTeam(i) ==  Botv2.Utilities.GameHelper.getPlayerTeam(Botv2.Bot.localEntityIndex) ? Color.Green : Color.Red;

					int boxWidth = 35;
					int boxHeight = 60;

					VertexPositionColor[] player = DrawHelper.createBox(boxWidth, boxHeight, color);

					float xDiff = (playerpos.x - (boxWidth / 2));
					float yDiff = (playerpos.y - (boxWidth / 2));
					float zDiff = (playerpos.z);

					for (int b = 0; b < player.Length; b++)
					{
						player[b].Position.X += xDiff;
						player[b].Position.Y += yDiff;
						player[b].Position.Z += zDiff;
					}

					this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, player, 0, player.Length / 2);
				}
			}
		}
	}
}
