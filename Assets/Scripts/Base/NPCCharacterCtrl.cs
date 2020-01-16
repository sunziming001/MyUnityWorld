// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;



namespace GameCtrl
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(Animator))]

	public class NPCCharacterCtrl : MonoBehaviour
	{
		private Animator anim = null;
		private NavMeshAgent agent = null;
		private Vector2 smoothDeltaPosition = Vector2.zero;
		private Vector2 velocity = Vector2.zero;

		public NPCType npcType = NPCType.Neutral;
		public Queue<Vector3> patrolPoints =new Queue<Vector3>();
		public float detectRange = 15.0f;

		void Start()
		{
			anim = GetComponent<Animator>();
			agent = GetComponent<NavMeshAgent>();

			// Don’t update position automatically

			agent.updatePosition = false;

			GoToNextNaviDestination();
		}

		void FootR()
		{

		}

		void FootL()
		{

		}

		void Update()
		{

			if(IsNaviDestinationReached())
			{
				anim.SetBool("isMoving", false);
				GoToNextNaviDestination();
			}
			else
			{
				anim.SetBool("isMoving", true);
			}
		

		}

		void OnAnimatorMove()
		{

			if (!IsNaviDestinationReached())
			{
				transform.position = agent.nextPosition;
			}

		}

		bool IsNaviDestinationReached()
		{
			return agent.remainingDistance < agent.radius;
		}

		void GoToNextNaviDestination()
		{
			if(patrolPoints != null
				&& patrolPoints.Count>=1)
			{
				Vector3 nextPt = patrolPoints.Dequeue();
				agent.destination = nextPt;
				
				patrolPoints.Enqueue(nextPt);
			}
		}
	}
}