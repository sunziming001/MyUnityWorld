using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKHandCtrl : MonoBehaviour
{

	protected Animator animator;

	public bool ikActive = false;
	public bool isRightHander = true;
	public Transform handObj = null;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if (animator)
		{

			AvatarIKGoal curIKGoal = AvatarIKGoal.LeftHand;
			if(!isRightHander)
			{
				curIKGoal = AvatarIKGoal.RightHand;
			}
			//if the IK is active, set the position and rotation directly to the goal.
			if (ikActive)
			{

				//weight = 1.0 for the right hand means position and rotation will be at the IK goal (the place the character wants to grab)
				animator.SetIKPositionWeight(curIKGoal, 1.0f);
				animator.SetIKRotationWeight(curIKGoal, 1.0f);

				//set the position and the rotation of the right hand where the external object is
				if (handObj != null)
				{
					animator.SetIKPosition(curIKGoal, handObj.position);
					animator.SetIKRotation(curIKGoal, handObj.rotation);
				}

			}

			//if the IK is not active, set the position and rotation of the hand back to the original position
			else
			{
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
			}
		}
	}
}
