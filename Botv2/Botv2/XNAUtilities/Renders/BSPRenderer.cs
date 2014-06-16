using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Botv2.Utilities.IO;
using Botv2.Utilities.IO.FileFormats.BSP;

namespace Botv2.XNAUtilities.Renderers
{
	public class BSPRenderer : Renderer
	{
		public string mapName;
		private string mapPath;
		private List<VertexPositionColor[]> Faces;
		private VertexPositionColor[] Edges;
		private BasicEffect EdgesEffect;

		public override void Load()
		{
			mapName = Botv2.Utilities.GameHelper.getMapName();
			mapPath = Path.GetDirectoryName(Bot.process.Modules[0].FileName) + "/csgo/maps/" + mapName;

			FileStream stream = File.OpenRead(mapPath);
			BSP map = new BSP(stream);

			List<Vector3> vertices = map.getVertices();
			List<ushort[]> edges = map.getEdges();
			List<Face> faces = map.getOriginalFaces();
			map.getFaces();
			int[] surfedges = map.getSurfedges();
			int[] textureData = map.getTextureInfo();

			stream.Close();

			this.EdgesEffect = new BasicEffect(this.GraphicsDevice);
			this.EdgesEffect.World = Matrix.Identity;
			this.EdgesEffect.TextureEnabled = false;
			this.EdgesEffect.VertexColorEnabled = true;

			this.Edges = new VertexPositionColor[edges.Count * 2];

			for (int i = 0; i < edges.Count; i++)
			{
				//if (edges[i][0] > vertices.Count - 1 || edges[i][1] > vertices.Count - 1) continue;

				Edges[i * 2] = new VertexPositionColor(vertices[edges[i][0]], Color.Black);
				Edges[(i * 2) + 1] = new VertexPositionColor(vertices[edges[i][1]], Color.Black);
			}

			Faces = new List<VertexPositionColor[]>();
			
			for (int i = 0; i < faces.Count; i++)
			{	
				Face face = faces[i];

				//if (faces[i].texinfo > textureData.Length) continue;

				List<VertexPositionColor> temp = new List<VertexPositionColor>();

				for (int b = 0; b <= face.numEdges; b++)
				{
					if (surfedges[face.firstEdge + b] < 0)
					{
						temp.Add(new VertexPositionColor(vertices[(int)edges[Math.Abs(surfedges[face.firstEdge + b])][0]], Color.LightGray));
						temp.Add(new VertexPositionColor(vertices[(int)edges[Math.Abs(surfedges[face.firstEdge + b])][1]], Color.LightGray));
						continue;
					}

					int edgeIndex = Math.Abs(surfedges[face.firstEdge + b]);
					int verticeOne = edges[edgeIndex][1];
					int verticeTwo = edges[edgeIndex][0];

					temp.Add(new VertexPositionColor(vertices[verticeOne], Color.LightGray));
					temp.Add(new VertexPositionColor(vertices[verticeTwo], Color.LightGray));
				}

				Faces.Add(temp.ToArray());
			}
		}

		public override void Draw()
		{
			this.EdgesEffect.View = this.Camera.ViewMatrix;
			this.EdgesEffect.Projection = this.Camera.ProjectionMatrix;

			/*foreach (EffectPass pass in this.EdgesEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				for (int i = 0; i < Faces.Count; i++)
				{
					this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Faces[i], 0, Faces[i].Length - 2);
				}
			}*/

			foreach (EffectPass pass in this.EdgesEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, this.Edges, 0, this.Edges.Length / 2);
			}		
		}
	}
}
