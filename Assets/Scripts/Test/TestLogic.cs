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
		if(characterRes)
		{
			character = Instantiate(characterRes);
			character.transform.localPosition = new Vector3(32, 0, 32);
			character.transform.localScale = new Vector3(5, 5, 5);
		}

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
