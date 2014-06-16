using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botv2.Utilities.IO.FileFormats
{
	public class Lump
	{
		public int type, offset, length, version, fourCC;

		public Lump(int type) 
		{
			this.type = type;
		}
	}
}
