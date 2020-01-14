using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : MonoBehaviour
{
	private GameObject character;
	private GameObject characterWeapon;

    // Start is called before the first frame update
    void Start()
    {
		GameObject characterRes = Resources.Load<GameObject>("Character/RPG-Character");
		UnityEditor.Animations.AnimatorController animatorController =  Resources.Load("Animator/CommonAnimatorController") as UnityEditor.Animations.AnimatorController;

		if (characterRes && animatorController)
		{
			character = Instantiate(characterRes);
			//characterWeapon = Instantiate(weaponRes);

			character.transform.localPosition = new Vector3(32, 0, 32);
			character.transform.localScale = new Vector3(5, 5, 5);

			character.AddComponent<CharacterInputCtrl>();
			Animator animator = character.GetComponent<Animator>();

			if(animator)
			{
				animator.runtimeAnimatorController = animatorController;

			}




		}

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
