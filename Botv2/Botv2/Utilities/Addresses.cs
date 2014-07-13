using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
	class Addresses
	{
		public enum CSGO
		{
			ENTITY_LIST = 0xA4E044,
			LOCAL_ENTITY = 0x9DEFD4,
			CAMERA = 0xA8297C,
		//	LOCAL_ENTITY_POSITION = 0x5C3714,
			LOCAL_CAMERA_POSITION = 0xA43300,
			CURRENT_MAP = 0x1DBFA0,
			VIEW_MATRIX = 0xA43694
		}
	}
}
