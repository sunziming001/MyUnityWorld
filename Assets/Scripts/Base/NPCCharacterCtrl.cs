// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;



namespace GameCtrl
{
	
	[RequireComponent(typeof(NaviAction))]
	[RequireComponent(typeof(AnimatorAction))]
	[RequireComponent(typeof(WeaponAction))]
	[RequireComponent(typeof(Animator))]

	public class NPCCharacterCtrl : AbsInputCtrl
	{
		private enum NPCState
		{
			Patrol,
			Story,
			Battle
		};

		private Animator anim = null;
		private NavMeshAgent agent = null;
		private NaviAction naviAction = null;
		private AnimatorAction animatorAction = null;

		public GameObject attackTarget = null;
		public WeaponType canEquipWeapn = WeaponType.Relax;
		public string rightWeaponRes;
		public string leftWeaponRes;
		public HanderType handerType;

		public Queue<Vector3> patrolPoints =new Queue<Vector3>();

		void FootR()
		{

		}

		void FootL()
		{

		}

		void Start()
		{
			anim = GetComponent<Animator>();
			agent = GetComponent<NavMeshAgent>();
			naviAction = GetComponent<NaviAction>();
			animatorAction = GetComponent<AnimatorAction>();

			agent.updatePosition = false;
			attackTarget = null;
		}



		protected override InputInfo OnCollectInputInfo()
		{
			InputInfo inputInfo = base.OnCollectInputInfo();
			inputInfo.aicmd2Arg = CollectAICmd();
			return inputInfo;
		}

		protected override void InitialAction2InputJudge()
		{
			base.InitialAction2InputJudge();
			appendAction2InputJudge(GetComponent<NaviAction>(), NaviInput);
			appendAction2InputJudge(GetComponent<AnimatorAction>(), AnimatorInput);
			appendAction2InputJudge(GetComponent<WeaponAction>(), WeaponInput);
		}

		void WeaponInput(InputInfo inputInfo, in ActionParam param)
		{
			var aicmd2Arg = inputInfo.aicmd2Arg;
			object tmp = null;
			WeaponAction.SetActionParamValid(param, true);
			WeaponAction.WeaponInfo info;

			if (aicmd2Arg.TryGetValue(AICmd.LockTarget, out tmp))
			{
				info.handerType = handerType;
				info.leftHandTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand);
				info.leftWeaponRes = leftWeaponRes;

				info.rightHandTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
				info.rightWeaponRes = rightWeaponRes;

				WeaponAction.SetWeaponInfo(param, info);
			}
			else
			{
				info.handerType = handerType;
				info.leftHandTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand);
				info.leftWeaponRes = null;

				info.rightHandTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
				info.rightWeaponRes = null;

				info.rightHandTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);

				WeaponAction.SetWeaponInfo(param, info);

			}
			
		}

		void AnimatorInput(InputInfo inputInfo, in ActionParam param)
		{
			AnimatorAction.SetActionParamValid(param, true);
			var aicmd2Arg = inputInfo.aicmd2Arg;
			object tmp = null;

			if (naviAction.IsDuringNavi() )
			{
				AnimatorAction.SetIsMoving(param, true);
			}
			else
			{
				AnimatorAction.SetIsMoving(param, false);
			}

			
			if (aicmd2Arg.TryGetValue(AICmd.LockTarget, out tmp))
			{
				AnimatorAction.SetWeaponType(param, WeaponType.TwoHandSword);
			}
			else
			{
				AnimatorAction.SetWeaponType(param, WeaponType.Relax);
			}

			if(aicmd2Arg.TryGetValue(AICmd.Attack,out tmp))
			{
				AnimatorAction.SetStartAttack(param, true);
			}
			else
			{
				AnimatorAction.SetStartAttack(param, false);
			}
			
		}

		void NaviInput(InputInfo inputInfo, in ActionParam param)
		{
			NaviAction.SetActionParamValid(param, true);
		
			var aicmd2Arg = inputInfo.aicmd2Arg;
			object tmp = null;

			if (aicmd2Arg.TryGetValue(AICmd.GoToPoint, out tmp))
			{
				Vector3 pt = (Vector3)tmp;
				NaviAction.SetGoToPoint(param, pt);
			}
		}

		private Dictionary<AICmd,object> CollectAICmd()
		{
			Dictionary<AICmd, object> ret = null;
			if(attackTarget == null)
			{
				OnCollectNaviAICmd(out ret);
			}
			else
			{
				OnCollectBattleAICmd(out ret);
			}
				

			return ret;
		}

		private void OnCollectBattleAICmd(out Dictionary<AICmd, object> ret)
		{
			ret = new Dictionary<AICmd, object>();
			ret.Add(AICmd.GoToPoint, attackTarget.transform.position);
			ret.Add(AICmd.LockTarget, attackTarget);
			
			if(!naviAction.IsDuringNavi()
				&& !animatorAction.IsDuringAttack())
			{
				ret.Add(AICmd.Attack, null);

			}
			else
			{

			}
			
		}

		private void OnCollectNaviAICmd(out Dictionary<AICmd, object> ret)
		{
			ret = new Dictionary<AICmd, object>();
			if (!naviAction.IsDuringNavi())
			{

				Nullable<Vector3> curNaviDestination = DequeueNextNaviDestination();
				if (curNaviDestination != null)
				{
					ret.Add(AICmd.GoToPoint, curNaviDestination.Value);
				}
			}

		}


		Nullable<Vector3> DequeueNextNaviDestination()
		{
			Nullable<Vector3> ret = null;
			if(patrolPoints != null
				&&patrolPoints.Count>=1)
			{
				ret = patrolPoints.Dequeue();
				patrolPoints.Enqueue(ret.Value);
			}

			return ret;
		}
	}
}