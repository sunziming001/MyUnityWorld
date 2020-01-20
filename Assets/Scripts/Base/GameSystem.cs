using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{
	[RequireComponent(typeof(GameObjManager))]
	[RequireComponent(typeof(MapStreamManager))]

	public class GameSystem : MonoBehaviour
	{
		private static GameSystem instance;
		public static GameSystem GetInstance() { return instance;  }

		private void Awake()
		{
			if (instance != null
				&& instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				instance = this;

			}
		}


	}
}

