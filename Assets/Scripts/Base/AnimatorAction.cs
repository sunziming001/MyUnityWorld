﻿using System.Collections;
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

			//StartCoroutine(OnAttackFinishedEx());
			//Animator anim = GetComponent<Animator>();
			isDuringAttack = false;
		}


		IEnumerator OnAttackFinishedEx()
		{
			Animator anim = GetComponent<Animator>();
			yield return new WaitForSeconds(0.0f);
			anim.SetTrigger("AttackFinished");
			anim.ResetTrigger("StartAttack");
			
		}

		void AutoResetTrigger(string triggerName, float resetSeconds = 0.0f)
		{
			Animator anim = GetComponent<Animator>();
			anim.SetTrigger(triggerName);

			StartCoroutine(DelayResetTrigger(triggerName, resetSeconds));

		}

		IEnumerator DelayResetTrigger(string triggerName, float resetSeconds)
		{
			Animator anim = GetComponent<Animator>();
			yield return new WaitForSeconds(resetSeconds);
			anim.ResetTrigger(triggerName);
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
			WeaponType newWeaponType = GetWeaponType(param);

			Animator anim = GetComponent<Animator>();
			anim.SetBool("isMoving", isMoving);

			if(newWeaponType != weaponType)
			{
				anim.SetInteger("WeaponType", (int)newWeaponType);
				AutoResetTrigger("SwithWeaponTriggered");
				weaponType = newWeaponType;
			}
			



			if(isStartAttack 
				&& weaponType != WeaponType.Relax
				&& !isDuringAttack)
			{
				isDuringAttack = true;

				AutoResetTrigger("StartAttack");
			}

		}

	}
}
