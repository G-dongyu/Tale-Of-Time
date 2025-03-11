using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecialNpc : ActiveItem
{
    public enum State
    {
        Null,
        GoToStore,
        Chat,
        GoBack,
    }

    public State state;
    private NavMeshAgent _agent;
    private ChatManager _chatManager;
    private DialogManager _dialogManager;
    public Transform storePoint;
    public Transform backPoint;
    private bool _isActive;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _dialogManager = GetComponent<DialogManager>();
        _dialogManager.AddEvent(DialogType.SpecialNpc, () => { _chatManager.ShowSelect(); });
        _chatManager.RegisterAction(0, () => { SwitchState(State.GoBack); });
        _chatManager.RegisterAction(1, () => { SwitchState(State.GoToStore); });
    }

    public new void Start()
    {
        SwitchState(State.GoToStore);
    }

    private void SwitchState(State state)
    {
        StateExit();
        this.state = state;
        StateEnter();
    }

    private void Update()
    {
        StateUpdate();
    }

    private void StateEnter()
    {
        switch (state)
        {
            case State.GoToStore:
                break;
            case State.Chat:
                _dialogManager.Show(DialogType.SpecialNpc);
                break;
            case State.GoBack:
                break;
        }
    }

    private void StateUpdate()
    {
        switch (state)
        {
            case State.GoToStore:
                if (SetTarget(storePoint.position, 0.3f))
                {
                    _isActive = true;
                }

                break;
            case State.Chat:
                break;
            case State.GoBack:
                if (SetTarget(backPoint.position, 0.3f))
                {
                }

                break;
        }
    }

    private void StateExit()
    {
        switch (state)
        {
            case State.GoToStore:
                _isActive = false;
                break;
            case State.Chat:
                break;
            case State.GoBack:
                break;
        }
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

    public override void OnAround()
    {
        TagSet("东方客商");
        TagLookAt();
    }

    public override void OnEnter()
    {
        TagShow(true);
    }

    public override void OnExit()
    {
        TagShow(false);
    }

    public override void Dete()
    {
        if (_isActive)
        {
            SwitchState(State.Chat);
            TagShow(false);
        }
    }
}