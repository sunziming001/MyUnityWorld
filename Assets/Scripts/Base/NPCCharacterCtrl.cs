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


		public Queue<Vector3> patrolPoints =new Queue<Vector3>();


		void Start()
		{
			anim = GetComponent<Animator>();
			agent = GetComponent<NavMeshAgent>();
			naviAction = GetComponent<NaviAction>();

			agent.updatePosition = false;
		}

		void FootR()
		{

		}

		void FootL()
		{

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
		}

		void AnimatorInput(InputInfo inputInfo, in ActionParam param)
		{
			AnimatorAction.SetActionParamValid(param, true);
			

			if (naviAction.IsDuringNavi())
			{
				AnimatorAction.SetIsMoving(param, true);
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
			Dictionary<AICmd, object> ret =new Dictionary<AICmd, object>();
			NaviAction naviAction = GetComponent<NaviAction>();
			if (!naviAction.IsDuringNavi())
			{
				
				Nullable<Vector3> curNaviDestination = DequeueNextNaviDestination();
				if(curNaviDestination != null)
				{
					ret.Add(AICmd.GoToPoint, curNaviDestination.Value);
				}
				
			}

			

			return ret;
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