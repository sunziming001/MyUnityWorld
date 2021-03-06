﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GameCtrl
{
	public class InputInfo
	{
		public Dictionary<InputCmd, object> inputCmd2Arg = null;
		public Dictionary<AICmd, object> aicmd2Arg =null ;
	}

	public class InputManager
	{
		public static Dictionary<InputCmd, object> collectInputCmds()
		{
			
			Dictionary<InputCmd, object> ret =new Dictionary<InputCmd, object>();

			if (Input.GetKey(KeyCode.W))
			{
				ret.Add(InputCmd.Forword, 1.0f);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				ret.Add(InputCmd.Forword, -1.0f);
			}

			if (Input.GetKey(KeyCode.A))
			{
				ret.Add(InputCmd.Rightword, -1.0f);
			}
			else if (Input.GetKey(KeyCode.D))
			{
				ret.Add(InputCmd.Rightword, 1.0f);
			}

			if(Input.GetKey(KeyCode.Alpha1))
			{
				ret.Add(InputCmd.EquipWeapon, null);
			}
			else if (Input.GetKey(KeyCode.Alpha0))
			{
				ret.Add(InputCmd.Relax, null);
			}

			if (Input.GetKey(KeyCode.R))
			{
				ret.Add(InputCmd.LightAttack, null);
			}

			if (Input.GetKey(KeyCode.Space))
			{
				ret.Add(InputCmd.SwitchRun, null);
			}

			
			return ret;
		}
	}

}

