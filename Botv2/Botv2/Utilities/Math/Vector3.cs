using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot
{
	public class Vector3
	{
		public float x, y, z;

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		#region Operator overloading
		static public Vector3 operator +(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z);
		}

		static public Vector3 operator-(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z);
		}

		/*static public bool operator==(Vector3 vector1, Vector3 vector2)
		{
			if (vector1.x == vector2.x && vector1.y == vector2.y && vector1.z == vector2.z)
				return true;

			return false;
		}

		static public bool operator !=(Vector3 vector1, Vector3 vector2)
		{
			if (vector1.x != vector2.x && vector1.y != vector2.y && vector1.z != vector2.z)
				return true;

			return false;
		}*/
		#endregion

		public Vector3 add(Vector3 vector)
		{
			x += vector.x;
			y += vector.y;
			z += vector.z;

			return this;
		}

		public Vector3 subtract(Vector3 vector)
		{
			x -= vector.x;
			y -= vector.y;
			z -= vector.z;

			return this;
		}

		public float magnitude()
		{
			return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
		}

		public Vector3 scale(float scalar)
		{
			x *= scalar;
			y *= scalar;
			z *= scalar;

			return this;
		}

		public float dotProduct(Vector3 vector)
		{
			return ((x * vector.x) + (y * vector.y) + (z * vector.z));
		}

		public Vector3 crossProduct(Vector3 vector)
		{
			return new Vector3((y * vector.z) - (z * vector.y), (z * vector.x) - (x * vector.z), (x * vector.y) - (y * vector.x)); 
		}

		public Vector3 normalize()
		{
			float length = magnitude();

			x /= length;
			y /= length;
			z /= length;

			return this;
		}

		public Boolean isZero()
		{
			return (x == 0 && y == 0 && z == 0);
		}

		public float distance(Vector3 target)
		{
			return (float) Math.Sqrt( Math.Pow(target.x - this.x, 2) + Math.Pow(target.y - this.y, 2) );
		}
	}
}
