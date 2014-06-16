using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botv2.Utilities.IO.FileFormats.BSP
{
	public class Face
	{
		public ushort planeNumber;
		public byte side;
		public byte onNode;
		public int firstEdge;
		public short numEdges;
		public short texinfo;
		public short dispinfo;
		public short surfaceFogVolumeID;
		public byte[] styles = new byte[4];
		public int lightOffset;
		public float area;
		public int[] LightmapTextureMinsInLuxels = new int[2];
		public int[] LightmapTextureSizeInLuxels = new int[2];
		public int originalFace;
		public ushort numPrims;
		public ushort firstPrimID;
		public uint smoothingGroups;

		public Face()
		{
		}
	}
}
