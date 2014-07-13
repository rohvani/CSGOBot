using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Botv2.Utilities.IO.FileFormats;
using Microsoft.Xna.Framework;
using Botv2.XNAUtilities.Renderers;

namespace Botv2.Utilities.IO.FileFormats.BSP
{
	public class BSP
	{
		Stream stream;

		int identifier;
		int version;
		int mapRevision;

		Lump[] lumps;
		object[] lumpData;

		public BSP(Stream stream)
		{
			this.stream = stream;
			this.lumps = new Lump[64];
			this.lumpData = new object[64];

			this.identifier = FileReader.readInt(stream);
			this.version = FileReader.readInt(stream);
			this.loadLumps();
			this.mapRevision = FileReader.readInt(stream);

			Console.WriteLine("[BSPLoader] File loaded");//, ((BSPRenderer)GameView.instance.BSPRenderer).mapName);
			Console.WriteLine("[BSPLoader] Version: {0}", version);
			Console.WriteLine("[BSPLoader] Map Revision: {0}", mapRevision);
			Console.WriteLine();
		}

		private void loadLumps()
		{
			for (int i = 0; i < lumps.Length; i++)
			{
				Lump lump = new Lump(i);
				lump.offset = FileReader.readInt(stream);
				lump.length = FileReader.readInt(stream);
				lump.version = FileReader.readInt(stream);
				lump.fourCC = FileReader.readInt(stream);
				lumps[i] = lump;
			}
		}

		public List<ushort[]> getEdges()
		{
			List<ushort[]> edges = new List<ushort[]>();
			Lump lump = lumps[12];
			stream.Position = lump.offset;

			for (int i = 0; i < (lump.length / 2) / 2; i++)
			{
				ushort[] edge = new ushort[2];
				edge[0] = FileReader.readUShort(stream);
				edge[1] = FileReader.readUShort(stream);
				edges.Add(edge);
			}

			lumpData[12] = edges;
			return edges;
		}

		public List<Vector3> getVertices()
		{
			List<Vector3> vertices = new List<Vector3>();
			Lump lump = lumps[3];
			stream.Position = lump.offset;

			for (int i = 0; i < (lump.length / 3) / 4; i++)
			{
				Vector3 vertice = new Vector3();
				vertice.X = FileReader.readFloat(stream);
				vertice.Y = FileReader.readFloat(stream);
				vertice.Z = FileReader.readFloat(stream);
				vertices.Add(vertice);
			}

			lumpData[3] = vertices;
			return vertices;
		}

		public List<Face> getOriginalFaces()
		{
			List<Face> faces = new List<Face>();
			Lump lump = lumps[27];
			stream.Position = lump.offset;

			for (int i = 0; i < lump.length / 56; i++)
			{
				Face face = new Face();
				face.planeNumber = FileReader.readUShort(stream);
				face.side = FileReader.readByte(stream);
				face.onNode = FileReader.readByte(stream);
				face.firstEdge = FileReader.readInt(stream);
				face.numEdges = FileReader.readShort(stream);
				face.texinfo = FileReader.readShort(stream);
				face.dispinfo = FileReader.readShort(stream);
				face.surfaceFogVolumeID = FileReader.readShort(stream);
				face.styles[0] = FileReader.readByte(stream);
				face.styles[1] = FileReader.readByte(stream);
				face.styles[2] = FileReader.readByte(stream);
				face.styles[3] = FileReader.readByte(stream);
				face.lightOffset = FileReader.readInt(stream);
				face.area = FileReader.readFloat(stream);
				face.LightmapTextureMinsInLuxels[0] = FileReader.readInt(stream);
				face.LightmapTextureMinsInLuxels[1] = FileReader.readInt(stream);
				face.LightmapTextureSizeInLuxels[0] = FileReader.readInt(stream);
				face.LightmapTextureSizeInLuxels[1] = FileReader.readInt(stream);
				face.originalFace = FileReader.readInt(stream);
				face.numPrims = FileReader.readUShort(stream);
				face.firstPrimID = FileReader.readUShort(stream);
				face.smoothingGroups = FileReader.readUInt(stream);
				faces.Add(face);
			}

			lumpData[27] = faces;
			return faces;
		}

		public List<Face> getFaces()
		{
			if (lumpData[7] != null) return (List<Face>)lumpData[7];

			List<Face> faces = new List<Face>();
			Lump lump = lumps[7];
			stream.Position = lump.offset;

			for (int i = 0; i < lump.length / 56; i++)
			{
				Face face = new Face();
				face.planeNumber = FileReader.readUShort(stream);
				face.side = FileReader.readByte(stream);
				face.onNode = FileReader.readByte(stream);
				face.firstEdge = FileReader.readInt(stream);
				face.numEdges = FileReader.readShort(stream);
				face.texinfo = FileReader.readShort(stream);
				face.dispinfo = FileReader.readShort(stream);
				face.surfaceFogVolumeID = FileReader.readShort(stream);
				face.styles[0] = FileReader.readByte(stream);
				face.styles[1] = FileReader.readByte(stream);
				face.styles[2] = FileReader.readByte(stream);
				face.styles[3] = FileReader.readByte(stream);
				face.lightOffset = FileReader.readInt(stream);
				face.area = FileReader.readFloat(stream);
				face.LightmapTextureMinsInLuxels[0] = FileReader.readInt(stream);
				face.LightmapTextureMinsInLuxels[1] = FileReader.readInt(stream);
				face.LightmapTextureSizeInLuxels[0] = FileReader.readInt(stream);
				face.LightmapTextureSizeInLuxels[1] = FileReader.readInt(stream);
				face.originalFace = FileReader.readInt(stream);
				face.numPrims = FileReader.readUShort(stream);
				face.firstPrimID = FileReader.readUShort(stream);
				face.smoothingGroups = FileReader.readUInt(stream);
				faces.Add(face);
			}

			lumpData[7] = faces;
			return faces;
		}

		public List<Plane> getPlanes()
		{
			List<Plane> planes = new List<Plane>();
			Lump lump = lumps[1];
			stream.Position = lump.offset;

			for (int i = 0; i < lump.length / 20; i++)
			{
				Plane plane = new Plane();

				Vector3 normal = new Vector3();
				normal.X = FileReader.readFloat(stream);
				normal.Y = FileReader.readFloat(stream);
				normal.Z = FileReader.readFloat(stream);

				plane.normal = normal;
				plane.distance = FileReader.readFloat(stream);
				plane.type = FileReader.readInt(stream);

				planes.Add(plane);
			}

			lumpData[1] = planes;
			return planes;
		}

		public int[] getSurfedges()
		{
			
			Lump lump = lumps[13];
			int[] surfedges = new int[lump.length / 4];
			stream.Position = lump.offset;

			for (int i = 0; i < lump.length / 4; i++)
			{
				surfedges[i] = FileReader.readInt(stream);
			}

			lumpData[13] = surfedges;
			return surfedges;
		}

		public int[] getTextureInfo()
		{
			Lump lump = lumps[6];
			int[] textureData = new int[lump.length / 72];
			stream.Position = lump.offset;

			for (int i = 0; i < textureData.Length; i++)
			{
				stream.Position += 64;
				textureData[i] = FileReader.readInt(stream);
				stream.Position += 4;
			}

			lumpData[6] = textureData;
			return textureData;
		}
	}
}
