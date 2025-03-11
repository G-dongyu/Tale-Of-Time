using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject selectPage;
    public List<Button> selectButton;
    public List<Action> actions;

    private void Awake()
    {
        selectButton[0].onClick.AddListener(() => { actions[0]?.Invoke(); });
        selectButton[1].onClick.AddListener(() => { actions[1]?.Invoke(); });
    }

    public void ShowSelect()
    {
        AddAction.instance.OpenPageBySize(selectPage);
    }

    public void RegisterAction(int id, Action action)
    {
        actions[id] += action;
    }
    
}