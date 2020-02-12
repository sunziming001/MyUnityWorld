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
		private Dictionary<string, bool> triggerName2IsTriggered = new Dictionary<string, bool>();

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
		}

		IEnumerator OnAttackFinishedEx()
		{
			Animator anim = GetComponent<Animator>();
			yield return new WaitForSeconds(0.0f);
			anim.SetBool("IsDuringAttack", false);
		}


		bool AutoResetTrigger(string triggerName, float resetSeconds = 0.0f)
		{
			Animator anim = GetComponent<Animator>();
			bool isTriggered = false;

			if (triggerName2IsTriggered.TryGetValue(triggerName, out isTriggered) == false)
			{
				triggerName2IsTriggered.Add(triggerName, false);
				isTriggered = false;
			}

			if (!isTriggered)
			{
				anim.SetTrigger(triggerName);
				triggerName2IsTriggered.Remove(triggerName);
				triggerName2IsTriggered.Add(triggerName, true);
				StartCoroutine(DelayResetTrigger(triggerName, resetSeconds));
			}




			return true;
		}

		IEnumerator DelayResetTrigger(string triggerName, float resetSeconds)
		{
			Animator anim = GetComponent<Animator>();
			yield return new WaitForSeconds(resetSeconds);
			anim.ResetTrigger(triggerName);

			triggerName2IsTriggered.Remove(triggerName);
			triggerName2IsTriggered.Add(triggerName, false);
		}

		void Hit()
		{

		}

		public bool IsMoveable()
		{
			return !IsDuringAttack();
		}

		public bool IsDuringAttack()
		{
			Animator anim = GetComponent<Animator>();
			return anim.GetBool("IsDuringAttack");
		}

		public bool IsDuringSwitchWeapon()
		{
			bool ret = false;

			triggerName2IsTriggered.TryGetValue("SwithWeaponTriggered", out ret);
			return ret;
		}


		protected override void OnActionExecuteUpdate()
		{
			base.OnActionExecuteUpdate();
			Animator anim = GetComponent<Animator>();
			ActionParam param = GetActionParam();

			isMoving = GetIsMoving(param);
			bool isStartAttack = GetStartAttack(param);
			WeaponType newWeaponType = GetWeaponType(param);
			WeaponType weaponType = (WeaponType)anim.GetInteger("WeaponType");

			
			anim.SetBool("isMoving", isMoving);

			if (newWeaponType != weaponType
				&& !IsDuringAttack())                               //switch weapon
			{
				if (AutoResetTrigger("SwithWeaponTriggered", 0.5f))
				{
					anim.SetInteger("WeaponType", (int)newWeaponType);
					weaponType = newWeaponType;
				}

			}
			else if (isStartAttack
				&& !IsDuringAttack()
				&& weaponType != WeaponType.Relax)              //trigger attack
			{
				if (AutoResetTrigger("StartAttack", 1.0f))
				{
					anim.SetBool("IsDuringAttack", true);
					anim.SetBool("isMoving", false);
				}
			}


		}

		public WeaponType GetCurWeaponType()
		{
			Animator anim = GetComponent<Animator>();
			return (WeaponType)anim.GetInteger("WeaponType");
		}

	}
}
