﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : MonoBehaviour
{
	private GameObject character;
	private Camera mainCamera = null;
    // Start is called before the first frame update
    void Awake()
    {

		initUserCtrlCharacter();
	}

	private void initUserCtrlCharacter()
	{
		GameObject characterRes = Resources.Load<GameObject>("Character/RPG-Character");
		UnityEditor.Animations.AnimatorController animatorController = Resources.Load("Animator/CommonAnimatorController") as UnityEditor.Animations.AnimatorController;
		mainCamera = gameObject.AddComponent<Camera>();

		if (mainCamera != null)
		{
			mainCamera.transform.position = new Vector3(0, 30, 0);
			mainCamera.transform.rotation = Quaternion.identity;
		}

		if (characterRes
			&& animatorController
			&& mainCamera)
		{
			character = Instantiate(characterRes);
			mainCamera.transform.parent = character.transform;
			mainCamera.transform.localPosition = character.transform.localPosition + new Vector3(0, 6, -5);
			mainCamera.transform.Rotate(new Vector3(30, 0, 0), Space.Self);

			character.transform.localPosition = new Vector3(0, 0, 0);
			character.transform.localScale = new Vector3(5, 5, 5);

			character.AddComponent<CharacterInputCtrl>();
			Animator animator = character.GetComponent<Animator>();

			if (animator)
			{
				animator.runtimeAnimatorController = animatorController;

			}

		}
	}
}
