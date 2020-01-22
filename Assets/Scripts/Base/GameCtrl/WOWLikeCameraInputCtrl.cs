using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrl;

public class WOWLikeCameraInputCtrl : AbsInputCtrl
{

	public float disStep = 1.0f;


	// Start is called before the first frame update
	void Start()
    {
        
    }

	//test function
	void MoveInput(InputInfo inputInfo, in ActionParam param)
	{
		MoveAction.SetActionParamValid(param, false);

		float theta = GetCameraDepresion();

		if (Input.GetKey(KeyCode.W))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetSelfTranslate(param, new Vector3(0, disStep * Mathf.Sin(theta), disStep * Mathf.Cos(theta)));
		}

		if (Input.GetKey(KeyCode.S))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetSelfTranslate(param, new Vector3(0, -1 * disStep * Mathf.Sin(theta), -1 * disStep * Mathf.Cos(theta)));
		}

		if (Input.GetKey(KeyCode.D))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetWorldRotation(param, new Vector3(0, -1 * disStep, 0));
		}

		if (Input.GetKey(KeyCode.A))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);
			MoveAction.SetWorldRotation(param, new Vector3(0, disStep, 0));
		}

		if (Input.GetKey(KeyCode.Q))
		{
			
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);

			Vector3 focusPt = GetCameraLandFocus();
			Vector3 axis = new Vector3(0, 1, 0);
			float angle = angleStep;

			MoveAction.SetRotateAround(param, focusPt, axis, angle);
		}

		if (Input.GetKey(KeyCode.E))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);

			Vector3 focusPt = GetCameraLandFocus();
			Vector3 axis = new Vector3(0, 1, 0);
			float angle =-1* angleStep;

			MoveAction.SetRotateAround(param, focusPt, axis, angle);
		}

		if(Input.GetKey(KeyCode.I))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);

			Vector3 focusPt = GetCameraLandFocus();
			Vector3 axis = new Vector3(1, 0, 0);
			float angle = angleStep;

			MoveAction.SetRotateAround(param, focusPt, axis, angle);
		}

		if (Input.GetKey(KeyCode.K))
		{
			MoveAction.SetActionParamValid(param, true);
			MoveAction.ClearActionParam(param);

			Vector3 focusPt = GetCameraLandFocus();
			Vector3 axis = new Vector3(1, 0, 0);
			float angle = -1*angleStep;

			MoveAction.SetRotateAround(param, focusPt, axis, angle);
		}

	}

	protected override void InitialAction2InputJudge()
	{
		MoveAction moveAction = GetComponent<MoveAction>();
		appendAction2InputJudge(moveAction, MoveInput);
	}


	private float GetCameraDepresion()
	{
		float theta = transform.eulerAngles.x / 180.0f * Mathf.PI;
		return theta;
	}

	private float GetCameraHeight()
	{
		return transform.position.y;
	}

	private Vector3 GetCameraLandFocus()
	{
		float theta = GetCameraDepresion();
		float height = GetCameraHeight();
		float d = height / Mathf.Tan(theta);

		Vector3 curPos = transform.position;
		Matrix4x4 m = Matrix4x4.Translate(new Vector3(0,height,d));


		return m.MultiplyPoint3x4(curPos);
	}
}
