using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using ProcessMemoryReaderLib;
using System.Threading;
using System.Runtime.InteropServices;
using Bot;
using Botv2.Utilities;

namespace Botv2
{
	class Bot
	{
		static public ProcessMemoryReader Mem = new ProcessMemoryReader();
		static public Process process = null;
		static public ProcessModule engine = null;
		static public ProcessModule client = null;
		static public string processName = "csgo";
		static public int localEntityIndex = -1;

		static public float mouseSensitivity = 1f;
		static public float mouseSmoothness = 2;
		static public float angleConeLimit = 35f;	
		static public bool aimEnabled = false;
		static public bool radarEnabled = false;
		static public bool lagReduction = false;

		static public int lastTargetIndex = -1;
		static public bool waited = false;
		static public bool ready = false;

		static public int numPlayers = 64;

		public static void Initialize()
		{
			Console.Title = "Bot - Console";

			mouseSensitivity = Convert.ToSingle(Config.getValue("mouseSensitivity"));
			angleConeLimit = Convert.ToSingle(Config.getValue("angleConeLimit"));
			aimEnabled = Convert.ToBoolean(Config.getValue("aimEnabled"));
			radarEnabled = Convert.ToBoolean(Config.getValue("radarEnabled"));
			lagReduction = Convert.ToBoolean(Config.getValue("lagReduction"));

			Console.WriteLine("[Config] Settings loaded from file");//, \"config.cfg\"");
			/*Console.WriteLine();
			Console.WriteLine("\t{0} = {1}", "mouseSensitivity", mouseSensitivity);
			Console.WriteLine("\t{0} = {1}", "angleConeLimit", angleConeLimit);
			Console.WriteLine("\t{0} = {1}", "aimEnabled", aimEnabled);
			Console.WriteLine("\t{0} = {1}", "radarEnabled", radarEnabled);
			Console.WriteLine("\t{0} = {1}", "lagReduction", lagReduction);*/
			Console.WriteLine();

			Console.WriteLine("[Info] Waiting for CS:GO");
			while (Process.GetProcessesByName(processName).Length == 0) Thread.Sleep(5000); // Lets wait for the CS:GO process

			if (Process.GetProcessesByName(processName).Length > 0)
			{
				process = Process.GetProcessesByName(processName)[0];
				Mem.ReadProcess = process;
				Mem.OpenProcess();

				foreach (ProcessModule a in process.Modules)
				{
					if (a.ModuleName == "engine.dll") engine = a;
					else if (a.ModuleName == "client.dll") client = a;
				}

				Console.WriteLine("[Info] Game found: {0} - 0x{1}", process.ProcessName, process.MainModule.BaseAddress.ToString("X4"));
				Console.WriteLine("[Info] Engine found: {0} - 0x{1}", engine.ModuleName, engine.BaseAddress.ToString("X4"));
				Console.WriteLine("[Info] Client found: {0} - 0x{1}", client.ModuleName, client.BaseAddress.ToString("X4"));
				Console.WriteLine();

				ready = true;

				if (radarEnabled)
				{
					Overlay overlay;
					Thread m_Thread = new Thread(new System.Threading.ThreadStart(delegate { overlay = new Overlay(); Application.Run(overlay); }));
					m_Thread.Start();
				}

				while (true)
				{
					Vector3 pos = GameHelper.getPlayerPositon(localEntityIndex);
					if (GameView.instance != null && GameView.instance.ready)
					{
						//GameHelper.detectLocalPlayerIndex();
						//GameView.instance.camera.SetCameraPosition(new Microsoft.Xna.Framework.Vector3(pos.x, pos.y, pos.z + 50));
					}

					//GameHelper.getMapName();

					if (aimEnabled)
					{
						if ((Control.ModifierKeys & Keys.Shift) != 0)
						{
							targetBestEnemyViaScore(); 
							GameHelper.detectLocalPlayerIndex();

							Colalision.test();
						}
					}
					if (lagReduction) Thread.Sleep(15);
				}
			}
			else
			{
				Console.WriteLine("Error: Could not find process!");
				Console.Read();
			}
		}

		static void targetClosestEnemy()
		{
			byte currentTeam = GameHelper.getPlayerTeam(localEntityIndex);
			Vector3 target = null;

			for (int i = 0; i < 32; i++)
			{
				if (GameHelper.getPlayerTeam(i) != currentTeam)
				{
					if (GameHelper.getPlayerPositon(i).distance(GameHelper.getCurrentPosition()) <= 9999)
					{
						if (GameHelper.getPlayerHealth(i) > 0)
						{
							if (GameHelper.getPlayerPositon(i).x == 0 && GameHelper.getPlayerPositon(i).y == 0) continue;
							if (target == null) target = GameHelper.getPlayerPositon(i);
							else if (target.distance(GameHelper.getCurrentPosition()) > GameHelper.getPlayerPositon(i).distance(GameHelper.getCurrentPosition())) target = GameHelper.getPlayerPositon(i);
						}
					}
				}
			}

			if (target != null)
			{
				AimHelper.setCameraPosition(target);
				Console.WriteLine("Distance: {0}", target.distance(GameHelper.getCurrentPosition()));
			}
		}

		/*
			Least obvious method, will only snap to enemies within <CONFIG:angleConeLimit> degrees of your view angle and will wait a second and a half before aiming to another enemy after killing
		*/
		static void targetBestEnemy()
		{
			byte currentTeam = GameHelper.getPlayerTeam(localEntityIndex);
			Vector3 currentPosition = GameHelper.getPlayerPositon(localEntityIndex);
			Vector2 bestAngleDifference = null;
			int target = -1;

			if (!waited && lastTargetIndex != -1 && GameHelper.getPlayerHealth(lastTargetIndex) <= 0)
			{
				Thread.Sleep(800);
				waited = true;
				return;
			}

			for (int i = 0; i < 32; i++)
			{
				Vector3 position = GameHelper.getPlayerPositon(i);
				byte team = GameHelper.getPlayerTeam(i);
				byte health = GameHelper.getPlayerHealth(i);

				if (team != currentTeam && !position.isZero() && health > 0)
				{
					Vector2 angleDifference = AimHelper.getAngleDifference(position);
					if ((bestAngleDifference == null || angleDifference.x < bestAngleDifference.x) && angleDifference.x <= angleConeLimit && angleDifference.y <= 15)
					{
						target = i;
						bestAngleDifference = angleDifference;
					}
				}
			}

			if (bestAngleDifference == null) return;
			if (target >= 0)
			{
				Vector3 vecToAim = GameHelper.getPlayerBonePosition(target, 10);
				//if (vecToAim.z < GameHelper.getPlayerPositon(target).z) vecToAim = GameHelper.getPlayerPositon(target);
				//if (vecToAim.z > GameHelper.getPlayerPositon(target).z + 40) vecToAim = GameHelper.getPlayerPositon(target);


				AimHelper.setCameraPosition(vecToAim);
				lastTargetIndex = target;
				waited = false;
			}
		}

		static void targetBestEnemyViaScore()
		{
			byte currentTeam = GameHelper.getPlayerTeam(localEntityIndex);
			Vector3 currentPosition = GameHelper.getPlayerPositon(localEntityIndex);

			float[] targetScores = new float[Botv2.Bot.numPlayers];

			if (!waited && lastTargetIndex != -1 && GameHelper.getPlayerHealth(lastTargetIndex) <= 0)
			{
				Thread.Sleep(800);
				waited = true;
				return;
			}

			for (int i = 0; i < Botv2.Bot.numPlayers; i++)
			{
				Vector3 position = GameHelper.getPlayerPositon(i);
				byte team = GameHelper.getPlayerTeam(i);
				byte health = GameHelper.getPlayerHealth(i);

				targetScores[i] = 1000000;

				if (team != currentTeam && !position.isZero() && health > 0 && i != localEntityIndex)
				{
					Vector2 angleDifference = AimHelper.getAngleDifference(position);

					if (angleDifference.x <= angleConeLimit && angleDifference.y <= 15)
					{
						targetScores[i] = 0;

						targetScores[i] += angleDifference.x * 7;
						targetScores[i] += angleDifference.y / 1.5f;
						targetScores[i] += position.distance(currentPosition) / 10;
					}
				}
			}

			float winnerScore = targetScores.Min();
			int winnerIndex = targetScores.ToList().IndexOf(winnerScore);

			if (winnerIndex != 0)
			{
				Vector3 vecToAim = GameHelper.getPlayerBonePosition(winnerIndex, 10);
				//if (vecToAim.z < GameHelper.getPlayerPositon(winnerIndex).z) vecToAim = GameHelper.getPlayerPositon(winnerIndex);

				AimHelper.setCameraPosition(vecToAim);
				lastTargetIndex = winnerIndex;
				waited = false;
			}

			Console.WriteLine("Winner has index of {0} with a score of {1}", winnerIndex, winnerScore);
		}

		static void displayAllPlayerPositions()
		{
			for (int i = 0; i < 7; i++)
			{
				Vector3 pos = GameHelper.getPlayerPositon(i);
				byte team = GameHelper.getPlayerTeam(i);
				byte health = GameHelper.getPlayerHealth(i);

				Console.WriteLine("Player {0}: ({1}, {2}, {3}), Distance: {4}, Team: {5}, Health: {6}", i, pos.x, pos.y, pos.z, GameHelper.getPlayerPositon(0).distance(pos), team, health);
			}
		}
	}
}
