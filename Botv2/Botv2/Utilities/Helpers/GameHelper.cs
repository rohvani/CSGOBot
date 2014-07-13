using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bot;

namespace Botv2.Utilities
{
	static public class GameHelper
	{
		static public Vector2 getCameraAngle()
		{
			float x = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.CAMERA + 4);
			float y = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.CAMERA + 0);

			return new Vector2(x, y);
		}

		static public Vector3 getCameraWorldPosition()
		{
			float x = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.LOCAL_CAMERA_POSITION + 0);
			float y = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.LOCAL_CAMERA_POSITION + 4);
			float z = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.LOCAL_CAMERA_POSITION + 8);

			return new Vector3(x, y, z);
		}

		static public Vector3 getCurrentPosition()
		{
			return getPlayerPositon(Bot.localEntityIndex);
		}

		static public Vector3 getPlayerPositon(int playerIndex)
		{
			int radarPointer = Bot.Mem.ReadMultiLevelPointer((int)Bot.client.BaseAddress + (int)Addresses.CSGO.ENTITY_LIST + (0x10 * playerIndex), 4, new int[] { 0x00 });

			float x = Bot.Mem.ReadFloat((int)radarPointer + 0x134);
			float y = Bot.Mem.ReadFloat((int)radarPointer + 0x138);
			float z = Bot.Mem.ReadFloat((int)radarPointer + 0x13c);

			return new Vector3(x, y, z);
		}

		static public byte getPlayerTeam(int playerIndex)
		{
			int radarPointer = Bot.Mem.ReadMultiLevelPointer((int)Bot.client.BaseAddress + (int)Addresses.CSGO.ENTITY_LIST + (0x10 * playerIndex), 4, new int[] { 0x00 });
			return Bot.Mem.ReadByte((int)radarPointer + 0xF0);
		}

		static public byte getPlayerHealth(int playerIndex)
		{
			int radarPointer = Bot.Mem.ReadMultiLevelPointer((int)Bot.client.BaseAddress + (int)Addresses.CSGO.ENTITY_LIST + (0x10 * playerIndex), 4, new int[] { 0x00 });
			return Bot.Mem.ReadByte((int)radarPointer + 0xFC);
		}

		static public Vector3 getPlayerBonePosition(int playerIndex, int boneIndex)
		{
			int entityPntr = Bot.Mem.ReadMultiLevelPointer((int)Bot.client.BaseAddress + (int)Addresses.CSGO.ENTITY_LIST + (0x10 * playerIndex), 4, new int[] { 0x00 });
			int bonePntr = Bot.Mem.ReadMultiLevelPointer(entityPntr + 0xA78, 4, new int[] { 0x00 });

			Vector3 bonePos = new Vector3(4,4,4);
			bonePos.x = Bot.Mem.ReadFloat(bonePntr + 48 * boneIndex + 0x0C);
			bonePos.y = Bot.Mem.ReadFloat(bonePntr + 48 * boneIndex + 0x1C);
			bonePos.z = Bot.Mem.ReadFloat(bonePntr + 48 * boneIndex + 0x2C);

			return bonePos;
		}

		static public int detectLocalPlayerIndex()
		{
			int entityPntr = Bot.Mem.ReadMultiLevelPointer((int)Bot.client.BaseAddress + (int)Addresses.CSGO.LOCAL_ENTITY, 4, new int[] { 0x00 });
			float x = Bot.Mem.ReadFloat(entityPntr + 0xA0);
			float y = Bot.Mem.ReadFloat(entityPntr + 0xA4);

			for (int i = 0; i < Botv2.Bot.numPlayers; i++)
			{
				Vector3 entityPosition = GameHelper.getPlayerPositon(i);

				if ((int)entityPosition.x == (int)x && (int)entityPosition.y == (int)y)
				{
					Bot.localEntityIndex = i;
					return Bot.localEntityIndex;
				}
			}
			return -1;
		}

		static public float getPunchAngle()
		{
			int entityPntr = Bot.Mem.ReadMultiLevelPointer((int)Bot.client.BaseAddress + (int)Addresses.CSGO.LOCAL_ENTITY, 4, new int[] { 0x00 });
			return Bot.Mem.ReadFloat(entityPntr + 0x13dc);
		}

		static public Microsoft.Xna.Framework.Matrix getViewMatrix()
		{
			Microsoft.Xna.Framework.Matrix viewMatrix = new Microsoft.Xna.Framework.Matrix();

			viewMatrix.M11 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (0 * 4));
			viewMatrix.M12 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (1 * 4));
			viewMatrix.M13 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (2 * 4));
			viewMatrix.M14 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (3 * 4));
			viewMatrix.M21 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (4 * 4));
			viewMatrix.M22 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (5 * 4));
			viewMatrix.M23 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (6 * 4));
			viewMatrix.M24 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (7 * 4));
			viewMatrix.M31 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (8 * 4));
			viewMatrix.M32 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (9 * 4));
			viewMatrix.M33 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (10 * 4));
			viewMatrix.M34 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (11 * 4));
			viewMatrix.M41 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (12 * 4));
			viewMatrix.M42 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (13 * 4));
			viewMatrix.M43 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (14 * 4));
			viewMatrix.M44 = Bot.Mem.ReadFloat((int)Bot.client.BaseAddress + (int)Addresses.CSGO.VIEW_MATRIX + (15 * 4));

			return viewMatrix;
		}

		static public Vector2 WorldToScreen(Vector3 worldPos)
		{
			Microsoft.Xna.Framework.Matrix transData = getViewMatrix();

			float num = transData.M41 * worldPos.x + transData.M42 * worldPos.y + transData.M43 * worldPos.z + transData.M44;
			Vector2 result;

			if ((double)num < 0.01)
			{
				result = new Vector2(-1f, -1f);
			}
			else
			{
				float num2 = 1f / num;
				float num3 = (transData.M11 * worldPos.x + transData.M12 * worldPos.y + transData.M13 * worldPos.z + transData.M14) * num2;
				float num4 = (transData.M21 * worldPos.x + transData.M22 * worldPos.y + transData.M23 * worldPos.z + transData.M24) * num2;
				result = new Vector2((num3 + 1f) * 0.5f * 1920, (num4 - 1f) * -0.5f * 1080);
			}
			return result;
		}

		static public string getMapName()
		{
			if (Bot.engine == null) return "";

			int pntr = Bot.Mem.ReadMultiLevelPointer((int)Bot.engine.BaseAddress + (int)Addresses.CSGO.CURRENT_MAP, 4, new int[] { 9 });
			return Bot.Mem.ReadString(pntr);
		}
	}
}
