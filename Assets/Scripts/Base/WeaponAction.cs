using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace GameCtrl
{



	[RequireComponent(typeof(IKHandCtrl))]
	public class WeaponAction : AbsAction
	{
		private static string KeyWeaponInfo = "KeyWeaponInfo";

		private GameObject leftWeapon = null;
		private string curLeftWeaponRes = null;
		private GameObject rightWeapon = null;
		private string curRightWeaponRes = null;
		//private bool isDuringEquip = false;

		public struct WeaponInfo
		{
			public string leftWeaponRes;
			public string rightWeaponRes;
			public Transform rightHandTransform;
			public Transform leftHandTransform;
			public HanderType handerType;
		}

		

		public static void SetWeaponInfo(in ActionParam param, WeaponInfo weaponInfo)
		{
			param.PutParam(KeyWeaponInfo, weaponInfo);
		}

		public static WeaponInfo GetWeaponInfo(ActionParam param)
		{
			return param.GetParam<WeaponInfo>(KeyWeaponInfo);
		}


		protected override void OnActionExecuteUpdate()
		{
			base.OnActionExecuteUpdate();

			ActionParam param = GetActionParam();
			IKHandCtrl ikHandCtrl = GetComponent<IKHandCtrl>();
			WeaponInfo weaponInfo = GetWeaponInfo(param);

			switch(weaponInfo.handerType)
			{
				case HanderType.None:
					ikHandCtrl.ikActive = false;
					break;
				case HanderType.RightHander:
					ikHandCtrl.ikActive = true;
					ikHandCtrl.isRightHander = true;
					break;
				case HanderType.LeftHander:
					ikHandCtrl.ikActive = true;
					ikHandCtrl.isRightHander = false;
					break;
				default:
					break;
			}

			if(curLeftWeaponRes != weaponInfo.leftWeaponRes)
			{
				LoadWeapon(in leftWeapon, out leftWeapon, weaponInfo.leftWeaponRes, weaponInfo.leftHandTransform);
			}
				

			if(curRightWeaponRes != weaponInfo.rightWeaponRes)
			{
				LoadWeapon(in rightWeapon, out rightWeapon, weaponInfo.rightWeaponRes, weaponInfo.rightHandTransform);
			}

			curLeftWeaponRes = weaponInfo.leftWeaponRes;
			curRightWeaponRes = weaponInfo.rightWeaponRes;
			
		}

		private void LoadWeapon(in GameObject originWeaponObj, out GameObject weaponObj, string resource, Transform handTransform)
		{

			if (originWeaponObj)
			{
				originWeaponObj.SetActive(false);
				Destroy(originWeaponObj);
			}

			weaponObj = null;

			if (resource == null
				|| handTransform == null)
			{
				return;
			}
			
			GameObject weaponRes = Resources.Load<GameObject>(resource);
			if(weaponRes)
			{
				
				weaponObj = Instantiate(weaponRes);
				if(weaponObj)
				{
					Transform attachPoint = weaponObj.transform.Find("AttachPoint");
					if(attachPoint != null)
					{
						//x:手臂 y:手心 z:虎口
						weaponObj.transform.parent = handTransform;
						weaponObj.transform.localPosition = attachPoint.position;
						weaponObj.transform.localRotation = Quaternion.identity;
						
						weaponObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

						IKHandCtrl ikCtrl = GetComponent<IKHandCtrl>();
						ikCtrl.handObj = weaponObj.transform;
					}
					else
					{
						Debug.LogWarning("Failed to Find Attach Point: " + resource+", Please add a Attach Point please");
					}

				}
				else
				{
					Debug.LogWarning("Failed to Instantiate: " + resource);
				}
				

			}
			else
			{
				Debug.LogWarning("No weapon resource: " + resource);
			}

		}

	}


}

