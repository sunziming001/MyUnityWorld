using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{
	public class GameObjManager : MonoBehaviour
	{
		private static GameObjManager instance;
		public static GameObjManager GetInstance() {  return instance;  }

		private bool isInited = false;
		private GameObject character;
		private Camera mainCamera;
		private Light lightSpot;


		private void Awake()
		{
			if (instance != null
				&& instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				instance = this;
				
			}
			Init();
		}

		private void Init()
		{
			if(isInited)
			{
				return;
			}
			else
			{
				isInited = true;
			}
			GameObject characterRes = Resources.Load<GameObject>("Character/RPG-Character");
			RuntimeAnimatorController animatorController = Resources.Load("Animator/CommonAnimatorController") as RuntimeAnimatorController;
			mainCamera = gameObject.AddComponent<Camera>();

			Vector3 cameraPos = new Vector3(0, 30, 0);
			Vector3 cameraLocPos = new Vector3(0, 6, -5);
			Vector3 cameraRotate = new Vector3(30, 0, 0);

			Vector3 characterPos = new Vector3(25, 0, 25);

			float characterHeight = 1.75f;
			float walkSpeed = 1.27f;
			float runSpeed = 2.70f;

			if (mainCamera != null)
			{
				mainCamera.transform.position = cameraPos;
				mainCamera.transform.rotation = Quaternion.identity;
			}

			if (characterRes
				&& animatorController
				&& mainCamera)
			{
				character = Instantiate(characterRes);
				mainCamera.transform.parent = character.transform;
				mainCamera.transform.localPosition = character.transform.localPosition + cameraLocPos;
				mainCamera.transform.Rotate(cameraRotate, Space.Self);

				character.transform.position = characterPos;
				ScaleCharacter(character, characterHeight);

				

				CharacterInputCtrl characterInputCtrl = character.AddComponent<CharacterInputCtrl>();
				Animator animator = character.GetComponent<Animator>();

				if (animator)
				{
					animator.runtimeAnimatorController = animatorController;

				}

				if (characterInputCtrl)
				{
					characterInputCtrl.walkSpeed = walkSpeed;
					characterInputCtrl.runSpeed = runSpeed;
				}

			}
		}

		public static void ScaleCharacter(in GameObject obj, float height)
		{
			Renderer render = obj.GetComponent<Renderer>();
			if (render == null)
			{
				render = obj.GetComponentInChildren<Renderer>();
			}

			if (render)
			{
				Vector3 trueSize = render.bounds.size;
				float factor = height / trueSize.y;
				obj.transform.localScale = new Vector3(factor, factor, factor);
			}
		}

		public Transform GetPlayerTransfrom()
		{
			Transform ret = null;

			if (character)
				ret = character.transform;
			return ret;
		}
	}
}

