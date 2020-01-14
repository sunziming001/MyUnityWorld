using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{
	public enum ActionType
	{
		Empty		= 0,
		Movement	= 1,
		Battle		= 2,
		UI			= 3

	}

	public enum WeaponType
	{
		Relax = 0,
		TwoHandSword = 1,
	};

	public enum HanderType
	{
		None = 0,
		RightHander = 1,
		LeftHander = 2,
	}

	public enum InputCmd
	{
		None = 0,
		Forword,			
		Rightword,			
		LightAttack,	
		HeavyAttack,
		SepcialAttack,
		EquipWeapon,	
		Relax,
		SwitchRun,		

	}
}
