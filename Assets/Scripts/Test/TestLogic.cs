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
		GameObject weaponRes = Resources.Load<GameObject>("Weapon/2HandSword/2Hand-Sword");
		if(characterRes)
		{
			character = Instantiate(characterRes);
			characterWeapon = Instantiate(weaponRes);

			character.transform.localPosition = new Vector3(32, 0, 32);
			character.transform.localScale = new Vector3(5, 5, 5);


			Animator animator = character.GetComponent<Animator>();
			Transform rightHandTransform = animator.GetBoneTransform(HumanBodyBones.RightHand);
			Transform attachPoint = characterWeapon.transform.Find("AttachPoint");

			characterWeapon.transform.parent = rightHandTransform;
			characterWeapon.transform.localPosition = rightHandTransform.localPosition - attachPoint.position;
			characterWeapon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			IKHandCtrl ikCtrl = character.GetComponent<IKHandCtrl>();
			ikCtrl.handObj = characterWeapon.transform;



		}

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
