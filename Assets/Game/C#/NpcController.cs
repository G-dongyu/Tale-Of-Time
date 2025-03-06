using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    private NavMeshAgent _agent;
    public DeskData deskData;
    private StateController _stateController;
    public GameObject[] modes;
    

    private void Awake()
    {
        RandomMode();
    }

    public void OnSpawn(Vector3 posStart, Desk desk, Check check)
    {
        deskData.desk = desk;
        deskData.check = check;
        deskData.check.isUSe = true;
        Vector3 startPos = CheckPoint(posStart);
        _agent = GetComponent<NavMeshAgent>();
        _agent.Warp(startPos);
        _stateController = GetComponent<StateController>();
        _stateController.SwitchState(StateController.State.MoveToPos);
    }

    public bool SetTarget(Vector3 pos, float minDistance)
    {
        Vector3 newPos = CheckPoint(pos);
        _agent.SetDestination(newPos);
        return Vector3.Distance(transform.position, newPos) < minDistance;
    }

    public Vector3 CheckPoint(Vector3 pos)
    {
        NavMeshHit hit;
        Vector3 newPos = Vector3.zero;
        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            newPos = hit.position;
        }

        return newPos;
    }

    private void RandomMode()
    {
        int id = Random.Range(0, modes.Length);
        GameObject a= Instantiate(modes[id], transform);
        a.transform.localPosition=Vector3.zero;
    }
    
  
}

[System.Serializable]
public class DeskData
{
    public Desk desk;
    public Check check;
}