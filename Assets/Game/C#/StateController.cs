using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class StateController : MonoBehaviour
{
    public enum State
    {
        Idle,
        MoveToPos,
        Sit,
        WaitAsk,
        WaitDo,
        Drink,
        ExitSit,
        MoveToHome,
    }

    public State currentState;
    private NpcController _npcController;
    private DeskData _deskData;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Vector3 exitSitPos;
    private Vector3 _startPos;

    public void SwitchState(State state)
    {
        _animator = GetComponentInChildren<Animator>();
        if (currentState == state) return;
        StateExit();
        currentState = state;
        StateEnter();
    }

    private void Awake()
    {
        _npcController = GetComponent<NpcController>();
        SwitchState(State.Idle);
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        StateUpdate();
    }

    private void StateEnter()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.MoveToPos:
                _startPos = transform.position;
                _navMeshAgent.enabled = true;
                _deskData = _npcController.deskData;
                _animator.SetBool("Walk", true);
                break;
            case State.Sit:
                _animator.SetBool("Sit", true);
                _navMeshAgent.enabled = false;
                exitSitPos = transform.position;
                transform.DOMove(_deskData.check.check.position, 1f);
                transform.DORotate(_deskData.check.check.eulerAngles, 1f).OnComplete(() =>
                {
                    SwitchState(State.WaitAsk);
                });
                break;
            case State.WaitAsk:
                AddAction.instance.DelayPlay(() => { SwitchState(State.ExitSit);},4f);
                break;
            case State.WaitDo:

                break;
            case State.Drink:

                break;
            case State.ExitSit:
                _animator.SetBool("Sit", false);
                transform.DOMove(exitSitPos, 1f).OnComplete(() =>
                {
                    SwitchState(State.MoveToHome);
                });
                break;
            case State.MoveToHome:
                _navMeshAgent.enabled = true;
                _animator.SetBool("Walk", true);
                break;
        }
    }

    private void StateUpdate()
    {
        switch (currentState)
        {
            case State.Idle:

                break;
            case State.MoveToPos:
                Vector3 pos = _deskData.check.check.transform.position;
                if (_npcController.SetTarget(pos, 0.3f))
                {
                    SwitchState(State.Sit);
                }

                break;
            case State.Sit:
                break;
            case State.WaitAsk:

                break;
            case State.WaitDo:

                break;
            case State.Drink:

                break;
            case State.ExitSit:
                
                break;
            case State.MoveToHome:
                pos = _startPos;
                if (_npcController.SetTarget(pos, 0.3f))
                {
                    Destroy(gameObject);
                }

                break;
        }
    }

    private void StateExit()
    {
        switch (currentState)
        {
            case State.Idle:

                break;
            case State.MoveToPos:
                _animator.SetBool("Walk", false);

                break;
            case State.Sit:

                break;
            case State.WaitAsk:

                break;
            case State.WaitDo:

                break;
            case State.Drink:

                break;
            case State.ExitSit:
                break;
            case State.MoveToHome:

                break;
        }
    }
}