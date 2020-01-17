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
		Vector3 position = new Vector3(30, 0, 35);

		float characterHeight = 1.75f;

		Queue<Vector3> patrolPoints = new Queue<Vector3>();
		patrolPoints.Enqueue(new Vector3(0, 0, 0));
		patrolPoints.Enqueue(new Vector3(20, 0, 0));
		patrolPoints.Enqueue(new Vector3(0, 0, 20));

		GameCtrl.WeaponType canEquipWeapon = GameCtrl.WeaponType.TwoHandSword;
		string rightWeaponRes = "Weapon/2HandSword/2Hand-Sword";
		string leftWeaponRes = null;

		float navMeshAgentRadius = 1.2f;
		float navMeshAgentSpeed = 1.6f;

		if (characterRes)
		{
			enemyCharacter = Instantiate(characterRes);
			if(enemyCharacter)
			{
				enemyCharacter.AddComponent<GameCtrl.NPCCharacterCtrl>();
				enemyCharacter.transform.position = position;
				ScaleCharacter(enemyCharacter, characterHeight);

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
					navMeshAgent.speed = navMeshAgentSpeed;
				}

				if(npcCharacterCtrl)
				{
					npcCharacterCtrl.patrolPoints = patrolPoints;
					npcCharacterCtrl.canEquipWeapn = canEquipWeapon;
					npcCharacterCtrl.rightWeaponRes = rightWeaponRes;
					npcCharacterCtrl.leftWeaponRes = leftWeaponRes;
				}
			}

		}
	}

	private void initUserCtrlCharacter()
	{
		GameObject characterRes = Resources.Load<GameObject>("Character/RPG-Character");
		UnityEditor.Animations.AnimatorController animatorController = Resources.Load("Animator/CommonAnimatorController") as UnityEditor.Animations.AnimatorController;
		mainCamera = gameObject.AddComponent<Camera>();
		float characterHeight = 1.75f;
		float walkSpeed = 1.27f;
		float runSpeed = 2.70f;

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

			character.transform.position = new Vector3(25, 0, 25);
			ScaleCharacter(character, characterHeight);

			//character.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
			//character.transform.localScale = new Vector3(5, 5, 5);

			CharacterInputCtrl characterInputCtrl = character.AddComponent<CharacterInputCtrl>();
			Animator animator = character.GetComponent<Animator>();
			
			if (animator)
			{
				animator.runtimeAnimatorController = animatorController;

			}

			if(characterInputCtrl)
			{
				characterInputCtrl.walkSpeed = walkSpeed;
				characterInputCtrl.runSpeed = runSpeed;
			}

		}
	}

	private void ScaleCharacter(in GameObject obj, float height)
	{
		Renderer render = obj.GetComponent<Renderer>();
		if(render == null)
		{
			render = obj.GetComponentInChildren<Renderer>();
		}

		if(render)
		{
			Vector3 trueSize = render.bounds.size;
			float factor = height / trueSize.y;
			obj.transform.localScale= new Vector3(factor, factor, factor);
		}
		

	}

	void Update()
	{
		if(Vector3.Distance(character.transform.position, enemyCharacter.transform.position)<=5.0f)
		{
			enemyCharacter.GetComponent<GameCtrl.NPCCharacterCtrl>().attackTarget = character;
		}
		else
		{
			enemyCharacter.GetComponent<GameCtrl.NPCCharacterCtrl>().attackTarget = null;
		}

		if(Input.GetKey(KeyCode.Z))
		{
			character.transform.Translate(new Vector3(0, 0, -2), Space.World);
		}
	}
}
