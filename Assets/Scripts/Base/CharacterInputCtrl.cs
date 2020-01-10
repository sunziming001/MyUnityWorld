using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrl;

public class CharacterInputCtrl :  AbsInputCtrl
{
	void Awake()
	{
		InitialAction2InputJudge();
	}

	void FootR()
	{

	}

	void FootL()
	{

	}

	// Start is called before the first frame update
	void Start()
	{

	}

	void Update()
	{
		executeActions();
	}

	void MoveInput(in ActionParam param)
	{
		AnimatorAction.SetActionParamValid(param, false);

		if (Input.GetKey(KeyCode.W))
		{
			AnimatorAction.SetActionParamValid(param, true);
			AnimatorAction.ClearActionParam(param);
		}
	}

	protected override void InitialAction2InputJudge()
	{
		AnimatorAction moveAction = GetComponent<AnimatorAction>();
		appendAction2InputJudge(moveAction, MoveInput);
	}
}
