using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrl;


[RequireComponent(typeof(MoveAction))]
[RequireComponent(typeof(AnimatorAction))]
[RequireComponent(typeof(WeaponAction))]
[RequireComponent(typeof(Animator))]

public class CharacterInputCtrl :  AbsInputCtrl
{
	public float angleStep = 1.0f;
	public float walkSpeed = 1.27f;
	public float runSpeed = 2.70f;
	void FootR()
	{

	}

	void FootL()
	{

	}

	void MoveInput(InputInfo inputInfo, in ActionParam param)
	{
		MoveAction.SetActionParamValid(param, false);
		MoveAction.ClearActionParam(param);
		if (IsMoveable() == false)
		{
			return;
		}
		var cmd2Arg = inputInfo.inputCmd2Arg;
		object tmpValue;
		if (cmd2Arg.TryGetValue(InputCmd.Forword, out tmpValue))
		{
			MoveAction.SetActionParamValid(param, true);
			float f = (float)tmpValue;
			MoveAction.SetSelfTranslate(param, new Vector3(0, 0 , GetSpeed()*f*Time.deltaTime));
		}


		if (cmd2Arg.TryGetValue(InputCmd.Rightword, out tmpValue))
		{
			MoveAction.SetActionParamValid(param, true);
			float f = (float)tmpValue;
			MoveAction.SetSelfRotation(param, new Vector3(0, angleStep*f, 0));
		}
	}


	void AnimatorInput(InputInfo inputInfo, in ActionParam param)
	{
		AnimatorAction.SetActionParamValid(param, true);

		bool isMoving = false;
		var cmd2Arg = inputInfo.inputCmd2Arg;
		object tmpValue;

		if (IsMoveable()
			&& cmd2Arg.TryGetValue(InputCmd.Forword, out tmpValue))
		{
			isMoving = true;
		}
		

		if(cmd2Arg.TryGetValue(InputCmd.Relax, out tmpValue))
		{
			AnimatorAction.SetWeaponType(param, WeaponType.Relax);
		}
		else if(cmd2Arg.TryGetValue(InputCmd.EquipWeapon, out tmpValue))
		{
			AnimatorAction.SetWeaponType(param, WeaponType.TwoHandSword);
		}

		if(cmd2Arg.TryGetValue(InputCmd.LightAttack, out tmpValue))
		{
			AnimatorAction.SetStartAttack(param, true);
		}
		else
		{
			AnimatorAction.SetStartAttack(param, false);
		}
		

		AnimatorAction.SetIsMoving(param, isMoving);

	}

	void WeaponInput(InputInfo inputInfo, in ActionParam param)
	{
		WeaponAction.SetActionParamValid(param, true);
		WeaponAction.WeaponInfo info;
		var cmd2Arg = inputInfo.inputCmd2Arg;
		object tmp = null;
		AnimatorAction animatorAction = GetComponent<AnimatorAction>();
		if(animatorAction)
		{
			if (animatorAction.GetCurWeaponType() == WeaponType.Relax)
			{
				info.handerType = HanderType.None;
				info.leftHandTransform = null;
				info.rightHandTransform = null;
				info.leftWeaponRes = null;
				info.rightWeaponRes = null;
				WeaponAction.SetWeaponInfo(param, info);

			}
			else if (animatorAction.GetCurWeaponType() == WeaponType.TwoHandSword)
			{
				info.handerType = HanderType.RightHander;
				info.leftHandTransform = null;
				info.leftWeaponRes = null;

				info.rightWeaponRes = "Weapon/2HandSword/2Hand-Sword";
				info.rightHandTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);

				WeaponAction.SetWeaponInfo(param, info);
			}
			else
			{
				WeaponAction.SetActionParamValid(param, false);
			}
		}
		

	}

	protected override void InitialAction2InputJudge()
	{
		MoveAction moveAction = GetComponent<MoveAction>();
		AnimatorAction animatorAction = GetComponent<AnimatorAction>();
		WeaponAction weaponAction = GetComponent<WeaponAction>();
		
		appendAction2InputJudge(animatorAction, AnimatorInput);
		appendAction2InputJudge(weaponAction, WeaponInput);
		appendAction2InputJudge(moveAction, MoveInput);
	}

	private bool IsMoveable()
	{
		AnimatorAction animatorAction = GetComponent<AnimatorAction>();
		return animatorAction.IsMoveable();
	}

	private bool IsDuringAttack()
	{
		AnimatorAction animatorAction = GetComponent<AnimatorAction>();
		return animatorAction.IsDuringAttack();
	}

	private float GetSpeed()
	{
		return walkSpeed;
	}
}
