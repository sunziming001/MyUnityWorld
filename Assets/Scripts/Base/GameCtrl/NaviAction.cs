using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameCtrl
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class NaviAction :AbsAction
	{
		
		private static string KeyGoToPoint = "KeyGoToPoint";

		private NavMeshAgent navAgent = null;
		private Nullable<Vector3> curTargetPosition = null;


		public static void SetGoToPoint(in ActionParam param, Nullable<Vector3> v)
		{
			param.PutParam(NaviAction.KeyGoToPoint, v);

		}

		public static Nullable<Vector3> GetGoToPoint(ActionParam param)
		{
			Nullable<Vector3> ret = null;
			if(param.HasValue(NaviAction.KeyGoToPoint))
			{
				ret = param.GetParam<Vector3>(NaviAction.KeyGoToPoint);
			}

			return ret;
		}

		public static void SetMoveSpeed(in ActionParam param, float v)
		{
			param.PutParam(NaviAction.KeyGoToPoint, v);
		}

		public static float GetMoveSpeed(ActionParam param)
		{
			return param.GetParam<float>(NaviAction.KeyGoToPoint);
		}

		void Start()
		{
			navAgent = GetComponent<NavMeshAgent>();
			navAgent.updatePosition = false;
			
		}

		protected override void OnActionExecuteUpdate()
		{
			base.OnActionExecuteUpdate();
			ActionParam actionParam = GetActionParam();

			curTargetPosition = GetGoToPoint(actionParam);
			
			if(curTargetPosition != null)
			{
				navAgent.SetDestination(curTargetPosition.Value);
				navAgent.isStopped = false;
			}
			else
			{
				navAgent.isStopped = true;
				navAgent.nextPosition = transform.position;
			}
			
		}

		protected override void OnActionExecuteAnimatorMove()
		{
			base.OnActionExecuteAnimatorMove();


			if (IsDuringNavi())
			{
				transform.position = navAgent.nextPosition;
			}
			else {
				navAgent.nextPosition = transform.position;
			}
			
		

		}

		public bool IsDuringNavi()
		{
			return curTargetPosition !=null
				&& !IsNaviDestinationReached();
		}

		public bool IsNaviDestinationReached()
		{
			if (curTargetPosition != null)
				return Vector3.Distance(curTargetPosition.Value, transform.position) < navAgent.radius;
			else
				return true;
		}
		
	}
}

