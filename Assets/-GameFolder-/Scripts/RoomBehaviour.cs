using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomBehaviour : MonoBehaviour, IState
{
    private StateMachine stateMachine;

    private Room myRoom;
    private float roomTime;
    private NavMeshAgent navMeshAgent;

    private bool firstMovement;

    public void SetRoom(Room room) { myRoom = room; }
    public void SetRoomTime(float time) { roomTime = time; }

    public void OnEnter()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        firstMovement = true;
        navMeshAgent.SetDestination(myRoom.transform.position);
    }

    public void OnExit() { }

    public void Tick()
    {
        if (firstMovement && !navMeshAgent.hasPath)
        {
            firstMovement = false;
            SetStateMachine();
        }
    }

    private void SetStateMachine()
    {
        Debug.Log("Wonder around the room");
        stateMachine = new StateMachine();
    }
}
