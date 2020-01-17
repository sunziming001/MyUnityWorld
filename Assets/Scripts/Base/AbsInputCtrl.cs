using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{
	public class AbsInputCtrl : MonoBehaviour
	{
		protected delegate void ParamCollector(InputInfo inputInfo, in ActionParam param);

		protected List<KeyValuePair<AbsAction, ParamCollector>> action2InputJudge = new List<KeyValuePair<AbsAction, ParamCollector>>();



		public virtual void Awake()
		{
			InitialAction2InputJudge();
		}

		protected virtual void InitialAction2InputJudge()
		{

		}

		protected void appendAction2InputJudge(AbsAction action, ParamCollector judge)
		{
			KeyValuePair<AbsAction, ParamCollector> pair = new KeyValuePair<AbsAction, ParamCollector>(action, judge);
			action2InputJudge.Add(pair);

		}

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		public virtual void Update()
		{
			executeUpdate();
		}

		void FixedUpdate()
		{
			triggerActions();
		}

		public virtual void OnAnimatorMove()
		{
			executeAnimatorMove();
		}


		private void triggerActions()
		{
			InputInfo info = OnCollectInputInfo();
			action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, ParamCollector> pair)
			{
				ParamCollector inputJudge = pair.Value;
				AbsAction action = pair.Key;
				ActionParam param = action.GetActionParam();
				inputJudge(info, param);
				action.trigger(param);

			});
		}

		protected virtual InputInfo OnCollectInputInfo()
		{
			InputInfo info = new InputInfo();
			info.inputCmd2Arg = InputManager.collectInputCmds();
			return info;
		}

		protected void executeUpdate()
		{
			action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, ParamCollector> pair)
			{

				AbsAction action = pair.Key;
				action.executeUpdate();

			});
		}

		protected void executeAnimatorMove()
		{
			action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, ParamCollector> pair)
			{

				AbsAction action = pair.Key;
				action.executeAnimatorMove();

			});
		}
    }

}
