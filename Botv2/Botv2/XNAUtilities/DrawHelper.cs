using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botv2.XNAUtilities
{
	// Helper for drawing shit, like boxes and stuff
	public class DrawHelper
	{
		public static VertexPositionColor[] createBox(int boxWidth, int boxHeight, Color color)
		{
			VertexPositionColor[] box = new VertexPositionColor[24];

			box[0] = new VertexPositionColor(new Vector3(0, 0, 0), color);
			box[1] = new VertexPositionColor(new Vector3(0, 0, boxHeight), color);

			box[2] = new VertexPositionColor(new Vector3(0, 0, 0), color);
			box[3] = new VertexPositionColor(new Vector3(0, boxWidth, 0), color);

			box[4] = new VertexPositionColor(new Vector3(0, 0, 0), color);
			box[5] = new VertexPositionColor(new Vector3(boxWidth, 0, 0), color);

			box[6] = new VertexPositionColor(new Vector3(0, 0, boxHeight), color);
			box[7] = new VertexPositionColor(new Vector3(boxWidth, 0, boxHeight), color);

			box[8] = new VertexPositionColor(new Vector3(0, 0, boxHeight), color);
			box[9] = new VertexPositionColor(new Vector3(0, boxWidth, boxHeight), color);

			box[10] = new VertexPositionColor(new Vector3(0, boxWidth, boxHeight), color);
			box[11] = new VertexPositionColor(new Vector3(0, boxWidth, 0), color);

			box[12] = new VertexPositionColor(new Vector3(boxWidth, 0, boxHeight), color);
			box[13] = new VertexPositionColor(new Vector3(boxWidth, 0, 0), color);

			box[14] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, 0), color);
			box[15] = new VertexPositionColor(new Vector3(boxWidth, 0, 0), color);

			box[16] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, 0), color);
			box[17] = new VertexPositionColor(new Vector3(0, boxWidth, 0), color);

			box[18] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, boxHeight), color);
			box[19] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, 0), color);

			box[20] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, boxHeight), color);
			box[21] = new VertexPositionColor(new Vector3(boxWidth, 0, boxHeight), color);

			box[22] = new VertexPositionColor(new Vector3(boxWidth, boxWidth, boxHeight), color);
			box[23] = new VertexPositionColor(new Vector3(0, boxWidth, boxHeight), color);

			return box;
		}
	}
}
