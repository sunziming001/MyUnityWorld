using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{

	public class AnimatorAction : AbsAction
	{
		private static string KeyIsMoving = "KeyIsMoving";
		private static string KeySetWeaponType = "KeySetWeaponType";
		private static string KeyStartAttack = "KeyStartAttack";

		private bool isMoving = false;
		private WeaponType weaponType = WeaponType.Relax;
		private bool isDuringAttack = false;

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

		public static void SetStartAttack(in ActionParam param, bool isStart)
		{
			param.PutParam(AnimatorAction.KeyStartAttack, isStart);
		}

		public static bool GetStartAttack(ActionParam param)
		{
			return param.GetParam<bool>(AnimatorAction.KeyStartAttack);
		}

		
		void OnAttackFinished()
		{
			isDuringAttack = false;
			Animator anim = GetComponent<Animator>();
			anim.SetTrigger("AttackFinished");
		}

		void Hit()
		{
			
		}

		public bool isMoveable()
		{
			return !isDuringAttack;
		}

		protected override void OnActionExecute()
		{
			base.OnActionExecute();

			ActionParam param = GetActionParam();
			isMoving = GetIsMoving(param);
			bool isStartAttack = GetStartAttack(param);
			weaponType = GetWeaponType(param);

			Animator anim = GetComponent<Animator>();
			anim.SetBool("isMoving", isMoving);
			anim.SetInteger("WeaponType", (int)weaponType);

			if(isStartAttack && weaponType != WeaponType.Relax)
			{
				isDuringAttack = true;
				anim.SetTrigger("StartAttack");
				anim.ResetTrigger("AttackFinished");
			}
			else
			{
				anim.ResetTrigger("StartAttack");
			}
		}
	}
}
