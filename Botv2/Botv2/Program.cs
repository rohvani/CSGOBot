using System;
using System.Threading;

namespace Botv2
{
	static class Program
	{
		static void Main(string[] args)
		{
			Thread _Bot = new Thread(new ThreadStart(StartBot));
			_Bot.Start();

			while (Bot.process == null || !Bot.ready || (Bot.ready && Utilities.GameHelper.getMapName().Length < 1)) Thread.Sleep(500); // Lets wait for the game to be ready before we start the game view

			//Thread _3DGameView = new Thread(new ThreadStart(StartView));
			//_3DGameView.Start();
		}

		static void StartView()
		{
			using (GameView game = new GameView()) game.Run();
		}

		static void StartBot()
		{
			Bot.Initialize();
		}
	}
}

