using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataBase
{
	public class MapInfo
	{
		public string mapName { get; set; }
		public int xCount { get; set; }
		public int zCount { get; set; }
		public int globalXIdx { get; set; }
		public int globalZIdx { get; set; }
		public int unitXSize { get; set; }
		public int unitZSize { get; set; }
	}

}
