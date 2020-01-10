using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{

	public class AnimatorAction : AbsAction
	{
		private static string KeyIsMoving = "KeyIsMoving";
		private static string KeySetWeaponType = "KeySetWeaponType";

		public enum WeaponType
		{
			Relax = 0,
			TwoHandSword = 1,
		};

		AnimatorAction()
		{
			SetIsBreakableByInput(true);
		}

		public static void SetIsMoving(in ActionParam param, bool v)
		{
			param.PutParam(AnimatorAction.KeyIsMoving, v);
		}

		public static bool GetIsMoving(ActionParam param)
		{
			return param.GetParam<bool>(AnimatorAction.KeyIsMoving);
		}

		public static void SetWeaponType(in ActionParam param, WeaponType weaponTyp)
		{
			param.PutParam(AnimatorAction.KeySetWeaponType, weaponTyp);
		}

		public static WeaponType GetWeaponType(ActionParam param)
		{
			return param.GetParam<WeaponType>(AnimatorAction.KeySetWeaponType);
		}

		protected override void OnActionExecute()
		{
			base.OnActionExecute();

			ActionParam param = GetActionParam();
			bool isMoving = GetIsMoving(param);
			WeaponType weaponType = GetWeaponType(param);

			Animator anim = GetComponent<Animator>();
			anim.SetBool("isMoving", isMoving);
			anim.SetInteger("WeaponType", (int)weaponType);
		}
	}
}
