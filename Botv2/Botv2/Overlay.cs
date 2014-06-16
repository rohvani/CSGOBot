using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Bot;
using Botv2.Utilities;

namespace Botv2
{
	class Overlay : Form
	{
		public Vector3[] positions;

		public Overlay() : base()
		{
			this.TopMost = true;
			this.DoubleBuffered = true;
			this.ShowInTaskbar = false;
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;
			this.BackColor = Color.Purple;
			this.TransparencyKey = Color.Purple;

			positions = new Vector3[32];

			//Bot.localEntityIndex;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			drawBlips(e);

			Thread.Sleep(20);
			this.Invalidate(); //cause repaint
		}

		private void drawBlips(PaintEventArgs e)
		{
			for (int i = 0; i < 16; i++ )
			{
				Vector3 position = GameHelper.getPlayerPositon(i);

				if (!position.isZero() && GameHelper.getPlayerHealth(i) > 0 && i != Bot.localEntityIndex)
				{
					Pen pen = GameHelper.getPlayerTeam(i) != GameHelper.getPlayerTeam(Bot.localEntityIndex) ? Pens.Red : Pens.Green;

					Vector2 test = GameHelper.WorldToScreen(position);

					Vector2 pointFeet = GameHelper.WorldToScreen(GameHelper.getPlayerBonePosition(i, 0));
					Vector2 pointHead = GameHelper.WorldToScreen(GameHelper.getPlayerBonePosition(i, 11));

					e.Graphics.DrawRectangle(pen, new Rectangle((int)pointHead.x - (((int)(test.y - pointHead.y) / 2) / 2), (int)pointHead.y, (int)(test.y - pointHead.y) / 2, (int)(test.y - pointHead.y)));

					//SolidBrush brush = new System.Drawing.SolidBrush(color);
					//e.Graphics.FillRectangle(brush, new Rectangle(((int)position.x / 25)  + 500, ((int)position.y / 25) * -1 + 150, 5, 5));
				}
			}

			
		}
	}
}