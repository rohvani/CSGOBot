using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Botv2.Utilities.IO
{
	public class FileReader
	{
		static Boolean bigEndian = false;

		static public byte readByte(Stream stream)
		{
			byte[] buffer = new byte[1];
			stream.Read(buffer, 0, 1);
			return buffer[0];
		}

		static public short readShort(Stream stream)
		{
			byte[] buffer = new byte[2];
			stream.Read(buffer, 0, 2);
			if (bigEndian) buffer.Reverse();
			return BitConverter.ToInt16(buffer, 0);
		}

		static public ushort readUShort(Stream stream)
		{
			byte[] buffer = new byte[2];
			stream.Read(buffer, 0, 2);
			if (bigEndian) buffer.Reverse();
			return BitConverter.ToUInt16(buffer, 0);
		}

		static public int readInt(Stream stream)
		{
			byte[] buffer = new byte[4];
			stream.Read(buffer, 0, 4);
			if (bigEndian) buffer.Reverse();
			return BitConverter.ToInt32(buffer, 0);
		}

		static public uint readUInt(Stream stream)
		{
			byte[] buffer = new byte[4];
			stream.Read(buffer, 0, 4);
			if (bigEndian) buffer.Reverse();
			return BitConverter.ToUInt32(buffer, 0);
		}

		static public long readLong(Stream stream)
		{
			byte[] buffer = new byte[8];
			stream.Read(buffer, 0, 8);
			if (bigEndian) buffer.Reverse();
			return BitConverter.ToInt64(buffer, 0);
		}

		static public float readFloat(Stream stream)
		{
			byte[] buffer = new byte[4];
			stream.Read(buffer, 0, 4);
			if (bigEndian) buffer.Reverse();
			return BitConverter.ToSingle(buffer, 0);
		}
	}
}
