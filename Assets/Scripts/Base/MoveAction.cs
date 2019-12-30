using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{

    public class MoveAction :AbsAction
    {
		public float stepDis = 1.0f;
		public float stepAngle = 1.0f;

		private const string KeySelfTrans = "SelfTrans";
		private const string KeyWorldTrans = "WorldTrans";
		private const string KeySelfRotate = "SelfRotate";
		private const string KeyWorldRotate = "WorldRotate";

		public MoveAction()
        {
            SetIsBreakableByInput(true);

        }

		public static void SetSelfTranslate(in ActionParam param, Vector3 v)
		{
			param.PutParam(MoveAction.KeySelfTrans, v);
		}

		public static Vector3 GetSelfTranslate(ActionParam param)
		{
	
			Vector3 ret = param.GetParam<Vector3>(MoveAction.KeySelfTrans);

			return ret;
		}

		public static void SetWorldTranslate(in ActionParam param, Vector3 v)
		{
			param.PutParam(MoveAction.KeyWorldTrans, v);
		}

		public static Vector3 GetWorldTranslate(ActionParam param)
		{
			Vector3 ret = param.GetParam<Vector3>(MoveAction.KeyWorldTrans);

			return ret;
		}

		public static void SetWorldRotation(in ActionParam param, Vector3 v)
		{
			param.PutParam(MoveAction.KeyWorldRotate, v);
		}

		public static Vector3 GetWorldRotation(ActionParam param)
		{
			Vector3 ret = param.GetParam<Vector3>(MoveAction.KeyWorldRotate);

			return ret;
		}

		public static void SetSelfRotation(in ActionParam param, Vector3 v)
		{
			param.PutParam(MoveAction.KeySelfRotate, v);
		}

		public static Vector3 GetSelfRotation(ActionParam param)
		{

			Vector3 ret = param.GetParam<Vector3>(MoveAction.KeySelfRotate);

			return ret;
		}

		protected override void OnActionExecute() 
        {
            base.OnActionExecute();

			ActionParam param = GetActionParam();

			Vector3 t3Self = GetSelfTranslate(param);
			Vector3 t3World = GetWorldTranslate(param);
			Vector3 r3Self = GetSelfRotation(param);
			Vector3 r3World = GetWorldRotation(param);


			transform.Translate(t3Self, Space.Self);
			transform.Translate(t3World, Space.World);
			transform.Rotate(r3Self, Space.Self);
			transform.Rotate(r3World, Space.World);
			
		}
    }
}


