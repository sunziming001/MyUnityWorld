using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{
	public class AbsAction
	{

		private bool isTriggered = false;
		private bool isBreakableByInput = true;
		private float msSinceTrigged = 0;

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
			if(isTriggered)
			{
				msSinceTrigged += Time.deltaTime;
			}
		}

		virtual protected void OnActionFinished()
		{
			isTriggered = false;
			msSinceTrigged = 0;
		}


		public void execute()
		{
			OnActionExecute();
		}

		public void trigger(bool v)
		{
			if (v)
			{
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

