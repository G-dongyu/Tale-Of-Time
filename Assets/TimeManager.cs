using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float eachDayTime;

    public enum DayState
    {
        Null,
        Better1,
        Less1,
        Less0
    }

    private DayState _dayState = DayState.Null;
    private IEnumerator _enumerator;
    private DialogManager _dialogManager;
    public GameObject sleepPage;
    public Text timeText;
    public Bed _bed;
    private NpcManager _npcManager;

    private void Awake()
    {
        _dialogManager = GetComponent<DialogManager>();
        _npcManager = GetComponent<NpcManager>();
    }

    private void Start()
    {
        GameProcessStart();
    }

    public DayState GetDayState()
    {
        return _dayState;
    }

    private void GameProcessStart()
    {
        if (_enumerator != null)
        {
            StopCoroutine(_enumerator);
        }
        _enumerator = Enumerator();
        StartCoroutine(_enumerator);
    }

    private IEnumerator Enumerator()
    {
        //大于一分钟
        _dayState = DayState.Better1;
        timeText.text = "营业时间";
        timeText.color = Color.green;
        yield return new WaitForSeconds((eachDayTime - 1) * 60);
        //小于一分钟
        _dayState = DayState.Less1;
        _dialogManager.Show(DialogType.TimeLess1);
        timeText.text = "关店时间";
        timeText.color = Color.red;

        yield return new WaitForSeconds(60);
        //时间结束
        _dayState = DayState.Less0;
        timeText.text = "睡眠时间";
        timeText.color = Color.white;
        _dialogManager.Show(DialogType.TimeLess0);
        Sleep();
    }


    public void Sleep()
    {
        if (_enumerator != null)
        {
            AddAction.instance.BecomeBlack(sleepPage.GetComponent<Image>(), () =>
            {
                _bed.UpSleep();
                GameProcessStart();
                _npcManager.DestroyAllNpc();
            });
            StopCoroutine(_enumerator);
            _enumerator = null;
        }
        
    }
}