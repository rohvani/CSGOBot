using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bot;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Botv2.Utilities
{
	static public class AimHelper
	{
		static public void setCameraPos(float x, float y)
		{
			Vector2 camera = GameHelper.getCameraAngle();

			if (camera.x != x && camera.y != y)
			{
				// Note: -0.022 seems to be the angle change when moving the cursor by one pixel along either the x or y axis
				Point cameraDifference = new Point((int)-((x + camera.x) / (Bot.mouseSensitivity * -0.022)) + Cursor.Position.X, (int)((y + camera.y) / (Bot.mouseSensitivity * -0.022)) + Cursor.Position.Y);

				Cursor.Position = cameraDifference;
				Thread.Sleep(10);
			}
		}

		public static void setCameraPosition(Vector3 target)
		{
			Vector3 playerPosition = GameHelper.getCameraWorldPosition();

			float xDistance = target.x - playerPosition.x;
			float yDistance = target.y - playerPosition.y;
			float zDistance = target.z - playerPosition.z;

			float xAngle = 0;
			float yAngle = 0;

			float RAD_TO_DEG = (float)(180 / Math.PI);

			if (xDistance <= 0)
			{
				if (yDistance < 0) xAngle = -(float)((Math.Atan(yDistance / xDistance) * RAD_TO_DEG) - 180);
				else if (yDistance > 0) xAngle = -(float)((Math.Atan(yDistance / xDistance) * RAD_TO_DEG) + 180);
			}
			else xAngle = -(float)(Math.Atan(yDistance / xDistance) * RAD_TO_DEG);

			yAngle = (float)(Math.Atan(zDistance / target.distance(playerPosition)) * RAD_TO_DEG);

			//xAngle += GameHelper.getPunchAngle();
			yAngle += GameHelper.getPunchAngle();

			setCameraPos(xAngle, yAngle);
		}

		public static Vector2 getAngleDifference(Vector3 target)
		{
			Vector3 playerPosition = GameHelper.getCurrentPosition();

			float xDistance = target.x - playerPosition.x;
			float yDistance = target.y - playerPosition.y;
			float zDistance = target.z - playerPosition.z;

			float xAngle = 0;
			float yAngle = 0;

			float RAD_TO_DEG = (float)(180 / Math.PI);

			if (xDistance <= 0)
			{
				if (yDistance < 0) xAngle = (float)((Math.Atan(yDistance / xDistance) * RAD_TO_DEG) - 180);
				else if (yDistance > 0) xAngle = (float)((Math.Atan(yDistance / xDistance) * RAD_TO_DEG) + 180);
			}
			else xAngle = (float)(Math.Atan(yDistance / xDistance) * RAD_TO_DEG);

			yAngle = -(float)(Math.Atan(zDistance / target.distance(playerPosition)) * RAD_TO_DEG);

			//Console.WriteLine("new {0} {1}", xAngle, yAngle);
			//Console.WriteLine("cur {0} {1}", getCameraPos().x, getCameraPos().y);
			//Console.WriteLine("diff {0} {1}", Math.Abs(getCameraPos().x - (xAngle * -1)), Math.Abs(getCameraPos().y - ( -1 * yAngle)));

			Vector2 cameraAngle = GameHelper.getCameraAngle();
			return new Vector2(Math.Abs(GameHelper.getCameraAngle().x - (xAngle)), Math.Abs(GameHelper.getCameraAngle().y - (yAngle)));
		}
	}
}
