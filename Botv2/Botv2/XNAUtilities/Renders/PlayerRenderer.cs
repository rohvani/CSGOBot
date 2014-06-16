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
				
					VertexPositionColor[] player = new VertexPositionColor[18 + 6];

					int boxWidth = 35;
					int boxHeight = 60;

					//  + (playerpos.x - (boxWidth / 2))

					float xDiff = (playerpos.x - (boxWidth / 2));
					float yDiff = (playerpos.y - (boxWidth / 2));
					float zDiff = (playerpos.z);

					player[0] = new VertexPositionColor(new Vector3(0, 0, 0), color);
					player[1] = new VertexPositionColor(new Vector3(0, 0, boxHeight), color);

					player[2] = new VertexPositionColor(new Vector3(0, 0, 0), color);
					player[3] = new VertexPositionColor(new Vector3(0, boxWidth, 0), color);

					player[4] = new VertexPositionColor(new Vector3(0, 0, 0), color);
					player[5] = new VertexPositionColor(new Vector3(boxWidth, 0, 0), color);

					player[6] = new VertexPositionColor(new Vector3(0, 0, boxHeight), color);
					player[7] = new VertexPositionColor(new Vector3(boxWidth, 0, boxHeight), color);

					player[8] = new VertexPositionColor(new Vector3(0, 0, boxHeight), color);
					player[9] = new VertexPositionColor(new Vector3(0, boxWidth, boxHeight), color);

					player[10] = new VertexPositionColor(new Vector3(0, boxWidth, boxHeight), color);
					player[11] = new VertexPositionColor(new Vector3(0, boxWidth, 0), color);

					player[12] = new VertexPositionColor(new Vector3(boxWidth, 0, boxHeight), color);
					player[13] = new VertexPositionColor(new Vector3(boxWidth, 0, 0), color);

					player[14] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, 0), color);
					player[15] = new VertexPositionColor(new Vector3(boxWidth, 0, 0), color);

					player[16] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, 0), color);
					player[17] = new VertexPositionColor(new Vector3(0, boxWidth, 0), color);

					player[18] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, boxHeight), color);
					player[19] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, 0), color);

					player[20] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, boxHeight), color);
					player[21] = new VertexPositionColor(new Vector3(boxWidth, 0, boxHeight), color);

					player[22] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, boxHeight), color);
					player[23] = new VertexPositionColor(new Vector3(0, boxWidth, boxHeight), color);

					for (int b = 0; b < player.Length; b++)
					{
						player[b].Position.X += xDiff;
						player[b].Position.Y += yDiff;
						player[b].Position.Z += zDiff;
					}

					//player[0] = new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(0 + playerpos.x, 0 + playerpos.y, 0 + playerpos.z), color);
					//player[1] = new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(0 + playerpos.x, 100 + playerpos.y, 0 + playerpos.z), color);
					//player[2] = new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(100 + playerpos.x, 0 + playerpos.y, 0 + playerpos.z), color);

					this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, player, 0, player.Length / 2);
				}
			}
		}
	}
}
