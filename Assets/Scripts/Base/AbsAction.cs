using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{
	public class ActionParam
	{
		public bool isValid = false;
		private Dictionary<string, object> data = new Dictionary<string, object>();

		public void ClearParam()
		{
			data.Clear();
		}

		public void PutParam(string key, object val)
		{
			data.Remove(key);
			data.Add(key, val);
		}

		public bool HasValue(string key)
		{
			object obj;
			bool hasValue = data.TryGetValue(key, out obj);
			return hasValue;
		}

		public T GetParam<T>(string key)
		{
			T ret = default(T);
			object obj;
			bool hasValue = data.TryGetValue(key, out obj);
			if(hasValue && obj.GetType() == typeof(T))
			{
				ret = (T)obj;
			}
			return ret;
		}

	}

	public class AbsAction : MonoBehaviour
	{

		private bool isTriggered = false;
		private bool isBreakableByInput = true;
		private float msSinceTrigged = 0;

		private ActionParam actionParam = new ActionParam();

		static public void SetActionParamValid(in ActionParam param, bool v)
		{
			param.isValid = v;
		}

		static public void ClearActionParam(in ActionParam param)
		{
			param.ClearParam();
		}

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

		virtual protected void OnActionExecuteUpdate()
		{
			msSinceTrigged += Time.deltaTime;
			
		}

		virtual protected void OnActionFinished()
		{
			isTriggered = false;
			msSinceTrigged = 0;
		}


		public void executeUpdate()
		{
			if (isTriggered)
			{
				OnActionExecuteUpdate();
			}
		}


		public void executeAnimatorMove()
		{
			if(isTriggered)
			{
				OnActionExecuteAnimatorMove();
			}
		}

		virtual protected void OnActionExecuteAnimatorMove()
		{

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

