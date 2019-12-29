using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{
	public class ActionParam
	{
		public bool isValid = false;
		private Dictionary<string, Object> data;
		public void PutParam(string key, Object val)
		{
			data.Add(key, val);
		}

		public Object GetParam(string key)
		{
			Object ret =null;
			data.TryGetValue(key, out ret);
			return ret;
		}

	}

	public class AbsAction : MonoBehaviour
	{

		private bool isTriggered = false;
		private bool isBreakableByInput = true;
		private float msSinceTrigged = 0;

		private ActionParam actionParam = new ActionParam();

		public void SetActionParam(ActionParam param)
		{
			actionParam = param;
		}

		public ActionParam GetActionParam()
		{
			return actionParam;
		}

		public void SetIsBreakableByInput(bool v)
		{
			isBreakableByInput = v;
		}

		virtual protected void OnActionUntriggered()
		{
			if(isBreakableByInput)
			{
				FinishAction();
			}
			else 
			{
			
			}
		}

		virtual protected void OnActionTriggered()
		{
			isTriggered = true;
			msSinceTrigged = 0;
		}

		virtual protected void OnActionExecute()
		{
			msSinceTrigged += Time.deltaTime;
			
		}

		virtual protected void OnActionFinished()
		{
			isTriggered = false;
			msSinceTrigged = 0;
		}


		public void execute()
		{
			if (isTriggered)
			{
				OnActionExecute();
			}
			
		}

		public void trigger(ActionParam param)
		{
			if (param.isValid)
			{
				SetActionParam(param);
				OnActionTriggered();
			}
			else 
			{
				OnActionUntriggered();
			}
		}

		public void FinishAction()
		{
			OnActionFinished();
		}

		public float GetMsSinceTrigged()
		{
			return msSinceTrigged;
		}


	}
}

