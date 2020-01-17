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

		Queue<Vector3> patrolPoints = new Queue<Vector3>();
		patrolPoints.Enqueue(new Vector3(0, 0, 0));
		patrolPoints.Enqueue(new Vector3(20, 0, 0));
		patrolPoints.Enqueue(new Vector3(0, 0, 20));

		float navMeshAgentRadius = 7.0f;
		if (characterRes)
		{
			enemyCharacter = Instantiate(characterRes);
			if(enemyCharacter)
			{
				enemyCharacter.AddComponent<GameCtrl.NPCCharacterCtrl>();
				enemyCharacter.transform.position = position;
				enemyCharacter.transform.localScale = scale;

				Animator animator = enemyCharacter.GetComponent<Animator>();
				NavMeshAgent navMeshAgent = enemyCharacter.GetComponent<NavMeshAgent>();
				GameCtrl.NPCCharacterCtrl npcCharacterCtrl = enemyCharacter.GetComponent<GameCtrl.NPCCharacterCtrl>();
				if (animator)
				{
					animator.runtimeAnimatorController = animatorController;

				}

				if(navMeshAgent)
				{
					navMeshAgent.radius = navMeshAgentRadius;
					navMeshAgent.speed = 20.0f;
				}

				if(npcCharacterCtrl)
				{
					npcCharacterCtrl.patrolPoints = patrolPoints;
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
		
	}
}
