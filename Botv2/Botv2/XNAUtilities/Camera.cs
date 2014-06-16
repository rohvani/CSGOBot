using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace XNAUtilities
{
	public class Camera
	{
		public Vector3 Position { get; set; }
		public float RotationYaw { get; set; }
		public float RotationPitch { get; set; }
		public float RotationSpeed { get; set; }
		public float MoveSpeed { get; set; }
		public Matrix ViewMatrix { get; set; }
		public Matrix ProjectionMatrix { get; set; }
		public BoundingFrustum Frustum { get; private set; }

		public event EventHandler<EventArgs> CameraUpdated;

		public Camera(Vector3 position, float cameraMoveSpeed)
		{
			this.Position = position;

			this.RotationYaw = 0;
			this.RotationPitch = 0;

			this.RotationSpeed = 0.3f;
			this.MoveSpeed = cameraMoveSpeed;

			this.ViewMatrix = new Matrix();
			this.ProjectionMatrix = new Matrix();

			this.Frustum = new BoundingFrustum(this.ViewMatrix * this.ProjectionMatrix);
		}

		public Camera(Vector3 position) : this(position, 30.0F) { }

		public Camera() : this(Vector3.Zero, 30.0F) { }

		public void SetViewAngle(float x, float y)
		{
			this.RotationYaw = x * (float)(Math.PI / 180);
			this.RotationPitch = y * (float)(Math.PI / 180);

			UpdateViewMatrix();
		}

		public void UpdateViewMatrix()
		{
			
			/*
			Vector3 pos = Position;
			pos.X *= -1;
			pos.Y *= -1;
			pos.Z *= -1;
			
			this.ViewMatrix = Matrix.CreateScale(1) * Matrix.CreateLookAt(Vector3.Zero, new Vector3(1, 0, 0), new Vector3(0, 0, 1)) * Matrix.CreateTranslation(pos) * Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, -MathHelper.PiOver2, MathHelper.Pi);
			*/

			/*
				Replace this shit with: (2:00:14 AM) gmxepi: ill probably write a method to get a direction vector given a yaw and pitch and position 
				and create a look at matrix from current position to the calculated direction/position, kinda like a raycast from the eyes 

			*/

			var rot = Botv2.Utilities.GameHelper.getCameraAngle();

			// y-axis is flipped in CS:GO
			Vector3 target = new Vector3((float)(Math.Cos(MathHelper.ToRadians(-rot.y)) * Math.Cos(MathHelper.ToRadians(rot.x))), (float)(Math.Sin(MathHelper.ToRadians(rot.x)) * Math.Cos(MathHelper.ToRadians(-rot.y))), (float)Math.Sin(MathHelper.ToRadians(-rot.y)));

			this.ViewMatrix = Matrix.CreateLookAt(Position, Position + target * 10, new Vector3(0, 0, 1));

			this.Frustum.Matrix = (this.ViewMatrix * this.ProjectionMatrix);
			if (this.CameraUpdated != null) this.CameraUpdated(this, new EventArgs());
		}

		public void SetCameraPosition(Vector3 vectorToAdd)
		{
			this.Position = vectorToAdd;
			UpdateViewMatrix();
		}
	}
}
