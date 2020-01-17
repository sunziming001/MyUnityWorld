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

			StartCoroutine(OnAttackFinishedEx());
			Animator anim = GetComponent<Animator>();
			
		}


		IEnumerator OnAttackFinishedEx()
		{
			Animator anim = GetComponent<Animator>();
			yield return new WaitForSeconds(0.0f);
			anim.SetTrigger("AttackFinished");
			anim.ResetTrigger("StartAttack");
			isDuringAttack = false;
		}

		void Hit()
		{

		}

		public bool IsMoveable()
		{
			return !isDuringAttack;
		}

		public bool IsDuringAttack()
		{
			return isDuringAttack;
		}

		protected override void OnActionExecuteUpdate()
		{
			base.OnActionExecuteUpdate();

			ActionParam param = GetActionParam();
			isMoving = GetIsMoving(param);
			bool isStartAttack = GetStartAttack(param);
			weaponType = GetWeaponType(param);

			Animator anim = GetComponent<Animator>();
			anim.SetBool("isMoving", isMoving);
			anim.SetInteger("WeaponType", (int)weaponType);

			if(isStartAttack 
				&& weaponType != WeaponType.Relax
				&& !isDuringAttack)
			{
				isDuringAttack = true;
				
				anim.SetTrigger("StartAttack");
				anim.ResetTrigger("AttackFinished");
			}

		}

	}
}
