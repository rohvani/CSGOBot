using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Botv2.Utilities.IO.FileFormats.BSP
{
	public class Plane
	{
		public Vector3 normal;
		public float distance;
		public int type;

		public Plane() { }
	}
}
