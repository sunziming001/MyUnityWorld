using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{

	public class AnimatorAction : AbsAction
	{
		private static string KeyIsMoving = "IsMoving";

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

		protected override void OnActionExecute()
		{
			base.OnActionExecute();

			ActionParam param = GetActionParam();
			bool isMoving = GetIsMoving(param);

			Animator anim = GetComponent<Animator>();
			anim.SetBool("IsMoving", isMoving);
		}
	}
}
