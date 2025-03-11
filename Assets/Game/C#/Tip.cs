using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    public Image image;
    public Image fill;
    public float waitDoTime;
    private float _time;
    private bool _isPlay;
    private Camera _camera;
    public Image trueOfFalseTip;
    public Sprite[] sprite;

    public void Init(Sprite sprite, float waitDoTime)
    {
        image.sprite = sprite;
        this.waitDoTime = waitDoTime;
        _time = waitDoTime;
        _isPlay = true;
        Open();
        fill.DOColor(Color.red, waitDoTime);
        trueOfFalseTip.gameObject.SetActive(false);
    }

    private void Open()
    {
        float size = transform.localScale.x;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.DOScale(size, 0.5f);
    }

    public void Close()
    {
        transform.DOScale(0, 0.5f);
    }

    public void ShowTrueOfFalse(bool isTrue)
    {
        trueOfFalseTip.gameObject.SetActive(true);
        trueOfFalseTip.transform.localScale = Vector3.zero;
        trueOfFalseTip.transform.DOScale(1, 0.5f);
        trueOfFalseTip.sprite = sprite[isTrue ? 0 : 1];
        
    }

    private void Update()
    {
        if (_isPlay)
        {
            if (_time >= 0)
            {
                _time -= Time.deltaTime;
                fill.fillAmount = _time / waitDoTime;
            }
            LookUp();
        }

    }

    private void LookUp()
    {
        _camera= Camera.main;
        transform.LookAt(_camera.transform);
    }
}