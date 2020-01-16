﻿// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

	public Transform goal;

	void Start()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		if(goal != null)
		{
			agent.destination = goal.position;
			
		}
		
	}

	void Update()
	{
		
	}
}