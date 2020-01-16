using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TestLogic : MonoBehaviour
{
	private GameObject character = null;
	private Camera mainCamera = null;
	private GameObject enemyCharacter = null;

    // Start is called before the first frame update
    void Awake()
    {
		SceneManager.LoadScene("Demo2Scene", LoadSceneMode.Additive);

		initUserCtrlCharacter();
		initEnemyCharacter();
		
	}

	private void initEnemyCharacter()
	{
		GameObject characterRes = Resources.Load<GameObject>("Character/RPG-Character");
		UnityEditor.Animations.AnimatorController animatorController = Resources.Load("Animator/CommonAnimatorController") as UnityEditor.Animations.AnimatorController;
		Vector3 position = new Vector3(0, 0, 40);
		Vector3 scale = new Vector3(5, 5, 5);
		//float navMeshAgentRadius = 2.5f;
		//float navMeshAgentSpeed = 5.0f;
		if (characterRes)
		{
			enemyCharacter = Instantiate(characterRes);
			if(enemyCharacter)
			{
				enemyCharacter.AddComponent<MoveTo>();
				enemyCharacter.transform.position = position;
				enemyCharacter.transform.localScale = scale;

				Animator animator = enemyCharacter.GetComponent<Animator>();
				NavMeshAgent navMeshAgent = enemyCharacter.GetComponent<NavMeshAgent>();

				if (animator)
				{
					animator.runtimeAnimatorController = animatorController;

				}

				if(navMeshAgent)
				{

				}
			}

		}
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

			character.transform.position = new Vector3(0, 0, 0);
			//character.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
			character.transform.localScale = new Vector3(5, 5, 5);

			character.AddComponent<CharacterInputCtrl>();
			Animator animator = character.GetComponent<Animator>();

			if (animator)
			{
				animator.runtimeAnimatorController = animatorController;

			}

		}
	}

	void Update()
	{
		MoveTo moveTo = enemyCharacter.AddComponent<MoveTo>();
		moveTo.goal = character.transform;
	}
}
