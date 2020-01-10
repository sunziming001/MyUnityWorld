using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrl;

public class CharacterInputCtrl :  AbsInputCtrl
{

	public float disStep = 1.0f;
	public float angleStep = 1.0f;

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

	void MoveInput(in ActionParam param)
	{
		MoveAction.SetActionParamValid(param, false);

		if (Input.GetKey(KeyCode.UpArrow))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetSelfTranslate(param, new Vector3(0, 0 , disStep));
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetSelfTranslate(param, new Vector3(0, 0, -1*disStep));
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetSelfRotation(param, new Vector3(0, -1*angleStep, 0));
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetSelfRotation(param, new Vector3(0, angleStep, 0));
		}
	}


	void AnimatorInput(in ActionParam param)
	{
		AnimatorAction.SetActionParamValid(param, true);
		bool isMoving = false;
		if (Input.GetKey(KeyCode.UpArrow))
		{
			isMoving = true;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			isMoving = true;
		}
		else
		{
			
		}
		AnimatorAction.SetIsMoving(param, isMoving);
	}

	protected override void InitialAction2InputJudge()
	{
		MoveAction moveAction = GetComponent<MoveAction>();
		AnimatorAction animatorAction = GetComponent<AnimatorAction>();

		
		appendAction2InputJudge(animatorAction, AnimatorInput);
		appendAction2InputJudge(moveAction, MoveInput);
	}
}
