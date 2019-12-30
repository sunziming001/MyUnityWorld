using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrl;

public class WOWLikeCameraInputCtrl : AbsInputCtrl
{

	public float disStep = 0.2f;
	public float angleStep = 0.2f;

	private bool isRightMousePress = false;

	void Awake()
	{
		InitialAction2InputJudge();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		executeActions();
	}

	//test function
	void MoveInput(in ActionParam param)
	{
		MoveAction.SetActionParamValid(param, false);

		float theta = transform.eulerAngles.x / 180.0f * Mathf.PI;

		if (Input.GetKey(KeyCode.W))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.SetSelfTranslate(param, new Vector3(0, disStep * Mathf.Sin(theta), disStep * Mathf.Cos(theta)));
		}

		if (Input.GetKey(KeyCode.S))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.SetSelfTranslate(param, new Vector3(0, -1 * disStep * Mathf.Sin(theta), -1 * disStep * Mathf.Cos(theta)));
		}

		if (Input.GetKey(KeyCode.D))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.SetSelfTranslate(param, new Vector3(disStep,0,0));
		}

		if (Input.GetKey(KeyCode.A))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.SetSelfTranslate(param, new Vector3(-1 * disStep,0,0));
		}

		if (Input.GetKey(KeyCode.E))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.SetWorldRotation(param, new Vector3(0, disStep, 0));
		}

		if (Input.GetKey(KeyCode.Q))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.SetWorldRotation(param, new Vector3(0, -1 * disStep,  0));
		}
	}

	protected override void InitialAction2InputJudge()
	{
		MoveAction moveAction = GetComponent<MoveAction>();
		appendAction2InputJudge(moveAction, MoveInput);
	}
}
