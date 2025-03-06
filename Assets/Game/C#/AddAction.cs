using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AddAction : MonoBehaviour
{
    public static AddAction instance; 
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void DelayPlay(Action action, float delay)
    {
        StartCoroutine(DelayPlayIEnumerator(action, delay));
    }

    private IEnumerator DelayPlayIEnumerator(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public void OpenPageBySize(GameObject page)
    {
        page.SetActive(true);
        page.transform.localScale=new Vector3(0,0,0);
        page.transform.DOScale(1, 1);
    }

    public void ClosePageBySize(GameObject page)
    {
        page.transform.DOScale(0, 1).OnComplete(()=>{page.SetActive(false);});
    }
}
